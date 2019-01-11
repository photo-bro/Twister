using System.Collections.Generic;
using Twister.Compiler.Parser.Enum;

namespace Twister.Compiler.Parser.Interface
{
    public interface IStatementNode : IValueNode<IList<INode>>
    {
        StatementKind StatementKind { get; }
    }
}
