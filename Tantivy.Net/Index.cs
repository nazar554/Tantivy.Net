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
            EnsureSchemaNotNull(schema);

            var index = Native.Types.Index.CreateInRam(schema._impl);
            return new Index(index);
        }

        public static Index CreateInDir(string path, Schema.BuiltSchema schema)
        {
            EnsureSchemaNotNull(schema);

            var index = Native.Types.Index.CreateInDir(path, schema._impl);
            return new Index(index);
        }

        public static Index CreateFromTempDir(Schema.BuiltSchema schema)
        {
            EnsureSchemaNotNull(schema);

            var index = Native.Types.Index.CreateFromTempDir(schema._impl);
            return new Index(index);
        }

        private static void EnsureSchemaNotNull(Schema.BuiltSchema schema)
        {
            if (schema == null)
            {
                throw new ArgumentNullException(nameof(schema));
            }
        }

        public IndexReader Reader() => new IndexReader(_impl.Reader());

        public IndexWriter Writer(long overallHeapSizeInBytes)
            => new IndexWriter(_impl.Writer(overallHeapSizeInBytes));

        public IndexWriter Writer(int numThreads, long overallHeapSizeInBytes)
            => new IndexWriter(_impl.Writer(numThreads, overallHeapSizeInBytes));

        public Schema.BuiltSchema Schema => new Schema.BuiltSchema(_impl.Schema);

        public void SetDefaultMultithreadExecutor() => _impl.SetDefaultMultithreadExecutor();

        public void SetMultithreadExecutor(int numThreads) => _impl.SetMultithreadExecutor(numThreads);

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _impl.Dispose();
            }
        }
    }
}