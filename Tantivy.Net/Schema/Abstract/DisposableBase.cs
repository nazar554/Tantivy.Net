namespace Tantivy.Net.Schema.Abstract
{
    using System;

    public abstract class DisposableBase : IDisposable
    {
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected abstract void Dispose(bool disposing);
    }
}