namespace Tantivy.Net.Tests.Schema
{
    using Tantivy.Net.Schema;
    using Xunit;

    public class TextOptionsTests
    {
        [Fact]
        public void DefaultsWork()
        {
            using (var builder = new TextOptions())
            {
                Assert.Null(builder.IndexingOptions);
                Assert.False(builder.IsStored);
            }
        }

    }
}