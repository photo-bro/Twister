﻿using Twister.Compiler.Lexer.Enum;
using Twister.Compiler.Lexer.Token;
using Twister.Compiler.Parser.Enum;
using Twister.Compiler.Parser.Interface;
using Twister.Compiler.Parser.Primitive;

namespace Twister.Compiler.Parser.Node
{
    public abstract class BaseExpressionNode : IExpressionNode<TwisterPrimitive, TwisterPrimitive>
    {
        protected BaseExpressionNode(IValueNode<TwisterPrimitive> left, IValueNode<TwisterPrimitive> right)
        {
            Left = left;
            Right = right;
        }

        public abstract ExpressionKind ExpressionKind { get; }

        public abstract Operator Operator { get; protected set; }

        public IValueNode<TwisterPrimitive> Left { get; protected set; }

        public IValueNode<TwisterPrimitive> Right { get; protected set; }

        public abstract TwisterPrimitive Value { get; }

        public NodeKind Kind => NodeKind.Expression;
    }

    public sealed class UnaryExpressionNode : BaseExpressionNode
    {
        public UnaryExpressionNode(IValueNode<TwisterPrimitive> right, Operator @operator)
            : base(null, right)
        {
            Operator = @operator;
        }

        private Operator _operator;
        public override Operator Operator
        {
            get => _operator;
            protected set
            {
                if (!value.IsUnaryArithmeticOperator())
                    throw new InvalidOperatorException("Expecting unary arithmetic operator")
                    { InvalidOperator = value };
                _operator = value;
            }
        }

        public override TwisterPrimitive Value
        {
            get
            {
                switch (_operator)
                {
                    case Operator.Plus:
                        return Right.Value;
                    case Operator.Minus:
                        return -1 * Right.Value;
                    case Operator.LogNot:
                        return !Right.Value;
                    case Operator.BitNot:
                        return ~Right.Value;
                }
                throw new InvalidOperatorException("Expecting unary arithmetic operator")
                { InvalidOperator = _operator };
            }
        }

        public override ExpressionKind ExpressionKind => ExpressionKind.Unary;
    }

    public sealed class AdditiveNode : BaseExpressionNode
    {
        public AdditiveNode(IValueNode<TwisterPrimitive> left, IValueNode<TwisterPrimitive> right, Operator op)
            : base(left, right)
        { Operator = op; }

        public override ExpressionKind ExpressionKind => ExpressionKind.Additive;

        private Operator _operator;
        public override Operator Operator
        {
            get => _operator;
            protected set
            {
                if (value != Operator.Plus && value != Operator.Minus)
                    throw new InvalidOperatorException("Expecting additive ('+' or '-') operator")
                    { InvalidOperator = value };
                _operator = value;
            }
        }

        public override TwisterPrimitive Value
        {
            get
            {
                switch (Operator)
                {
                    case Operator.Plus:
                        return Left.Value + Right.Value;
                    case Operator.Minus:
                        return Left.Value - Right.Value;
                }
                throw new InvalidOperatorException("Expecting additive ('+' or '-') operator")
                { InvalidOperator = _operator };
            }
        }
    }

    public sealed class MultiplicativeNode : BaseExpressionNode
    {
        public MultiplicativeNode(IValueNode<TwisterPrimitive> left, IValueNode<TwisterPrimitive> right, Operator op)
            : base(left, right)
        { Operator = op; }

        public override ExpressionKind ExpressionKind => ExpressionKind.Multiplicative;

        private Operator _operator;
        public override Operator Operator
        {
            get => _operator;
            protected set
            {
                if (value != Operator.Multiplication && value != Operator.ForwardSlash && value != Operator.Modulo)
                    throw new InvalidOperatorException("Expecting Multiplicative ('*', '/', or '%') operator")
                    { InvalidOperator = value };
                _operator = value;
            }
        }

        public override TwisterPrimitive Value
        {
            get
            {
                switch (Operator)
                {
                    case Operator.Multiplication:
                        return Left.Value * Right.Value;
                    case Operator.ForwardSlash:
                        return Left.Value / Right.Value;
                    case Operator.Modulo:
                        return Left.Value % Right.Value;
                }
                throw new InvalidOperatorException("Expecting Multiplicative ('*', '/', or '%') operator")
                { InvalidOperator = _operator };
            }
        }
    }

