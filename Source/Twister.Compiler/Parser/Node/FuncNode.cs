using System;
using Twister.Compiler.Parser.Enum;
using Twister.Compiler.Parser.Interface;

namespace Twister.Compiler.Parser.Node
{
    public class FuncNode<T> : IValueNode<T>
    {
        public T Value { get; set; }

        public NodeKind Kind => NodeKind.Function;

        public INode Left { get; set; }

        public INode Right { get; set; }
    }
}
