namespace Tantivy.Net.Tests.Schema
{
    using Tantivy.Net.Schema;
    using Xunit;

    public class IndexTests
    {
        [Fact]
        public void DefaultsWork()
        {
            using (var builder = new SchemaBuilder())
            {
                builder.AddBytesField("field1");
                uint field2 = builder.AddFacetField(nameof(field2));
                var schema = builder.Build();

                using (var index = Index.CreateInRam(schema))
                {
                    index.SetDefaultMultithreadExecutor();
                    var indexSchema = index.Schema;

                    Assert.Equal(nameof(field2), indexSchema.GetFieldName(field2));
                }
            }
        }
    }
}