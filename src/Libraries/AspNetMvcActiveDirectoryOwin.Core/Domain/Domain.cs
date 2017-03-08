using System;

namespace AspNetMvcActiveDirectoryOwin.Core.Domain
{
    public partial class Domain : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public override int EntityId => Id;
    }
}
