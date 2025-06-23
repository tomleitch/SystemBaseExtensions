using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Module Name: DateTimeBase.cs

namespace System
{
    public static class DateTimeBase
    {

   

        private static object lockisTime = new object();


        /// <summary>
        /// Returns true if the string can be parsed as a time.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool isTime(this string input)
        {
            lock (lockisTime)
            {
                TimeSpan dummyOutput;
                bool ret = TimeSpan.TryParse(input, out dummyOutput);
                return ret;
            }
        }

        private static object lockInSessionRange = new object();

        /// <summary>
        /// Returns true if the Date is in session range (UTC is assumed).
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool InSessionRange(this DateTime value)
        {
            lock (lockInSessionRange)
            {
                if (value < DateTime.UtcNow.AddMinutes(-20) || value > DateTime.UtcNow.AddSeconds(5))
                    return false;
                return true;
            }
        }

        private static object lockSetSeconds = new object();

        /// <summary>
        /// Set the seconds in this DateTime value/
        /// </summary>
        /// <param name="value"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static DateTime SetSeconds(this DateTime value, int seconds)
        {
            lock (lockSetSeconds)
            {
                string s = value.ToString("dd MMM yyyy HH:mm");

                s += ":" + seconds.ToString("00");

                return s.ToDate();
            }
        }

        private static object lockFromTicks = new object();

        /// <summary>
        /// Set the DateTime value from the supplied ticks.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="ticks"></param>
        /// <returns></returns>
        public static DateTime FromTicks(this DateTime value, long ticks)
        {
            lock (lockFromTicks)
            {
                value = new DateTime(ticks);
                return value;
            }
        }


        private static object lockTodaysUTCTime = new object();
        public static DateTime TodaysUTCTime(this DateTime dt)
        {
            lock (lockTodaysUTCTime)
            {
                string tm = dt.ToString("HH:mm:ss");
                DateTime ret = DateTime.UtcNow.SetTime(tm);
                return ret;
            }
        }

        private static object lockTodaysTime = new object();
        public static DateTime TodaysTime(this DateTime dt)
        {
            lock(lockTodaysTime)
            {
                string tm = dt.ToString("HH:mm:ss");
                DateTime ret = DateTime.Now.SetTime(tm);
                return ret;
            }
        }

        private static object lockSetTime = new object();
        public static DateTime SetTime(this DateTime dt, string newTime)
        {
            lock (lockSetTime)
            {
                string strDT = dt.ToString("dd MMM yyyy");
                string tm;

                if (newTime.isTime())
                    try
                    {
                        TimeSpan sp;
                        TimeSpan.TryParse(newTime, out sp);

                        tm = dt.ToString("dd MMM yyyy") + " " + sp.ToString();
                        return tm.ToDate();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                return dt;
            }
        }

        private static object lockFromBetfairTime = new object();
        /// <summary>
        /// Betfair uses UTC so convert to UK local time
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime FromBetfairTime(this DateTime value)
        {
            lock (lockFromBetfairTime)
            {
                DateTime dt = DateTime.SpecifyKind(value, DateTimeKind.Utc);

                return dt.ToLocalTime();
            }
        }

        private static object lockLocalToUtc = new object();
        public static DateTime LocalToUtc(this DateTime value, string timezone)
        {
            lock (lockLocalToUtc)
            {
                TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(timezone);
                DateTime dt = TimeZoneInfo.ConvertTimeToUtc(value, tz);
                return dt;
            }
        }

        private static object lockUtcToLocal = new object();
        public static DateTime UtcToLocal(this DateTime value, string timezone)
        {
            lock (lockUtcToLocal)
            {
                DateTime utcDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
                TimeZoneInfo userTimeZone1 = TimeZoneInfo.FindSystemTimeZoneById(timezone);
                DateTimeOffset utcOffset = new DateTimeOffset(utcDate, TimeSpan.Zero);
                DateTimeOffset uo1 = utcOffset.ToOffset(userTimeZone1.GetUtcOffset(utcOffset));
                DateTime cvtDate = uo1.DateTime;

                return cvtDate;
            }
        }

        private static object lockFloor = new object();
        private static DateTime Floor(DateTime dateTime, TimeSpan interval)
        {
            lock (lockFloor)
            {
                return dateTime.AddTicks(-(dateTime.Ticks % interval.Ticks));
            }
        }

        private static object lockCeiling = new object();
        private static DateTime Ceiling(DateTime dateTime, TimeSpan interval)
        {
            lock (lockCeiling)
            {
                var overflow = dateTime.Ticks % interval.Ticks;

                return overflow == 0 ? dateTime : dateTime.AddTicks(interval.Ticks - overflow);
            }
        }

        private static object lockRound = new object();
        private static DateTime Round(DateTime dateTime, TimeSpan interval)
        {
            lock (lockRound)
            {
                var halfIntervelTicks = ((interval.Ticks + 1) >> 1);

                return dateTime.AddTicks(halfIntervelTicks - ((dateTime.Ticks + halfIntervelTicks) % interval.Ticks));
            }
        }
    }

}
