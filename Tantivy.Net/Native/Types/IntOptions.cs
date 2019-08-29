namespace Tantivy.Net.Native.Types
{
    using System;
    using System.Runtime.InteropServices;
    using Helpers;
    using Schema;

    internal sealed class IntOptions : Abstract.SafeHandleZeroIsInvalid<IntOptions>
    {
        private IntOptions()
        {
        }

        protected override bool ReleaseHandle()
        {
            Destroy(handle);
            return true;
        }

        public bool IsStored => IsStoredImpl(this);

        public void SetStored() => SetStoredImpl(this);

        public bool IsIndexed => IsIndexedImpl(this);

        public void SetIndexed() => SetIndexedImpl(this);

        public bool IsFast => IsFastImpl(this);

        public void SetFast(Cardinality cardinality)
        {
            if (cardinality < Cardinality.SingleValue || cardinality > Cardinality.MultiValues)
            {
                throw new ArgumentOutOfRangeException(nameof(cardinality));
            }

            SetFastImpl(this, (int)cardinality + 1);
        }

        public Cardinality? FastFieldCardinality
        {
            get
            {
                int value = GetFastFieldCardinalityImpl(this);
                return value == 0 ? (Cardinality?)null : EnumHelper.VerifyEnum((Cardinality)(value - 1));
            }
        }

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_new_int_options", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern IntOptions Create();

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_drop_int_options", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void Destroy(IntPtr handle);

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_int_options_is_stored", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool IsStoredImpl(IntOptions options);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_int_options_set_stored", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void SetStoredImpl(IntOptions options);

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_int_options_is_indexed", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool IsIndexedImpl(IntOptions options);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_int_options_set_indexed", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void SetIndexedImpl(IntOptions options);

        /****************************************************************/

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_int_options_is_fast", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool IsFastImpl(IntOptions options);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_int_options_set_fast", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void SetFastImpl(IntOptions options, int cardinality);

        [DllImport(Constants.DllName, EntryPoint = "tantivy_schema_int_options_get_fastfield_cardinality", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern int GetFastFieldCardinalityImpl(IntOptions options);

        /****************************************************************/
    }
}