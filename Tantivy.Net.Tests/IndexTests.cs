﻿namespace Tantivy.Net.Tests
{
    using System;
    using System.IO;
    using Tantivy.Net.Schema;
    using Xunit;

    using TantivyIndex = Index;

    public class IndexTests : IDisposable
    {
        private readonly BuiltSchema schema;
        private readonly uint field2;
        private DirectoryInfo indexDirectory;

        private const long DefaultMaxHeapSize = 150 * 1024 * 1024; // 150 MB

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
            using (var index = TantivyIndex.CreateInRam(schema))
            {
                index.SetDefaultMultithreadExecutor();
                var indexSchema = index.Schema;
                Assert.Equal(nameof(field2), indexSchema.GetFieldName(field2));

                var reader = index.Reader();
                Assert.NotNull(reader);
                var writer = index.Writer(DefaultMaxHeapSize);
                Assert.NotNull(writer);
            }
        }

        [Fact]
        public void DefaultsNumThreadsCreateInRamWorks()
        {
            using (var index = TantivyIndex.CreateInRam(schema))
            {
                index.SetMultithreadExecutor(4);
                var indexSchema = index.Schema;
                Assert.Equal(nameof(field2), indexSchema.GetFieldName(field2));

                var reader = index.Reader();
                Assert.NotNull(reader);
                var writer = index.Writer(DefaultMaxHeapSize);
                Assert.NotNull(writer);
            }
        }

        [Fact]
        public void DefaultsCreateInDirWorks()
        {
            using (var index = TantivyIndex.CreateInDir(indexDirectory.FullName, schema))
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

            Assert.Throws<TantivyException>(() => TantivyIndex.CreateInDir(path, schema));
        }

        [Fact]
        public void DefaultsCreateFromTempDirWorks()
        {
            using (var index = TantivyIndex.CreateFromTempDir(schema))
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