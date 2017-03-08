using System;
using System.Collections.Generic;

namespace AspNetMvcActiveDirectoryOwin.Core.Domain
{
    public partial class User : BaseEntity
    {
        public User()
        {
            Roles = new List<Role>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Active { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public override int EntityId => Id;
    }
}
