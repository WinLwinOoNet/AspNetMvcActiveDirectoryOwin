using System.Configuration;
using System.Data.Entity;
using System.Web;
using AspNetMvcActiveDirectoryOwin.Core;
using AspNetMvcActiveDirectoryOwin.Core.Caching;
using AspNetMvcActiveDirectoryOwin.Core.Data;
using AspNetMvcActiveDirectoryOwin.Data;
using AspNetMvcActiveDirectoryOwin.Emails;
using AspNetMvcActiveDirectoryOwin.Logging;
using AspNetMvcActiveDirectoryOwin.Services.Domains;
using AspNetMvcActiveDirectoryOwin.Services.Logging;
using AspNetMvcActiveDirectoryOwin.Services.Messages;
using AspNetMvcActiveDirectoryOwin.Services.Roles;
using AspNetMvcActiveDirectoryOwin.Services.Settings;
using AspNetMvcActiveDirectoryOwin.Services.Users;
using AspNetMvcActiveDirectoryOwin.Web.Common.Debugging;
using AspNetMvcActiveDirectoryOwin.Web.Common.Security;
using Autofac;
using Autofac.Integration.Mvc;

namespace AspNetMvcActiveDirectoryOwin.Web.Infrastructure.DependencyManagement
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;

            ConfigureUserSession(builder);
            ConfigureLog4Net(builder, connectionString);

            // HttpContext
            builder.Register(c => new HttpContextWrapper(HttpContext.Current) as HttpContextBase).As<HttpContextBase>().InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Request).As<HttpRequestBase>().InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Response).As<HttpResponseBase>().InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Server).As<HttpServerUtilityBase>().InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Session).As<HttpSessionStateBase>().InstancePerLifetimeScope();

            // Services
            builder.RegisterType<ActiveDirectoryService>().As<IActiveDirectoryService>().InstancePerLifetimeScope();
            builder.RegisterType<DateTimeAdapter>().As<IDateTime>().InstancePerLifetimeScope();
            builder.RegisterType<DomainService>().As<IDomainService>().InstancePerLifetimeScope();
            builder.RegisterType<EmailSender>().As<IEmailSender>().InstancePerLifetimeScope();
            builder.RegisterType<EmailTemplateService>().As<IEmailTemplateService>().InstancePerLifetimeScope();
            builder.RegisterType<LogService>().As<ILogService>().InstancePerLifetimeScope();
            builder.RegisterType<MessageService>().As<IMessageService>().InstancePerLifetimeScope();
            builder.RegisterType<MemoryCacheService>().As<ICacheService>().SingleInstance();
            builder.RegisterType<OwinAuthenticationService>().As<IAuthenticationService>().InstancePerLifetimeScope();
            builder.RegisterType<RoleService>().As<IRoleService>().InstancePerLifetimeScope();
            builder.RegisterType<SettingService>().As<ISettingService>().InstancePerLifetimeScope();
            builder.RegisterType<TraceLogService>().As<ITraceLogService>().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();

            // Trace listener
            builder.RegisterType<DatabaseTraceListener>().As<ITraceListener>()
                .WithParameter("connectionString", connectionString)
                .InstancePerLifetimeScope();
            
            // Respository
            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            builder.Register<DbContext>(c => new AppDbContext(connectionString)).InstancePerLifetimeScope();

            // Register controllers
            builder.RegisterControllers(typeof(Global).Assembly);

            // Register Mvc filters
            builder.RegisterFilterProvider();
        }

        private void ConfigureUserSession(ContainerBuilder builder)
        {
            var userSession = new UserSession();
            builder.Register(c => userSession).As<IUserSession>().SingleInstance();
            builder.Register(c => userSession).As<IWebUserSession>().SingleInstance();
        }

        private void ConfigureLog4Net(ContainerBuilder builder, string connectionString)
        {
            Log4NetLogger.Configure(connectionString);
            builder.Register(c => new LogManagerAdapter()).As<ILogManager>().InstancePerLifetimeScope();
        }
    }
}
