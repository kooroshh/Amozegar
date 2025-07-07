using System.Globalization;

namespace Amozegar.Utilities
{
    public static class DateConvertor
    {
        public static string ToShamsi(this DateTime date)
        {
            var pc = new PersianCalendar();

            return $"{pc.GetYear(date)}/{pc.GetMonth(date).ToString("00")}/{pc.GetDayOfMonth(date).ToString("00")} -- {pc.GetHour(date).ToString("00")}:{pc.GetMinute(date).ToString("00")}";
        }

        public static string ToShamsi(this DateTime? date)
        {
            if (date == null)
            {
                return "";
            }
            return date.Value.ToShamsi();
        }

        public static string ToShamsiDate(this DateTime date)
        {
            var pc = new PersianCalendar();
            return $"{pc.GetYear(date)}/{pc.GetMonth(date).ToString("00")}/{pc.GetDayOfMonth(date).ToString("00")}";
        }

        public static string ToShamsiTime(this DateTime date)
        {
            var pc = new PersianCalendar();
            return $"{pc.GetHour(date).ToString("00")}:{pc.GetMinute(date).ToString("00")}";
        }

        public static DateTime ToDateTime(string persianDate, string time)
        {
            var dateParts = persianDate.Split('/');
            var timeParts = time.Split(':');
            int year = int.Parse(dateParts[0]);
            int month = int.Parse(dateParts[1]);
            int day = int.Parse(dateParts[2]);
            int hours = int.Parse(timeParts[0]);
            int minute = int.Parse(timeParts[1]);

            var pc = new PersianCalendar();
            return pc.ToDateTime(year, month, day, hours, minute, 0, 0);
        }

        public static int GetDaysOfPersianDate(int year, int month)
        {
            var pc = new PersianCalendar();
            return pc.GetDaysInMonth(year, month);
        }

    }
}
