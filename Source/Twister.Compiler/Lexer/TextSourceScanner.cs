using Twister.Compiler.Lexer.Interface;
using System;

namespace Twister.Compiler.Lexer
{
    public class TextSourceScanner : ISourceScanner
    {
        private const char INVALIDCHAR = char.MaxValue;
        private readonly string NewLine = Environment.NewLine;
        private readonly ReadOnlyMemory<char> _source;

        public TextSourceScanner(string source)
        {
            _source = source.AsMemory();
        }

        public char InvalidChar => INVALIDCHAR;

        public int CurrentSourceLine { get; private set; } = 0;

        public int Offset => Position - Base;

        public int Base { get; set; } = 0;

        public int Position { get; private set; } = 0;

        public int SourceLength => _source.Length;

        // Memory<char>.ToString converts char buffer to proper string
        // https://docs.microsoft.com/en-us/dotnet/api/system.memory-1.tostring?view=netcore-2.2#System_Memory_1_ToString
        public string CurrentWindow => _source.Slice(Base, Offset).ToString();

        public char Advance() => Advance(1);

        public char Advance(int count)
        {
            if (IsAtEnd())
                return InvalidChar;

            if (Position + count > SourceLength)
                return InvalidChar;

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

        public char PeekNext() => PeekNext(1);

        public char PeekNext(int count)
        {
            if (Position + count > SourceLength)
                return InvalidChar;

            if (Position + count < 1)
                return InvalidChar;

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
                CurrentSourceLine += currentSlice.Count(NewLine.AsSpan());
                return;
            }

            if (currentSlice[0] == '\n')
            {
                // if non-unix newlines only increment if there is a carriage return
                if (NewLine[0] == '\r' && PeekNext(-1) != '\r')
                    return;
                CurrentSourceLine++;
            }
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