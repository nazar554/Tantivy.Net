namespace Tantivy.Net.Schema
{
    using System;

    public class BuiltSchema : Abstract.DisposableBase
    {
        internal readonly Native.Types.BuiltSchema _impl;

        internal BuiltSchema(Native.Types.BuiltSchema impl)
        {
            _impl = impl ?? throw new ArgumentNullException(nameof(impl));
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