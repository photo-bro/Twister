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
            return ArithmeticExpression();
        }

        /// <summary>
        /// return_expr ::= ret general_exp
        /// </summary>
        private IValueNode<TwisterPrimitive> ReturnExpression()
        {
            _matcher.Match<IValueToken<Keyword>>(t => t.Value == Keyword.Return);

            return ArithmeticExpression();
        }

        /// <summary>
        /// arith_exp ::= {unary_op} (primitive | arith_exp arith_rest_exp)
        /// </summary>
        private IValueNode<TwisterPrimitive> ArithmeticExpression()
        {
            if (_matcher.IsNext<LeftParenToken>())
                _matcher.Match();

            IValueNode<TwisterPrimitive> left;
            if (_matcher.IsNext<IValueToken<Operator>>(t => t.Value.IsUnaryArithmeticOperator()))
            {
                var uop = _matcher.MatchAndGet<IValueToken<Operator>>(t => t.Value.IsUnaryArithmeticOperator());
                left = new UnaryExpressionNode(ArithmeticExpression(), uop.Value);
            }
            else
            {
                var peek = _matcher.PeekNext();
                left = peek.Kind.IsValueToken() || peek.Kind == TokenKind.Identifier
                    ? Primitive()
                    : ArithmeticExpression();
            }
            return ArithmeticRestExpression(left);
        }

        /// <summary>
        /// arith_rest_exp ::= {arith_op primitive arith_rest_exp}
        /// </summary>
        private IExpressionNode<TwisterPrimitive, TwisterPrimitive> ArithmeticRestExpression
            (IValueNode<TwisterPrimitive> left)
        {
            if (_matcher.IsNext<IValueToken<Operator>>(t => t.Value.IsBinaryArithmeticOperator()))
            {
                var op = _matcher.MatchAndGet<IValueToken<Operator>>(t => t.Value.IsBinaryArithmeticOperator());

               
            }
            // TODO
            return null; //left;
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
                default:
                    {
                        throw new UnexpectedTokenException("Expecting numeric literal or identifer")
                        { UnexpectedToken = peek };
                    }
            }
        }
    }
}
