namespace Tantivy.Net.Tests.Schema
{
    using System;
    using System.IO;
    using Tantivy.Net.Schema;
    using Xunit;

    public class IndexTests : IDisposable
    {
        private readonly BuiltSchema schema;
        private readonly uint field2;
        private DirectoryInfo indexDirectory;

        public IndexTests()
        {
            using (var builder = new SchemaBuilder())
            {
                builder.AddBytesField("field1");
                field2 = builder.AddFacetField(nameof(field2));
                schema = builder.Build();
            }

            var path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            indexDirectory = Directory.CreateDirectory(path);
        }

        [Fact]
        public void DefaultsCreateInRamWorks()
        {
            using (var index = Index.CreateInRam(schema))
            {
                index.SetDefaultMultithreadExecutor();
                var indexSchema = index.Schema;
                Assert.Equal(nameof(field2), indexSchema.GetFieldName(field2));
            }
        }

        [Fact]
        public void DefaultsCreateInDirWorks()
        {
            using (var index = Index.CreateInDir(indexDirectory.FullName, schema))
            {
                index.SetDefaultMultithreadExecutor();
                var indexSchema = index.Schema;
                Assert.Equal(nameof(field2), indexSchema.GetFieldName(field2));
            }
        }

        [Fact]
        public void DefaultsCreateInNotExistingDirThrows()
        {
            string path = indexDirectory.FullName;
            indexDirectory.Delete();
            indexDirectory = null;

            Assert.Throws<TantivyException>(() => Index.CreateInDir(path, schema));
        }

        [Fact]
        public void DefaultsCreateFromTempDirWorks()
        {
            using (var index = Index.CreateFromTempDir(schema))
            {
                index.SetDefaultMultithreadExecutor();
                var indexSchema = index.Schema;
                Assert.Equal(nameof(field2), indexSchema.GetFieldName(field2));
            }
        }

        public void Dispose()
        {
            schema.Dispose();
            indexDirectory?.Delete(recursive: true);
        }
    }
}