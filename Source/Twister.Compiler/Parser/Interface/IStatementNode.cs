using Twister.Compiler.Parser.Enum;

namespace Twister.Compiler.Parser.Interface
{
    public interface IStatementNode : IValueNode<INode[]>
    {
        StatementKind StatementKind { get; }
    }
}
