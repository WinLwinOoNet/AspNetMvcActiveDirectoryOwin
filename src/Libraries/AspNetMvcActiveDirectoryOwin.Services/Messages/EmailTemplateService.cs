using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using AspNetMvcActiveDirectoryOwin.Core.Data;
using AspNetMvcActiveDirectoryOwin.Core.Domain;

namespace AspNetMvcActiveDirectoryOwin.Services.Messages
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly IRepository<EmailTemplate> _repository;

        public EmailTemplateService(IRepository<EmailTemplate> repository)
        {
            _repository = repository;
        }

        public async Task<IList<EmailTemplate>> GetAllEmailTemplates()
        {
            var query = _repository.Entities;

            return await query.ToListAsync();
        }

        public async Task<EmailTemplate> GetEmailTemplateById(int templateId)
        {
            var query = _repository.Entities
                .Where(x => x.Id == templateId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<EmailTemplate> GetEmailTemplateByName(string name)
        {
            var query = _repository.Entities
                .Where(x => x.Name == name);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<int> InsertEmailTemplate(EmailTemplate template)
        {
            _repository.Entities.Add(template);
            await _repository.SaveChangesAsync();

            return template.Id;
        }

        public async Task UpdateEmailTemplate(EmailTemplate template)
        {
            _repository.Entities.AddOrUpdate(template);
            await _repository.SaveChangesAsync();
        }
    }
}
