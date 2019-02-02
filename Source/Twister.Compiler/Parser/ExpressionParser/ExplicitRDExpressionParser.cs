using System;
using Twister.Compiler.Lexer.Enum;
using Twister.Compiler.Lexer.Interface;
using Twister.Compiler.Lexer.Token;
using Twister.Compiler.Parser.Interface;
using Twister.Compiler.Parser.Node;
using Twister.Compiler.Parser.Primitive;

namespace Twister.Compiler.Parser.ExpressionParser
{
    public class ExplicitRDExpressionParser : IExpressionParser
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

        private IValueNode<TwisterPrimitive> Evaluate()
        {
            switch (_matcher.Peek.Kind)
            {
                case var k when k.IsPrimitive():
                case TokenKind.Identifier:
                case TokenKind.LeftParen:
                    return LogOrExpr(
                            LogAndExpr(
                            BitOrExpr(
                            BitExOrExpr(
                            BitAndExpr(
                            EqualExpr(
                            RelationExpr(
                            ShiftExpr(
                            AddExpr(
                            MultExpr(
                            Primitive()
                            ))))))))));

            }

            return LogOrExpr(
                    LogAndExpr(
                    BitOrExpr(
                    BitExOrExpr(
                    BitAndExpr(
                    EqualExpr(
                    RelationExpr(
                    ShiftExpr(
                    AddExpr(
                    MultExpr(
                    UnaryExpression()
                    ))))))))));
        }

        private IValueNode<TwisterPrimitive> UnaryExpression()
        {
            var op_tok = _matcher.Peek as IValueToken<Operator>;

            if (op_tok.Value.IsUnaryArithmeticOperator())
            {
                var uop = _matcher.MatchAndGet<IValueToken<Operator>>(t => t.Value.IsUnaryArithmeticOperator());

                switch (_matcher.Peek.Kind)
                {
                    case var k when k.IsPrimitive():
                    case TokenKind.Identifier:
                    case TokenKind.LeftParen:
                        return new UnaryExpressionNode(Primitive().Value, uop.Value);
                }

                if (_matcher.Peek is IValueToken<Operator>)
                    return new UnaryExpressionNode(UnaryExpression().Value, uop.Value);
            }

            throw new UnexpectedTokenException("Expecting numeric literal or identifer")
            { UnexpectedToken = op_tok };
        }

        private IValueNode<TwisterPrimitive> MultExpr(IValueNode<TwisterPrimitive> left)
        {
            var op_tok = _matcher.Peek as IValueToken<Operator>;
            if (op_tok == null)
                return left;

            switch (op_tok.Value)
            {
                case Operator.Multiplication:
                case Operator.ForwardSlash:
                case Operator.Modulo:
                    _matcher.Match();
                    return MultExpr(new MultiplicativeNode(left, Evaluate(), op_tok.Value));
            }

            return left;
        }

        private IValueNode<TwisterPrimitive> AddExpr(IValueNode<TwisterPrimitive> left)
        {
            var op_tok = _matcher.Peek as IValueToken<Operator>;
            if (op_tok == null)
                return left;

            switch (op_tok.Value)
            {
                case Operator.Plus:
                case Operator.Minus:
                    _matcher.Match();
                    return AddExpr(new AdditiveNode(left, MultExpr(Evaluate()), op_tok.Value));
            }

            return left;
        }

        private IValueNode<TwisterPrimitive> ShiftExpr(IValueNode<TwisterPrimitive> left)
        {
            var op_tok = _matcher.Peek as IValueToken<Operator>;
            if (op_tok == null)
                return left;

            switch (op_tok.Value)
            {
                case Operator.LeftShift:
                case Operator.RightShift:
                    _matcher.Match();
                    return ShiftExpr(new ShiftNode(left, AddExpr(MultExpr(Evaluate())), op_tok.Value));
            }

            return left;
        }

