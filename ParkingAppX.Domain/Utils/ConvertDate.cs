using System;
using ParkingAppX.Domain.Enums;

namespace ParkingAppX.Domain.Utils
{
    public static class ConvertDate
    {
        public static int GetCheckInAndCheckOutTimes(long checkIn, long checkOut)
        {
            int hours;
            long result = checkOut - checkIn;
            TimeSpan t = TimeSpan.FromTicks(result);
            hours = t.Hours;
            if (t.Days > 0)
            {
                hours += (t.Days * 24);
            }
            return hours;
        }

        public static string ConvertLongToTime(this long time)
        {
            DateTime date = new DateTime(time);
            return date.ToString("dd/MM/yyyy HH:mm");
        }

        public static long GetLongDate(this string time)
        {
            try
            {
                var dateTime = DateTime.ParseExact(time, "dd/MM/yyyy HH:mm", null);
                return dateTime.Ticks;
            }
            catch (FormatException)
            {
                return 0;
            }
        }

        public static string CompleteFormat(this int time) => (time <= 9) ? ("0" + time) : time.ToString();
    }
}
