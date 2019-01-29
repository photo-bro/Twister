using Twister.Compiler.Lexer.Enum;
using Twister.Compiler.Lexer.Interface;
using Twister.Compiler.Lexer.Token;
using Twister.Compiler.Parser.Interface;
using Twister.Compiler.Parser.Primitive;
using Twister.Compiler.Parser.Node;
using System;

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
            //return EvalRecursive();
            return EvalIterative();
        }

        /// <summary>
        /// return_expr ::= ret general_exp
        /// </summary>
        private IValueNode<TwisterPrimitive> ReturnExpression()
        {
            _matcher.Match<IValueToken<Keyword>>(t => t.Value == Keyword.Return);

            return EvalRecursive();
        }

        private IValueNode<TwisterPrimitive> EvalIterative(PrecedenceLevel precedence = PrecedenceLevel.Unary,
            IValueNode<TwisterPrimitive> left = null)
        {
            var op = Operator.None;

            switch (_matcher.Peek.Kind)
            {
                case var k when k.IsPrimitive():
                case TokenKind.Identifier:
                case TokenKind.LeftParen:
                    return EvalIterative(PrecedenceLevel.Multiplicative, Primitive());
                case TokenKind.Operator:
                    op = _matcher.MatchAndGet<IValueToken<Operator>>().Value;
                    break;
                default:
                    return left;
            }


            Func<IValueNode<TwisterPrimitive>> evalNode = 
                () => EvalIterative(++precedence, new BinaryExpressionNode(left, EvalIterative(), op));
            switch (precedence)
            {
                case PrecedenceLevel.Unary:
                    if (op.IsUnaryArithmeticOperator())
                        return EvalIterative(PrecedenceLevel.Multiplicative, UnaryIterative(op));
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
                    if (op == Operator.BitAnd) return evalNode();
                    break;
                case PrecedenceLevel.BitExor:
                    if (op == Operator.BitExOr) return evalNode();
                    break;
                case PrecedenceLevel.BitOr:
                    if (op == Operator.BitOr) return evalNode();
                    break;
                case PrecedenceLevel.LogAnd:
                    if (op == Operator.LogAnd) return evalNode();
                    break;
                case PrecedenceLevel.LogOr:
                    if (op == Operator.LogOr) return evalNode();
                    break;
                default:
                    throw new UnexpectedTokenException("Unexpected arithmetic operator")
                    { UnexpectedToken = _matcher.Current };
            }

            IValueNode<TwisterPrimitive> right = null;
            for (var nextprecedence = PrecedenceLevel.Unary; nextprecedence <= precedence; ++nextprecedence)
            {
                right = EvalIterative(nextprecedence, right);
            }

            return EvalIterative(
                precedence: precedence,
                left: new BinaryExpressionNode(
                    left: left,
                    right: right,
                    op: op));
        }

        private IValueNode<TwisterPrimitive> UnaryIterative(Operator op)
        {
            switch (_matcher.Peek.Kind)
            {
                case var k when k.IsPrimitive():
                case TokenKind.Identifier:
                case TokenKind.LeftParen:
                    return new UnaryExpressionNode(Primitive().Value, op);
                case TokenKind.Operator:
                    var nextOp = _matcher.MatchAndGet<IValueToken<Operator>>().Value;
                    return new UnaryExpressionNode(UnaryIterative(nextOp).Value, op);
            }

            throw new UnexpectedTokenException("Expecting numeric literal or identifer")
            { UnexpectedToken = _matcher.Peek };
        }

        internal enum PrecedenceLevel
        {
            Unary = 0,
            Multiplicative,
            Addition,
            Shift,
            Relational,
            Equality,
            BitAnd,
            BitExor,
            BitOr,
            LogAnd,
            LogOr
        }

        private IValueNode<TwisterPrimitive> EvalRecursive()
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
                    return MultExpr(new MultiplicativeNode(left, EvalRecursive(), op_tok.Value));
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
                    return AddExpr(new AdditiveNode(left, MultExpr(EvalRecursive()), op_tok.Value));
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
                    return ShiftExpr(new ShiftNode(left, AddExpr(MultExpr(EvalRecursive())), op_tok.Value));
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
                    return RelationExpr(new RelationalNode(left, ShiftExpr(AddExpr(MultExpr(EvalRecursive()))), op_tok.Value));
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
                    return EqualExpr(new EqualityNode(left, RelationExpr(ShiftExpr(AddExpr(MultExpr(EvalRecursive())))), op_tok.Value));
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
                return BitAndExpr(new BitAndNode(left, EqualExpr(RelationExpr(ShiftExpr(AddExpr(MultExpr(EvalRecursive())))))));
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
                return BitExOrExpr(new BitExOrNode(left, BitAndExpr(EqualExpr(RelationExpr(ShiftExpr(AddExpr(MultExpr(EvalRecursive()))))))));
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
                return BitOrExpr(new BitOrNode(left, BitExOrExpr(BitAndExpr(EqualExpr(RelationExpr(ShiftExpr(AddExpr(MultExpr(EvalRecursive())))))))));
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
                return LogAndExpr(new LogAndNode(left, BitOrExpr(BitExOrExpr(BitAndExpr(EqualExpr(RelationExpr(ShiftExpr(AddExpr(MultExpr(EvalRecursive()))))))))));
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
                return LogOrExpr(new LogAndNode(left, LogAndExpr(BitOrExpr(BitExOrExpr(BitAndExpr(EqualExpr(RelationExpr(ShiftExpr(AddExpr(MultExpr(EvalRecursive())))))))))));
            }
            return left;
        }

        /// <summary>
        /// (numeric_lit | identifier)
        /// </summary>
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
                        var node = EvalRecursive();
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
