using System;
using Twister.Compiler.Parser.Node;
using Twister.Compiler.Parser.Interface;
using Twister.Compiler.Parser.Primitive;
using Twister.Compiler.Lexer.Interface;
using Twister.Compiler.Lexer.Enum;
using Twister.Compiler.Lexer.Token;
using Twister.Compiler.Parser.Enum;

namespace Twister.Compiler.Parser
{
    public partial class TwisterParser
    {
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

            return new SymbolNode(SymbolKind.Variable, id.Value, right.Value); 
        }
    }
}
