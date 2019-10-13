namespace Tantivy.Net.Tests.Infrastructure
{
    using System.Linq.Expressions;
    using Tantivy.Net.Schema;

    public class TextOptionsBuilder<T> : BaseBuilder<T, TextOptionsBuilder<T>>
    {
        public TextOptionsBuilder(Expression expression) : base(expression)
        {
        }

        public override uint AddField(SchemaBuilder builder)
        {
            throw new System.NotImplementedException();
        }
    }
}