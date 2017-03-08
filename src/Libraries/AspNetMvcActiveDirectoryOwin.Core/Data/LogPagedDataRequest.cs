using System;

namespace AspNetMvcActiveDirectoryOwin.Core.Data
{
    public class LogPagedDataRequest : PagedDataRequest
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Username { get; set; }
        public string Thread { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public LogSortField SortField { get; set; }
        public SortOrder SortOrder { get; set; }

        public LogPagedDataRequest()
        {
            SortOrder = SortOrder.Descending;
            SortField = LogSortField.Date;
        }
    }

    public enum LogSortField
    {
        Id,
        Date,
        Thread,
        Level,
        Logger,
        Message,
        Exception
    }
}
