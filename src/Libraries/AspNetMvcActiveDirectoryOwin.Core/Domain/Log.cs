using System;

namespace AspNetMvcActiveDirectoryOwin.Core.Domain
{
    public partial class Log : BaseEntity
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Username { get; set; }
        public string Thread { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public override int EntityId => Id;
    }
}
