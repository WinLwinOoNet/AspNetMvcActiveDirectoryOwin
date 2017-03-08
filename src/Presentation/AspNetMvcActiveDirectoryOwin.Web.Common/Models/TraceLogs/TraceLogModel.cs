using System;
using System.ComponentModel.DataAnnotations;

namespace AspNetMvcActiveDirectoryOwin.Web.Common.Models.TraceLogs
{
    public class TraceLogModel
    {
        public int Id { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string Message { get; set; }

        [Display(Name = "Access Date")]
        public DateTime PerformedOn { get; set; }

        [Display(Name = "Username")]
        public string PerformedBy { get; set; }
    }
}
