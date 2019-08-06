﻿namespace Tantivy.Net.Tests.Schema
{
    using System;
    using System.Collections.Generic;
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

            yield return new object[] { DateTime.SpecifyKind(pastDate, DateTimeKind.Utc)};
            yield return new object[] { DateTime.SpecifyKind(pastDate, DateTimeKind.Local) };
            yield return new object[] { DateTime.SpecifyKind(pastDate, DateTimeKind.Unspecified) };

            yield return new object[] { DateTime.SpecifyKind(futureDate, DateTimeKind.Utc) };
            yield return new object[] { DateTime.SpecifyKind(futureDate, DateTimeKind.Local) };
            yield return new object[] { DateTime.SpecifyKind(futureDate, DateTimeKind.Unspecified) };

            yield return new object[] { new DateTimeOffset(pastDate) };
            yield return new object[] { new DateTimeOffset(futureDate) };
            yield return new object[] { new DateTimeOffset(pastDate, offset) };
            yield return new object[] { new DateTimeOffset(futureDate, offset) };

            yield return new object[] { new byte[] { 0x21, 0x46, 0x15 } };
            yield return new object[] { new byte[0] };
        }

        public static IEnumerable<object[]> GetInvalidFieldValues()
        {
            yield return new object[] { DateTime.MinValue };
            yield return new object[] { DateTime.MaxValue };
            yield return new object[] { DateTimeOffset.MaxValue };
            yield return new object[] { DateTimeOffset.MinValue };
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

        [Theory]
        [MemberData(nameof(GetInvalidFieldValues))]
        public void ThrowsOnLargeDates(dynamic value)
        {
            using (var document = new Document())
            {
                Assert.Equal(0, document.Length);
                Assert.True(document.IsEmpty);
                Assert.Throws<OverflowException>(() =>
                {
                    document.Add(0, value);
                });
            }
        }
    }
}