using Microsoft.Practices.ServiceLocation;

namespace AspNetMvcActiveDirectoryOwin.Web.Infrastructure
{
    public interface IEngine
    {
        IServiceLocator Locator { get; }

        void Initialize();
    }
}
