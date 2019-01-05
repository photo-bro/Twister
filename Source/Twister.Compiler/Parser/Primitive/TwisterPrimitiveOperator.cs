using System;
using Twister.Compiler.Common;
using Twister.Compiler.Lexer.Enum;
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
            // Implicit string/char concatenation
            switch (instance.Type)
            {
                case PrimitiveType.Str when other.Type == PrimitiveType.Str:
                    {
                        var l = instance.GetValueOrDefault<string>();
                        var r = other.GetValueOrDefault<string>();
                        return $"{l}{r}";
                    }
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
                case PrimitiveType.Char when other.Type == PrimitiveType.Char:
                    {
                        var l = instance.GetValueOrDefault<char>();
                        var r = other.GetValueOrDefault<char>();
                        return $"{l}{r}";
                    }
            }

            // Numeric
            var result = Calculate(instance, other, Operator.Plus);
            if (result != null)
                return result.Value;

            throw new InvalidExpressionException("Cannot evaluate expression")
            {
                Left = $"{instance.Type}",
                Right = $"{other.Type}",
                Operation = $"{Operator.Plus}"
            };
        }

        public static TwisterPrimitive operator -(TwisterPrimitive instance, TwisterPrimitive other)
        {
            var result = Calculate(instance, other, Operator.Minus);
            if (result != null)
                return result.Value;

            throw new InvalidExpressionException("Cannot evaluate expression")
            {
                Left = $"{instance.Type}",
                Right = $"{other.Type}",
                Operation = $"{Operator.Minus}"
            };
        }

        public static TwisterPrimitive operator %(TwisterPrimitive instance, TwisterPrimitive other)
        {
            var result = Calculate(instance, other, Operator.Modulo);
            if (result != null)
                return result.Value;

            throw new InvalidExpressionException("Cannot evaluate expression")
            {
                Left = $"{instance.Type}",
                Right = $"{other.Type}",
                Operation = $"{Operator.Modulo}"
            };
        }

        public static TwisterPrimitive operator *(TwisterPrimitive instance, TwisterPrimitive other)
        {
            var result = Calculate(instance, other, Operator.Multiplication);
            if (result != null)
                return result.Value;

            throw new InvalidExpressionException("Cannot evaluate expression")
            {
                Left = $"{instance.Type}",
                Right = $"{other.Type}",
                Operation = $"{Operator.Multiplication}"
            };
        }

        public static TwisterPrimitive operator /(TwisterPrimitive instance, TwisterPrimitive other)
        {
            var result = Calculate(instance, other, Operator.ForwardSlash);
            if (result != null)
                return result.Value;

            throw new InvalidExpressionException("Cannot evaluate expression")
            {
                Left = $"{instance.Type}",
                Right = $"{other.Type}",
                Operation = $"{Operator.ForwardSlash}"
            };
        }

        public static TwisterPrimitive operator &(TwisterPrimitive instance, TwisterPrimitive other)
        {
            var result = Calculate(instance, other, Operator.BitAnd);
            if (result != null)
                return result.Value;

            throw new InvalidExpressionException("Cannot evaluate expression")
            {
                Left = $"{instance.Type}",
                Right = $"{other.Type}",
                Operation = $"{Operator.BitAnd}"
            };
        }

        public static TwisterPrimitive operator |(TwisterPrimitive instance, TwisterPrimitive other)
        {
            var result = Calculate(instance, other, Operator.BitOr);
            if (result != null)
                return result.Value;

            throw new InvalidExpressionException("Cannot evaluate expression")
            {
                Left = $"{instance.Type}",
                Right = $"{other.Type}",
                Operation = $"{Operator.BitOr}"
            };
        }

        public static TwisterPrimitive operator ^(TwisterPrimitive instance, TwisterPrimitive other)
        {
            var result = Calculate(instance, other, Operator.BitExOr);
            if (result != null)
                return result.Value;

            throw new InvalidExpressionException("Cannot evaluate expression")
            {
                Left = $"{instance.Type}",
                Right = $"{other.Type}",
                Operation = $"{Operator.BitExOr}"
            };
        }

        public static TwisterPrimitive operator !(TwisterPrimitive instance)
        {
            switch (instance.Type)
            {
                case PrimitiveType.Bool:
                    return !instance.GetValueOrDefault<bool>();
            }

            throw new InvalidExpressionException("Cannot evaluate expression")
            {
                Left = $"{instance.Type}",
                Right = $"",
                Operation = $"{Operator.LogNot}"
            };
        }

        public static TwisterPrimitive operator ~(TwisterPrimitive instance)
        {
            switch (instance.Type)
            {
                case PrimitiveType.Bool:
                    return !instance.GetValueOrDefault<bool>();
                case PrimitiveType.Int:
                    return ~instance.GetValueOrDefault<int>();
                case PrimitiveType.UInt:
                    return ~instance.GetValueOrDefault<uint>();
            }

            throw new InvalidExpressionException("Cannot evaluate expression")
            {
                Left = $"{instance.Type}",
                Right = $"",
                Operation = $"{Operator.BitNot}"
            };
        }

        public static TwisterPrimitive operator <<(TwisterPrimitive instance, int shift)
        {
            var result = Calculate(instance, shift, Operator.LeftShift);
            if (result != null)
                return result.Value;

            throw new InvalidExpressionException("Cannot evaluate expression")
            {
                Left = $"{instance.Type}",
                Right = $"int",
                Operation = $"{Operator.LeftShift}"
            };
        }

        public static TwisterPrimitive operator >>(TwisterPrimitive instance, int shift)
        {
            {
                var result = Calculate(instance, shift, Operator.RightShift);
                if (result != null)
                    return result.Value;

                throw new InvalidExpressionException("Cannot evaluate expression")
                {
                    Left = $"{instance.Type}",
                    Right = $"int",
                    Operation = $"{Operator.RightShift}"
                };
            }
        }

        private static TwisterPrimitive? Calculate(TwisterPrimitive left, TwisterPrimitive right, Operator o)
        {
            switch (left.Type)
            {
                case PrimitiveType.Int:
                    {
                        var l = left.GetValueOrDefault<int>();
                        switch (right.Type)
                        {
                            case PrimitiveType.Int:
                                {
                                    var r = right.GetValueOrDefault<int>();
                                    switch (o)
                                    {
                                        case Operator.Plus: return l + r;
                                        case Operator.Minus: return l - r;
                                        case Operator.Modulo: return l % r;
                                        case Operator.Multiplication: return l * r;
                                        case Operator.ForwardSlash: return l / r;
                                        case Operator.BitAnd: return l & r;
                                        case Operator.BitOr: return l | r;
                                        case Operator.BitExOr: return l ^ r;
                                        case Operator.LeftShift: return l << r;
                                        case Operator.RightShift: return l >> r;
                                    }
                                    break;
                                }
                            case PrimitiveType.UInt:
                                {
                                    var r = right.GetValueOrDefault<uint>();
                                    switch (o)
                                    {
                                        case Operator.Plus: return (uint)(l + r);
                                        case Operator.Minus: return (uint)(l - r);
                                        case Operator.Modulo: return (uint)(l % r);
                                        case Operator.Multiplication: return (uint)(l * r);
                                        case Operator.ForwardSlash: return (uint)(l / r);
                                        case Operator.BitAnd: return (uint)(l & r);
                                        case Operator.BitOr: return (uint)(l | (int)r);
                                        case Operator.BitExOr: return (uint)(l ^ r);
                                        case Operator.LeftShift: return (uint)(l << (int)r);
                                        case Operator.RightShift: return (uint)(l >> (int)r);
                                    }
                                    break;
                                }
                            case PrimitiveType.Float:
                                {
                                    var r = right.GetValueOrDefault<double>();
                                    switch (o)
                                    {
                                        case Operator.Plus: return l + r;
                                        case Operator.Minus: return l - r;
                                        case Operator.Modulo: return l % r;
                                        case Operator.Multiplication: return l * r;
                                        case Operator.ForwardSlash: return l / r;
                                    }
                                    break;
                                }
                            case PrimitiveType.Char:
                                {
                                    var r = right.GetValueOrDefault<char>();
                                    switch (o)
                                    {
                                        case Operator.Plus: return l + r;
                                        case Operator.Minus: return l - r;
                                        case Operator.Modulo: return l % r;
                                        case Operator.Multiplication: return l * r;
                                        case Operator.ForwardSlash: return l / r;
                                        case Operator.BitAnd: return l & r;
                                        case Operator.BitOr: return l | r;
                                        case Operator.BitExOr: return l ^ r;
                                        case Operator.LeftShift: return l << r;
                                        case Operator.RightShift: return l >> r;
                                    }
                                    break;
                                }
                        }
                        break;
                    }
                case PrimitiveType.UInt:
                    {
                        var l = left.GetValueOrDefault<uint>();
                        switch (right.Type)
                        {
                            case PrimitiveType.Int:
                                {
                                    var r = right.GetValueOrDefault<int>();
                                    switch (o)
                                    {
                                        case Operator.Plus: return (int)(l + r);
                                        case Operator.Minus: return (int)(l - r);
                                        case Operator.Modulo: return (int)(l % r);
                                        case Operator.Multiplication: return (int)(l * r);
                                        case Operator.ForwardSlash: return (int)(l / r);
                                        case Operator.BitAnd: return (int)(l & r);
                                        case Operator.BitOr: return (int)l | r;
                                        case Operator.BitExOr: return (int)(l ^ r);
                                        case Operator.LeftShift: return (int)(l << r);
                                        case Operator.RightShift: return (int)(l >> r);
                                    }
                                    break;
                                }
                            case PrimitiveType.UInt:
                                {
                                    var r = right.GetValueOrDefault<uint>();
                                    switch (o)
                                    {
                                        case Operator.Plus: return l + r;
                                        case Operator.Minus: return l - r;
                                        case Operator.Modulo: return l % r;
                                        case Operator.Multiplication: return l * r;
                                        case Operator.ForwardSlash: return l / r;
                                        case Operator.BitAnd: return l & r;
                                        case Operator.BitOr: return l | r;
                                        case Operator.BitExOr: return l ^ r;
                                        case Operator.LeftShift: return l << (int)r;
                                        case Operator.RightShift: return l >> (int)r;
                                    }
                                    break;
                                }
                            case PrimitiveType.Float:
                                {
                                    var r = right.GetValueOrDefault<double>();
                                    switch (o)
                                    {
                                        case Operator.Plus: return l + r;
                                        case Operator.Minus: return l - r;
                                        case Operator.Modulo: return l % r;
                                        case Operator.Multiplication: return l * r;
                                        case Operator.ForwardSlash: return l / r;
                                    }
                                    break;
                                }
                            case PrimitiveType.Char:
                                {
                                    var r = right.GetValueOrDefault<char>();
                                    switch (o)
                                    {
                                        case Operator.Plus: return l + r;
                                        case Operator.Minus: return l - r;
                                        case Operator.Modulo: return l % r;
                                        case Operator.Multiplication: return l * r;
                                        case Operator.ForwardSlash: return l / r;
                                        case Operator.BitAnd: return l & r;
                                        case Operator.BitOr: return l | r;
                                        case Operator.BitExOr: return l ^ r;
                                        case Operator.LeftShift: return l << r;
                                        case Operator.RightShift: return l >> r;
                                    }
                                    break;
                                }
                        }
                        break;
                    }
                case PrimitiveType.Float:
                    {
                        var l = left.GetValueOrDefault<double>();
                        switch (right.Type)
                        {
                            case PrimitiveType.Int:
                                {
                                    var r = right.GetValueOrDefault<int>();
                                    switch (o)
                                    {
                                        case Operator.Plus: return l + r;
                                        case Operator.Minus: return l - r;
                                        case Operator.Modulo: return l % r;
                                        case Operator.Multiplication: return l * r;
                                        case Operator.ForwardSlash: return l / r;
                                    }
                                    break;
                                }
                            case PrimitiveType.UInt:
                                {
                                    var r = right.GetValueOrDefault<uint>();
                                    switch (o)
                                    {
                                        case Operator.Plus: return l + r;
                                        case Operator.Minus: return l - r;
                                        case Operator.Modulo: return l % r;
                                        case Operator.Multiplication: return l * r;
                                        case Operator.ForwardSlash: return l / r;
                                    }
                                    break;
                                }
                            case PrimitiveType.Float:
                                {
                                    var r = right.GetValueOrDefault<double>();
                                    switch (o)
                                    {
                                        case Operator.Plus: return l + r;
                                        case Operator.Minus: return l - r;
                                        case Operator.Modulo: return l % r;
                                        case Operator.Multiplication: return l * r;
                                        case Operator.ForwardSlash: return l / r;
                                    }
                                    break;
                                }
                            case PrimitiveType.Char:
                                {
                                    var r = right.GetValueOrDefault<char>();
                                    switch (o)
                                    {
                                        case Operator.Plus: return l + r;
                                        case Operator.Minus: return l - r;
                                        case Operator.Modulo: return l % r;
                                        case Operator.Multiplication: return l * r;
                                        case Operator.ForwardSlash: return l / r;
                                    }
                                    break;
                                }
                        }
                        break;
                    }
                case PrimitiveType.Char:
                    {
                        var l = left.GetValueOrDefault<char>();
                        switch (right.Type)
                        {
                            case PrimitiveType.Int:
                                {
                                    var r = right.GetValueOrDefault<int>();
                                    switch (o)
                                    {
                                        case Operator.Plus: return l + r;
                                        case Operator.Minus: return l - r;
                                        case Operator.Modulo: return l % r;
                                        case Operator.Multiplication: return l * r;
                                        case Operator.ForwardSlash: return l / r;
                                        case Operator.BitAnd: return l & r;
                                        case Operator.BitOr: return l | r;
                                        case Operator.BitExOr: return l ^ r;
                                        case Operator.LeftShift: return l << r;
                                        case Operator.RightShift: return l >> r;
                                    }
                                    break;
                                }
                            case PrimitiveType.UInt:
                                {
                                    var r = right.GetValueOrDefault<uint>();
                                    switch (o)
                                    {
                                        case Operator.Plus: return l + r;
                                        case Operator.Minus: return l - r;
                                        case Operator.Modulo: return l % r;
                                        case Operator.Multiplication: return l * r;
                                        case Operator.ForwardSlash: return l / r;
                                        case Operator.BitAnd: return l & r;
                                        case Operator.BitOr: return l | r;
                                        case Operator.BitExOr: return l ^ r;
                                        case Operator.LeftShift: return (uint)l << (int)r;
                                        case Operator.RightShift: return (uint)l >> (int)r;
                                    }
                                    break;
                                }
                            case PrimitiveType.Float:
                                {
                                    var r = right.GetValueOrDefault<double>();
                                    switch (o)
                                    {
                                        case Operator.Plus: return l + r;
                                        case Operator.Minus: return l - r;
                                        case Operator.Modulo: return l % r;
                                        case Operator.Multiplication: return l * r;
                                        case Operator.ForwardSlash: return l / r;
                                    }
                                    break;
                                }
                            case PrimitiveType.Char:
                                {
                                    var r = right.GetValueOrDefault<char>();
                                    switch (o)
                                    {
                                        case Operator.Plus: return l + r;
                                        case Operator.Minus: return l - r;
                                        case Operator.Modulo: return l % r;
                                        case Operator.Multiplication: return l * r;
                                        case Operator.ForwardSlash: return l / r;
                                        case Operator.BitAnd: return l & r;
                                        case Operator.BitOr: return l | r;
                                        case Operator.BitExOr: return l ^ r;
                                        case Operator.LeftShift: return l << r;
                                        case Operator.RightShift: return l >> r;
                                    }
                                    break;
                                }
                        }
                        break;
                    }
            }

            return null;
        }
    }
}
