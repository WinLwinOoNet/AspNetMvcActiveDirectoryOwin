using System;

namespace AspNetMvcActiveDirectoryOwin.Core.Extensions
{
	public static class DateExtensions
	{
		public static DateTime ToStartOfMonth(this DateTime date)
		{
			return new DateTime(date.Year, date.Month, 1);
		}

		public static DateTime ToEndOfMonth(this DateTime date)
		{
			return new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);
		}

		public static DateTime ToEndOfDay(this DateTime date)
		{
			return date.Date.AddDays(1).AddTicks(-1);
		}

        /// <summary>
        /// Combine start date and end date. 
        /// For example, 
        ///    12/30/2016 8:00 AM - 12/30/2016 5:00 PM
        ///    12/30/2016 8:00 AM - 11:00 AM (if same date)
        /// </summary>
        /// <param name="startDateTime">Start date time</param>
        /// <param name="endDateTime">End date time</param>
        /// <returns></returns>
        public static string CombineStartDateAndEndDate(DateTime? startDateTime, DateTime? endDateTime)
        {
            string result = "N/A";
            if (startDateTime.HasValue && endDateTime.HasValue)
            {
                // Same date
                if (startDateTime.Value.Date == endDateTime.Value.Date)
                    result = string.Format("{0:g} - {1:t}", startDateTime.Value, endDateTime.Value);
                else
                    result = string.Format("{0:g} - {1:g}", startDateTime.Value, endDateTime.Value);
            }
            else if (startDateTime.HasValue)
            {
                result = string.Format("{0:g}", startDateTime.Value);

            }
            else if (endDateTime.HasValue)
            {
                result = string.Format("{0:g}", endDateTime.Value);
            }
            return result;
        }
    }
}