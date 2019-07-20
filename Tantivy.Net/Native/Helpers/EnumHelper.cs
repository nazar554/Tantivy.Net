namespace Tantivy.Net.Native.Helpers
{
    using System;
    using System.Diagnostics;

    public static class EnumHelper
    {
        public static T VerifyEnum<T>(T value)
            where T : Enum
        {
#if DEBUG
            Debug.Assert(Enum.IsDefined(typeof(T), value));
#endif
            return value;
        }
    }
}