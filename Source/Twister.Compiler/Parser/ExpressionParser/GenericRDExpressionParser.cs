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

        private IValueNode<TwisterPrimitive> Evaluate(PrecedenceLevel precedence = PrecedenceLevel.LogOr)
        {
            var nextPrecedence = (PrecedenceLevel)((int)precedence - 1);
            var left = EvaluateNext(precedence, nextPrecedence);

            var nextOperator = (_matcher.Peek as IValueToken<Operator>)?.Value;
            while (nextOperator.HasValue && IsOperatorInPrecedence(nextOperator.Value, precedence))
            {
                var op = _matcher.MatchAndGet<IValueToken<Operator>>().Value;

                var right = precedence <= PrecedenceLevel.Multiplicative
                    ? EvaluateUnary()
                    : Evaluate(nextPrecedence);

                left = new BinaryExpressionNode(left, right, op);
                nextOperator = (_matcher.Peek as IValueToken<Operator>)?.Value;
            }

            return left;
        }

        private IValueNode<TwisterPrimitive> EvaluateNext(PrecedenceLevel precedence, PrecedenceLevel nextPrecedence) =>
            precedence <= PrecedenceLevel.Multiplicative
                ? EvaluateUnary()
                : Evaluate(nextPrecedence);

        private IValueNode<TwisterPrimitive> EvaluateUnary()
        {
            var op = (_matcher.Peek as IValueToken<Operator>)?.Value;
            if (op.HasValue && op.Value.IsUnaryArithmeticOperator())
            {
                _matcher.Match();
                var right = EvaluateUnary();
                return new UnaryExpressionNode(right, op.Value);
            }

            return Primitive();
        }

        private bool IsOperatorInPrecedence(Operator op, PrecedenceLevel precedence)
        {
            var isPrecedenceOperator = false;
            switch (precedence)
            {
                case PrecedenceLevel.Unary:
                    isPrecedenceOperator |= op.IsUnaryArithmeticOperator();
                    break;
                case PrecedenceLevel.Multiplicative:
                    switch (op)
                    {
                        case Operator.Multiplication:
                        case Operator.ForwardSlash:
                        case Operator.Modulo:
                            isPrecedenceOperator = true;
                            break;
                    }
                    break;
                case PrecedenceLevel.Addition:
                    switch (op)
                    {
                        case Operator.Plus:
                        case Operator.Minus:
                            isPrecedenceOperator = true;
                            break;
                    }
                    break;
                case PrecedenceLevel.Shift:
                    switch (op)
                    {
                        case Operator.LeftShift:
                        case Operator.RightShift:
                            isPrecedenceOperator = true;
                            break;
                    }
                    break;
                case PrecedenceLevel.Relational:
                    switch (op)
                    {
                        case Operator.LogGreater:
                        case Operator.LogGreaterEqual:
                        case Operator.LogLess:
                        case Operator.LogLessEqual:
                            isPrecedenceOperator = true;
                            break;
                    }
                    break;
                case PrecedenceLevel.Equality:
                    switch (op)
                    {
                        case Operator.LogEqual:
                        case Operator.LogNotEqual:
                            isPrecedenceOperator = true;
                            break;
                    }
                    break;
                case PrecedenceLevel.BitAnd:
                    switch (op)
                    {
                        case Operator.BitAnd:
                            isPrecedenceOperator = true;
                            break;
                    }
                    break;
                case PrecedenceLevel.BitExor:
                    switch (op)
                    {
                        case Operator.BitExOr:
                            isPrecedenceOperator = true;
                            break;
                    }
                    break;
                case PrecedenceLevel.BitOr:
                    switch (op)
                    {
                        case Operator.BitOr:
                            isPrecedenceOperator = true;
                            break;
                    }
                    break;
                case PrecedenceLevel.LogAnd:
                    switch (op)
                    {
                        case Operator.LogAnd:
                            isPrecedenceOperator = true;
                            break;
                    }
                    break;
                case PrecedenceLevel.LogOr:
                    switch (op)
                    {
                        case Operator.LogOr:
                            isPrecedenceOperator = true;
                            break;
                    }
                    break;
                default:
                    throw new UnexpectedTokenException("Unexpected arithmetic operator")
                    { UnexpectedToken = _matcher.Current };
            }

            return isPrecedenceOperator;
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
