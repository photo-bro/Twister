using System;
using Twister.Compiler.Lexer.Enum;
using Twister.Compiler.Lexer.Interface;
using Twister.Compiler.Lexer.Token;
using Twister.Compiler.Parser.Enum;
using Twister.Compiler.Parser.Interface;
using Twister.Compiler.Parser.Node;
using Twister.Compiler.Parser.Primitive;

namespace Twister.Compiler.Parser.ExpressionParser
{
    public class GenericRDExpressionParser : IExpressionParser
    {
        private ITokenMatcher _matcher;
        private IScopeManager _scopeManager;
        private Func<IValueNode<TwisterPrimitive>> _assignmentCallback;

        public IValueNode<TwisterPrimitive> ParseArithmeticExpression(ITokenMatcher matcher, IScopeManager scopeManager,
            Func<IValueNode<TwisterPrimitive>> assignmentCallback)
        {
            _matcher = matcher;
            _scopeManager = scopeManager;
            _assignmentCallback = assignmentCallback;
            return Evaluate();
        }

        private IValueNode<TwisterPrimitive> Evaluate(PrecedenceLevel precedence = PrecedenceLevel.Unary,
            IValueNode<TwisterPrimitive> left = null)
        {
            var op = Operator.None;
            var peekKind = _matcher.Peek.Kind;
            switch (peekKind)
            {
                case var k when k.IsPrimitive():
                case TokenKind.Identifier:
                case TokenKind.LeftParen:
                    return Evaluate(PrecedenceLevel.Multiplicative, Primitive());
                case TokenKind.Operator:
                    //op = _matcher.MatchAndGet<IValueToken<Operator>>().Value;
                    //op = (_matcher.Peek as IValueToken<Operator>).Value;
                    break;
                default:
                    return left;
            }


            IValueNode<TwisterPrimitive> evalNode()
            {
                _matcher.Match<IValueToken<Operator>>(t => t.Value == op);
                return Evaluate(++precedence, new BinaryExpressionNode(left, Evaluate(), op));
            }

            switch (precedence)
            {
                case PrecedenceLevel.Unary:
                    if (op.IsUnaryArithmeticOperator())
                        return Evaluate(PrecedenceLevel.Multiplicative, Unary(op));
                    break;
                case PrecedenceLevel.Multiplicative:
                    switch (op)
                    {
                        case Operator.Multiplication:
                        case Operator.ForwardSlash:
                        case Operator.Modulo:
                            return evalNode();
                    }
                    break;
                case PrecedenceLevel.Addition:
                    switch (op)
                    {
                        case Operator.Plus:
                        case Operator.Minus:
                            return evalNode();
                    }
                    break;
                case PrecedenceLevel.Shift:
                    switch (op)
                    {
                        case Operator.LeftShift:
                        case Operator.RightShift:
                            return evalNode();
                    }
                    break;
                case PrecedenceLevel.Relational:
                    switch (op)
                    {
                        case Operator.LogGreater:
                        case Operator.LogGreaterEqual:
                        case Operator.LogLess:
                        case Operator.LogLessEqual:
                            return evalNode();
                    }
                    break;
                case PrecedenceLevel.Equality:
                    switch (op)
                    {
                        case Operator.LogEqual:
                        case Operator.LogNotEqual:
                            return evalNode();
                    }
                    break;
                case PrecedenceLevel.BitAnd:
                    switch (op)
                    {
                        case Operator.BitAnd:
                            return evalNode();
                    }
                    break;
                case PrecedenceLevel.BitExor:
                    switch (op)
                    {
                        case Operator.BitExOr:
                            return evalNode();
                    }
                    break;
                case PrecedenceLevel.BitOr:
                    switch (op)
                    {
                        case Operator.BitOr:
                            return evalNode();
                    }
                    break;
                case PrecedenceLevel.LogAnd:
                    switch (op)
                    {
                        case Operator.LogAnd:
                            return evalNode();
                    }
                    break;
                case PrecedenceLevel.LogOr:
                    switch (op)
                    {
                        case Operator.LogOr:
                            return evalNode();
                    }
                    break;
                default:
                    throw new UnexpectedTokenException("Unexpected arithmetic operator")
                    { UnexpectedToken = _matcher.Current };
            }

            IValueNode<TwisterPrimitive> right = null;
            for (var nextprecedence = PrecedenceLevel.Unary; nextprecedence <= PrecedenceLevel.LogOr; ++nextprecedence)
            {
                right = Evaluate(nextprecedence, right);
            }

            return Evaluate(
                precedence: precedence,
                left: new BinaryExpressionNode(
                    left: left,
                    right: right,
                    op: op));
        }

        private IValueNode<TwisterPrimitive> Unary(Operator op)
        {
            switch (_matcher.Peek.Kind)
            {
                case var k when k.IsPrimitive():
                case TokenKind.Identifier:
                case TokenKind.LeftParen:
                    return new UnaryExpressionNode(Primitive().Value, op);
                case TokenKind.Operator:
                    var nextOp = _matcher.MatchAndGet<IValueToken<Operator>>().Value;
                    return new UnaryExpressionNode(Unary(nextOp).Value, op);
            }

            throw new UnexpectedTokenException("Expecting numeric literal or identifer")
            { UnexpectedToken = _matcher.Peek };
        }

        private IValueNode<TwisterPrimitive> Primitive()
        {
            var peek = _matcher.Peek;
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
                        return new SymbolNode(tok.Value, _scopeManager.ActiveScope);
                    }
                case TokenKind.LeftParen:
                    {
                        _matcher.Match<LeftParenToken>();
                        var node = Evaluate();
                        _matcher.Match<IToken>(t => t.Kind == TokenKind.RightParen);
                        return node;
                    }
                case TokenKind.Semicolon:
                    {
                        _matcher.Match<SemiColonToken>();
                        return new PrimitiveNode(null);
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
