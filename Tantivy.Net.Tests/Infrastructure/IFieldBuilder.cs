namespace Tantivy.Net.Tests.Infrastructure
{
    using System.Linq.Expressions;
    using Tantivy.Net.Schema;
    using System.Reflection;

    public interface IFieldBuilder<T>
    {
        string FieldName { get; }

        MemberInfo MemberInfo { get; }

        uint AddField(SchemaBuilder builder);

        Expression GetReadFieldsFromDocumentExpression(Expression paramDocument, Expression paramValue);

        Expression GetWriteFieldsToDocumentExpression(Expression paramValue, Expression paramDocument);
    }
}