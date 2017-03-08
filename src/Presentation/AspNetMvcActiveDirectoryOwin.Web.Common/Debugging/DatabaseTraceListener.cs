using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using AspNetMvcActiveDirectoryOwin.Core.Domain;
using Dapper;

namespace AspNetMvcActiveDirectoryOwin.Web.Common.Debugging
{
    public class DatabaseTraceListener : ITraceListener
    {
        private readonly string _connectionString;

        public DatabaseTraceListener(string connectionString)
        {
            _connectionString = connectionString;
        }

        private void AddTraceLog(TraceLog traceLog)
        {
            Func<string, string> convertEmptyStringToNull = value => string.IsNullOrWhiteSpace(value) ? null : value;

            var query = @"INSERT INTO TraceLog (Controller, Action, Message, PerformedOn, PerformedBy)
	                    VALUES (@Controller, @Action, @Message, @PerformedOn, @PerformedBy);";

            var parameters = new DynamicParameters();
            parameters.Add("@Controller", convertEmptyStringToNull(traceLog.Controller));
            parameters.Add("@Action", convertEmptyStringToNull(traceLog.Action));
            parameters.Add("@Message", convertEmptyStringToNull(traceLog.Message));
            parameters.Add("@PerformedOn", traceLog.PerformedOn);
            parameters.Add("@PerformedBy", convertEmptyStringToNull(traceLog.PerformedBy));

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(query, parameters, commandType: CommandType.Text);
            }
        }

        public Task AddTraceLogAsync(TraceLog traceLog)
        {
            return Task.Run(() => AddTraceLog(traceLog));
        }
    }
} 