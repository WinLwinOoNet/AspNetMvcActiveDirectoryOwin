using System;

namespace AspNetMvcActiveDirectoryOwin.Core
{
    public class DateTimeAdapter : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
