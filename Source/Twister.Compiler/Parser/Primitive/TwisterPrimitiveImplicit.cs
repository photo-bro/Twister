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


        public static implicit operator int(TwisterPrimitive p)
        {
            switch (p.Type)
            {
                case PrimitiveType.Bool:
                    return p.Bool ? 1 : 0;
                case PrimitiveType.Int:
                    return p.Int;
                case PrimitiveType.UInt:
                    return (int)p.UInt;
                case PrimitiveType.Float:
                    return (int)p.Float;
                case PrimitiveType.Char:
                    return p.Char;
            }

            throw new InvalidCastException("Cannot implicitly cast to value to int")
            {
                FromType = $"{p.Type}",
                ToType = "int"
            };
        }

        public static implicit operator uint(TwisterPrimitive p)
        {
            switch (p.Type)
            {
                case PrimitiveType.Bool:
                    return p.Bool ? 1u : 0u;
                case PrimitiveType.Int:
                    return (uint)p.Int;
                case PrimitiveType.UInt:
                    return p.UInt;
                case PrimitiveType.Float:
                    return (uint)p.Float;
                case PrimitiveType.Char:
                    return p.Char;
            }

            throw new InvalidCastException("Cannot implicitly cast to value to uint")
            {
                FromType = $"{p.Type}",
                ToType = "uint"
            };
        }
        public static implicit operator double(TwisterPrimitive p)
        {
            switch (p.Type)
            {
                case PrimitiveType.Bool:
                    return p.Bool ? 1 : 0;
                case PrimitiveType.Int:
                    return p.Int;
                case PrimitiveType.UInt:
                    return p.UInt;
                case PrimitiveType.Float:
                    return p.Float;
                case PrimitiveType.Char:
                    return p.Char;
            }

            throw new InvalidCastException("Cannot implicitly cast to value to double")
            {
                FromType = $"{p.Type}",
                ToType = "double"
            };
        }

        public static implicit operator char(TwisterPrimitive p)
        {
            switch (p.Type)
            {
                case PrimitiveType.Int:
                    return (char)p.Int;
                case PrimitiveType.UInt:
                    return (char)p.UInt;
                case PrimitiveType.Float:
                    return (char)p.Float;
                case PrimitiveType.Char:
                    return p.Char;
            }

            throw new InvalidCastException("Cannot implicitly cast to value to char")
            {
                FromType = $"{p.Type}",
                ToType = "char"
            };
        }

        public static implicit operator string(TwisterPrimitive p)
        {
            switch (p.Type)
            {
                case PrimitiveType.Bool:
                    return p.Bool ? "true" : "false";
                case PrimitiveType.Int:
                    return $"{p.Int}";
                case PrimitiveType.UInt:
                    return $"{p.UInt}";
                case PrimitiveType.Float:
                    return $"{p.Float}";
                case PrimitiveType.Char:
                    return $"{p.Char}";
                case PrimitiveType.Str:
                    return p.Str;
            }

            throw new InvalidCastException("Cannot implicitly cast to value to string")
            {
                FromType = $"{p.Type}",
                ToType = "string"
            };
        }

        public static implicit operator TwisterPrimitive(int i) => new TwisterPrimitive(PrimitiveType.Int) { Int = i };
        public static implicit operator TwisterPrimitive(uint u) => new TwisterPrimitive(PrimitiveType.UInt) { UInt = u };
        public static implicit operator TwisterPrimitive(double d) => new TwisterPrimitive(PrimitiveType.Float) { Float = d };
        public static implicit operator TwisterPrimitive(bool b) => new TwisterPrimitive(PrimitiveType.Bool) { Bool = b };
        public static implicit operator TwisterPrimitive(char c) => new TwisterPrimitive(PrimitiveType.Char) { Char = c };
        public static implicit operator TwisterPrimitive(string s) => new TwisterPrimitive(PrimitiveType.Str) { Str = s };

    }
}
