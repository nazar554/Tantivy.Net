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

        [Fact]
        public void AddI64FieldWorks()
        {
            using (var builder = new SchemaBuilder())
            {
                uint field1 = builder.AddI64Field("test1", new IntOptions());
                uint field2 = builder.AddI64Field("test2", new IntOptions());
                Assert.NotEqual(field1, field2);
            }
        }

        [Fact]
        public void AddDateFieldWorks()
        {
            using (var builder = new SchemaBuilder())
            {
                uint field1 = builder.AddDateField("test1", new IntOptions());
                uint field2 = builder.AddDateField("test2", new IntOptions());
                Assert.NotEqual(field1, field2);
            }
        }

        [Fact]
        public void AddFacetFieldWorks()
        {
            using (var builder = new SchemaBuilder())
            {
                uint field1 = builder.AddFacetField("test1");
                uint field2 = builder.AddFacetField("test2");
                Assert.NotEqual(field1, field2);
            }
        }

        [Fact]
        public void AddBytesFieldWorks()
        {
            using (var builder = new SchemaBuilder())
            {
                uint field1 = builder.AddBytesField("test1");
                uint field2 = builder.AddBytesField("test2");
                Assert.NotEqual(field1, field2);
            }
        }

        [Fact]
        public void AddTextFieldWorks()
        {
            using (var builder = new SchemaBuilder())
            using (var options = new TextOptions())
            {
                uint field1 = builder.AddTextField("test1", options);
                uint field2 = builder.AddTextField("test2", options);
                Assert.NotEqual(field1, field2);
            }
        }
    }
}