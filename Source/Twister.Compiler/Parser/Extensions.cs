using System;
using Twister.Compiler.Lexer.Enum;
using Twister.Compiler.Parser.Enum;

namespace Twister.Compiler.Parser
{
    public static class Extensions
    {

        public static TwisterType ToTwisterType(this Keyword keyword)
        {
            switch (keyword)
            {
                case Keyword.Bool:
                    return TwisterType.Bool;
                case Keyword.Char:
                    return TwisterType.Char;
                case Keyword.Int:
                    return TwisterType.Int;
                case Keyword.UInt:
                    return TwisterType.UInt;
                case Keyword.Float:
                    return TwisterType.Float;
                case Keyword.Str:
                    return TwisterType.Str;
                case Keyword.Struct:
                    return TwisterType.Struct;
                default:
                    throw new InvalidTypeException("Specified keyword is not a valid type") { InvalidType = $"{keyword}" };
            }
        }
    }
}
