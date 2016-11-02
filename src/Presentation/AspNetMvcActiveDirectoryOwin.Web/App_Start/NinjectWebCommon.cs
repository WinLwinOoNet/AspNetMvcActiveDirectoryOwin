using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.Mvc;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(AspNetMvcActiveDirectoryOwin.Web.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(AspNetMvcActiveDirectoryOwin.Web.NinjectWebCommon), "Stop")]

namespace AspNetMvcActiveDirectoryOwin.Web
{
    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();
        
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));

            IKernel container = null;
            Bootstrapper.Initialize(() =>
            {
                container = CreateKernel();
                return container;
            });

            var resolver = new NinjectDependencyResolver(container);
            DependencyResolver.SetResolver(resolver);
        }
        
        public static void Stop()
        {
            Bootstrapper.ShutDown();
        }
        
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        private static void RegisterServices(IKernel kernel)
        {
            var containerConfigurator = new NinjectConfigurator();
            containerConfigurator.Configure(kernel);
        }
    }
}
