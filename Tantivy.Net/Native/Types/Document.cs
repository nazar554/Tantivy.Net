namespace Tantivy.Net.Native.Types
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Helpers;
    using NodaTime;

    internal sealed class Document : Abstract.SafeHandleZeroIsInvalid<Document>
    {
        private Document()
        {
        }

        protected override bool ReleaseHandle()
        {
            Destroy(handle);
            return true;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int Length
        {
            get
            {
                checked
                {
                    return (int)LengthImpl(this);
                }
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsEmpty => IsEmptyImpl(this);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public delegate bool FieldFilter(uint field);

        public void FilterFields(FieldFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }
            FilterFieldsImpl(this, filter);
        }

        public void AddText(uint field, string text)
        {
            unsafe
            {
                MarshalHelper.Utf8Call(text, (buffer, length) => AddTextImpl(this, field, buffer, length));
            }
        }

        public void AddU64(uint field, ulong value) => AddU64Impl(this, field, value);

        public void AddI64(uint field, long value) => AddI64Impl(this, field, value);

        public void AddBytes(uint field, in ReadOnlySpan<byte> bytes)
        {
            unsafe
            {
                if (bytes.IsEmpty)
                {
                    AddBytesImpl(this, field, null, UIntPtr.Zero);
                }
                else
                {
                    fixed (byte* buffer = bytes)
                    {
                        AddBytesImpl(this, field, buffer, new UIntPtr((uint)bytes.Length));
                    }
                }
            }
        }

        public void AddDate(uint field, ZonedDateTime dateTime)
        {
            dateTime = dateTime.WithCalendar(CalendarSystem.Iso)
                               .WithZone(DateTimeZone.Utc);

            long totalNanoSeconds = dateTime.NanosecondOfDay;
            uint dayOfYear;
            uint seconds;
            uint nanoseconds;
#if DEBUG
            checked
#endif
            {
                const long NanosecondsInSecond = 1_000_000_000;
                dayOfYear = (uint)dateTime.DayOfYear;
                seconds = (uint)(totalNanoSeconds / NanosecondsInSecond);
                nanoseconds = (uint)(totalNanoSeconds % NanosecondsInSecond);
            }
            AddDateImpl(
                this,
                field,
                dateTime.Year,
                dayOfYear,
                seconds,
                nanoseconds
            );
        }

        public void AddDate(uint field, DateTime date)
        {
            date = date.ToUniversalTime();
            long ticks = date.TimeOfDay.Ticks;

            uint dayOfYear;
            uint seconds;
            uint nanoseconds;

#if DEBUG
            checked
#endif
            {
                dayOfYear = (uint)date.DayOfYear;

                const long NanosecondsInMillisecond = 1_000_000;
                const long NanosecondsPerTick = NanosecondsInMillisecond / TimeSpan.TicksPerMillisecond;
                seconds = (uint)(ticks / TimeSpan.TicksPerSecond);
                nanoseconds = (uint)(ticks % TimeSpan.TicksPerSecond * NanosecondsPerTick);
            }

            AddDateImpl(
                this,
                field,
                date.Year,
                dayOfYear,
                seconds,
                nanoseconds
            );
        }

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_new_document", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern Document Create();

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_drop_document", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void Destroy(IntPtr handle);

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_document_len", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern UIntPtr LengthImpl(Document document);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_document_is_empty", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool IsEmptyImpl(Document document);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_document_filter_fields", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void FilterFieldsImpl(Document document, [MarshalAs(UnmanagedType.FunctionPtr)] FieldFilter filter);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_document_add_text", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern unsafe void AddTextImpl(Document document, uint field, byte* buffer, UIntPtr length);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_document_add_u64", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void AddU64Impl(Document document, uint field, ulong value);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_document_add_i64", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void AddI64Impl(Document document, uint field, long value);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_document_add_date", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void AddDateImpl(
            Document document,
            uint field,
            int year,
            uint ordinal,
            uint seconds,
            uint nanoseconds
         );

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_document_add_bytes", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern unsafe void AddBytesImpl(Document document, uint field, byte* buffer, UIntPtr length);
    }
}