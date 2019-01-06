using System;
using Twister.Compiler.Parser.Enum;
using Twister.Compiler.Parser.Interface;
using Twister.Compiler.Parser.Primitive;

namespace Twister.Compiler.Parser.Node
{
    public class PrimitiveNode : IValueNode<TwisterPrimitive>
    {
        public PrimitiveNode(TwisterPrimitive value)
        { Value = value; }

        public TwisterPrimitive Value { get; private set; }

        public NodeKind Kind =>NodeKind.Primitive;
    }
}
