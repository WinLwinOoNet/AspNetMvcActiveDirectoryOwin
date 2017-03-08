using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace AspNetMvcActiveDirectoryOwin.Logging
{
    public class Log4NetLogger
    {
        public static void Configure(string connectionString)
        {
            var hierarchy = LogManager.GetRepository() as Hierarchy;
            hierarchy.Root.AddAppender(CreatAdoNetAppender(connectionString));
            hierarchy.Root.Level = Level.Debug;
            hierarchy.Configured = true;
        }

        private static IAppender CreatAdoNetAppender(string connectionString)
        {
            var appender = new AdoNetAppender
            {
                Name = "AdoNetAppender",
                ConnectionType = "System.Data.SqlClient.SqlConnection, System.Data, Version = 1.0.3300.0, Culture = neutral, PublicKeyToken = b77a5c561934e089",
                ConnectionString = connectionString,
                BufferSize = 1,
                CommandText = "INSERT INTO Log ([Date],[Username],[Thread],[Level],[Logger],[Message],[Exception]) " +
                              "VALUES (@log_date, @log_username, @thread, @log_level, @logger, @message, @exception)",
                CommandType = System.Data.CommandType.Text
            };
            appender.AddParameter(new AdoNetAppenderParameter
            {
                ParameterName = "@log_date",
                DbType = System.Data.DbType.DateTime,
                Layout = new RawTimeStampLayout()
            });
            appender.AddParameter(new AdoNetAppenderParameter
            {
                ParameterName = "@log_username",
                DbType = System.Data.DbType.String,
                Layout = new Layout2RawLayoutAdapter(new PatternLayout("%aspnet-request{AUTH_USER}"))
            });
            appender.AddParameter(new AdoNetAppenderParameter
            {
                ParameterName = "@thread",
                DbType = System.Data.DbType.String,
                Size = 255,
                Layout = new Layout2RawLayoutAdapter(new PatternLayout("%thread")) 
            });
            appender.AddParameter(new AdoNetAppenderParameter
            {
                ParameterName = "@log_level",
                DbType = System.Data.DbType.String,
                Size = 50,
                Layout = new Layout2RawLayoutAdapter(new PatternLayout("%level"))
            });
            appender.AddParameter(new AdoNetAppenderParameter
            {
                ParameterName = "@logger",
                DbType = System.Data.DbType.String,
                Size = 255,
                Layout = new Layout2RawLayoutAdapter(new PatternLayout("%logger")) 
            });
            appender.AddParameter(new AdoNetAppenderParameter
            {
                ParameterName = "@message",
                DbType = System.Data.DbType.String,
                Size = 4000,
                Layout = new Layout2RawLayoutAdapter(new PatternLayout("%message"))
            });
            appender.AddParameter(new AdoNetAppenderParameter
            {
                ParameterName = "@exception",
                DbType = System.Data.DbType.String,
                Size = 2000,
                Layout = new Layout2RawLayoutAdapter(new ExceptionLayout())
            });

            appender.ActivateOptions();
            return appender;
        }
    }
}
