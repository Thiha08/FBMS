using FBMS.Core.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToTimeZoneTime(this DateTime time, string format, string timeZoneId = CoreConstants.SystemTimeZone)
        {
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            time = TimeZoneInfo.ConvertTimeFromUtc(time, timeZoneInfo);
            return time.ToString(format);
        }

        //public static DateTime ToTimeZoneTime(this DateTime time, string timeZoneId = CoreConstants.SystemTimeZone)
        //{
        //    TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        //    return time.ToTimeZoneTime(timeZoneInfo);
        //}

        //public static DateTime ToTimeZoneTime(this DateTime time, TimeZoneInfo timeZoneInfo)
        //{
        //    return TimeZoneInfo.ConvertTimeFromUtc(time, timeZoneInfo);
        //}

        public static DateTime ToUtcTime(this string timeString, string format, string timeZoneId)
        {
            DateTime time;
            try
            {
                time = DateTime.ParseExact(timeString, format, null);
                TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                time = TimeZoneInfo.ConvertTimeToUtc(time, timeZoneInfo);
            }
            catch (Exception)
            {
                time = DateTime.UtcNow;
            }
            return time;
        }
    }
}
