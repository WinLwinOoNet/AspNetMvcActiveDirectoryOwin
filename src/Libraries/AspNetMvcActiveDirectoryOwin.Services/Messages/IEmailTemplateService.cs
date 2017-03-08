using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetMvcActiveDirectoryOwin.Core.Domain;

namespace AspNetMvcActiveDirectoryOwin.Services.Messages
{
    public interface IEmailTemplateService
    {
        Task<IList<EmailTemplate>> GetAllEmailTemplates();

        Task<EmailTemplate> GetEmailTemplateById(int templateId);

        Task<EmailTemplate> GetEmailTemplateByName(string name);

        Task<int> InsertEmailTemplate(EmailTemplate template);

        Task UpdateEmailTemplate(EmailTemplate template);
    }
}