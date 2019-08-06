namespace Tantivy.Net.Tests.Schema
{
    using Xunit;
    using Tantivy.Net.Schema;

    public class DocumentTests
    {
        [Fact]
        public void CreateDefaultsWorks()
        {
            using (var document = new Document())
            {
            }
        }
    }
}