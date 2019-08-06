namespace Tantivy.Net.Native.Types
{
    using System;
    using System.Runtime.InteropServices;
    using Helpers;

    internal sealed class Index : Abstract.SafeHandleZeroIsInvalid<BuiltSchema>
    {
        private Index()
        {
        }

        public BuiltSchema Schema => SchemaImpl(this);

        public static Index CreateInRam(BuiltSchema schema)
        {
            if (schema == null)
            {
                throw new ArgumentNullException(nameof(schema));
            }
            return CreateInRamImpl(schema);
        }

        public static Index CreateInDir(string path, BuiltSchema schema)
        {
            if (schema == null)
            {
                throw new ArgumentNullException(nameof(schema));
            }
            return MarshalHelper.Utf8Call(path, (buffer, len) =>
            {
                unsafe
                {
                    var index = CreateInDirImpl(buffer, len, schema, out var e);
                    if (index.IsInvalid)
                    {
                        throw new TantivyException(e);
                    }
                    return index;
                }
            });
        }

        public static Index CreateFromTempDir(BuiltSchema schema)
        {
            var index = CreateFromTempDirImpl(schema, out var e);
            if (index.IsInvalid)
            {
                throw new TantivyException(e);
            }
            return index;
        }

        public IndexReader Reader()
        {
            var reader = ReaderImpl(this, out var e);
            if (reader.IsInvalid)
            {
                throw new TantivyException(e);
            }
            return reader;
        }

        public IndexWriter Writer(long overallHeapSizeInBytes)
        {
            checked
            {
                var writer = WriterImpl(this, new UIntPtr((ulong)overallHeapSizeInBytes), out var e);
                if (writer.IsInvalid)
                {
                    throw new TantivyException(e);
                }
                return writer;
            }
        }

        public IndexWriter Writer(int numThreads, long overallHeapSizeInBytes)
        {
            checked
            {
                var writer = WriterImpl(
                    this,
                    new UIntPtr((uint)numThreads),
                    new UIntPtr((ulong)overallHeapSizeInBytes),
                    out var e
                );
                if (writer.IsInvalid)
                {
                    throw new TantivyException(e);
                }
                return writer;
            }
        }

        public void SetMultithreadExecutor(int numThreads)
        {
            checked
            {
                lock (this)
                {
                    SetMultithreadExecutorImpl(this, new UIntPtr((uint)numThreads));
                }
            }
        }

        public void SetDefaultMultithreadExecutor()
        {
            lock (this)
            {
                SetDefaultMultithreadExecutorImpl(this);
            }
        }

        protected override bool ReleaseHandle()
        {
            Destroy(handle);
            return true;
        }

        [DllImport(Constants.DllName, EntryPoint = "tantivy_drop_index", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void Destroy(IntPtr handle);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_index_create_in_ram", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern Index CreateInRamImpl(BuiltSchema schema);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_index_create_from_tempdir", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern Index CreateFromTempDirImpl(BuiltSchema schema, out TantivyError error);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_index_set_multithread_executor", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void SetMultithreadExecutorImpl(Index index, UIntPtr numThreads);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_index_set_default_multithread_executor", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void SetDefaultMultithreadExecutorImpl(Index index);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_index_schema", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern BuiltSchema SchemaImpl(Index index);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_index_create_in_dir", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static unsafe extern Index CreateInDirImpl(byte* path, UIntPtr pathLength, BuiltSchema schema, out TantivyError error);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_index_writer", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern IndexWriter WriterImpl(Index index, UIntPtr overallHeapSizeInBytes, out TantivyError error);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_index_writer_with_num_threads", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern IndexWriter WriterImpl(Index index, UIntPtr numThreads, UIntPtr overallHeapSizeInBytes, out TantivyError error);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_index_reader", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern IndexReader ReaderImpl(Index index, out TantivyError error);
    }
}