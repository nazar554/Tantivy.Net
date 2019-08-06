namespace Tantivy.Net.Native.Helpers
{
    using System;

    internal static class DateTimeHelper
    {
        public static DateTime Epoch => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public const long NanosecondsPerTick = 1000000 / TimeSpan.TicksPerMillisecond;

        public static long ToNanosecondsSinceEpoch(this DateTime date)
        {
            checked
            {
                long delta = date.ToUniversalTime().Ticks - Epoch.Ticks;
                return delta * NanosecondsPerTick;
            }
        }

        public static long ToNanosecondsSinceEpoch(this DateTimeOffset date)
        {
            checked
            {
                long delta = date.UtcTicks - Epoch.Ticks;
                return delta * NanosecondsPerTick;
            }
        }
    }
}