    public sealed class ShiftNode : BaseExpressionNode
    {
        public ShiftNode(IValueNode<TwisterPrimitive> left, IValueNode<TwisterPrimitive> right, Operator op)
            : base(left, right)
        {
            Operator = op;
            if (right.Value.Type != PrimitiveType.Int || right.Value.Type != PrimitiveType.UInt)
                throw new InvalidOperatorException("Right side of shift operator must be integer type");
        }

        public override ExpressionKind ExpressionKind => ExpressionKind.Shift;

        private Operator _operator;
        public override Operator Operator
        {
            get => _operator;
            protected set
            {
                if (value != Operator.LeftShift && value != Operator.RightShift)
                    throw new InvalidOperatorException("Expecting shift ('<<' or '>>') operator")
                    { InvalidOperator = value };
                _operator = value;
            }
        }

        public override TwisterPrimitive Value
        {
            get
            {
                switch (Operator)
                {
                    case Operator.LeftShift:
                        return Left.Value << Right.Value;
                    case Operator.RightShift:
                        return Left.Value >> Right.Value;
                }
                throw new InvalidOperatorException("Expecting shift ('<<' or '>>') operator")
                { InvalidOperator = _operator };
            }
        }
    }

    public sealed class RelationalNode : BaseExpressionNode
    {
        public RelationalNode(IValueNode<TwisterPrimitive> left, IValueNode<TwisterPrimitive> right, Operator op)
            : base(left, right)
        { Operator = op; }

        public override ExpressionKind ExpressionKind => ExpressionKind.Relational;

        private Operator _operator;
        public override Operator Operator
        {
            get => _operator;
            protected set
            {
                if (value != Operator.LogLess && value != Operator.LogGreater && value != Operator.LogLessEqual &&
                    value != Operator.LogGreaterEqual)
                    throw new InvalidOperatorException("Expecting relational ('<', '>', '<=', or '>=') operator")
                    { InvalidOperator = value };
                _operator = value;
            }
        }

        public override TwisterPrimitive Value
        {
            get
            {
                switch (Operator)
                {
                    case Operator.LogLess:
                        return Left.Value < Right.Value;
                    case Operator.LogGreater:
                        return Left.Value > Right.Value;
                    case Operator.LogLessEqual:
                        return Left.Value <= Right.Value;
                    case Operator.LogGreaterEqual:
                        return Left.Value >= Right.Value;
                }
                throw new InvalidOperatorException("Expecting relational ('<', '>', '<=', or '>=') operator")
                { InvalidOperator = _operator };
            }
        }
    }

    public sealed class EqualityNode : BaseExpressionNode
    {
        public EqualityNode(IValueNode<TwisterPrimitive> left, IValueNode<TwisterPrimitive> right, Operator op)
            : base(left, right)
        { Operator = op; }

        public override ExpressionKind ExpressionKind => ExpressionKind.Equality;

        private Operator _operator;
        public override Operator Operator
        {
            get => _operator;
            protected set
            {
                if (value != Operator.LogEqual && value != Operator.LogNotEqual)
                    throw new InvalidOperatorException("Expecting equality ('==' or '!=') operator")
                    { InvalidOperator = value };
                _operator = value;
            }
        }

        public override TwisterPrimitive Value
        {
            get
            {
                switch (Operator)
                {
                    case Operator.LogEqual:
                        return Left.Value == Right.Value;
                    case Operator.LogNotEqual:
                        return Left.Value != Right.Value;
                }
                throw new InvalidOperatorException("Expecting equality ('==' or '!=') operator")
                { InvalidOperator = _operator };
            }
        }
    }

    public sealed class BitAndNode : BaseExpressionNode
    {
        public BitAndNode(IValueNode<TwisterPrimitive> left, IValueNode<TwisterPrimitive> right)
            : base(left, right)
        { Operator = Operator.BitAnd; }

        public override ExpressionKind ExpressionKind => ExpressionKind.BitAnd;

        private Operator _operator;
        public override Operator Operator
        {
            get => _operator;
            protected set
            {
                if (value != Operator.BitAnd)
                    throw new InvalidOperatorException("Expecting bitand ('&') operator")
                    { InvalidOperator = value };
                _operator = value;
            }
        }

        public override TwisterPrimitive Value => Left.Value & Right.Value;
    }

