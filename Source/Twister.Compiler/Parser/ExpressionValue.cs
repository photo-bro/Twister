using Twister.Compiler.Parser.Primitive;

namespace Twister.Compiler.Parser
{
    public class ExpressionValue
    {
        public TwisterPrimitive Primitive { get; set; }

        // TODO : Add complex types (structs and arrays)

        public bool IsPrimitive => Primitive.GetValue() != null;
    }
}
