using Twister.Compiler.Lexer.Interface;
using System;
using System.Collections.Generic;
using Twister.Compiler.Common.Interface;

namespace Twister.Compiler.Lexer
{
    public class TextSourceScanner : ISourceScanner
    {
        private const char Invalidchar = char.MaxValue;
        private readonly string _newLine;
        private readonly ReadOnlyMemory<char> _source;

        public TextSourceScanner(string source, string newline = null)
        {
            _source = source.AsMemory();
            _newLine = newline ?? Environment.NewLine;
        }

        public char InvalidItem => Invalidchar;

        public int CurrentSourceLine { get; private set; }

        public int Offset => Position - Base;

        public int Base { get; set; }

        public int Position { get; private set; }

        public int SourceLength => _source.Length;

        // Memory<char>.ToString converts char buffer to proper string
        // https://docs.microsoft.com/en-us/dotnet/api/system.memory-1.tostring?view=netcore-2.2#System_Memory_1_ToString
        public string CurrentWindow
        {
            get
            {
                if (Base > SourceLength)
                    return string.Empty;

                return Base + Offset > SourceLength 
                    ? _source.Slice(Base).ToString() 
                    : _source.Slice(Base, Offset).ToString();
            }
        }

        IEnumerable<char> IScanner<char>.CurrentWindow => CurrentWindow;

        public char Advance() => Advance(1);

        public char Advance(int count)
        {
            if (IsAtEnd())
                return InvalidItem;

            if (Position + count > SourceLength)
            {
                // Even though we are advancing past the end of the source we still need to track the newlines
                // we are advancing past
                var span = _source.Slice(Position).Span;
                CheckForNewlines(ref span);
                // We also need to update position too or else IsAtEnd() will still think we're inside the source
                Position += count;
                return InvalidItem;
            }

            if (count == 0)
                return _source.Span[Position];

            if (count < 0)
                throw new InvalidOperationException($"{nameof(TextSourceScanner)}" +
                    $".{nameof(TextSourceScanner.Advance)} can only advance forward");

            var currentSpan = _source.Slice(Position, count).Span;
            CheckForNewlines(ref currentSpan);

            Position += count;

            // Return last char in slice
            return currentSpan[count - 1];
        }

        public char Peek() => Peek(1);

        public char Peek(int count)
        {
            if (Position + count > SourceLength)
                return InvalidItem;

            if (Position + count == 0)
                return _source.Span[0];

            if (Position + count < 1)
                return InvalidItem;

            return _source.Span[Position + count - 1];
        }

        public bool IsAtEnd()
        {
            if (SourceLength == 0) return true;
            return Position >= SourceLength;
        }

        public void Reset()
        {
            CurrentSourceLine = 0;
            Base = 0;
            Position = -1;
        }

        private void CheckForNewlines(ref ReadOnlySpan<char> currentSlice)
        {
            if (currentSlice.Length != 1)
            {
                CurrentSourceLine += currentSlice.Count(_newLine.AsSpan());
                return;
            }

            if (currentSlice[0] != '\n')
                return;

            // if non-unix newlines only increment if there is a carriage return
            if (_newLine[0] == '\r' && Peek(-1) != '\r')
                return;
            CurrentSourceLine++;
        }
    }

    public static class SourceScannerExtensions
    {
        public static int Count(this ReadOnlySpan<char> span, ReadOnlySpan<char> item)
        {
            if (item.Length < 1 || span.Length < 1 || item.Length > span.Length)
                return 0;

            if (!span.Contains(item, StringComparison.InvariantCulture))
                return 0;

            var indexOfItem = span.IndexOf(item);
            if (indexOfItem + item.Length >= span.Length)
                return 1;

            return span.Slice(indexOfItem + item.Length).Count(item) + 1;
        }
    }
}