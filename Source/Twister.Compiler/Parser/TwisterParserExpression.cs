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
            var peek = _matcher.Peek;
            switch (peek.Kind)
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

            return UnaryExpression(Primitive());
        }

        /// <summary>
        /// unary ::= (- | ~ | !) primitive | expression
        /// </summary>
        private IValueNode<TwisterPrimitive> UnaryExpression(IValueNode<TwisterPrimitive> right)
        {
            if (_matcher.IsNext<IValueToken<Operator>>(t => t.Value.IsUnaryArithmeticOperator()))
            {
                var uop = _matcher.MatchAndGet<IValueToken<Operator>>(t => t.Value.IsUnaryArithmeticOperator());
                return new UnaryExpressionNode(ArithExpression(), uop.Value);
            }

            return right;
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
                    return MultExpr(new MultiplicativeNode(left, ArithExpression(), op_tok.Value));
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
                    return AddExpr(new AdditiveNode(left, MultExpr(ArithExpression()), op_tok.Value));
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
                    return ShiftExpr(new ShiftNode(left, AddExpr(MultExpr(ArithExpression())), op_tok.Value));
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
                    return RelationExpr(new RelationalNode(left, ShiftExpr(AddExpr(MultExpr(ArithExpression()))), op_tok.Value));
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
                    return EqualExpr(new EqualityNode(left, RelationExpr(ShiftExpr(AddExpr(MultExpr(ArithExpression())))), op_tok.Value));
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
                return BitAndExpr(new BitAndNode(left, EqualExpr(RelationExpr(ShiftExpr(AddExpr(MultExpr(ArithExpression())))))));
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
                return BitExOrExpr(new BitExOrNode(left, BitAndExpr(EqualExpr(RelationExpr(ShiftExpr(AddExpr(MultExpr(ArithExpression()))))))));
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
                return BitOrExpr(new BitOrNode(left, BitExOrExpr(BitAndExpr(EqualExpr(RelationExpr(ShiftExpr(AddExpr(MultExpr(ArithExpression())))))))));
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
                return LogAndExpr(new LogAndNode(left, BitOrExpr(BitExOrExpr(BitAndExpr(EqualExpr(RelationExpr(ShiftExpr(AddExpr(MultExpr(ArithExpression()))))))))));
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
                return LogOrExpr(new LogAndNode(left, LogAndExpr(BitOrExpr(BitExOrExpr(BitAndExpr(EqualExpr(RelationExpr(ShiftExpr(AddExpr(MultExpr(ArithExpression())))))))))));
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
                        return new PrimitiveNode(null); // TODO : How to handle symbols...?
                    }
                case TokenKind.LeftParen:
                    {
                        _matcher.Match<LeftParenToken>();
                        var node = ArithExpression();
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
