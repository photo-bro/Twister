using System;
using Twister.Compiler.Lexer;
using Xunit;

namespace Twister.Test.UnitTest.Lexer
{
    public class SourceScannerExtensionsTest
    {
#pragma warning disable CS1701 // Assuming assembly reference matches identity

        [Theory]
        [InlineData("abcdefghijklmnopqrstuvq", "ghi", 1)]
        [InlineData("", "", 0)]
        [InlineData("aabbccddeeaa", "aa", 2)]
        [InlineData("int main()\r\nHello World;\r\n", "\r\n", 2)]
        public void Test_Count(string initialSpan, string initialItem, int expectedCount)
        {
            var span = initialSpan.AsSpan();
            var item = initialItem.AsSpan();

            var actualCount = span.Count(item);

            Assert.Equal(expectedCount, actualCount);
        }
    }

#pragma warning restore CS1701 // Assuming assembly reference matches identity
}
