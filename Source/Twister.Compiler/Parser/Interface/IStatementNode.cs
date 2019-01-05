using Twister.Compiler.Parser.Enum;

namespace Twister.Compiler.Parser.Interface
{
    public interface IStatementNode : INode
    {
        StatementKind StatementKind { get; }

        INode[] Body { get; }
    }
}
