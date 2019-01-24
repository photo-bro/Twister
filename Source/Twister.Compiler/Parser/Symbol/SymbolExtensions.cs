using Twister.Compiler.Parser.Interface;
using Twister.Compiler.Parser.Primitive;

namespace Twister.Compiler.Parser.Symbol
{
    public static class SymbolExtensions
    {
        public static TwisterPrimitive GetPrimitiveValue(this ISymbol instance)
        {

            switch (instance.DataType)
            {
                case Enum.TwisterType.Bool:
                    return (bool)instance.Value;
                case Enum.TwisterType.Char:
                    return (char)instance.Value;
                case Enum.TwisterType.Int:
                    return (int)instance.Value;
                case Enum.TwisterType.UInt:
                    return (uint)instance.Value;
                case Enum.TwisterType.Float:
                    return (double)instance.Value;
                case Enum.TwisterType.Str:
                    return (string)instance.Value;
            }

            throw new InvalidTypeException("Symbol is not of a primitive type")
            {
                InvalidType = $"{instance.DataType}"
            };
        }
    }
}