namespace Tantivy.Net.Query
{
    using System;

    public sealed class QueryParser : IDisposable
    {
        internal readonly Native.Types.QueryParser _impl;

        private QueryParser(Native.Types.QueryParser impl) => _impl = impl ?? throw new ArgumentNullException(nameof(impl));

        public static QueryParser ForIndex(Index index, in ReadOnlySpan<uint> fields)
        {
            if (index == null)
            {
                throw new ArgumentNullException(nameof(index));
            }

            return new QueryParser(
                Native.Types.QueryParser.ForIndex(index._impl, fields)
            );
        }

        public ParsedQuery ParseQuery(string query) => new ParsedQuery(_impl.ParseQuery(query));

        public void Dispose() => _impl.Dispose();
    }
}