namespace Tantivy.Net.Native.Types
{
    using System;
    using System.Runtime.InteropServices;
    using Helpers;

    internal sealed class Index : Abstract.SafeHandleZeroIsInvalid<Index>
    {
        private Index()
        {
        }

        public BuiltSchema Schema => SchemaImpl(this);

        public static Index CreateInRam(BuiltSchema schema, bool copy = false)
        {
            if (schema == null)
            {
                throw new ArgumentNullException(nameof(schema));
            }

            Index index;
            if (copy)
            {
                index = CreateInRamCopyImpl(schema);
            }
            else
            {
                index = CreateInRamMoveImpl(schema);
                schema.SetHandleAsInvalid();
            }
            return index;
        }

        public static Index CreateInDir(string path, BuiltSchema schema, bool copy = false)
        {
            if (schema == null)
            {
                throw new ArgumentNullException(nameof(schema));
            }
            unsafe
            {
                return MarshalHelper.Utf8Call(path, (buffer, len) =>
                {
                    {
                        Index index;
                        TantivyError e;

                        if (copy)
                        {
                            index = CreateInDirCopyImpl(buffer, len, schema, out e);
                        }
                        else
                        {
                            index = CreateInDirMoveImpl(buffer, len, schema, out e);
                            schema.SetHandleAsInvalid();
                        }

                        if (index.IsInvalid)
                        {
                            throw new TantivyException(e);
                        }
                        return index;
                    }
                });
            }
        }

        public static Index CreateFromTempDir(BuiltSchema schema, bool copy = false)
        {
            Index index;
            TantivyError e;

            if (copy)
            {
                index = CreateFromTempDirCopyImpl(schema, out e);
            }
            else
            {
                index = CreateFromTempDirMoveImpl(schema, out e);
                schema.SetHandleAsInvalid();
            }

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

        public IndexWriter Writer(ulong overallHeapSizeInBytes)
        {
            checked
            {
                var writer = WriterImpl(this, new UIntPtr(overallHeapSizeInBytes), out var e);
                if (writer.IsInvalid)
                {
                    throw new TantivyException(e);
                }
                return writer;
            }
        }

        public IndexWriter Writer(uint numThreads, ulong overallHeapSizeInBytes)
        {
            checked
            {
                var writer = WriterImpl(
                    this,
                    new UIntPtr(numThreads),
                    new UIntPtr(overallHeapSizeInBytes),
                    out var e
                );
                if (writer.IsInvalid)
                {
                    throw new TantivyException(e);
                }
                return writer;
            }
        }

        public void SetMultithreadExecutor(uint numThreads)
        {
            SetMultithreadExecutorImpl(this, new UIntPtr(numThreads), out var e);
            if (!e.IsInvalid)
            {
                throw new TantivyException(e);
            }
        }

        public void SetDefaultMultithreadExecutor()
        {
            SetDefaultMultithreadExecutorImpl(this, out var e);
            if (!e.IsInvalid)
            {
                throw new TantivyException(e);
            }
        }

        protected override bool ReleaseHandle()
        {
            Destroy(handle);
            return true;
        }

        [DllImport(Constants.DllName, EntryPoint = "tantivy_drop_index", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void Destroy(IntPtr handle);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_index_create_in_ram_copy", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern Index CreateInRamCopyImpl(BuiltSchema schema);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_index_create_in_ram_move", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern Index CreateInRamMoveImpl(BuiltSchema schema);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_index_create_from_tempdir_copy", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern Index CreateFromTempDirCopyImpl(BuiltSchema schema, out TantivyError error);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_index_create_from_tempdir_move", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern Index CreateFromTempDirMoveImpl(BuiltSchema schema, out TantivyError error);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_index_set_multithread_executor", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void SetMultithreadExecutorImpl(Index index, UIntPtr numThreads, out TantivyError error);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_index_set_default_multithread_executor", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void SetDefaultMultithreadExecutorImpl(Index index, out TantivyError error);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_index_schema", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern BuiltSchema SchemaImpl(Index index);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_index_create_in_dir_copy", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static unsafe extern Index CreateInDirCopyImpl(byte* path, UIntPtr pathLength, BuiltSchema schema, out TantivyError error);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_index_create_in_dir_move", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static unsafe extern Index CreateInDirMoveImpl(byte* path, UIntPtr pathLength, BuiltSchema schema, out TantivyError error);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_index_writer", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern IndexWriter WriterImpl(Index index, UIntPtr overallHeapSizeInBytes, out TantivyError error);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_index_writer_with_num_threads", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern IndexWriter WriterImpl(Index index, UIntPtr numThreads, UIntPtr overallHeapSizeInBytes, out TantivyError error);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_index_reader", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern IndexReader ReaderImpl(Index index, out TantivyError error);
    }
}