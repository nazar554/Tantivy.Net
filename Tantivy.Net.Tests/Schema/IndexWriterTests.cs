namespace Tantivy.Net.Tests.Schema
{
    using System;
    using Tantivy.Net.Schema;
    using Tantivy.Net.Tokenizer;
    using Xunit;

    public class IndexWriterTests : IDisposable
    {
        private const ulong HeapSize = 128 * 1024 * 1024;

        private readonly Index Index;
        private readonly uint Id;
        private readonly uint Text;

        public IndexWriterTests()
        {
            using (var builder = new SchemaBuilder())
            {
                using (var primaryKeyOptions = new IntOptions().Stored().Fast(Cardinality.SingleValue).Indexed())
                {
                    Id = builder.AddI64Field(nameof(Id), primaryKeyOptions);
                }

                using (var textFieldOptions = new TextFieldIndexing { Tokenizer = DefaultTokenizers.EnglishStemming })
                using (var textOptions = new TextOptions { IndexingOptions = textFieldOptions })
                {
                    Text = builder.AddTextField(nameof(Text), textOptions);
                }

                Index = Index.CreateInRam(builder.Build());
            }
        }

        [Fact]
        public void WriterWorks()
        {
            using (var writer = Index.Writer(HeapSize))
            {
                for (long id = 0; id < 100; ++id)
                {
                    using (var document = new Document())
                    {
                        document.Add(Id, id);
                        document.Add(Text, Guid.NewGuid().ToString());

                        writer.AddDocument(document);
                    }
                }
                writer.Commit();
            }
        }

        public void Dispose()
        {
            Index.Dispose();
        }
    }
}