using System;
using AspNetMvcActiveDirectoryOwin.Services.Logging;
using log4net;

namespace AspNetMvcActiveDirectoryOwin.Logging
{
    public class LogManagerAdapter : ILogManager
    {
        public ILog GetLog(Type typeAssociatedWithRequestedLog)
        {
            var log = LogManager.GetLogger(typeAssociatedWithRequestedLog);
            return log;
        }
    }
}