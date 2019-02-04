using System.Collections.Generic;
using Twister.Compiler.Common;
using Twister.Compiler.Lexer;
using Twister.Compiler.Lexer.Interface;
using Twister.Compiler.Lexer.Token;
using Twister.Compiler.Parser;
using Twister.Compiler.Parser.ExpressionParser;

namespace Twister.Test.UnitTest.Parser
{
    public static class TestUtility
    {
        public static TwisterParser CreateParser()
        {
            return new TwisterParser(
                createTokenMatcher: tokens => new TokenMatcher(new Scanner<IToken>(tokens, new EmptyToken())),
                expressionParser: new RecursiveDescentExpressionParser());
        }

        public static IEnumerable<IToken> Tokenize(this string program) =>
            new SourceLexer(s => new TextSourceScanner(s)).Tokenize(program, LexerFlag.None);

    }
}
