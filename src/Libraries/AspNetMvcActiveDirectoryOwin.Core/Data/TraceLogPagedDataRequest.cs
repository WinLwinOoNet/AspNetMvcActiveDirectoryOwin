using System;

namespace AspNetMvcActiveDirectoryOwin.Core.Data
{
    public class TraceLogPagedDataRequest : PagedDataRequest
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Message { get; set; }
        public string PerformedBy { get; set; }
        public TraceLogSortField SortField { get; set; }
        public SortOrder SortOrder { get; set; }

        public TraceLogPagedDataRequest()
        {
            SortOrder = SortOrder.Descending;
            SortField = TraceLogSortField.PerformedOn;
        }
    }

    public enum TraceLogSortField
    {
        Id,
        Controller,
        Action,
        Message,
        PerformedOn,
        PerformedBy
    }
}
