using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBasedSystem.ViewModels
{
    public class UserModel
    {

        [Display(Name = "Email:")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        //[Display(Name = "User Name:")]
        //[Required(ErrorMessage = "User Name is required.")]
        //[MinLength(6, ErrorMessage = "User Name should have atleast  6 characters.")]
        //[MaxLength(10)]
        //public string UserName { get; set; }
        [Display(Name = "Password:")]
        [Required(ErrorMessage = "Password is required."), MaxLength(10)]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Password should be atleast 4 characters long")]
        public string Password { get; set; }
        //[Display(Name = "Confirm Password:")]
        //[Required(ErrorMessage = "Confirm Password is required."), MaxLength(10)]
        //[DataType(DataType.Password)]
        //[MinLength(4, ErrorMessage = "Password should be atleast 4 characters long")]
        //[Compare("Password")]

        //public string ConfirmPassword { get; set; }
    }
}