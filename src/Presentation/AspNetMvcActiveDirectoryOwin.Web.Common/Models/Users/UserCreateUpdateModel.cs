using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AspNetMvcActiveDirectoryOwin.Web.Common.Models.Users
{
    public class UserCreateUpdateModel
    {
        public int Id { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(50, ErrorMessage = "{0} must not exceed {1} characters.")]
        public string UserName { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(50, ErrorMessage = "{0} must not exceed {1} characters.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(50, ErrorMessage = "{0} must not exceed {1} characters.")]
        public string LastName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        [Display(Name = "Last Login Date")]
        [DisplayFormat(DataFormatString = "{0:M/d/yy}")]
        public DateTime LastLoginDate { get; set; }

        [Display(Name = "Created Date")]
        [DisplayFormat(DataFormatString = "{0:M/d/yy}")]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Last Updated Date")]
        [DisplayFormat(DataFormatString = "{0:M/d/yy}")]
        public DateTime ModifiedOn { get; set; }

        [Display(Name = "Last Updated By")]
        public string ModifiedBy { get; set; }

        [Display(Name = "Roles")]
        public IList<int> SelectedRoleIds { get; set; }

        public IList<SelectListItem> AvailableRoleNames { get; set; }
        public IList<SelectListItem> AvailableDomains { get; set; }

        public UserCreateUpdateModel()
        {
            SelectedRoleIds = new List<int>();
            AvailableRoleNames = new List<SelectListItem>();
            AvailableDomains = new List<SelectListItem>();
        }
        
    }
}