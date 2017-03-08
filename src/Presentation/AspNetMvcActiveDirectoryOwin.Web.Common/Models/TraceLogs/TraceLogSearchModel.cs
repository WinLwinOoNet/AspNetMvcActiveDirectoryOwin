using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AspNetMvcActiveDirectoryOwin.Web.Common.Models.TraceLogs
{
    public class TraceLogSearchModel
    {
        [Display(Name = "Controller Name")]
        public string Controller { get; set; }

        [Display(Name = "Action Name")]
        public string Action { get; set; }

        public string Message { get; set; }

        [Display(Name = "User")]
        public string PerformedBy { get; set; }

        [Display(Name = "From Date Time")]
        public DateTime? FromDate { get; set; }

        [Display(Name = "To Date Time")]
        public DateTime? ToDate { get; set; }
        
        public IList<SelectListItem> AvailableUsernames { get; set; }

        public TraceLogSearchModel()
        {
            AvailableUsernames = new List<SelectListItem>();
        }
    }
}