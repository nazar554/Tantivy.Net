namespace Tantivy.Net.Tests.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using NodaTime;
    using Tantivy.Net.Schema;

    public abstract class SchemaProfile<T>
    {
        protected SchemaProfile()
        {
            _delegates = new Lazy<Delegates>(CreateDelegates);
            _memberBuilders = new Dictionary<MemberInfo, IFieldBuilder<T>>();
            _nonMemberBuilders = new List<IFieldBuilder<T>>();
        }

        private class Delegates
        {
            public Delegates(Action<Document, T> readFieldsFromDocument, Action<T, Document> writeFieldsToDocument)
            {
                ReadFieldsFromDocument = readFieldsFromDocument ?? throw new ArgumentNullException(nameof(readFieldsFromDocument));
                WriteFieldsToDocument = writeFieldsToDocument ?? throw new ArgumentNullException(nameof(writeFieldsToDocument));
            }

            public Action<Document, T> ReadFieldsFromDocument { get; }

            public Action<T, Document> WriteFieldsToDocument { get; }
        }

        private readonly Lazy<Delegates> _delegates;
        private readonly Dictionary<MemberInfo, IFieldBuilder<T>> _memberBuilders;
        private readonly List<IFieldBuilder<T>> _nonMemberBuilders;

        private Delegates CreateDelegates()
        {
            var paramDocument = Expression.Parameter(typeof(Document), "document");
            var paramValue = Expression.Parameter(typeof(T), "value");

            var readExpressions = new List<Expression>();
            var writeExpressions = new List<Expression>();

            foreach (var memberBuilder in _memberBuilders.Values.Concat(_nonMemberBuilders))
            {
                var readExpression = memberBuilder.GetReadFieldsFromDocumentExpression(paramDocument, paramValue);
                if (readExpression != null)
                {
                    readExpressions.Add(readExpression);
                }

                var writeExpression = memberBuilder.GetWriteFieldsToDocumentExpression(paramValue, paramDocument);
                if (writeExpression != null)
                {
                    writeExpressions.Add(writeExpression);
                }
            }

            var readFieldsFromDocument = Expression.Lambda<Action<Document, T>>(
                    Expression.Block(readExpressions),
                    paramDocument,
                    paramValue
                )
                .Compile();
            var writeFieldsToDocument = Expression.Lambda<Action<T, Document>>(
                    Expression.Block(writeExpressions),
                    paramValue,
                    paramDocument
                )
                .Compile();

            return new Delegates(readFieldsFromDocument, writeFieldsToDocument);
        }

        public BuiltSchema GetSchema()
        {
            using (var builder = new SchemaBuilder())
            {
                BuildSchema(builder);

                return builder.Build();
            }
        }

        public void BuildSchema(SchemaBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            foreach (var memberBuilder in _memberBuilders.Values.Concat(_nonMemberBuilders))
            {
                memberBuilder.AddField(builder);
            }
        }

        public Document WriteDocument(T value)
        {
            var document = new Document();
            try
            {
                WriteFieldsToDocument(value, document);

                return document;
            }
            catch
            {
                document.Dispose();
                throw;
            }
        }

        public T ReadValue(Document document)
        {
            EnsureDocumentNotNull(document);

            // somehow create T here without new() constraint???
            throw new NotImplementedException();
        }

        public void ReadFieldsFromDocument(Document document, T value)
        {
            EnsureDocumentNotNull(document);

            _delegates.Value.ReadFieldsFromDocument(document, value);
        }

        public void WriteFieldsToDocument(T value, Document document)
        {
            EnsureDocumentNotNull(document);

            _delegates.Value.WriteFieldsToDocument(value, document);
        }

        private static void EnsureDocumentNotNull(Document document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }
        }

        private void AddBuilder(IFieldBuilder<T> builder)
        {
            var memberInfo = builder.MemberInfo;
            if (memberInfo != null)
            {
                _memberBuilders.Add(memberInfo, builder);
            }
            else
            {
                _nonMemberBuilders.Add(builder);
            }
        }

        public IntOptionsBuilder<T> Property(Expression<Func<T, long>> property)
        {
            var builder = new IntOptionsBuilder<T>(property.Body);
            AddBuilder(builder);
            return builder;
        }

        public IntOptionsBuilder<T> Property(Expression<Func<T, IEnumerable<long>>> property)
        {
            var builder = new IntOptionsBuilder<T>(property.Body);
            AddBuilder(builder);
            return builder;
        }

        public IntOptionsBuilder<T> Property(Expression<Func<T, long?>> property)
        {
            var builder = new IntOptionsBuilder<T>(property.Body);
            AddBuilder(builder);
            return builder;
        }

        public IntOptionsBuilder<T> Property(Expression<Func<T, ulong>> property)
        {
            var builder = new IntOptionsBuilder<T>(property.Body);
            AddBuilder(builder);
            return builder;
        }

        public IntOptionsBuilder<T> Property(Expression<Func<T, IEnumerable<ulong>>> property)
        {
            var builder = new IntOptionsBuilder<T>(property.Body);
            AddBuilder(builder);
            return builder;
        }

        public IntOptionsBuilder<T> Property(Expression<Func<T, ulong?>> property)
        {
            var builder = new IntOptionsBuilder<T>(property.Body);
            AddBuilder(builder);
            return builder;
        }

        public IntOptionsBuilder<T> Property(Expression<Func<T, DateTime>> property)
        {
            var builder = new IntOptionsBuilder<T>(property.Body);
            AddBuilder(builder);
            return builder;
        }

        public IntOptionsBuilder<T> Property(Expression<Func<T, IEnumerable<DateTime>>> property)
        {
            var builder = new IntOptionsBuilder<T>(property.Body);
            AddBuilder(builder);
            return builder;
        }

        public IntOptionsBuilder<T> Property(Expression<Func<T, DateTime?>> property)
        {
            var builder = new IntOptionsBuilder<T>(property.Body);
            AddBuilder(builder);
            return builder;
        }

        public IntOptionsBuilder<T> Property(Expression<Func<T, ZonedDateTime>> property)
        {
            var builder = new IntOptionsBuilder<T>(property.Body);
            AddBuilder(builder);
            return builder;
        }

        public IntOptionsBuilder<T> Property(Expression<Func<T, IEnumerable<ZonedDateTime>>> property)
        {
            var builder = new IntOptionsBuilder<T>(property.Body);
            AddBuilder(builder);
            return builder;
        }

        public IntOptionsBuilder<T> Property(Expression<Func<T, ZonedDateTime?>> property)
        {
            var builder = new IntOptionsBuilder<T>(property.Body);
            AddBuilder(builder);
            return builder;
        }

        public IntOptionsBuilder<T> Property(Expression<Func<T, IEnumerable<DateTimeOffset>>> property)
        {
            var builder = new IntOptionsBuilder<T>(property.Body);
            AddBuilder(builder);
            return builder;
        }

        public IntOptionsBuilder<T> Property(Expression<Func<T, DateTimeOffset>> property)
        {
            var builder = new IntOptionsBuilder<T>(property.Body);
            AddBuilder(builder);
            return builder;
        }

        public IntOptionsBuilder<T> Property(Expression<Func<T, DateTimeOffset?>> property)
        {
            var builder = new IntOptionsBuilder<T>(property.Body);
            AddBuilder(builder);
            return builder;
        }

        public TextOptionsBuilder<T> Property(Expression<Func<T, string>> property)
        {
            var builder = new TextOptionsBuilder<T>(property.Body);
            AddBuilder(builder);
            return builder;
        }

        public TextOptionsBuilder<T> Property(Expression<Func<T, IEnumerable<string>>> property)
        {
            var builder = new TextOptionsBuilder<T>(property.Body);
            AddBuilder(builder);
            return builder;
        }

        public FacetBuilder<T> Facet(Expression<Func<T, string>> property)
        {
            var builder = new FacetBuilder<T>(property.Body);
            AddBuilder(builder);
            return builder;
        }

        public FacetBuilder<T> Facet(Expression<Func<T, IEnumerable<string>>> property)
        {
            var builder = new FacetBuilder<T>(property.Body);
            AddBuilder(builder);
            return builder;
        }

        public FacetBuilder<T> Facet(Expression<Func<T, byte[]>> property)
        {
            var builder = new FacetBuilder<T>(property.Body);
            AddBuilder(builder);
            return builder;
        }

        public BytesBuilder<T> Property(Expression<Func<T, byte[]>> property)
        {
            var builder = new BytesBuilder<T>(property.Body);
            AddBuilder(builder);
            return builder;
        }

        public BytesBuilder<T> Property(Expression<Func<T, IEnumerable<byte[]>>> property)
        {
            var builder = new BytesBuilder<T>(property.Body);
            AddBuilder(builder);
            return builder;
        }
    }
}