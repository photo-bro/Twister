using System;
using Twister.Compiler.Lexer.Enum;
using Twister.Compiler.Lexer.Token;
using Twister.Compiler.Parser.Enum;
using Twister.Compiler.Parser.Interface;

namespace Twister.Compiler.Parser.Node
{
    public class ExpressionNode : IExpressionNode<PrimitiveValue>
    {
        public PrimitiveValue Value { get; set; }

        public NodeKind Kind => NodeKind.Expression;

        public ExpressionKind ExpressionKind => ExpressionKind.General;

        public INode Left { get; set; }

        public INode Right { get; set; }
    }

    public class ConditionalExpressionNode : IExpressionNode<bool>
    {
        private Operator _operator;

        public ConditionalExpressionNode(IExpressionNode<PrimitiveValue> left, IExpressionNode<PrimitiveValue> right)
        {
            Left = Left;
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
                switch (_operator)
                {
                    case Operator.LogAnd:
                        return ((IExpressionNode<PrimitiveValue>)Left).Value 
                            && ((IExpressionNode<PrimitiveValue>)Right).Value;
                    case Operator.LogOr:
                        return ((IExpressionNode<PrimitiveValue>)Left).Value
                            || ((IExpressionNode<PrimitiveValue>)Right).Value;
                    case Operator.LogEqual:
                        return ((IExpressionNode<PrimitiveValue>)Left).Value
                            == ((IExpressionNode<PrimitiveValue>)Right).Value;
                    case Operator.LogNotEqual:
                        return ((IExpressionNode<PrimitiveValue>)Left).Value
                            != ((IExpressionNode<PrimitiveValue>)Right).Value;
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
}
