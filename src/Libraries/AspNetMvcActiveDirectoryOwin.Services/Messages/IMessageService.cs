using System.Threading.Tasks;
using AspNetMvcActiveDirectoryOwin.Core.Domain;

namespace AspNetMvcActiveDirectoryOwin.Services.Messages
{
    public interface IMessageService
    {
        Task SendAddNewUserNotification(User user);
    }
}