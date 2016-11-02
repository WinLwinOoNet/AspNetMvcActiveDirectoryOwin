using System.Web;
using AspNetMvcActiveDirectoryOwin.Web.Common.Security;
using Ninject;
using Ninject.Web.Common;

namespace AspNetMvcActiveDirectoryOwin.Web
{
    public class NinjectConfigurator
    {
        public void Configure(IKernel container)
        {
            AddBindings(container);
        }

        private void AddBindings(IKernel container)
        {
            container.Bind<IActiveDirectoryService>().To<ActiveDirectoryService>().InRequestScope();
            container.Bind<IAuthenticationService>().To<OwinAuthenticationService>().InRequestScope();
            container.Bind<IWebUserSession>().ToMethod(ctx => new UserSession()).InRequestScope();
        }
    }
}