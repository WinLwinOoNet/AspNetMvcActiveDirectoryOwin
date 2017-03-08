using AspNetMvcActiveDirectoryOwin.Core.Domain;
using AspNetMvcActiveDirectoryOwin.Web.Common.Models.EmailTemplates;
using AspNetMvcActiveDirectoryOwin.Web.Common.Models.Logs;
using AspNetMvcActiveDirectoryOwin.Web.Common.Models.Settings;
using AspNetMvcActiveDirectoryOwin.Web.Common.Models.TraceLogs;
using AspNetMvcActiveDirectoryOwin.Web.Common.Models.Users;

namespace AspNetMvcActiveDirectoryOwin.Web.Common.Mapper
{
    public static class MappingExtensions
    {
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return AutoMapperConfiguration.Mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return AutoMapperConfiguration.Mapper.Map(source, destination);
        }

        #region EmailTemplate

        public static EmailTemplateModel ToModel(this EmailTemplate entity)
        {
            return entity.MapTo<EmailTemplate, EmailTemplateModel>();
        }

        public static EmailTemplate ToEntity(this EmailTemplateModel model)
        {
            return model.MapTo<EmailTemplateModel, EmailTemplate>();
        }

        public static EmailTemplate ToEntity(this EmailTemplateModel model, EmailTemplate destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Log

        public static LogModel ToModel(this Log entity)
        {
            return entity.MapTo<Log, LogModel>();
        }

        public static Log ToEntity(this LogModel model)
        {
            return model.MapTo<LogModel, Log>();
        }

        #endregion

        #region Setting

        public static SettingModel ToModel(this Setting entity)
        {
            return entity.MapTo<Setting, SettingModel>();
        }

        public static Setting ToEntity(this SettingModel model)
        {
            return model.MapTo<SettingModel, Setting>();
        }

        public static Setting ToEntity(this SettingModel model, Setting destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region TraceLog

        public static TraceLogModel ToModel(this TraceLog entity)
        {
            return entity.MapTo<TraceLog, TraceLogModel>();
        }

        public static TraceLog ToEntity(this TraceLogModel model)
        {
            return model.MapTo<TraceLogModel, TraceLog>();
        }

        public static TraceLog ToEntity(this TraceLogModel model, TraceLog destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region User

        public static UserModel ToModel(this User entity)
        {
            return entity.MapTo<User, UserModel>();
        }

        public static UserCreateUpdateModel ToCreateUpdateModel(this User entity)
        {
            return entity.MapTo<User, UserCreateUpdateModel>();
        }
        
        #endregion
    }
}
