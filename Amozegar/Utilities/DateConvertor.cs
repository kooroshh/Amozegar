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

    }
}
