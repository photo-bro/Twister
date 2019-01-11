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

namespace Twister.Test.UnitTest.Parser
{
#pragma warning disable CS1701 // Assuming assembly reference matches identity

    public class ExpressionParserTest
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
        public void Test_SimpleArithmeticExpression()
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

            var actualNode = _parser.Parse(expression) as  IValueNode<TwisterPrimitive>;

            var actual = actualNode.Value;

            Assert.Equal((TwisterPrimitive)3, actual);
        }




    }
#pragma warning restore CS1701 // Assuming assembly reference matches identity

}
