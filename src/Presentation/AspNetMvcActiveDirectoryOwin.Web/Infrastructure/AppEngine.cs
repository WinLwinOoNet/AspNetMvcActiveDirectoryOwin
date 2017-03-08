using AspNetMvcActiveDirectoryOwin.Web.Infrastructure.DependencyManagement;
using Autofac;
using Autofac.Extras.CommonServiceLocator;
using Autofac.Integration.Mvc;
using Microsoft.Practices.ServiceLocation;

namespace AspNetMvcActiveDirectoryOwin.Web.Infrastructure
{
    public class AppEngine : IEngine
    {
        public IServiceLocator Locator { get; set; }

        public void Initialize()
        {
            RegisterDependencies();
        }

        private void RegisterDependencies()
        {
            var builder = new ContainerBuilder();
            var container = builder.Build();

            builder = new ContainerBuilder();

            builder.RegisterInstance(this).As<IEngine>().SingleInstance();

            IDependencyRegistrar dependencyRegistrar = new DependencyRegistrar();
            dependencyRegistrar.Register(builder);
            builder.Update(container);

            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));
            Locator = ServiceLocator.Current;

            System.Web.Mvc.DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}