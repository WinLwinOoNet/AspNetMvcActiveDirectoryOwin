using System.Collections.Generic;

namespace AspNetMvcActiveDirectoryOwin.Core.Domain
{
    public partial class Role : BaseEntity
    {
        public Role()
        {
            Users = new List<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public override int EntityId => Id;
    }
}
