using System;
using Twister.Compiler.Lexer.Enum;
using Twister.Compiler.Lexer.Token;
using Twister.Compiler.Parser.Enum;
using Twister.Compiler.Parser.Interface;

namespace Twister.Compiler.Parser.Node
{
    //public class ExpressionNode : IExpressionNode<TwisterPrimitive>
    //{
    //    public TwisterPrimitive Value { get; set; }

    //    public NodeKind Kind => NodeKind.Expression;

    //    public ExpressionKind ExpressionKind => ExpressionKind.General;

    //    public INode Left { get; set; }

    //    public INode Right { get; set; }
    //}

    public class ConditionalExpressionNode : IExpressionNode<bool>
    {
        private Operator _operator;

        public ConditionalExpressionNode(IExpressionNode<TwisterPrimitive> left, IExpressionNode<TwisterPrimitive> right)
        {
            Left = left;
            Right = right;
        }

        public Operator Operator
        {
            get => _operator;
            set
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
                var l = (IExpressionNode<TwisterPrimitive>)Left;
                var r = (IExpressionNode<TwisterPrimitive>)Right;

                switch (_operator)
                {
                    case Operator.LogAnd:
                        return l.Value && r.Value;
                    case Operator.LogOr:
                        return l.Value || r.Value;
                    case Operator.LogEqual:
                        return l.Value == r.Value;
                    case Operator.LogNotEqual:
                        return l.Value != r.Value;
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

    public class ArithemeticExpressionNode : IExpressionNode<TwisterPrimitive>
    {
        private Operator _operator;

        public ArithemeticExpressionNode(IExpressionNode<TwisterPrimitive> left, IExpressionNode<TwisterPrimitive> right)
        {
            Left = Left;
            Right = right;
        }

        public Operator Operator
        {
            get => _operator;
            set
            {
                if (!value.IsArithmeticOperator())
                    throw new InvalidOperatorException("Expecting conditional operator")
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
                    //case Operator.BitNot:
                    //    return !l;  // TODO - Move to unary expression?
                    //case Operator.LeftShift:
                    //    return l << r; // TODO - Evaluate r as int
                    //case Operator.RightShift:
                        //return l >> r; // TODO - Evaluate r as int
                }
                throw new InvalidOperatorException("Expecting arithmetic operator")
                { InvalidOperator = _operator };
            }
        }

        public NodeKind Kind => NodeKind.Expression;

        public ExpressionKind ExpressionKind => ExpressionKind.Arithemtic;

        public INode Left { get; set; }

        public INode Right { get; set; }


    }

}
