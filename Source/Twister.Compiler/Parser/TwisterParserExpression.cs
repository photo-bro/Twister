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
            return UnaryExpression();
        }

        /// <summary>
        /// return_expr ::= ret general_exp
        /// </summary>
        private IValueNode<TwisterPrimitive> ReturnExpression()
        {
            _matcher.Match<IValueToken<Keyword>>(t => t.Value == Keyword.Return);

            return UnaryExpression();
        }

        /// <summary>
        /// expr ::=  primitive | unary  
        /// </summary>
        private IValueNode<TwisterPrimitive> ArithExpression()
        {
            var peek = _matcher.PeekNext();
            switch (peek.Kind)
            {
                case var k when k.IsValueToken():
                case TokenKind.Identifier:
                case TokenKind.LeftParen:
                    return Primitive();
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
                return new UnaryExpressionNode(UnaryExpression(), uop.Value);
            }

            if (_matcher.IsNext<IToken>(t => t.Kind.IsValueToken()) ||
                _matcher.IsNext<IToken>(t => t.Kind == TokenKind.LeftParen))
                return Primitive();

            return MultExpr(UnaryExpression());
        }

        private IValueNode<TwisterPrimitive> MultExpr(IValueNode<TwisterPrimitive> left)
        {
            var op_tok = _matcher.MatchAndGet<IValueToken<Operator>>();
            switch (op_tok.Value)
            {
                case Operator.Multiplication:
                case Operator.ForwardSlash:
                case Operator.Modulo:
                    return new MultiplicativeNode(left, ArithExpression(), op_tok.Value);
            }

            return AddExpr(left);
        }

        private IValueNode<TwisterPrimitive> AddExpr(IValueNode<TwisterPrimitive> left)
        {
            var op_tok = _matcher.MatchAndGet<IValueToken<Operator>>();
            switch (op_tok.Value)
            {
                case Operator.Plus:
                case Operator.Minus:
                    return new AdditiveNode(left, ArithExpression(), op_tok.Value);
            }

            return ShiftExpr(left);
        }

        private IValueNode<TwisterPrimitive> ShiftExpr(IValueNode<TwisterPrimitive> left)
        {
            var op_tok = _matcher.MatchAndGet<IValueToken<Operator>>();
            switch (op_tok.Value)
            {
                case Operator.LeftShift:
                case Operator.RightShift:
                    return new ShiftNode(left, ArithExpression(), op_tok.Value);
            }

            return RelationExpr(left);
        }

        private IValueNode<TwisterPrimitive> RelationExpr(IValueNode<TwisterPrimitive> left)
        {
            var op_tok = _matcher.MatchAndGet<IValueToken<Operator>>();
            switch (op_tok.Value)
            {
                case Operator.LogLess:
                case Operator.LogLessEqual:
                case Operator.LogGreater:
                case Operator.LogGreaterEqual:
                    return new RelationalNode(left, ArithExpression(), op_tok.Value);
            }

            return EqualExpr(left);
        }

        private IValueNode<TwisterPrimitive> EqualExpr(IValueNode<TwisterPrimitive> left)
        {
            var op_tok = _matcher.MatchAndGet<IValueToken<Operator>>();
            switch (op_tok.Value)
            {
                case Operator.LogEqual:
                case Operator.LogNotEqual:
                    return new EqualityNode(left, ArithExpression(), op_tok.Value);
            }

            return BitAndExpr(left);
        }

        private IValueNode<TwisterPrimitive> BitAndExpr(IValueNode<TwisterPrimitive> left)
        {
            var op_tok = _matcher.MatchAndGet<IValueToken<Operator>>();

            return op_tok.Value == Operator.BitAnd
                ? new BitAndNode(left, ArithExpression())
                : BitExOrExpr(left);
        }

        private IValueNode<TwisterPrimitive> BitExOrExpr(IValueNode<TwisterPrimitive> left)
        {
            var op_tok = _matcher.MatchAndGet<IValueToken<Operator>>();

            return op_tok.Value == Operator.BitExOr
                ? new BitExOrNode(left, ArithExpression())
                : BitOrExpr(left);
        }

        private IValueNode<TwisterPrimitive> BitOrExpr(IValueNode<TwisterPrimitive> left)
        {
            var op_tok = _matcher.MatchAndGet<IValueToken<Operator>>();

            return op_tok.Value == Operator.BitOr
                ? new BitOrNode(left, ArithExpression())
                : LogOrExpr(left);
        }

        private IValueNode<TwisterPrimitive> LogAndExpr(IValueNode<TwisterPrimitive> left)
        {
            var op_tok = _matcher.MatchAndGet<IValueToken<Operator>>();

            return op_tok.Value == Operator.LogAnd
                ? new LogAndNode(left, ArithExpression())
                : LogOrExpr(left);
        }

        private IValueNode<TwisterPrimitive> LogOrExpr(IValueNode<TwisterPrimitive> left)
        {
            var peek = _matcher.PeekNext();

            if (peek?.Kind == TokenKind.Operator)
            {
                var op_tok = _matcher.MatchAndGet<IValueToken<Operator>>(t => t.Value == Operator.LogOr);
                return new LogOrNode(left, ArithExpression());
            }

            throw new UnexpectedTokenException("Expected arithmetic operator")
            { UnexpectedToken = peek };
        }

        /// <summary>
        /// (numeric_lit | identifier)
        /// </summary>
        private IValueNode<TwisterPrimitive> Primitive()
        {
            var peek = _matcher.PeekNext();
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
                        var tok = _matcher.MatchAndGet<IValueToken<int>>();
                        return new PrimitiveNode(tok.Value);
                    }
                case TokenKind.UnsignedInt:
                    {
                        var tok = _matcher.MatchAndGet<IValueToken<uint>>();
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
                        return new SymbolNode(SymbolKind.Variable, tok.Value, 0); // TODO : How to handle symbol table...
                    }
                case TokenKind.LeftParen:
                    {
                        _matcher.Match();
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
