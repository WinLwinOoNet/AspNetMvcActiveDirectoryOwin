using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Text;
using System.Threading.Tasks;
using AspNetMvcActiveDirectoryOwin.Core;
using AspNetMvcActiveDirectoryOwin.Core.Data;

namespace AspNetMvcActiveDirectoryOwin.Data
{
    public class EfRepository<T> : DbContext, IRepository<T> where T : BaseEntity
    {
        private readonly DbContext _context;
        private IDbSet<T> _entities;

        public EfRepository(DbContext context)
        {
            _context = context;
        }

        public virtual IDbSet<T> Entities => _entities ?? (_entities = _context.Set<T>());

        public override async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();

                foreach (var validationErrors in ex.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        sb.AppendLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");

                throw new Exception(sb.ToString(), ex);
            }
        }
    }
}