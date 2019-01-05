using System;
using Twister.Compiler.Common;
using Twister.Compiler.Parser.Enum;

namespace Twister.Compiler.Parser.Primitive
{
    public partial struct TwisterPrimitive
    {
        public static implicit operator bool(TwisterPrimitive instance)
        {
            if (instance.Type == PrimitiveType.Bool)
                return instance.GetValueOrDefault<bool>();

            if (instance.Type == PrimitiveType.Str)
                throw new InvalidComparisonException("Cannot compare.")
                { Type = $"{instance.Type}" };

            var c = instance.GetValueOrNull<char>();
            var u = instance.GetValueOrNull<uint>();
            var i = instance.GetValueOrNull<int>();
            var f = instance.GetValueOrNull<double>();

            return (c ?? u ?? i ?? f ?? default(double)) > default(double);
        }

        // C# primitive to TwisterPrimitive
        public static implicit operator int(TwisterPrimitive p)
        {
            var val = p.GetValue(out var t);

            switch (t)
            {
                case PrimitiveType.Bool:
                    break;
                case PrimitiveType.Int:
                    break;
                case PrimitiveType.UInt:
                    break;
                case PrimitiveType.Float:
                    break;
                case PrimitiveType.Char:
                    break;
                case PrimitiveType.Str:
                    break;
            }


        }
        public static implicit operator uint(TwisterPrimitive p) => p.UInt;
        public static implicit operator double(TwisterPrimitive p) => p.Float;
        public static implicit operator char(TwisterPrimitive p) => p.Char;
        public static implicit operator string(TwisterPrimitive p) => p.Str;

        public static implicit operator TwisterPrimitive(int i) => new TwisterPrimitive(PrimitiveType.Int) { Int = i };
        public static implicit operator TwisterPrimitive(uint u) => new TwisterPrimitive(PrimitiveType.UInt) { UInt = u };
        public static implicit operator TwisterPrimitive(double d) => new TwisterPrimitive(PrimitiveType.Float) { Float = d };
        public static implicit operator TwisterPrimitive(bool b) => new TwisterPrimitive(PrimitiveType.Bool) { Bool = b };
        public static implicit operator TwisterPrimitive(char c) => new TwisterPrimitive(PrimitiveType.Char) { Char = c };
        public static implicit operator TwisterPrimitive(string s) => new TwisterPrimitive(PrimitiveType.Str) { Str = s };

    }
}
