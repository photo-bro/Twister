using System;
using System.Collections.Generic;
using System.Linq;
using Twister.Compiler.Lexer;
using Twister.Compiler.Lexer.Interface;
using Twister.Compiler.Lexer.Token;
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
        [InlineData("123u ", 1)]
        [InlineData("1.0", 1)]
        [InlineData(".999", 1)]
        [InlineData("1234567890", 1)]
        [InlineData("1234567890 123 1.12321 9u", 4)]
        public void Test_Lexer_NumericLiteral(string source, int expectedTokenCount)
        {
            var tokens = GetTokens(source, LexerFlag.None);
            Assert.Equal(expectedTokenCount,
                         tokens.Count(tk=> tk is SignedIntToken || tk is UnsignedIntToken || tk is RealToken));
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
            Assert.Equal(expectedTokenCount,
                         tokens.Count(tk => tk is StringLiteralToken || tk is CharLiteralToken));
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

        [Theory]
        [InlineData("myVar", 1)]
        [InlineData("myVa Joshua", 2)]
        [InlineData("_twister", 1)]
        [InlineData("_twister123", 1)]
        [InlineData("_twister___ hellostring", 2)]
        [InlineData("a b c  d       e f g", 7)]
        public void Test_Lexer_Identifier(string source, int expectedTokenCount)
        {
            var tokens = GetTokens(source, LexerFlag.None);
            Assert.Equal(expectedTokenCount, tokens.OfType<IdToken>().Count());
        }

        [Theory]
        [InlineData("1myVar", typeof(UnexpectedCharacterException))]
        [InlineData("1123myVar", typeof(UnexpectedCharacterException))]
        [InlineData("11.23myVar", typeof(UnexpectedCharacterException))]
        public void Test_Lexer_BadIdentifier(string source, Type expectedExceptionType)
        {
            Assert.Throws(expectedExceptionType, () => GetTokens(source, LexerFlag.None).ToList());
        }

        [Theory]
        [InlineData("func", 1)]
        [InlineData("int uint float str char struct", 6)]
        [InlineData("if else while break cont return", 6)]
        public void Test_Lexer_Keyword(string source, int expectedTokenCount)
        {
            var tokens = GetTokens(source, LexerFlag.None);
            Assert.Equal(expectedTokenCount, tokens.OfType<KeywordToken>().Count());
        }

        // TODO Rainy Day cases too

        // TODO
        //[Theory]
        //public void Test_Lexer_RegularTokens(string source, int expectedTokenCount)
        //{
        //    var tokens = GetTokens(source, LexerFlag.None);
        //    Assert.Equal(expectedTokenCount, tokens.Count());
        //}

        //[Theory]
        //public void Test_Lexer_Flag_AllowUnicode(string source, int expectedTokenCount)
        //{

        //}

        //[Theory]
        //public void Test_Lexer_Flag_NoUnicode(string source, Type expectedExceptionType)
        //{
        //    Assert.Throws(expectedExceptionType, () => GetTokens(source, LexerFlag.None));

        //}

        //    [Theory]
        //public void Test_Lexer_Combined(string source, int expectedTokenCount)
        //{
        //    var tokens = GetTokens(source, LexerFlag.None);
        //    Assert.Equal(expectedTokenCount, tokens.Count());
        //}
#pragma warning restore CS1701 // Assuming assembly reference matches identity
    }
}
