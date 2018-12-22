using System;
using Twister.Compiler.Lexer;
using Xunit;

namespace Twister.Test.UnitTest.Lexer
{
    public class TextSourceScannerTest
    {
#pragma warning disable CS1701 // Assuming assembly reference matches identity

        /* TODO Test cases:
         * 
         * Happy Path ----
         * 
         * - IsAtEnd()
         * - CurrentSourceLine Unix
         * - CurrentSourceLine Non-Unix
         * - CurrentWindow
         * 
         * UnHappy Path ----
         * - Advance -> at source end
         * - Peek -> at source end
         * - CurrentWindow -> at source end
         * - 
         * 
         */

        [Theory]
        [InlineData("", 0, 1, (char)0xFFFF)]
        [InlineData("", 0, 0, (char)0xFFFF)]
        [InlineData("123", 0, 1, '1')]
        [InlineData("abcdefghijklmnopqrstuv", 0, 10, 'j')]
        [InlineData("abcdefghijklmnopqrstuv", 0, 50, (char)0xFFFF)]
        [InlineData("abcdefghijklmnopqrstuv", 5, 1, 'f')]
        [InlineData("abcdefghijklmnopqrstuv", 5, 5, 'j')]
        [InlineData("123", 3, 1, (char)0xFFFF)]
        [InlineData("123", 0, 3, '3')]
        [InlineData("123", 0, 4, (char)0xFFFF)]
        [InlineData("123", 2, 1, '3')]
        public void Test_Advance_Happy(string source, int initialAdvance, int advanceCount, char expected)
        {
            var scanner = new TextSourceScanner(source);

            if (initialAdvance > 0)
                scanner.Advance(initialAdvance);

            var startPosition = scanner.Position;

            var actual = scanner.Advance(advanceCount);
            Assert.Equal(expected, actual);

            if (source.Length > startPosition + advanceCount)
                Assert.Equal(startPosition + advanceCount, scanner.Position);
        }

        [Theory]
        [InlineData("123", -1)]
        public void Test_Advance_NegativeCount(string source, int advanceCount)
        {
            var scanner = new TextSourceScanner(source);

            Assert.Throws<InvalidOperationException>(() => scanner.Advance(advanceCount));
        }

        [Theory]
        [InlineData("", 0, 1, (char)0xFFFF)]
        [InlineData("123", 0, 1, '1')]
        [InlineData("abcdefghijklmnopqrstuv", 0, 10, 'j')]
        [InlineData("abcdefghijklmnopqrstuv", 0, 50, (char)0xFFFF)]
        [InlineData("abcdefghijklmnopqrstuv", 5, 1, 'f')]
        [InlineData("abcdefghijklmnopqrstuv", 5, 5, 'j')]
        [InlineData("123", 3, 1, (char)0xFFFF)]
        [InlineData("123", 0, 3, '3')]
        [InlineData("123", 0, 4, (char)0xFFFF)]
        [InlineData("123", 3, -1, '2')]
        [InlineData("abcdefghijklmnopqrstuv", 5, -5, (char)0xFFFF)]
        [InlineData("abcdefghijklmnopqrstuv", 1, 0, 'a')]
        [InlineData("abcdefghijklmnopqrstuv", 2, -1, 'a')]
        [InlineData("abcdefghijklmnopqrstuv", 1, -1, (char)0xFFFF)]
        [InlineData("abcdefghijklmnopqrstuv", 5, -10, (char)0xFFFF)]
        public void Test_PeekNext_Happy(string source, int initialAdvanceCount, int peekCount, char expected)
        {
            var scanner = new TextSourceScanner(source);

            if (initialAdvanceCount > 0)
                scanner.Advance(initialAdvanceCount);

            var startPosition = scanner.Position;

            var actual = scanner.PeekNext(peekCount);
            Assert.Equal(expected, actual);
            Assert.Equal(startPosition, scanner.Position);
        }

#pragma warning restore CS1701 // Assuming assembly reference matches identity
    }
}