    public sealed class BitExOrNode : BaseExpressionNode
    {
        public BitExOrNode(IValueNode<TwisterPrimitive> left, IValueNode<TwisterPrimitive> right)
            : base(left, right)
        { Operator = Operator.BitExOr; }

        public override ExpressionKind ExpressionKind => ExpressionKind.BitExOr;

        private Operator _operator;
        public override Operator Operator
        {
            get => _operator;
            protected set
            {
                if (value != Operator.BitExOr)
                    throw new InvalidOperatorException("Expecting bitexor ('^') operator")
                    { InvalidOperator = value };
                _operator = value;
            }
        }

        public override TwisterPrimitive Value => Left.Value ^ Right.Value;
    }

    public sealed class BitOrNode : BaseExpressionNode
    {
        public BitOrNode(IValueNode<TwisterPrimitive> left, IValueNode<TwisterPrimitive> right)
            : base(left, right)
        { Operator = Operator.BitOr; }

        public override ExpressionKind ExpressionKind => ExpressionKind.BitOr;

        private Operator _operator;
        public override Operator Operator
        {
            get => _operator;
            protected set
            {
                if (value != Operator.BitOr)
                    throw new InvalidOperatorException("Expecting bitor ('|') operator")
                    { InvalidOperator = value };
                _operator = value;
            }
        }

        public override TwisterPrimitive Value => Left.Value | Right.Value;
    }

    public sealed class LogAndNode : BaseExpressionNode
    {
        public LogAndNode(IValueNode<TwisterPrimitive> left, IValueNode<TwisterPrimitive> right)
            : base(left, right)
        { Operator = Operator.LogAnd; }

        public override ExpressionKind ExpressionKind => ExpressionKind.LogAnd;

        private Operator _operator;
        public override Operator Operator
        {
            get => _operator;
            protected set
            {
                if (value != Operator.LogAnd)
                    throw new InvalidOperatorException("Expecting logand ('&&') operator")
                    { InvalidOperator = value };
                _operator = value;
            }
        }

        public override TwisterPrimitive Value => Left.Value && Right.Value;
    }

    public sealed class LogOrNode : BaseExpressionNode
    {
        public LogOrNode(IValueNode<TwisterPrimitive> left, IValueNode<TwisterPrimitive> right)
            : base(left, right)
        { Operator = Operator.LogOr; }

        public override ExpressionKind ExpressionKind => ExpressionKind.LogOr;

        private Operator _operator;
        public override Operator Operator
        {
            get => _operator;
            protected set
            {
                if (value != Operator.LogOr)
                    throw new InvalidOperatorException("Expecting logor ('||') operator")
                    { InvalidOperator = value };
                _operator = value;
            }
        }

        public override TwisterPrimitive Value => Left.Value || Right.Value;
    }

    public sealed class BinaryExpressionNode : BaseExpressionNode
    {
        public BinaryExpressionNode(IValueNode<TwisterPrimitive> left, IValueNode<TwisterPrimitive> right,
         Operator op)
            : base(left, right)
        { Operator = op; }

        public override ExpressionKind ExpressionKind => ExpressionKind.General;

        public override Operator Operator { get; protected set; }

        public override TwisterPrimitive Value
        {
            get
            {
                var l = Left.Value;
                var r = Right.Value;
                switch (Operator)
                {
                    case Operator.Plus:
                        return l + r;
                    case Operator.Minus:
                        return l - r;
                    case Operator.Modulo:
                        return l % r;
                    case Operator.Multiplication:
                        return l * r;
                    case Operator.ForwardSlash:
                        return l / r;
                    case Operator.BitAnd:
                        return l & r;
                    case Operator.BitOr:
                        return l | r;
                    case Operator.BitExOr:
                        return l ^ r;
                    case Operator.LeftShift:
                        return l << r;
                    case Operator.RightShift:
                        return l >> r;
                    case Operator.LogAnd:
                        return l && r;
                    case Operator.LogOr:
                        return l || r;
                    case Operator.LogEqual:
                        return l == r;
                    case Operator.LogNotEqual:
                        return l != r;
                    case Operator.LogLess:
                        return l < r;
                    case Operator.LogGreater:
                        return l > r;
                    case Operator.LogLessEqual:
                        return l <= r;
                    case Operator.LogGreaterEqual:
                        return l >= r;
                    default:
                        throw new InvalidOperatorException("Expecting only binary operators")
                        { InvalidOperator = Operator };
                }
            }
        }
    }
}
