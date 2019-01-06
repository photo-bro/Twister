using Twister.Compiler.Lexer.Enum;
using Twister.Compiler.Lexer.Token;
using Twister.Compiler.Parser.Enum;
using Twister.Compiler.Parser.Interface;
using Twister.Compiler.Parser.Primitive;

namespace Twister.Compiler.Parser.Node
{
    public class ConditionalExpressionNode : IExpressionNode<bool>
    {
        private Operator _operator;

        public ConditionalExpressionNode(IExpressionNode<TwisterPrimitive> left, IExpressionNode<TwisterPrimitive> right,
            Operator @operator)
        {
            Left = left;
            Right = right;
            Operator = @operator;
        }

        public Operator Operator
        {
            get => _operator;
            private set
            {
                if (!value.IsConditionalOperator())
                    throw new InvalidOperatorException("Expecting conditional operator")
                    { InvalidOperator = value };
                _operator = value;
            }
        }

        public bool Value
        {
            get
            {
                var l = ((IExpressionNode<TwisterPrimitive>)Left).Value;
                var r = ((IExpressionNode<TwisterPrimitive>)Right).Value;

                switch (_operator)
                {
                    case Operator.LogAnd:
                        return l && r;
                    case Operator.LogOr:
                        return l || r;
                    case Operator.LogEqual:
                        return l == r;
                    case Operator.LogNotEqual:
                        return l != r;
                    default:
                        throw new InvalidOperatorException("Expecting conditional operator")
                        { InvalidOperator = _operator };
                }
            }
        }

        public NodeKind Kind => NodeKind.Expression;

        public ExpressionKind ExpressionKind => ExpressionKind.Conditional;

        public INode Left { get; private set; }

        public INode Right { get; private set; }

    }

    public class BinaryArithemeticExpressionNode : IExpressionNode<TwisterPrimitive>
    {
        private Operator _operator;

        public BinaryArithemeticExpressionNode(IExpressionNode<TwisterPrimitive> left, IExpressionNode<TwisterPrimitive> right,
                Operator @operator)
        {
            Left = left;
            Right = right;
            Operator = @Operator;
        }

        public Operator Operator
        {
            get => _operator;
            private set
            {
                if (!value.IsBinaryArithmeticOperator())
                    throw new InvalidOperatorException("Expecting binary arithmetic operator")
                    { InvalidOperator = value };
                _operator = value;
            }
        }

        public TwisterPrimitive Value
        {
            get
            {
                var l = ((IExpressionNode<TwisterPrimitive>)Left).Value;
                var r = ((IExpressionNode<TwisterPrimitive>)Right).Value;

                switch (_operator)
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
                        {
                            switch (r.Type)
                            {
                                case PrimitiveType.Int:
                                    break;
                                case PrimitiveType.UInt:
                                    break;
                                default:
                                    throw new InvalidCastException("Shift can only be performed with int or uint types.")
                                    {
                                        FromType = $"{r.Type}",
                                        ToType = $"int"
                                    };
                            }

                            return l << r;
                        }
                    case Operator.RightShift:
                        {
                            switch (r.Type)
                            {
                                case PrimitiveType.Int:
                                    break;
                                case PrimitiveType.UInt:
                                    break;
                                default:
                                    throw new InvalidCastException("Shift can only be performed with int or uint types.")
                                    {
                                        FromType = $"{r.Type}",
                                        ToType = $"int"
                                    };
                            }

                            return l >> r;
                        }
                }
                throw new InvalidOperatorException("Expecting binary arithmetic operator")
                { InvalidOperator = _operator };
            }
        }

        public NodeKind Kind => NodeKind.Expression;

        public ExpressionKind ExpressionKind => ExpressionKind.BinaryArithemtic;

        public INode Left { get; private set; }

        public INode Right { get; private set; }
    }


    public class UnaryExpressionNode : IExpressionNode<TwisterPrimitive>
    {
        private Operator _operator;

        public UnaryExpressionNode(IExpressionNode<TwisterPrimitive> node, Operator @operator)
        {
            Node = node;
            Operator = @operator;
        }

        public Operator Operator
        {
            get => _operator;
            private set
            {
                if (!value.IsUnaryArithmeticOperator())
                    throw new InvalidOperatorException("Expecting unary arithmetic operator")
                    { InvalidOperator = value };
                _operator = value;
            }
        }

        public TwisterPrimitive Value
        {
            get
            {
                var l = ((IExpressionNode<TwisterPrimitive>)Node).Value;
                switch (_operator)
                {
                    case Operator.Plus:
                        return l;
                    case Operator.Minus:
                        return -1 * l;
                    case Operator.LogNot:
                        return !l;
                    case Operator.BitNot:
                        return ~l;
                }
                throw new InvalidOperatorException("Expecting unary arithmetic operator")
                { InvalidOperator = _operator };
            }
        }

        public NodeKind Kind => NodeKind.Expression;

        public ExpressionKind ExpressionKind => ExpressionKind.Unary;

        public IValueNode<TwisterPrimitive> Node { get; private set; }
    }

}
