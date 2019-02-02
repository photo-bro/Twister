using System;
using Twister.Compiler.Lexer.Enum;
using Twister.Compiler.Lexer.Interface;
using Twister.Compiler.Lexer.Token;
using Twister.Compiler.Parser.Interface;
using Twister.Compiler.Parser.Primitive;

namespace Twister.Compiler.Parser
{
    public partial class TwisterParser
    {
        private IValueNode<TwisterPrimitive> Assignment()
        {
            throw new NotImplementedException(); // TODO
        }

        /// <summary>
        /// assignment ::= type identifier assign (arith_exp | func_call)
        /// </summary>
        private IValueNode<TwisterPrimitive> Assign(IValueNode<TwisterPrimitive> right)
        {
            var type = _matcher.MatchAndGet<IValueToken<Keyword>>(t => t.Value.IsTypeKeyword());
            var id = _matcher.MatchAndGet<IValueToken<string>>();
            _matcher.Match<AssignToken>();

            var value = right.Value;
            var expectedType = type.Value.ToPrimitiveType();
            // TODO : Check primitive precedence and allow implicit coercion
            if (expectedType != value.Type)
                throw new InvalidAssignmentException("Cannot mix types")
                {
                    LeftType = $"{expectedType}",
                    RightType = $"{value.Type}"
                };

            return null;//new SymbolNode(SymbolKind.Variable, id.Value, right.Value); TODO
        }
    }
}
