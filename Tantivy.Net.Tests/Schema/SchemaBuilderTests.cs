namespace Tantivy.Net.Tests.Schema
{
    using Tantivy.Net.Schema;
    using Xunit;

    public class SchemaBuilderTests
    {
        [Fact]
        public void ConstructorWorks()
        {
            using (var builder = new SchemaBuilder())
            {
            }
        }

        [Fact]
        public void AddU64FieldWorks()
        {
            using (var builder = new SchemaBuilder())
            {
                uint field1 = builder.AddU64Field("test1", new IntOptions());
                uint field2 = builder.AddU64Field("test2", new IntOptions());
                Assert.NotEqual(field1, field2);
            }
        }
    }
}