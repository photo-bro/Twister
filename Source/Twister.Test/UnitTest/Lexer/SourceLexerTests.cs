using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twister.Compiler.Lexer;
using Twister.Compiler.Lexer.Interface;
using Twister.Test.Data;
using Xunit;

namespace Twister.Test.UnitTest.Lexer
{
    public class SourceLexerTests
    {
#pragma warning disable CS1701 // Assuming assembly reference matches identity

        private IEnumerable<IToken> GetTokens(string source, LexerFlag flags)
        {
            var lexer = new SourceLexer((s) => new TextSourceScanner(s));

            return lexer.Tokenize(source, flags);
        }

        [Theory]
        [InlineData("", 0)]
        [InlineData("123", 1)]
        [InlineData("123u", 1)]
        [InlineData("1.0", 1)]
        [InlineData(".999", 1)]
        [InlineData("1234567890", 1)]
        [InlineData("1234567890 123 1.12321 9u", 4)]
        public void Test_Lexer_NumericLiteral(string source, int expectedTokenCount)
        {
            var tokens = GetTokens(source, LexerFlag.None);
            Assert.Equal(expectedTokenCount, tokens.Count());

            Console.Write(FormattedTokenString(tokens));
        }

        [Theory]
        [InlineData("", 0)]
        [InlineData("'c'", 1)]
        [InlineData("'\n'", 1)]
        [InlineData("\"Hello World!\"", 1)]
        [InlineData("\"\"", 1)]
        public void Test_Lexer_StringCharLiteral(string source, int expectedTokenCount)
        {
            var tokens = GetTokens(source, LexerFlag.None);
            Assert.Equal(expectedTokenCount, tokens.Count());

            Console.Write(FormattedTokenString(tokens));
        }

        [Theory] 
        [InlineData("", 0)]
        [InlineData("(*   *)", 0)]
        [InlineData("(* abcdefhuasdfjh  *) 123u (* *)", 1)]
        [InlineData("(* \n *) 123u (* *)", 1)]
        public void Test_Lexer_Comments(string source, int expectedTokenCount)
        {
            var tokens = GetTokens(source, LexerFlag.None);
            Assert.Equal(expectedTokenCount, tokens.Count());

        }

        private string FormattedTokenString(IEnumerable<IToken> tokens)
        {
            var sb = new StringBuilder();
            var count = 0;
            foreach (var token in tokens)
                sb.AppendLine($"{count++}: {token}");
            return sb.ToString();
        }
#pragma warning restore CS1701 // Assuming assembly reference matches identity
    }
}
