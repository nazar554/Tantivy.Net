namespace Tantivy.Net.Tests.Schema
{
    using Tantivy.Net.Schema;
    using Tantivy.Net.Tokenizer;
    using Xunit;

    public class TextFieldIndexingTests
    {
        [Fact]
        public void DefaultsWork()
        {
            using (var value = new TextFieldIndexing())
            {
                Assert.Equal(DefaultTokenizers.Default, value.Tokenizer);
                Assert.Equal(IndexRecordOption.Basic, value.IndexRecordOptions);
                Assert.False(value.IsReadOnly);
                Assert.Null(value.TextOptions);
            }
        }
    }
}