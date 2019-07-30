namespace Tantivy.Net.Tests.Schema
{
    using Tantivy.Net.Schema;
    using Xunit;

    public class FunctionsTests
    {
        [Theory]
        [InlineData("test1")]
        [InlineData("B2131")]
        [InlineData("Ax_cvwq12")]
        [InlineData("Hello_World")]
        public void IsValidFieldNameCorrectTrue(string fieldName)
        {
            bool valid = Functions.IsValidFieldName(fieldName);
            Assert.True(valid);
        }

        [Theory]
        [InlineData("%!22")]
        [InlineData("32131")]
        [InlineData("4242_22")]
        [InlineData("A!fafs2_11")]
        public void IsValidFieldNameIncorrectFalse(string fieldName)
        {
            bool valid = Functions.IsValidFieldName(fieldName);
            Assert.False(valid);
        }
    }
}