﻿namespace Tantivy.Net.Schema
{
    using System;

    public sealed class TextFieldIndexing : IDisposable
    {
        internal readonly Native.Types.TextFieldIndexing _impl;

        internal TextFieldIndexing(TextOptions textOptions, Native.Types.TextFieldIndexing impl)
        {
            TextOptions = textOptions ?? throw new ArgumentNullException(nameof(textOptions));
            _impl = impl ?? throw new ArgumentNullException(nameof(impl));
        }

        public TextFieldIndexing()
        {
            _impl = Native.Types.TextFieldIndexing.Create();
        }

        public TextOptions TextOptions { get; }

        public string Tokenizer
        {
            get => _impl.Tokenizer;
            set => _impl.Tokenizer = value;
        }

        public IndexRecordOption IndexRecordOptions
        {
            get => _impl.IndexRecordOptions;
            set => _impl.IndexRecordOptions = value;
        }

        public void Dispose() => _impl.Dispose();
    }
}