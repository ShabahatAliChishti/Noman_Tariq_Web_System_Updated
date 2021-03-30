using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using WebBasedSystem.Models;
using WebBasedSystem.ViewModels;

namespace WebBasedSystem.Controllers
{
    public class AccountController : Controller
    {
        WebBaseSystemEntities db = new WebBaseSystemEntities();
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserModel u)
        {
            User user = new User();

            if (u != null)
            {
                user.EmailAddress = u.Email;
                user.Pass = u.Password;

                var x = db.Users.Where(a => a.EmailAddress == user.EmailAddress && a.Pass == user.Pass).SingleOrDefault();
                if (x == null)
                {
                    ViewBag.ErrorMessage = "Login Failed";
                    return View();

                }
                else
                {
                    if (x.RoleId == 1)
                    {
                        Session["AdminId"] = x.UserId;
                        Session["AdminName"] = x.FirstName + ' ' + x.LastName;
                        return RedirectToAction("Dashboard", "Admin");
                    }
                    else if (x.RoleId == 2)
                    {
                        Session["StandardUserId"] = x.UserId;
                        Session["StandardUserName"] = x.FirstName + ' ' + x.LastName;
                        return RedirectToAction("Dashboard", "StandardUser");
                    }

                }

            }


            return View();
        }




        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string EmailID)
        {
            string resetCode = Guid.NewGuid().ToString();
            var verifyUrl = "/Account/ResetPassword/" + resetCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            using (var context = new WebBaseSystemEntities())
            {
                var getUser = (from s in context.Users where s.EmailAddress == EmailID select s).FirstOrDefault();
                if (getUser != null)
                {
                    getUser.ResetPasswordCode= resetCode;
                    //This line I have added here to avoid confirm password not match issue , as we had added a confirm password property 
                    context.Configuration.ValidateOnSaveEnabled = false;
                    context.SaveChanges();

                    var subject = "Password Reset Request";
                    var body = "Hi " + getUser.FirstName + ", <br/> You recently requested to reset your password for your account. Click the link below to reset it. " +

                         " <br/><br/><a href='" + link + "'>" + link + "</a> <br/><br/>" +
                         "If you did not request a password reset, please ignore this email or reply to let us know.<br/><br/> Thank you";

                    SendEmail(getUser.EmailAddress, body, subject);

                    ViewBag.Message = "Reset password link has been sent to your email id.";
                }
                else
                {
                    ViewBag.Message = "User doesn't exists.";
                    return View();
                }
            }

            return View();
        }


        public ActionResult ResetPassword(string id)
        {
            //Verify the reset password link
            //Find account associated with this link
            //redirect to reset password page
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }

            using (var context = new WebBaseSystemEntities())
            {
                var user = context.Users.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
                if (user != null)
                {
                    ResetPassword model = new ResetPassword();
                    model.ResetCode = id;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
            [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPassword model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                using (var context = new WebBaseSystemEntities())
                {
                    var user = context.Users.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                    if (user != null)
                    {
                        //you can encrypt password here, we are not doing it
                        user.Pass = model.NewPassword;
                        //make resetpasswordcode empty string now
                        user.ResetPasswordCode = "";
                        //to avoid validation issues, disable it
                        context.Configuration.ValidateOnSaveEnabled = false;
                        context.SaveChanges();
                        message = "New password updated successfully";
                    }
                }
            }
            else
            {
                message = "Something invalid";
            }
            ViewBag.Message = message;
            return View(model);
        }

        private void SendEmail(string emailAddress, string body, string subject)
        {
            try
            {
                using (MailMessage mm = new MailMessage("shabahatalichishti@gmail.com", emailAddress))
                {
                    mm.Subject = subject;
                    mm.Body = body;

                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential("shabahatalichishti@gmail.com", "orangeapple1234576.");
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);

                }
            }
            catch(Exception ex)
            {
                
            }
        }



    }
}
