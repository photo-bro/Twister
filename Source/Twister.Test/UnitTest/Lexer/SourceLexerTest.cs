using System;
using System.Collections.Generic;
using System.Linq;
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
        [InlineData("123u ", 1)]
        [InlineData("1.0", 1)]
        [InlineData(".999", 1)]
        [InlineData("1234567890", 1)]
        [InlineData("1234567890 123 1.12321 9u", 4)]
        public void NumericLiteral(string source, int expectedTokenCount)
        {
            var tokens = GetTokens(source, LexerFlag.None);
            Assert.Equal(expectedTokenCount,
                         tokens.Count(tk => tk is SignedIntToken || tk is UnsignedIntToken || tk is RealToken));
        }

        [Theory]
        [InlineData("", 0)]
        [InlineData("'c'", 1)]
        [InlineData("'\n'", 1)]
        [InlineData("\"Hello World!\"", 1)]
        [InlineData("\"\"", 1)]
        [InlineData("\"123 \\\" \"", 1)]
        public void StringCharLiteral(string source, int expectedTokenCount)
        {
            var tokens = GetTokens(source, LexerFlag.None);
            Assert.Equal(expectedTokenCount,
                         tokens.Count(tk => tk is StringLiteralToken || tk is CharLiteralToken));
        }

        [Theory]
        [InlineData("true", 1)]
        [InlineData("toot", 0)]
        [InlineData("false", 1)]
        [InlineData("fools", 0)]
        [InlineData("truefalse", 0)]
        [InlineData("true false", 2)]
        [InlineData(" != true", 1)]
        public void BoolLiteral(string source, int expectedTokenCount)
        {
            var tokens = GetTokens(source, LexerFlag.None);
            Assert.Equal(expectedTokenCount, tokens.Count(tk => tk is BoolLiteralToken));
        }

        [Theory]
        [InlineData("fALSE", typeof(InvalidTokenException))]
        [InlineData("truE", typeof(InvalidTokenException))]
        public void InvalidBool(string source, Type expectedExceptionType)
        {
            Assert.Throws(expectedExceptionType, () => GetTokens(source, LexerFlag.None).ToList());
        }

        [Theory]
        [InlineData("", 0)]
        [InlineData("(*   *)", 0)]
        [InlineData("(* abcdefhuasdfjh  *)123u(* *)", 1)]
        [InlineData("(* \n *)123u(* *)", 1)]
        [InlineData("(* \n *) 123u (* *)", 1)]
        public void Comments(string source, int expectedTokenCount)
        {
            var tokens = GetTokens(source, LexerFlag.None);
            Assert.Equal(expectedTokenCount, tokens.Count());
        }

        [Theory]
        [InlineData("   ", 0)]
        [InlineData("\t\t\t\t\t\t\t", 0)]
        [InlineData("\r\n\r\n\r\n\t\t", 0)]
        [InlineData("    123 1.000 \"Hello World\"   ", 3)]
        public void Whitespace(string source, int expectedTokenCount)
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
        public void Identifier(string source, int expectedTokenCount)
        {
            var tokens = GetTokens(source, LexerFlag.None);
            Assert.Equal(expectedTokenCount, tokens.OfType<IdToken>().Count());
        }

        [Theory]
        [InlineData("1myVar", typeof(UnexpectedCharacterException))]
        [InlineData("1123myVar", typeof(UnexpectedCharacterException))]
        [InlineData("11.23myVar", typeof(UnexpectedCharacterException))]
        public void BadIdentifier(string source, Type expectedExceptionType)
        {
            Assert.Throws(expectedExceptionType, () => GetTokens(source, LexerFlag.None).ToList());
        }

        [Theory]
        [InlineData("func", 1)]
        [InlineData("int uint float str char struct bool", 7)]
        [InlineData("if else while break cont return", 6)]
        public void Keyword(string source, int expectedTokenCount)
        {
            var tokens = GetTokens(source, LexerFlag.None);
            Assert.Equal(expectedTokenCount, tokens.OfType<KeywordToken>().Count());
        }

        // TODO Rainy Day cases too

        [Theory]
        [InlineData("+-%*", 4)]
        [InlineData("/^&|", 4)]
        [InlineData("&& ||", 2)]
        [InlineData("&&||", 2)]
        [InlineData("&||", 2)]
        [InlineData("&&&", 2)]
        [InlineData("|||", 2)]
        [InlineData("<< >>", 2)]
        [InlineData("== !=", 2)]
        public void Operator(string source, int expectedTokenCount)
        {
            var tokens = GetTokens(source, LexerFlag.None);
            Assert.Equal(expectedTokenCount, tokens.OfType<OperatorToken>().Count());
        }

        [Theory]
        [InlineData("{}()[]<>", 8)]
        [InlineData(".,:;?", 5)]
        [InlineData("...", 2)]
        [InlineData("<<<", 2)]
        [InlineData("===", 2)]
        [InlineData(">>>", 2)]
        [InlineData("=>", 1)]
        [InlineData("==>", 2)]
        [InlineData("==>>", 2)]
        [InlineData("==>=>", 3)]
        public void RegularTokens(string source, int expectedTokenCount)
        {
            var tokens = GetTokens(source, LexerFlag.None);
            Assert.Equal(expectedTokenCount, tokens.Count());
        }

        [Theory]
        [InlineData("\"\"", 1)]
        [InlineData("\"åéô\"", 1)]
        [InlineData("\"©™Ω\" \"ºℨ\"", 2)]
        public void Flag_AllowUnicode(string source, int expectedTokenCount)
        {
            var tokens = GetTokens(source, LexerFlag.AllowUnicode);
            Assert.Equal(expectedTokenCount, tokens.Count());
        }

        [Theory]
        [InlineData("\"\"", typeof(IllegalCharacterException))]
        [InlineData("\"åéô\"", typeof(IllegalCharacterException))]
        [InlineData("\"©™Ω\" \"ºℨ\"", typeof(IllegalCharacterException))]
        public void Flag_NoUnicode(string source, Type expectedExceptionType)
        {
            Assert.Throws(expectedExceptionType, () => GetTokens(source, LexerFlag.None).ToList());
        }

        // TODO - Defect somewhere (could be VS Mac) where newline escapes are getting duplicated, 
        // which is breaking char parsing
        //[Fact]
        //public void   Combined()
        //{
        //    foreach (var file in TestProgramLoader.AllPrograms())
        //    {
        //        var tokens = GetTokens(file, LexerFlag.None).ToList();
        //    }
        //}
#pragma warning restore CS1701 // Assuming assembly reference matches identity
    }
}
