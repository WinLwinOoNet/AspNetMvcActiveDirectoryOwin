using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AspNetMvcActiveDirectoryOwin.Web.Common.Models.Users
{
    public class UserSearchModel
    {
        [Display(Name = "Roles")]
        public string RoleName { get; set; }

        public IList<SelectListItem> AvailableRoles { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }
        
        public IList<SelectListItem> AvailableStatus { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public UserSearchModel()
        {
            AvailableRoles = new List<SelectListItem>();
            AvailableStatus = new List<SelectListItem>();
        }
    }
}
