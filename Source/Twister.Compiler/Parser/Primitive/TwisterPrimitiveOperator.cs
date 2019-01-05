using System;
using Twister.Compiler.Common;
using Twister.Compiler.Parser.Enum;

namespace Twister.Compiler.Parser.Primitive
{
    public partial struct TwisterPrimitive
    {
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

        public static bool operator true(TwisterPrimitive instance) => instance == true;

        public static bool operator false(TwisterPrimitive instance) => instance == false;

        public static TwisterPrimitive operator +(TwisterPrimitive instance, TwisterPrimitive other)
        {
            if (instance.Type == other.Type)
            {
                switch (instance.Type)
                {
                    case PrimitiveType.Int:
                        {
                            var l = instance.GetValueOrDefault<int>();
                            var r = other.GetValueOrDefault<int>();
                            return l + r;
                        }
                    case PrimitiveType.UInt:
                        {
                            var l = instance.GetValueOrDefault<uint>();
                            var r = other.GetValueOrDefault<uint>();
                            return l + r;
                        }
                    case PrimitiveType.Float:
                        {
                            var l = instance.GetValueOrDefault<double>();
                            var r = other.GetValueOrDefault<double>();
                            return l + r;
                        }
                    case PrimitiveType.Char:
                        {
                            var l = instance.GetValueOrDefault<char>();
                            var r = other.GetValueOrDefault<char>();
                            return (char)(l + r);
                        }
                    case PrimitiveType.Str: // allow for implicit string concatenation
                        {
                            var l = instance.GetValueOrDefault<string>();
                            var r = other.GetValueOrDefault<string>();
                            return $"{l}{r}";
                        }
                }
            }

            // More implicit string/char concatenation
            switch (instance.Type)
            {
                case PrimitiveType.Str when other.Type == PrimitiveType.Char:
                    {
                        var l = instance.GetValueOrDefault<string>();
                        var r = other.GetValueOrDefault<char>();
                        return $"{l}{r}";
                    }
                case PrimitiveType.Char when other.Type == PrimitiveType.Str:
                    {
                        var l = instance.GetValueOrDefault<char>();
                        var r = other.GetValueOrDefault<string>();
                        return $"{l}{r}";
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

            // This is sloppy and lazy.....
            var left = lc ?? lu ?? li ?? lf ?? default(double);
            var right = rc ?? ru ?? ri ?? rf ?? default(double);

            return left + right;
        }

        public static TwisterPrimitive operator -(TwisterPrimitive instance, TwisterPrimitive other)
        {
            return default(TwisterPrimitive);
        }

        public static TwisterPrimitive operator %(TwisterPrimitive instance, TwisterPrimitive other)
        {
            return default(TwisterPrimitive);
        }

        public static TwisterPrimitive operator *(TwisterPrimitive instance, TwisterPrimitive other)
        {
            return default(TwisterPrimitive);
        }

        public static TwisterPrimitive operator /(TwisterPrimitive instance, TwisterPrimitive other)
        {
            return default(TwisterPrimitive);
        }

        public static TwisterPrimitive operator &(TwisterPrimitive instance, TwisterPrimitive other)
        {
            return default(TwisterPrimitive);
        }

        public static TwisterPrimitive operator |(TwisterPrimitive instance, TwisterPrimitive other)
        {
            return default(TwisterPrimitive);
        }

        public static TwisterPrimitive operator ^(TwisterPrimitive instance, TwisterPrimitive other)
        {
            return default(TwisterPrimitive);
        }

        public static TwisterPrimitive operator !(TwisterPrimitive instance)
        {
            return default(TwisterPrimitive);
        }

        public static TwisterPrimitive operator <<(TwisterPrimitive instance, int shift)
        {
            return default(TwisterPrimitive);
        }

        public static TwisterPrimitive operator >>(TwisterPrimitive instance, int shift)
        {
            return default(TwisterPrimitive);
        }

    }
}
