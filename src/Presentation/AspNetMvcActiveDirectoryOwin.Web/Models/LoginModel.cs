using System.ComponentModel.DataAnnotations;

namespace AspNetMvcActiveDirectoryOwin.Web.Models
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
    }
}