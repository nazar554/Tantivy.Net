namespace Tantivy.Net.Tests.Infrastructure
{
    using System;
    using System.Linq.Expressions;
    using Tantivy.Net.Schema;

    public class IntOptionsBuilder<T> : BaseBuilder<T, IntOptionsBuilder<T>>
    {
        public IntOptionsBuilder(Expression expression) : base(expression)
        {
        }

        public override uint AddField(SchemaBuilder builder)
        {
            throw new NotImplementedException();
        }
    }
}