namespace Tantivy.Net.Native.Helpers
{
    using System;
    using System.Diagnostics;

    internal static class EnumHelper
    {
        public static T VerifyEnum<T>(T value)
            where T : struct, Enum
        {
#if DEBUG
            Debug.Assert(Enum.IsDefined(typeof(T), value));
#endif
            return value;
        }
    }
}