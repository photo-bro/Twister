using System.Collections.Generic;
using Twister.Compiler.Common;
using Twister.Compiler.Lexer.Enum;
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

    public class SimpleExpressionTest
    {
        private IValueNode<TwisterPrimitive> ParseExpression(IEnumerable<IToken> expression)
        {
            var parser = new RecursiveDescentExpressionParser();
            return parser.ParseArithmeticExpression(
                matcher: new TokenMatcher(new Scanner<IToken>(expression, new EmptyToken())),
                scopeManager: new ScopeManager(),
                assignmentCallback: null);
        }

        [Fact]
        public void Addition()
        {
            IToken[] expression =
            {
                new SignedIntToken{Value = 1},
                new OperatorToken{Value = Operator.Plus},
                new SignedIntToken{Value = 2},
                new SemiColonToken()
            };

            var actualNode = ParseExpression(expression);

            var actual = actualNode.Value;

            Assert.Equal((TwisterPrimitive)3, actual);
        }

        [Fact]
        public void Mult()
        {
            IToken[] expression =
            {
                new SignedIntToken{Value = 9},
                new OperatorToken{Value = Operator.Multiplication},
                new SignedIntToken{Value = 10},
                new SemiColonToken()
            };

            var actualNode = ParseExpression(expression);

            var actual = actualNode.Value;

            Assert.Equal((TwisterPrimitive)90, actual);
        }

        [Fact]
        public void Shift()
        {
            IToken[] expression =
            {
                new SignedIntToken{Value = 1},
                new OperatorToken{Value = Operator.LeftShift},
                new SignedIntToken{Value = 4 },
                new SemiColonToken()
            };

            var actualNode = ParseExpression(expression);

            var actual = actualNode.Value;

            Assert.Equal((TwisterPrimitive)(1 << 4), actual);
        }

        [Fact]
        public void Relation()
        {
            IToken[] expression =
            {
                new SignedIntToken{Value = 9},
                new OperatorToken{Value = Operator.LogGreater},
                new SignedIntToken{Value = 10},
                new SemiColonToken()
            };

            var actualNode = ParseExpression(expression);

            var actual = actualNode.Value;

            Assert.Equal((TwisterPrimitive)false, actual);
        }

        [Fact]
        public void Eq()
        {
            IToken[] expression =
            {
                new SignedIntToken{Value = 10},
                new OperatorToken{Value = Operator.LogEqual},
                new SignedIntToken{Value = 10},
                new SemiColonToken()
            };

            var actualNode = ParseExpression(expression);

            var actual = actualNode.Value;

            Assert.Equal((TwisterPrimitive)true, actual);
        }

        [Fact]
        public void BitAnd()
        {
            IToken[] expression =
            {
                new SignedIntToken{Value = 0xFF},
                new OperatorToken{Value = Operator.BitAnd},
                new SignedIntToken{Value = 0X01},
                new SemiColonToken()
            };

            var actualNode = ParseExpression(expression);

            var actual = actualNode.Value;

            Assert.Equal((TwisterPrimitive)(0xFF & 0x01), actual);
        }

        [Fact]
        public void BitExOr()
        {
            IToken[] expression =
            {
                new SignedIntToken{Value = 0xFF},
                new OperatorToken{Value = Operator.BitExOr},
                new SignedIntToken{Value = 0xFF},
                new SemiColonToken()
            };

            var actualNode = ParseExpression(expression);

            var actual = actualNode.Value;

            Assert.Equal((TwisterPrimitive)0x0, actual);
        }

        [Fact]
        public void BitOr()
        {
            IToken[] expression =
            {
                new SignedIntToken{Value = 0x0},
                new OperatorToken{Value = Operator.BitOr},
                new SignedIntToken{Value = 0x5},
                new SemiColonToken()
            };

            var actualNode = ParseExpression(expression);

            var actual = actualNode.Value;

            Assert.Equal((TwisterPrimitive)0x5, actual);
        }

        [Fact]
        public void LogAnd()
        {
            IToken[] expression =
            {
                new BoolLiteralToken{Value = true},
                new OperatorToken{Value = Operator.LogAnd},
                new BoolLiteralToken{Value = false},
                new SemiColonToken()
            };

            var actualNode = ParseExpression(expression);

            var actual = actualNode.Value;

            Assert.Equal((TwisterPrimitive)false, actual);
        }

        [Fact]
        public void LogOr()
        {
            IToken[] expression =
            {
                new BoolLiteralToken{Value = true},
                new OperatorToken{Value = Operator.LogOr},
                new BoolLiteralToken{Value = false},
                new SemiColonToken()
            };

            var actualNode = ParseExpression(expression);

            var actual = actualNode.Value;

            Assert.Equal((TwisterPrimitive)true, actual);
        }

        [Fact]
        public void Paren()
        {
            IToken[] expression =
            {   new LeftParenToken(),
                new SignedIntToken {Value = 10},
                new RightParenToken(),
                new SemiColonToken()
            };

            var actualNode = ParseExpression(expression);

            var actual = actualNode.Value;

            Assert.Equal((TwisterPrimitive)10, actual);
        }

        [Fact]
        public void Unary()
        {
            IToken[] expression = {
                new OperatorToken {Value = Operator.Minus},
                new RealToken { Value = 10d },
                new SemiColonToken()
            };

            var actualNode = ParseExpression(expression);

            var actual = actualNode.Value;

            Assert.Equal((TwisterPrimitive)(-10d), actual);
        }
    }
#pragma warning restore CS1701 // Assuming assembly reference matches identity
}
