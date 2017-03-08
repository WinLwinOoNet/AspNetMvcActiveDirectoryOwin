using System;
using System.ComponentModel.DataAnnotations;

namespace AspNetMvcActiveDirectoryOwin.Web.Common.Models.Logs
{
    public class LogModel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }
        public string Username { get; set; }

        [StringLength(225, ErrorMessage = "{0} must not exceed {1} characters.")]
        public string Thread { get; set; }

        [StringLength(50, ErrorMessage = "{0} must not exceed {1} characters.")]
        public string Level { get; set; }

        [StringLength(225, ErrorMessage = "{0} must not exceed {1} characters.")]
        public string Logger { get; set; }

        [StringLength(4000, ErrorMessage = "{0} must not exceed {1} characters.")]
        public string Message { get; set; }

        [StringLength(2000, ErrorMessage = "{0} must not exceed {1} characters.")]
        public string Exception { get; set; }
    }
}
