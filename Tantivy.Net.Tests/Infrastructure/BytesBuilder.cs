namespace Tantivy.Net.Tests.Infrastructure
{
    using System.Linq.Expressions;
    using Tantivy.Net.Schema;

    public class BytesBuilder<T> : BaseBuilder<T, BytesBuilder<T>>
    {
        public BytesBuilder(Expression expression) : base(expression)
        {
        }

        public override uint AddField(SchemaBuilder builder)
        {
            return builder.AddBytesField(FieldName);
        }
    }
}