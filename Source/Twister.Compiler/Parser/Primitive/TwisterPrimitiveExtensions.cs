using Twister.Compiler.Lexer.Enum;
using Twister.Compiler.Lexer.Token;
using Twister.Compiler.Parser.Enum;

namespace Twister.Compiler.Parser.Primitive
{
    public static class TwisterPrimitiveExtensions
    {
        /// <summary>
        /// Returns value for Twister equivalent type of T, returns null if instance
        /// has a differing PrimitiveType
        /// </summary>
        public static T? GetValueOrNull<T>(this TwisterPrimitive instance) where T : struct => GetValueOrNull<T>(instance);

        /// <summary>
        /// Returns value for Twister equivalent type of T, returns default(T) if instance
        /// has a differing PrimitiveType
        /// </summary>
        public static T GetValueOrDefault<T>(this TwisterPrimitive instance) => GetValueOrDefault<T>(instance);

        /// <summary>
        /// Return the value of the <see cref="TwisterPrimitive"/> as a boxed object with the 
        /// <see cref="PrimitiveType"/> as an out parameter
        /// </summary>
        public static object GetValue(this TwisterPrimitive instance, out PrimitiveType type) => GetValue(instance, out type);

        /// <summary>
        /// Return the value of the <see cref="TwisterPrimitive"/> as a boxed object
        /// </summary>
        public static object GetValue(this TwisterPrimitive instance) => GetValue(instance, out var type);

        public static bool IsNumeric(this TwisterPrimitive instance) => instance.Type == PrimitiveType.Int ||
                                                                        instance.Type == PrimitiveType.UInt ||
                                                                        instance.Type == PrimitiveType.Float ||
                                                                        instance.Type == PrimitiveType.Char;

        public static PrimitiveType ToPrimitiveType(this Keyword keyword)
        {
            switch (keyword)
            {
                case Keyword.Bool:
                    return PrimitiveType.Bool;
                case Keyword.Char:
                    return PrimitiveType.Char;
                case Keyword.Int:
                    return PrimitiveType.Int;
                case Keyword.UInt:
                    return PrimitiveType.UInt;
                case Keyword.Float:
                    return PrimitiveType.Float;
                case Keyword.Str:
                    return PrimitiveType.Str;
                default:
                    throw new InvalidTypeException("Specified keyword is not a valid type") { InvalidType = $"{keyword}" };
            }
        }
    }
}
