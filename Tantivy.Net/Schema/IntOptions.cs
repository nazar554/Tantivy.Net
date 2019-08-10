namespace Tantivy.Net.Schema
{
    using System;

    public sealed class IntOptions : IDisposable
    {
        internal readonly Native.Types.IntOptions _impl;

        public IntOptions()
        {
            _impl = Native.Types.IntOptions.Create();
        }

        public bool IsStored => _impl.IsStored;

        public bool IsFast => _impl.IsFast;

        public bool IsIndexed => _impl.IsIndexed;

        public Cardinality? FastFieldCardinality => _impl.FastFieldCardinality;

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

        public void Dispose() => _impl.Dispose();

        public override int GetHashCode() => _impl.GetHashCode();

        public override bool Equals(object obj) => _impl.Equals((obj as IntOptions)?._impl);
    }
}