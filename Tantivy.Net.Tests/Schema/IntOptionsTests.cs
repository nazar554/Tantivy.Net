namespace Tantivy.Net.Tests.Schema
{
    using Tantivy.Net.Schema;
    using Xunit;

    public class IntOptionsTests
    {
        [Fact]
        public void DefaultsWork()
        {
            using (var builder = new IntOptions())
            {
                Assert.False(builder.IsStored);
                Assert.False(builder.IsFast);
                Assert.Null(builder.FastCardinality);
                Assert.False(builder.IsIndexed);
            }
        }

        [Fact]
        public void StoredWorks()
        {
            using (var builder = new IntOptions().Stored())
            {
                Assert.True(builder.IsStored);
                Assert.False(builder.IsFast);
                Assert.Null(builder.FastCardinality);
                Assert.False(builder.IsIndexed);
            }
        }

        [Fact]
        public void IndexedWorks()
        {
            using (var builder = new IntOptions().Indexed())
            {
                Assert.True(builder.IsIndexed);
                Assert.False(builder.IsFast);
                Assert.Null(builder.FastCardinality);
                Assert.False(builder.IsStored);
            }
        }

        [Theory]
        [InlineData(Cardinality.MultiValues)]
        [InlineData(Cardinality.SingleValue)]
        public void FastWorks(Cardinality cardinality)
        {
            using (var builder = new IntOptions().Fast(cardinality))
            {
                Assert.False(builder.IsIndexed);
                Assert.True(builder.IsFast);
                Assert.Equal(cardinality, builder.FastCardinality);
                Assert.False(builder.IsStored);
            }
        }
    }
}