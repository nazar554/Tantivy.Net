namespace Tantivy.Net.Schema
{
    using System;
    using NodaTime;

    public class Document : Abstract.DisposableBase
    {
        internal readonly Native.Types.Document _impl;

        public Document()
        {
            _impl = Native.Types.Document.Create();
        }

        public int Length => _impl.Length;

        public bool IsEmpty => _impl.IsEmpty;

        public void FilterFields(Func<uint, bool> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            _impl.FilterFields(new Native.Types.Document.FieldFilter(filter));
        }

        public void Add(uint field, string value) => _impl.AddText(field, value);

        public void Add(uint field, ulong value) => _impl.AddU64(field, value);

        public void Add(uint field, long value) => _impl.AddI64(field, value);

        public void Add(uint field, DateTime value) => _impl.AddDate(field, value);

        public void Add(uint field, ZonedDateTime value) => _impl.AddDate(field, value);

        public void Add(uint field, DateTimeOffset value) => _impl.AddDate(field, value.UtcDateTime);

        public void Add(uint field, ReadOnlySpan<byte> value) => _impl.AddBytes(field, value);

        public void Add(uint field, byte[] value) => _impl.AddBytes(field, value);

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _impl.Dispose();
            }
        }
    }
}