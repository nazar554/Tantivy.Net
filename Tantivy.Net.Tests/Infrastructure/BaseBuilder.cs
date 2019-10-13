namespace Tantivy.Net.Tests.Infrastructure
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using Tantivy.Net.Schema;

    public abstract class BaseBuilder<T, TBuilder> : IFieldBuilder<T>
        where TBuilder : BaseBuilder<T, TBuilder>
    {
        protected BaseBuilder(Expression expression)
        {
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
            MemberExpression = expression as MemberExpression;
            MemberInfo = MemberExpression?.Member;
            FieldName = MemberInfo?.Name;
        }

        protected Expression Expression { get; set; }

        protected MemberExpression MemberExpression { get; }

        public string FieldName { get; private set; }

        public MemberInfo MemberInfo { get; }

        public TBuilder HasFieldName(string fieldName)
        {
            FieldName = fieldName ?? throw new ArgumentNullException(nameof(fieldName));
            return (TBuilder)this;
        }

        public Expression GetReadFieldsFromDocumentExpression(Expression paramDocument, Expression paramValue)
        {
            throw new NotImplementedException();
        }

        public Expression GetWriteFieldsToDocumentExpression(Expression paramValue, Expression paramDocument)
        {
            throw new NotImplementedException();
        }

        public abstract uint AddField(SchemaBuilder builder);
    }
}