namespace Tantivy.Net.Schema
{
    public class IntOptions : Abstract.DisposableBase
    {
        internal readonly Native.Types.IntOptions _impl;

        public IntOptions()
        {
            _impl = Native.Types.IntOptions.Create();
        }

        public virtual bool IsStored => _impl.IsStored;

        public virtual bool IsFast => _impl.IsFast;

        public virtual bool IsIndexed => _impl.IsIndexed;

        public virtual Cardinality? FastCardinality => _impl.FastCardinality;

        public virtual IntOptions Stored()
        {
            _impl.SetStored();
            return this;
        }

        public virtual IntOptions Indexed()
        {
            _impl.SetIndexed();
            return this;
        }

        public virtual IntOptions Fast(Cardinality cardinality)
        {
            _impl.SetFast(cardinality);
            return this;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _impl.Dispose();
            }
        }
    }
}