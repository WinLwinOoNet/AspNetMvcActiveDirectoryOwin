using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AspNetMvcActiveDirectoryOwin.Web.Common.Models.Logs
{
    public class LogSearchModel
    {
        [Display(Name = "From Date Time")]
        public DateTime? FromDate { get; set; }

        [Display(Name = "To Date Time")]
        public DateTime? ToDate { get; set; }

        [Display(Name = "User")]
        public string SelectedUsername { get; set; }

        public IList<SelectListItem> AvailableUsernames { get; set; }

        public string Thread { get; set; }

        public IList<SelectListItem> AvailableLevels { get; set; }

        [Display(Name = "Level")]
        public string SelectedLevel { get; set; }
        
        public string Message { get; set; }
        
        public string Exception { get; set; }

        public LogSearchModel()
        {
            AvailableLevels = new List<SelectListItem>();
            AvailableUsernames = new List<SelectListItem>();
        }
    }
}
