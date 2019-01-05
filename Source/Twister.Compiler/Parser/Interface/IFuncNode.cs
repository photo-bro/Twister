using System;
using Twister.Compiler.Parser.Primitive;

namespace Twister.Compiler.Parser.Interface
{
    public interface IFuncNode<T> : IValueNode<T>
    {
        ISymbolNode<TwisterPrimitive>[] Arguments { get; }

        string Identifier { get; }
    }
}
