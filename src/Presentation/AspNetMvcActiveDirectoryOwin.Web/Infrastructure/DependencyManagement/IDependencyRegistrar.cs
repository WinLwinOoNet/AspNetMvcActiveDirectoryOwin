using Autofac;

namespace AspNetMvcActiveDirectoryOwin.Web.Infrastructure.DependencyManagement
{
    public interface IDependencyRegistrar
    {
        void Register(ContainerBuilder builder);
    }
}
