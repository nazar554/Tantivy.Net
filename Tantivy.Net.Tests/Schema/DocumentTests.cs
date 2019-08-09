namespace Tantivy.Net.Tests.Schema
{
    using System;
    using System.Collections.Generic;
    using NodaTime;
    using Tantivy.Net.Schema;
    using Xunit;

    public class DocumentTests
    {
        [Fact]
        public void CreateDefaultsWorks()
        {
            using (var document = new Document())
            {
                Assert.Equal(0, document.Length);
                Assert.True(document.IsEmpty);
                document.Add(1, 32);
                document.Add(2, "test");
                Assert.Equal(2, document.Length);
                Assert.False(document.IsEmpty);
            }
        }

        public static IEnumerable<object[]> GetValidFieldValues()
        {
            yield return new object[] { 5UL };
            yield return new object[] { 5L };
            yield return new object[] { "5L" };

            var pastDate = new DateTime(1980, 1, 1, 0, 0, 0);
            var futureDate = new DateTime(2180, 1, 1, 0, 0, 0);

            var offset = new TimeSpan(3, 0, 0);

            yield return new object[] { DateTime.SpecifyKind(pastDate, DateTimeKind.Utc) };
            yield return new object[] { DateTime.SpecifyKind(pastDate, DateTimeKind.Local) };
            yield return new object[] { DateTime.SpecifyKind(pastDate, DateTimeKind.Unspecified) };

            yield return new object[] { DateTime.SpecifyKind(futureDate, DateTimeKind.Utc) };
            yield return new object[] { DateTime.SpecifyKind(futureDate, DateTimeKind.Local) };
            yield return new object[] { DateTime.SpecifyKind(futureDate, DateTimeKind.Unspecified) };

            yield return new object[] { new ZonedDateTime(Instant.MinValue, DateTimeZone.Utc) };
            yield return new object[] { new ZonedDateTime(Instant.MaxValue, DateTimeZone.Utc) };

            yield return new object[] { DateTime.MinValue };
            yield return new object[] { DateTime.MaxValue };
            yield return new object[] { DateTimeOffset.MaxValue };
            yield return new object[] { DateTimeOffset.MinValue };

            yield return new object[] { new DateTimeOffset(pastDate) };
            yield return new object[] { new DateTimeOffset(futureDate) };
            yield return new object[] { new DateTimeOffset(pastDate, offset) };
            yield return new object[] { new DateTimeOffset(futureDate, offset) };

            yield return new object[] { new byte[] { 0x21, 0x46, 0x15 } };
            yield return new object[] { new byte[0] };
        }

        [Theory]
        [MemberData(nameof(GetValidFieldValues))]
        public void AddWorks(dynamic value)
        {
            using (var document = new Document())
            {
                Assert.Equal(0, document.Length);
                Assert.True(document.IsEmpty);
                const int NumFields = 5;
                for (uint field = 0; field < NumFields; ++field)
                {
                    document.Add(field, value);
                }
                Assert.Equal(NumFields, document.Length);
                Assert.False(document.IsEmpty);
            }
        }

        [Fact]
        public void FilterWorks()
        {
            using (var document = new Document())
            {
                const int NumFields = 10;
                for (uint i = 0; i < NumFields; ++i)
                {
                    document.Add(i, i);
                }
                document.FilterFields(i => i % 2 == 0);
                Assert.False(document.IsEmpty);
                Assert.Equal(NumFields / 2, document.Length);
            }
        }
    }
}