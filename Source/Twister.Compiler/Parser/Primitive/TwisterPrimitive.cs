using System;
using Twister.Compiler.Parser.Enum;
namespace Twister.Compiler.Parser.Primitive
{
    public partial struct TwisterPrimitive : IEquatable<TwisterPrimitive>
    {
        public PrimitiveType Type { get; set; }

        public bool Bool { get; set; }

        public int Int { get; set; }

        public uint UInt { get; set; }

        public double Float { get; set; }

        public char Char { get; set; }

        public string Str { get; set; }

        public TwisterPrimitive(PrimitiveType type)
        {
            Type = type;
            Bool = default(bool);
            Int = default(int);
            UInt = default(uint);
            Float = default(double);
            Char = default(char);
            Str = string.Empty;
        }

        public bool Equals(TwisterPrimitive other) => Equals(this, other);

        public override bool Equals(object obj) => Equals(this, obj);

        public override int GetHashCode()
        {
            var hash = 19;
            hash = (hash * 7) + Bool.GetHashCode();
            hash = (hash * 7) + Int.GetHashCode();
            hash = (hash * 7) + UInt.GetHashCode();
            hash = (hash * 7) + Float.GetHashCode();
            hash = (hash * 7) + Char.GetHashCode();
            hash = (hash * 7) + Str.GetHashCode();

            return hash;
        }

        public static bool Equals(TwisterPrimitive instance, object obj)
        {
            if (!(obj is TwisterPrimitive))
                return false;

            var other = (TwisterPrimitive)obj;

            if (instance.Type != other.Type)
                return false;

            var areValuesSame = instance.Bool == other.Bool &&
                                instance.Int == other.Int &&
                                instance.UInt == other.UInt &&
                                Math.Abs(instance.Float - other.Float) < .00001 &&
                                instance.Char == other.Char &&
                                instance.Str == other.Str;

            return areValuesSame;
        }

        /// <summary>
        /// Returns value for Twister equivalent type of T, returns null if instance
        /// has a differing PrimitiveType
        /// </summary>
        public static T? GetValueOrNull<T>(TwisterPrimitive instance) where T : struct
        {
            var type = typeof(T);

            // Currently relying on boxing for the generic casting, would be nice to find a cleaner solution
            switch (instance.Type)
            {
                case PrimitiveType.Bool when type == typeof(bool):
                    return (T)(object)instance.Bool;
                case PrimitiveType.Int when type == typeof(int):
                    return (T)(object)instance.Int;
                case PrimitiveType.UInt when type == typeof(uint):
                    return (T)(object)instance.UInt;
                case PrimitiveType.Float when type == typeof(double):
                    return (T)(object)instance.Float;
                case PrimitiveType.Char when type == typeof(char):
                    return (T)(object)instance.Char;
                case PrimitiveType.Str when type == typeof(string):
                    return (T)(object)instance.Str;
            }
            return null;
        }

        /// <summary>
        /// Returns value for Twister equivalent type of T, returns default(T) if instance
        /// has a differing PrimitiveType
        /// </summary>
        public static T GetValueOrDefault<T>(TwisterPrimitive instance)
        {
            var type = typeof(T);
            T value = default(T);

            // Currently relying on boxing for the generic casting, would be nice to find a cleaner solution
            switch (instance.Type)
            {
                case PrimitiveType.Bool when type == typeof(bool):
                    value = (T)(object)instance.Bool;
                    break;
                case PrimitiveType.Int when type == typeof(int):
                    value = (T)(object)instance.Int;
                    break;
                case PrimitiveType.UInt when type == typeof(uint):
                    value = (T)(object)instance.UInt;
                    break;
                case PrimitiveType.Float when type == typeof(double):
                    value = (T)(object)instance.Float;
                    break;
                case PrimitiveType.Char when type == typeof(char):
                    value = (T)(object)instance.Char;
                    break;
                case PrimitiveType.Str when type == typeof(string):
                    value = (T)(object)instance.Str;
                    break;
            }
            return value;
        }

        /// <summary>
        /// Return the value of the <see cref="TwisterPrimitive"/> as a boxed object with the 
        /// <see cref="PrimitiveType"/> as an out parameter
        /// </summary>
        public static object GetValue(TwisterPrimitive instance, out PrimitiveType type)
        {
            switch (type = instance.Type)
            {
                case PrimitiveType.None:
                    return null;
                case PrimitiveType.Bool:
                    return instance.Bool;
                case PrimitiveType.Int:
                    return instance.Int;
                case PrimitiveType.UInt:
                    return instance.UInt;
                case PrimitiveType.Float:
                    return instance.Float;
                case PrimitiveType.Char:
                    return instance.Char;
                case PrimitiveType.Str:
                    return instance.Str;
                default:
                    return null;
            }
        }
    }
}
