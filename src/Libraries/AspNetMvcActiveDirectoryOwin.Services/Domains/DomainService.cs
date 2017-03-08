using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using AspNetMvcActiveDirectoryOwin.Core.Data;
using AspNetMvcActiveDirectoryOwin.Core.Domain;

namespace AspNetMvcActiveDirectoryOwin.Services.Domains
{
    public class DomainService : IDomainService
    {
        private readonly IRepository<Domain> _repository;

        public DomainService(IRepository<Domain> repository)
        {
            _repository = repository;
        }

        public async Task<IList<Domain>> GetAllDomainsAsync()
        {
            var query = _repository.Entities.AsQueryable();

            return await query.ToListAsync();
        }
        
        public async Task<Domain> GetDomainByIdAsync(int userId)
        {
            var query = _repository.Entities
                .Where(x => x.Id == userId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<int> AddDomainAsync(Domain user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _repository.Entities.Add(user);
            await _repository.SaveChangesAsync();

            return user.Id;
        }

        public async Task UpdateDomainAsync(Domain user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _repository.Entities.AddOrUpdate(user);
            await _repository.SaveChangesAsync();
        }
    }
}