namespace Tantivy.Net
{
    using System;
    using System.Runtime.Serialization;
    using Tantivy.Net.Native.Types;

    [Serializable]
    public class TantivyException : Exception
    {
        public TantivyException()
        {
        }

        public TantivyException(string message) : base(message)
        {
        }

        public TantivyException(string message, Exception inner) : base(message, inner)
        {
        }

        internal TantivyException(TantivyError error) :
            base(error != null ? error.ToString() : throw new ArgumentNullException(nameof(error)))
        {
            error.Dispose();
        }

        protected TantivyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}