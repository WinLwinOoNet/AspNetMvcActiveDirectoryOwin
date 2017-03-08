using System;

namespace AspNetMvcActiveDirectoryOwin.Core.Domain
{
    public partial class TraceLog : BaseEntity
    {
        public int Id { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Message { get; set; }
        public DateTime PerformedOn { get; set; }
        public string PerformedBy { get; set; }
        public override int EntityId => Id;
    }
}
