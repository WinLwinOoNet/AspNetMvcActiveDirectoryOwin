using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AspNetMvcActiveDirectoryOwin.Web.Common.Models.Account
{
    public class LoginModel
    {
        [Display(Name = "Domain")]
        [Required(ErrorMessage = "Please enter your domain.")]
        public string Domain { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "Please enter your username.")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter your password.")]
        public string Password { get; set; }

        public IList<SelectListItem> AvailableDomains { get; set; }

        public LoginModel()
        {
            AvailableDomains = new List<SelectListItem>();
        }
    }
}