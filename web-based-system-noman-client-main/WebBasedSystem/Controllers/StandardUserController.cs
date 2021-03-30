using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBasedSystem.Controllers
{
    public class StandardUserController : Controller
    {
        // GET: StandardUser
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}