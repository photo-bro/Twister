using System;
using System.Collections.Generic;
using Twister.Compiler.Lexer.Interface;
using Twister.Compiler.Lexer.Token;
using Twister.Compiler.Parser.Interface;
using Twister.Compiler.Parser;
using Xunit;
using Twister.Compiler.Lexer.Enum;
using Twister.Compiler.Parser.Node;
using Twister.Compiler.Common;
using Twister.Compiler.Parser.Primitive;

namespace Twister.Test.UnitTest.Parser.Expression
{
#pragma warning disable CS1701 // Assuming assembly reference matches identity

    public class SimpleExpressionTest
    {
        ITwisterParser _parser;

        private void SetupParser()
        {
            _parser = new TwisterParser((tokens) =>
                new TokenMatcher(
                    new Scanner<IToken>(tokens, new EmptyToken())
                ));
        }

        [Fact]
        public void SimpleArith_Addition()
        {
            SetupParser();
            IToken[] expression =
            {
                new SignedIntToken{Value = 1},
                new OperatorToken{Value = Operator.Plus},
                new SignedIntToken{Value = 2},
                new SemiColonToken()
            };

            var expected = new PrimitiveNode(2);

            var actualNode = _parser.ParseExpression(expression) as IValueNode<TwisterPrimitive>;

            var actual = actualNode.Value;

            Assert.Equal((TwisterPrimitive)3, actual);
        }

        [Fact]
        public void SimpleArith_Mult()
        {
            SetupParser();
            IToken[] expression =
            {
                new SignedIntToken{Value = 9},
                new OperatorToken{Value = Operator.Multiplication},
                new SignedIntToken{Value = 10},
                new SemiColonToken()
            };

            var expected = new PrimitiveNode(2);

            var actualNode = _parser.ParseExpression(expression) as IValueNode<TwisterPrimitive>;

            var actual = actualNode.Value;

            Assert.Equal((TwisterPrimitive)90, actual);
        }

        [Fact]
        public void SimpleArith_Shift()
        {
            SetupParser();
            IToken[] expression =
            {
                new SignedIntToken{Value = 1},
                new OperatorToken{Value = Operator.LeftShift},
                new SignedIntToken{Value = 4 },
                new SemiColonToken()
            };

            var expected = new PrimitiveNode(2);

            var actualNode = _parser.ParseExpression(expression) as IValueNode<TwisterPrimitive>;

            var actual = actualNode.Value;

            Assert.Equal((TwisterPrimitive)(1 << 4), actual);
        }

        [Fact]
        public void SimpleArith_Relation()
        {
            SetupParser();
            IToken[] expression =
            {
                new SignedIntToken{Value = 9},
                new OperatorToken{Value = Operator.LogGreater},
                new SignedIntToken{Value = 10},
                new SemiColonToken()
            };

            var expected = new PrimitiveNode(2);

            var actualNode = _parser.ParseExpression(expression) as IValueNode<TwisterPrimitive>;

            var actual = actualNode.Value;

            Assert.Equal((TwisterPrimitive)true, actual);
        }

        [Fact]
        public void SimpleArith__Eq()
        {
            SetupParser();
            IToken[] expression =
            {
                new SignedIntToken{Value = 10},
                new OperatorToken{Value = Operator.LogEqual},
                new SignedIntToken{Value = 10},
                new SemiColonToken()
            };

            var expected = new PrimitiveNode(2);

            var actualNode = _parser.ParseExpression(expression) as IValueNode<TwisterPrimitive>;

            var actual = actualNode.Value;

            Assert.Equal((TwisterPrimitive)true, actual);
        }

        [Fact]
        public void SimpleArith_BitAnd()
        {
            SetupParser();
            IToken[] expression =
            {
                new SignedIntToken{Value = 0xFF},
                new OperatorToken{Value = Operator.BitAnd},
                new SignedIntToken{Value = 0X01},
                new SemiColonToken()
            };

            var expected = new PrimitiveNode(2);

            var actualNode = _parser.ParseExpression(expression) as IValueNode<TwisterPrimitive>;

            var actual = actualNode.Value;

            Assert.Equal((TwisterPrimitive)(0xFF & 0x01), actual);
        }

        [Fact]
        public void SimpleArith_BitExOr()
        {
            SetupParser();
            IToken[] expression =
            {
                new SignedIntToken{Value = 0xFF},
                new OperatorToken{Value = Operator.BitExOr},
                new SignedIntToken{Value = 0xFF},
                new SemiColonToken()
            };

            var expected = new PrimitiveNode(2);

            var actualNode = _parser.ParseExpression(expression) as IValueNode<TwisterPrimitive>;

            var actual = actualNode.Value;

            Assert.Equal((TwisterPrimitive)0x0, actual);
        }

        [Fact]
        public void SimpleArith_BitOr()
        {
            SetupParser();
            IToken[] expression =
            {
                new SignedIntToken{Value = 0x0},
                new OperatorToken{Value = Operator.BitOr},
                new SignedIntToken{Value = 0x5},
                new SemiColonToken()
            };

            var expected = new PrimitiveNode(2);

            var actualNode = _parser.ParseExpression(expression) as IValueNode<TwisterPrimitive>;

            var actual = actualNode.Value;

            Assert.Equal((TwisterPrimitive)0x5, actual);
        }

        [Fact]
        public void SimpleArith_LogAnd()
        {
            SetupParser();
            IToken[] expression =
            {
                new BoolLiteralToken{Value = true},
                new OperatorToken{Value = Operator.LogAnd},
                new BoolLiteralToken{Value = false},
                new SemiColonToken()
            };

            var expected = new PrimitiveNode(2);

            var actualNode = _parser.ParseExpression(expression) as IValueNode<TwisterPrimitive>;

            var actual = actualNode.Value;

            Assert.Equal((TwisterPrimitive)false, actual);
        }

        [Fact]
        public void SimpleArith_LogOr()
        {
            SetupParser();
            IToken[] expression =
            {
                new BoolLiteralToken{Value = true},
                new OperatorToken{Value = Operator.LogOr},
                new BoolLiteralToken{Value = false},
                new SemiColonToken()
            };

            var expected = new PrimitiveNode(2);

            var actualNode = _parser.ParseExpression(expression) as IValueNode<TwisterPrimitive>;

            var actual = actualNode.Value;

            Assert.Equal((TwisterPrimitive)90, actual);
        }
    }
#pragma warning restore CS1701 // Assuming assembly reference matches identity

}
