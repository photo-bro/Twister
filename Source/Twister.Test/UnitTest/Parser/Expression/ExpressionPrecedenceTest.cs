using System.Collections.Generic;
using Twister.Compiler.Common;
using Twister.Compiler.Lexer;
using Twister.Compiler.Lexer.Interface;
using Twister.Compiler.Lexer.Token;
using Twister.Compiler.Parser;
using Twister.Compiler.Parser.ExpressionParser;
using Twister.Compiler.Parser.Interface;
using Twister.Compiler.Parser.Primitive;
using Xunit;

namespace Twister.Test.UnitTest.Parser.Expression
{
#pragma warning disable CS1701 // Assuming assembly reference matches identity

    public class ExpressionPrecedenceTest
    {
        private IValueNode<TwisterPrimitive> ParseExpression(IEnumerable<IToken> expression)
        {
            var parser = new RecursiveDescentExpressionParser();
            return parser.ParseArithmeticExpression(
                matcher: new TokenMatcher(new Scanner<IToken>(expression, new EmptyToken())),
                scopeManager: new ScopeManager(),
                assignmentCallback: null);
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
        [InlineData("-1 - - - - +2", 1)]
        [InlineData("1 + 2 * -3", -5)]
        [InlineData("4 | 8 ^ 16 & 12 << 2 + 24 * -12", 28)]
        [InlineData("-256 / 64 - 72 >> 3 & 55 ^ 23 | 5", 37)]
        [InlineData("-256 / -64 - 72 << -3 & 55 ^ -23 | 99", -21)]
        public void Integer(string expression, int expected)
        {
            var tokens = GetTokens(expression);

            var node = ParseExpression(tokens);

            Assert.NotNull(node);

            var actual = node.Value;

            Assert.Equal<int>(expected, actual);
        }

        [Theory]
        [InlineData("48 ^ 2", 50)]
        [InlineData("6 << 3 ^ 2", 50)]
        [InlineData("2 + 4 << 3 ^ 2", 50)]
        [InlineData("2 + 8 / 2 << 3 ^ 2", 50)]
        [InlineData("2 + -2 * -4 / 2 << 3 ^ 2", 50)]
        public void IntegerAllPrecedence(string expression, int expected)
        {
            var tokens = GetTokens(expression);

            var node = ParseExpression(tokens);

            Assert.NotNull(node);

            var actual = node.Value;

            Assert.Equal<int>(expected, actual);
        }

    }
#pragma warning restore CS1701 // Assuming assembly reference matches identity
}
