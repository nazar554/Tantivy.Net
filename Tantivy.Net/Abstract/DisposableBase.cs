namespace Tantivy.Net.Abstract
{
    using System;

    public abstract class DisposableBase : IDisposable
    {
        private bool _disposed;

        public void Dispose()
        {
            if (!_disposed)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
                _disposed = true;
            }
        }

        protected abstract void Dispose(bool disposing);
    }
}