using System;
using Twister.Compiler.Common;
using Twister.Compiler.Parser.Enum;
namespace Twister.Compiler.Parser
{
    public struct TwisterPrimitive : IEquatable<TwisterPrimitive>
    {
        public PrimitiveType Type { get; set; }

        public bool Bool { get; set; }

        public int Int { get; set; }

        public uint UInt { get; set; }

        public double Float { get; set; }

        public char Char { get; set; }

        public string Str { get; set; }

        #region Operator overrides

        public static bool operator ==(TwisterPrimitive instance, TwisterPrimitive other)
        {
            if (instance.Type == other.Type)
            {
                switch (instance.Type)
                {
                    case PrimitiveType.Str:
                        {
                            var l = instance.GetValueOrDefault<string>();
                            var r = other.GetValueOrDefault<string>();
                            return string.Compare(l, r) == 0;
                        }
                    case PrimitiveType.Int:
                        {
                            var l = instance.GetValueOrDefault<int>();
                            var r = other.GetValueOrDefault<int>();
                            return l == r;
                        }
                    case PrimitiveType.UInt:
                        {
                            var l = instance.GetValueOrDefault<uint>();
                            var r = other.GetValueOrDefault<uint>();
                            return l == r;
                        }
                    case PrimitiveType.Float:
                        {
                            var l = instance.GetValueOrDefault<double>();
                            var r = other.GetValueOrDefault<double>();
                            return Math.Abs(l - r) < Constants.DefaultEpsilon;
                        }
                    case PrimitiveType.Char:
                        {
                            var l = instance.GetValueOrDefault<char>();
                            var r = other.GetValueOrDefault<char>();
                            return l == r;
                        }
                    default:
                        return false;
                }
            }

            if ((instance.Type == PrimitiveType.Str && other.Type != PrimitiveType.Str) ||
                (instance.Type != PrimitiveType.Str && other.Type == PrimitiveType.Str))
                throw new InvalidComparisonException("Cannot compare str against numeric type.")
                { Type = $"{PrimitiveType.Str}" };

            var lc = instance.GetValueOrNull<char>();
            var rc = other.GetValueOrNull<char>();
            var li = instance.GetValueOrNull<int>();
            var ri = other.GetValueOrNull<int>();
            var lu = instance.GetValueOrNull<uint>();
            var ru = other.GetValueOrNull<uint>();
            var lf = instance.GetValueOrNull<double>();
            var rf = other.GetValueOrNull<double>();

            var left = lc ?? lu ?? li ?? lf ?? default(double);
            var right = rc ?? ru ?? ri ?? rf ?? default(double);

            return Math.Abs(left - right) < Constants.DefaultEpsilon;
        }

        public static bool operator !=(TwisterPrimitive instance, TwisterPrimitive other) => !(instance == other);

        public static bool operator >(TwisterPrimitive instance, TwisterPrimitive other)
        {
            if (instance.Type == PrimitiveType.Str || other.Type == PrimitiveType.Str)
                throw new InvalidComparisonException("Cannot compare str types for greater or less than.")
                { Type = $"{instance.Type}" };

            if (instance.Type == other.Type)
            {
                switch (instance.Type)
                {
                    case PrimitiveType.Int:
                        {
                            var l = instance.GetValueOrDefault<int>();
                            var r = other.GetValueOrDefault<int>();
                            return l > r;
                        }
                    case PrimitiveType.UInt:
                        {
                            var l = instance.GetValueOrDefault<uint>();
                            var r = other.GetValueOrDefault<uint>();
                            return l > r;
                        }
                    case PrimitiveType.Float:
                        {
                            var l = instance.GetValueOrDefault<double>();
                            var r = other.GetValueOrDefault<double>();
                            return l > r;
                        }
                    case PrimitiveType.Char:
                        {
                            var l = instance.GetValueOrDefault<char>();
                            var r = other.GetValueOrDefault<char>();
                            return l > r;
                        }
                    default:
                        return false;
                }
            }

            var lc = instance.GetValueOrNull<char>();
            var rc = other.GetValueOrNull<char>();
            var li = instance.GetValueOrNull<int>();
            var ri = other.GetValueOrNull<int>();
            var lu = instance.GetValueOrNull<uint>();
            var ru = other.GetValueOrNull<uint>();
            var lf = instance.GetValueOrNull<double>();
            var rf = other.GetValueOrNull<double>();

            var left = lc ?? lu ?? li ?? lf ?? default(double);
            var right = rc ?? ru ?? ri ?? rf ?? default(double);

            return left > right;
        }

        public static bool operator <(TwisterPrimitive instance, TwisterPrimitive other) =>
                                                                            !(instance > other) && instance != other;

        public static bool operator >=(TwisterPrimitive instance, TwisterPrimitive other) =>
                                                                            instance > other || instance == other;

        public static bool operator <=(TwisterPrimitive instance, TwisterPrimitive other) =>
                                                                            instance < other || instance == other;

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

            return (c ?? u ?? i ?? f ?? 0.0d) > 0.0d;
        }

        #endregion

        #region IEquatable and Equal overrides

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

        #endregion

        /// <summary>
        /// Returns value for Twister equivalent type of T, returns null if instance
        /// has a differing PrimitiveType
        /// </summary>
        private static T? GetValueOrNull<T>(TwisterPrimitive instance) where T : struct
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
        private static T GetValueOrDefault<T>(TwisterPrimitive instance)
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
    }

    public static class PrimitiveExtensions
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

        public static bool IsNumeric(this TwisterPrimitive instance) => instance.Type == PrimitiveType.Int ||
                                                                      instance.Type == PrimitiveType.UInt ||
                                                                      instance.Type == PrimitiveType.Float ||
                                                                      instance.Type == PrimitiveType.Char;
    }
}
