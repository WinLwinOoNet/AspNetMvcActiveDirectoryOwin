using System;
using System.ComponentModel.DataAnnotations;

namespace AspNetMvcActiveDirectoryOwin.Web.Common.Models.Users
{
    public class UserModel
    {
        public int Id { get; set; }
        
        public string UserName { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        [Display(Name = "Last Login Date")]
        [DisplayFormat(DataFormatString = "{0:M/d/yy}")]
        public DateTime LastLoginDate { get; set; }

        [Display(Name = "Create Date")]
        [DisplayFormat(DataFormatString = "{0:M/d/yy}")]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Create By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Last Updated Date")]
        [DisplayFormat(DataFormatString = "{0:M/d/yy}")]
        public DateTime ModifiedOn { get; set; }

        [Display(Name = "Last Updated By")]
        public string ModifiedBy { get; set; }

        [Display(Name = "Name")]
        public string FullName => !string.IsNullOrWhiteSpace(FirstName) ? $"{LastName}, {FirstName}" : LastName;

        [Display(Name = "Authorized Roles")]
        public string AuthorizedRoleNames { get; set; }
    }
}
