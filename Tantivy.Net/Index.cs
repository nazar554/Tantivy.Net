namespace Tantivy.Net
{
    using System;

    public class Index : Abstract.DisposableBase
    {
        internal Native.Types.Index _impl;

        private Index(Native.Types.Index impl)
        {
            _impl = impl ?? throw new ArgumentNullException(nameof(impl));
        }

        public static Index CreateInRam(Schema.BuiltSchema schema)
        {
            if (schema == null)
            {
                throw new ArgumentNullException(nameof(schema));
            }

            var index = Native.Types.Index.CreateInRam(schema._impl);
            return new Index(index);
        }

        public Schema.BuiltSchema Schema => new Schema.BuiltSchema(_impl.Schema);

        public void SetDefaultMultithreadExecutor() => _impl.SetDefaultMultithreadExecutor();

        public void SetMultithreadExecutor(int numThreads) => _impl.SetMultithreadExecutor(numThreads);

        public void SetMultithreadExecutor(long numThreads) => _impl.SetMultithreadExecutor(numThreads);

        public void SetMultithreadExecutor(uint numThreads) => _impl.SetMultithreadExecutor(numThreads);

        public void SetMultithreadExecutor(ulong numThreads) => _impl.SetMultithreadExecutor(numThreads);

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _impl.Dispose();
            }
        }
    }
}