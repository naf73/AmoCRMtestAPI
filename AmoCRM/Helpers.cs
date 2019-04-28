using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmoCRM
{
    public static class Helpers
    {
        public static DateTime TimeFromUnixTimestamp(int unixTimestamp)
        {
            DateTime unixYear0 = new DateTime(1970, 1, 1);
            long unixTimeStampInTicks = unixTimestamp * TimeSpan.TicksPerSecond;
            DateTime dtUnix = new DateTime(unixYear0.Ticks + unixTimeStampInTicks);
            return dtUnix;
        }

        public static long UnixTimestampFromDateTime(DateTime date)
        {
            long unixTimestamp = date.Ticks - new DateTime(1970, 1, 1).Ticks;
            unixTimestamp /= TimeSpan.TicksPerSecond;
            return unixTimestamp;
        }

        public static DateTime TimeFromJavaTimestamp(long javaTimestamp)
        {
            return TimeFromUnixTimestamp((int)(javaTimestamp / 1000));
        }

        public static long JavaTimestampFromDateTime(DateTime date)
        {
            return (UnixTimestampFromDateTime(date) * 1000);
        }

    }
}
