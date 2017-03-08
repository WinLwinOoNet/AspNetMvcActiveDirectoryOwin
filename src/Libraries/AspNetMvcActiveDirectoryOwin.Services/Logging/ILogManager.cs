using System;
using log4net;

namespace AspNetMvcActiveDirectoryOwin.Services.Logging
{
    public interface ILogManager
    {
        ILog GetLog(Type typeAssociatedWithRequestedLog);
    }
}
