namespace Tantivy.Net.Native.Types
{
    using System;
    using System.Runtime.InteropServices;

    internal sealed class Index : Abstract.SafeHandleZeroIsInvalid<BuiltSchema>
    {
        private Index()
        {
        }

        public static Index CreateInRam(BuiltSchema schema)
        {
            lock (schema)
            {
                var index = CreateInRamImpl(schema);
                schema.SetHandleAsInvalid();
                return index;
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
    }
}