namespace Tantivy.Net.Schema
{
    using System;

    public class IntOptions : IDisposable
    {
        internal readonly Native.Types.IntOptions _impl;

        public IntOptions()
        {
            _impl = Native.Types.IntOptions.Create();
        }

        public bool IsStored => _impl.IsStored;

        public bool IsFast => _impl.IsFast;

        public bool IsIndexed => _impl.IsIndexed;

        public Cardinality? FastCardinality => _impl.FastCardinality;

        public IntOptions Stored()
        {
            _impl.SetStored();
            return this;
        }

        public IntOptions Indexed()
        {
            _impl.SetIndexed();
            return this;
        }

        public IntOptions Fast(Cardinality cardinality)
        {
            _impl.SetFast(cardinality);
            return this;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _impl.Dispose();
            }
        }
    }
}