using System.Collections.Generic;
using Twister.Compiler.Parser.Enum;

namespace Twister.Compiler.Parser.Interface
{
    public interface IFuncNode<T> : IValueNode<T>
    {
        TwisterType? ReturnType { get; }

        string Identifier { get; }

        IList<INode> Body { get; set; }

        IScope Scope { get; }
    }
}