        private IValueNode<TwisterPrimitive> RelationExpr(IValueNode<TwisterPrimitive> left)
        {
            var op_tok = _matcher.Peek as IValueToken<Operator>;
            if (op_tok == null)
                return left;

            switch (op_tok.Value)
            {
                case Operator.LogLess:
                case Operator.LogLessEqual:
                case Operator.LogGreater:
                case Operator.LogGreaterEqual:
                    _matcher.Match();
                    return RelationExpr(new RelationalNode(left, ShiftExpr(AddExpr(MultExpr(Evaluate()))), op_tok.Value));
            }

            return left;
        }

        private IValueNode<TwisterPrimitive> EqualExpr(IValueNode<TwisterPrimitive> left)
        {
            var op_tok = _matcher.Peek as IValueToken<Operator>;
            if (op_tok == null)
                return left;

            switch (op_tok.Value)
            {
                case Operator.LogEqual:
                case Operator.LogNotEqual:
                    _matcher.Match();
                    return EqualExpr(new EqualityNode(left, RelationExpr(ShiftExpr(AddExpr(MultExpr(Evaluate())))), op_tok.Value));
            }

            return left;
        }

        private IValueNode<TwisterPrimitive> BitAndExpr(IValueNode<TwisterPrimitive> left)
        {
            var op_tok = _matcher.Peek as IValueToken<Operator>;
            if (op_tok == null)
                return left;

            if (op_tok.Value == Operator.BitAnd)
            {
                _matcher.Match();
                return BitAndExpr(new BitAndNode(left, EqualExpr(RelationExpr(ShiftExpr(AddExpr(MultExpr(Evaluate())))))));
            }
            return left;
        }

        private IValueNode<TwisterPrimitive> BitExOrExpr(IValueNode<TwisterPrimitive> left)
        {
            var op_tok = _matcher.Peek as IValueToken<Operator>;
            if (op_tok == null)
                return left;

            if (op_tok.Value == Operator.BitExOr)
            {
                _matcher.Match();
                return BitExOrExpr(new BitExOrNode(left, BitAndExpr(EqualExpr(RelationExpr(ShiftExpr(AddExpr(MultExpr(Evaluate()))))))));
            }
            return left;
        }

        private IValueNode<TwisterPrimitive> BitOrExpr(IValueNode<TwisterPrimitive> left)
        {
            var op_tok = _matcher.Peek as IValueToken<Operator>;
            if (op_tok == null)
                return left;

            if (op_tok.Value == Operator.BitOr)
            {
                _matcher.Match();
                return BitOrExpr(new BitOrNode(left, BitExOrExpr(BitAndExpr(EqualExpr(RelationExpr(ShiftExpr(AddExpr(MultExpr(Evaluate())))))))));
            }
            return left;
        }

        private IValueNode<TwisterPrimitive> LogAndExpr(IValueNode<TwisterPrimitive> left)
        {
            var op_tok = _matcher.Peek as IValueToken<Operator>;
            if (op_tok == null)
                return left;

            if (op_tok.Value == Operator.LogAnd)
            {
                _matcher.Match();
                return LogAndExpr(new LogAndNode(left, BitOrExpr(BitExOrExpr(BitAndExpr(EqualExpr(RelationExpr(ShiftExpr(AddExpr(MultExpr(Evaluate()))))))))));
            }
            return left;
        }

        private IValueNode<TwisterPrimitive> LogOrExpr(IValueNode<TwisterPrimitive> left)
        {
            var op_tok = _matcher.Peek as IValueToken<Operator>;
            if (op_tok == null)
                return left;

            if (op_tok.Value == Operator.LogAnd)
            {
                _matcher.Match();
                return LogOrExpr(new LogAndNode(left, LogAndExpr(BitOrExpr(BitExOrExpr(BitAndExpr(EqualExpr(RelationExpr(ShiftExpr(AddExpr(MultExpr(Evaluate())))))))))));
            }
            return left;
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
