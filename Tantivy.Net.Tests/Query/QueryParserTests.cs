namespace Tantivy.Net.Tests.Query
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Tantivy.Net.Schema;
    using Tantivy.Net.Tokenizer;
    using TantivyIndex = Index;

    public class QueryParserTests : IDisposable
    {
        private readonly TantivyIndex _index;
        private readonly uint Id;
        private readonly uint Name;
        private readonly uint Birthday;
        private readonly uint Photo;

        private class Person
        {
            public long Id { get; set; }

            public string Name { get; set; }

            public DateTime Birthday { get; set; }

            public byte[] Photo { get; set; }
        }

        public QueryParserTests()
        {
            using (var builder = new SchemaBuilder())
            {
                using (var options = new IntOptions().Fast(Cardinality.SingleValue).Stored())
                {
                    Id = builder.AddI64Field(nameof(Id), options);
                }

                using (var indexingOpts = new TextFieldIndexing { Tokenizer = DefaultTokenizers.EnglishStemming })
                using (var textOptions = new TextOptions { IndexingOptions = indexingOpts }.Stored())
                {
                    Name = builder.AddTextField(nameof(Name), textOptions);
                }

                using (var options = new IntOptions().Fast(Cardinality.SingleValue).Stored())
                {
                    Birthday = builder.AddDateField(nameof(Birthday), options);
                }
                Photo = builder.AddBytesField(nameof(Photo));
                _index = TantivyIndex.CreateInRam(builder.Build());
            }
        }

        private void WriteDocuments(IEnumerable<Person> people)
        {
            if (people == null)
            {
                throw new ArgumentNullException(nameof(people));
            }

            using (var writer = _index.Writer(10 * 1024 * 1024))
            {
                foreach (var document in people.Select(CreateDocument))
                {
                    writer.AddDocument(document);
                }

                writer.Commit();
            }
        }

        private Document CreateDocument(Person person)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person));
            }

            var document = new Document();
            try
            {
                document.Add(Id, person.Id);
                if (person.Name != null)
                {
                    document.Add(Name, person.Name);
                }
                document.Add(Birthday, person.Birthday);
                if (person.Photo != null)
                {
                    document.Add(Photo, person.Photo);
                }

                return document;
            }
            catch
            {
                document.Dispose();
                throw;
            }
        }

        public void Dispose()
        {
            _index.Dispose();
        }
    }
}