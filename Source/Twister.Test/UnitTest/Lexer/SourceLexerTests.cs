using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twister.Compiler.Lexer;
using Twister.Compiler.Lexer.Interface;
using Twister.Compiler.Lexer.Token;
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

            Console.Write(tokens.ToFormattedString());
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

            Console.Write(tokens.ToFormattedString());
        }

        [Theory] 
        [InlineData("", 0)]
        [InlineData("(*   *)", 0)]
        [InlineData("(* abcdefhuasdfjh  *)123u(* *)", 1)]
        [InlineData("(* \n *)123u(* *)", 1)]
        [InlineData("(* \n *) 123u (* *)", 1)]
        public void Test_Lexer_Comments(string source, int expectedTokenCount)
        {
            var tokens = GetTokens(source, LexerFlag.None);
            Assert.Equal(expectedTokenCount, tokens.Count());
        }

        [Theory]
        [InlineData("   ", 0)]
        [InlineData("\t\t\t\t\t\t\t", 0)]
        [InlineData("\r\n\r\n\r\n\t\t", 0)]
        [InlineData("    123 1.000 \"Hello World\"   ", 3)]
        public void Test_Lexer_Whitespace(string source, int expectedTokenCount)
        {
            var tokens = GetTokens(source, LexerFlag.None);
            Assert.Equal(expectedTokenCount, tokens.Count());
        }
#pragma warning restore CS1701 // Assuming assembly reference matches identity
    }
}
