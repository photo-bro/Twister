﻿using System.Collections.Generic;
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
    public class ComplexExpressionTest
    {
        ITwisterParser _parser;

        private void SetupParser()
        {
            _parser = new TwisterParser(
                createTokenMatcher: t => new TokenMatcher(new Scanner<IToken>(t, new EmptyToken())),
                expressionParser: new GenericRDExpressionParser());
        }

        private IEnumerable<IToken> GetTokens(string expression) =>
            new SourceLexer((s) => new TextSourceScanner(s))
                .Tokenize(expression, LexerFlag.None);

        [Theory]
        [InlineData("(1 + 2 * ( 1 << 4) / (1024 >> 8)) % 4", 1)]
        public void Integer(string expressionStr, int expected)
        {
            SetupParser();
            var expression = GetTokens(expressionStr);

            var actualNode = _parser.ParseExpression(expression) as IValueNode<TwisterPrimitive>;

            var actual = actualNode.Value;

            Assert.Equal((TwisterPrimitive)expected, actual);
        }

        [Theory]
        [InlineData("((1 + 2 * ( 1 << 4) / (1024 >> 8)) % 4) > (16 ^ 16 | (4 & 32)) != false", true)]
        public void Bool(string expressionStr, bool expected)
        {
            SetupParser();
            var expression = GetTokens(expressionStr);

            var actualNode = _parser.ParseExpression(expression) as IValueNode<TwisterPrimitive>;

            var actual = actualNode.Value;

            Assert.Equal((TwisterPrimitive)expected, actual);
        }
    }
#pragma warning restore CS1701 // Assuming assembly reference matches identity
}
