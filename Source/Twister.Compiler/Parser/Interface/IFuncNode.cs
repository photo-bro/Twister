using System.Collections.Generic;
using Twister.Compiler.Parser.Enum;

namespace Twister.Compiler.Parser.Interface
{
    public interface IFuncNode<T> : IValueNode<T>
    {
        IList<IValueNode<ISymbol>> Parameters { get; }

        TwisterType? ReturnType { get; }

        string Identifier { get; }

        IList<INode> Body { get; set; }
    }
}
