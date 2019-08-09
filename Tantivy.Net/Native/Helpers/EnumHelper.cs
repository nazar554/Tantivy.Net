namespace Tantivy.Net.Native.Helpers
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    internal static class EnumHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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