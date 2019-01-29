using Xunit;
using Twister.Compiler.Common;
using Twister.Compiler.Lexer.Interface;
using Twister.Compiler.Lexer.Token;
using Twister.Compiler.Parser;
using Twister.Compiler.Parser.Interface;
using Twister.Compiler.Parser.Primitive;
using System.Collections.Generic;
using Twister.Compiler.Lexer;

namespace Twister.Test.UnitTest.Parser.Expression
{
#pragma warning disable CS1701 // Assuming assembly reference matches identity

    public class ExpressionPrecedenceTest
    {
        ITwisterParser _parser;

        private void SetupParser()
        {
            _parser = new TwisterParser((tokens) =>
                new TokenMatcher(
                    new Scanner<IToken>(tokens, new EmptyToken())
                ));
        }

        private IEnumerable<IToken> GetTokens(string expression) =>
            new SourceLexer((s) => new TextSourceScanner(s)).Tokenize(expression, LexerFlag.None);

        [Theory]
        [InlineData("1 + 2", 3)]
        [InlineData("1 + 2 * 3", 7)]
        [InlineData("1 + 4 / 2", 3)]
        [InlineData("-1 + 2", 1)]
        [InlineData("-1 + 2 * 3", 5)]
        [InlineData("-1 + -2", -3)]
        [InlineData("-1 - -2", 1)]
        [InlineData("-1 - - - - -2", -3)]
        [InlineData("1 + 2 * -3", -5)] 
        public void Integer(string expression, int expected)
        {
            SetupParser();

            var tokens = GetTokens(expression);

            var node = _parser.ParseExpression(tokens) as IValueNode<TwisterPrimitive>;

            Assert.NotNull(node);

            var actual = node.Value;

            Assert.Equal<int>(expected, actual);

        }

    }
#pragma warning restore CS1701 // Assuming assembly reference matches identity
}
