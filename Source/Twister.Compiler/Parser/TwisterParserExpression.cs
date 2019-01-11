using Twister.Compiler.Lexer.Enum;
using Twister.Compiler.Lexer.Interface;
using Twister.Compiler.Lexer.Token;
using Twister.Compiler.Parser.Interface;
using Twister.Compiler.Parser.Primitive;
using Twister.Compiler.Parser.Node;
using Twister.Compiler.Parser.Enum;

namespace Twister.Compiler.Parser
{
    public partial class TwisterParser
    {
        /// <summary>
        /// expression ::= (general_exp | return_exp)
        /// </summary>
        private IValueNode<TwisterPrimitive> Expression()
        {
            if (_matcher.IsNext<IValueToken<Keyword>>(t => t.Value == Keyword.Return))
                return ReturnExpression();
            return ArithExpression();
        }

        /// <summary>
        /// return_expr ::= ret general_exp
        /// </summary>
        private IValueNode<TwisterPrimitive> ReturnExpression()
        {
            _matcher.Match<IValueToken<Keyword>>(t => t.Value == Keyword.Return);

            return ArithExpression();
        }

        /// <summary>
        /// expr ::=  primitive | unary  
        /// </summary>
        private IValueNode<TwisterPrimitive> ArithExpression()
        {
            var peek = _matcher.PeekNext();
            switch (peek.Kind)
            {
                case var k when k.IsPrimitive():
                case TokenKind.Identifier:
                case TokenKind.LeftParen:
                    return RestArithExpression(Primitive(peek));
            }

            return UnaryExpression();
        }

        /// <summary>
        /// unary ::= (- | ~ | !) primitive | expression
        /// </summary>
        private IValueNode<TwisterPrimitive> UnaryExpression()
        {
            if (_matcher.IsNext<IValueToken<Operator>>(t => t.Value.IsUnaryArithmeticOperator()))
            {
                var uop = _matcher.MatchAndGet<IValueToken<Operator>>(t => t.Value.IsUnaryArithmeticOperator());
                return new UnaryExpressionNode(ArithExpression(), uop.Value);
            }

            return RestArithExpression(ArithExpression());
        }

        private IValueNode<TwisterPrimitive> RestArithExpression(IValueNode<TwisterPrimitive> left)
        {
            if (_matcher.IsNext<SemiColonToken>())
            {
                _matcher.Match();
                return left;
            }

            var op_tok = _matcher.MatchAndGet<IValueToken<Operator>>();
            switch (op_tok.Value)
            {
                // Precedence descending highest to lowest
                case Operator.Multiplication:
                case Operator.ForwardSlash:
                case Operator.Modulo:
                    return new MultiplicativeNode(left, ArithExpression(), op_tok.Value);
                case Operator.Plus:
                case Operator.Minus:
                    return new AdditiveNode(left, ArithExpression(), op_tok.Value);
                case Operator.LeftShift:
                case Operator.RightShift:
                    return new ShiftNode(left, ArithExpression(), op_tok.Value);
                case Operator.LogLess:
                case Operator.LogLessEqual:
                case Operator.LogGreater:
                case Operator.LogGreaterEqual:
                    return new RelationalNode(left, ArithExpression(), op_tok.Value);
                case Operator.LogEqual:
                case Operator.LogNotEqual:
                    return new EqualityNode(left, ArithExpression(), op_tok.Value);
                case Operator.BitAnd:
                    return new BitAndNode(left, ArithExpression());
                case Operator.BitExOr:
                    return new BitExOrNode(left, ArithExpression());
                case Operator.BitOr:
                    return new BitOrNode(left, ArithExpression());
                case Operator.LogAnd:
                    return new LogAndNode(left, ArithExpression());
                case Operator.LogOr:
                    return new LogOrNode(left, ArithExpression());
                default:
                    throw new UnexpectedTokenException("Expected arithmetic operator")
                    { UnexpectedToken = op_tok };

            }
        }

        /// <summary>
        /// (numeric_lit | identifier)
        /// </summary>
        private IValueNode<TwisterPrimitive> Primitive(IToken peek)
        {
            switch (peek.Kind)
            {
                case TokenKind.BoolLiteral:
                    {
                        var tok = _matcher.MatchAndGet<IValueToken<bool>>();
                        return new PrimitiveNode(tok.Value);
                    }
                case TokenKind.CharLiteral:
                    {
                        var tok = _matcher.MatchAndGet<IValueToken<char>>();
                        return new PrimitiveNode(tok.Value);
                    }
                case TokenKind.SignedInt:
                    {
                        var tok = _matcher.MatchAndGet<IValueToken<long>>();
                        return new PrimitiveNode(tok.Value);
                    }
                case TokenKind.UnsignedInt:
                    {
                        var tok = _matcher.MatchAndGet<IValueToken<ulong>>();
                        return new PrimitiveNode(tok.Value);
                    }
                case TokenKind.Real:
                    {
                        var tok = _matcher.MatchAndGet<IValueToken<double>>();
                        return new PrimitiveNode(tok.Value);
                    }
                case TokenKind.Identifier:
                    {
                        var tok = _matcher.MatchAndGet<IValueToken<string>>();
                        return new PrimitiveNode(null); // TODO : How to handle symbol table...
                    }
                case TokenKind.LeftParen:
                    {
                        _matcher.Match<LeftParenToken>();
                        var node = ArithExpression();
                        _matcher.Match<IToken>(t => t.Kind == TokenKind.RightParen);
                        return node;
                    }
                default:
                    {
                        throw new UnexpectedTokenException("Expecting numeric literal or identifer")
                        { UnexpectedToken = peek };
                    }
            }
        }
    }
}
