using Twister.Compiler.Lexer.Enum;
using Twister.Compiler.Lexer.Interface;
using Twister.Compiler.Parser.Interface;
using Twister.Compiler.Parser.Primitive;

namespace Twister.Compiler.Parser
{
    public partial class TwisterParser
    {
        private IValueNode<TwisterPrimitive> Expression()
        {
            if (_matcher.IsNext<IValueToken<Keyword>>(t => t.Value == Keyword.Return))
                ReturnExpression();

            return _expressionParser.ParseArithmeticExpression(_matcher, _scopeManager, Assignment);
        }

        private void ReturnExpression()
        {
            _matcher.Match<IValueToken<Keyword>>(t => t.Value == Keyword.Return);
        }
    }
}
