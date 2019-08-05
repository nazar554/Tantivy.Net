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
                Assert.Null(value.TextOptions);
            }
        }

        [Theory]
        [InlineData(DefaultTokenizers.Default)]
        [InlineData(DefaultTokenizers.EnglishStemming)]
        [InlineData(DefaultTokenizers.Raw)]
        public void TokenizerWorks(string tokenizer)
        {
            using (var value = new TextFieldIndexing())
            {
                value.Tokenizer = tokenizer;
                Assert.Equal(tokenizer, value.Tokenizer);
            }
        }

        [Theory]
        [InlineData(IndexRecordOption.Basic)]
        [InlineData(IndexRecordOption.WithFreqs)]
        [InlineData(IndexRecordOption.WithFreqsAndPositions)]
        public void IndexRecordOptionWorks(IndexRecordOption indexRecordOptions)
        {
            using (var value = new TextFieldIndexing())
            {
                value.IndexRecordOptions = indexRecordOptions;
                Assert.Equal(indexRecordOptions, value.IndexRecordOptions);
            }
        }
    }
}