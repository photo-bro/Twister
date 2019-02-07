using System;
using System.Reflection;
using Twister.Compiler.Lexer;
using Xunit;

namespace Twister.Test.UnitTest.Lexer
{
    public class TextSourceScannerTest
    {
        private const char EOF = (char)0xFFFF;

#pragma warning disable CS1701 // Assuming assembly reference matches identity

        [Theory]
        [InlineData("", 0, 1, EOF)]
        [InlineData("", 0, 0, EOF)]
        [InlineData("123", 0, 1, '1')]
        [InlineData("abcdefghijklmnopqrstuv", 0, 10, 'j')]
        [InlineData("abcdefghijklmnopqrstuv", 0, 50, EOF)]
        [InlineData("abcdefghijklmnopqrstuv", 5, 1, 'f')]
        [InlineData("abcdefghijklmnopqrstuv", 5, 5, 'j')]
        [InlineData("123", 3, 1, EOF)]
        [InlineData("123", 0, 3, '3')]
        [InlineData("123", 0, 4, EOF)]
        [InlineData("123", 2, 1, '3')]
        public void Advance_Happy(string source, int position, int advanceCount, char expected)
        {
            var scanner = new TextSourceScanner(source);

            if (position > 0)
                scanner.Advance(position);

            var startPosition = scanner.Position;

            var actual = scanner.Advance(advanceCount);
            Assert.Equal(expected, actual);

            if (source.Length > startPosition + advanceCount)
                Assert.Equal(startPosition + advanceCount, scanner.Position);
        }

        [Theory]
        [InlineData("123", -1)]
        public void Advance_NegativeCount(string source, int advanceCount)
        {
            var scanner = new TextSourceScanner(source);

            Assert.Throws<InvalidOperationException>(() => scanner.Advance(advanceCount));
        }

        [Theory]
        [InlineData("", 0, 1, EOF)]
        [InlineData("123", 0, 1, '1')]
        [InlineData("abcdefghijklmnopqrstuv", 0, 10, 'j')]
        [InlineData("abcdefghijklmnopqrstuv", 0, 50, EOF)]
        [InlineData("abcdefghijklmnopqrstuv", 5, 1, 'f')]
        [InlineData("abcdefghijklmnopqrstuv", 5, 5, 'j')]
        [InlineData("123", 3, 1, EOF)]
        [InlineData("123", 0, 3, '3')]
        [InlineData("123", 0, 4, EOF)]
        [InlineData("123", 3, -1, '2')]
        [InlineData("abcdefghijklmnopqrstuv", 5, -5, 'a')]
        [InlineData("abcdefghijklmnopqrstuv", 1, 0, 'a')]
        [InlineData("abcdefghijklmnopqrstuv", 2, -1, 'a')]
        [InlineData("abcdefghijklmnopqrstuv", 10, -5, 'e')]
        [InlineData("true", 4, -4, 't')]
        [InlineData("false", 5, -5, 'f')]
        [InlineData("abcdefghijklmnopqrstuv", 1, -1, 'a')]
        [InlineData("abcdefghijklmnopqrstuv", 5, -10, EOF)]
        public void PeekNext_Happy(string source, int position, int peekCount, char expected)
        {
            var scanner = new TextSourceScanner(source);

            if (position > 0)
                scanner.Advance(position);

            var startPosition = scanner.Position;

            var actual = scanner.Peek(peekCount);
            Assert.Equal(expected, actual);
            Assert.Equal(startPosition, scanner.Position);
        }


        [Theory]
        [InlineData("1234", 4, true)]
        [InlineData("12345", 4, false)]
        [InlineData("1234", 50, true)]
        [InlineData("", 0, true)]
        [InlineData("", 1, true)]
        [InlineData("a", 1, true)]
        public void IsAtEnd(string source, int position, bool expected)
        {
            var scanner = new TextSourceScanner(source);

            scanner.Advance(position);

            var actual = scanner.IsAtEnd();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", 0, 0)]
        [InlineData("\n\n\n\n", 4, 4)]
        [InlineData("\n\n\n\n", 1, 1)]
        [InlineData("int Main()\n{\n}\n\n", 25, 4)]
        [InlineData("int Main(){}", 25, 0)]
        public void CurrentSourceLine_Unix(string source, int position, int expected)
        {
            var scanner = new TextSourceScanner(source, "\n");

            scanner.Advance(position);

            var actual = scanner.CurrentSourceLine;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", 0, 0)]
        [InlineData("\r\n\r\n\r\n\r\n", 8, 4)]
        [InlineData("\r\n\r\n\r\n\r\n", 2, 1)]
        [InlineData("\r\n\r\n\r\n\r\n", 1, 0)]
        [InlineData("int Main()\r\n{\r\n}\r\n\r\n", 25, 4)]
        [InlineData("int Main(){}", 25, 0)]
        public void CurrentSourceLine_Windows(string source, int position, int expected)
        {
            var scanner = new TextSourceScanner(source, "\r\n");

            scanner.Advance(position);

            var actual = scanner.CurrentSourceLine;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", 0, 0, "")]
        [InlineData("", 0, 1, "")]
        [InlineData("", 1, 1, "")]
        [InlineData("0123456789", 0, 9, "012345678")]
        [InlineData("0123456789", 0, 10, "0123456789")]
        [InlineData("0123456789", 9, 10, "9")]
        [InlineData("0123456789", 3, 10, "3456789")]
        [InlineData("0123456789", 10, 10, "")]
        public void CurrentWindow(string source, int @base, int position, string expected)
        {
            var scanner = new TextSourceScanner(source);

            scanner.Advance(position);
            scanner.Base = @base;

            var actual = scanner.CurrentWindow;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", 0, 0, "")]
        [InlineData("", 0, 1, "")]
        [InlineData("", 1, 1, "")]
        [InlineData("0123456789", 0, 9, "012345678")]
        [InlineData("0123456789", 0, 10, "0123456789")]
        [InlineData("0123456789", 9, 10, "9")]
        [InlineData("0123456789", 3, 10, "3456789")]
        [InlineData("0123456789", 10, 10, "")]
        [InlineData("(* \n *) 123u (* *)", 8, 4, "123u")]
        public void CurrentWindow_WithBaseAdjust(string source, int position, int advanceCount, string expected)
        {
            var scanner = new TextSourceScanner(source);

            scanner.Advance(position);
            scanner.Base = scanner.Position;

            scanner.Advance(advanceCount);

            var actual = scanner.CurrentWindow;

            Assert.Equal(expected, actual);
        }
#pragma warning restore CS1701 // Assuming assembly reference matches identity
    }
}
