namespace Tantivy.Net.Tests.Infrastructure
{
    using System.Linq.Expressions;
    using Tantivy.Net.Schema;

    public class FacetBuilder<T> : BaseBuilder<T, FacetBuilder<T>>
    {
        public FacetBuilder(Expression expression) : base(expression)
        {
        }

        public override uint AddField(SchemaBuilder builder)
        {
            return builder.AddFacetField(FieldName);
        }
    }
}