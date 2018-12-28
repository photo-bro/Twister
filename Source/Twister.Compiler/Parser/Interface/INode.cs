using Twister.Compiler.Parser.Enum;

namespace Twister.Compiler.Parser.Interface
{
    public interface INode
    {
        NodeKind Kind { get; }

        INode Left { get; }

        INode Right { get; }
    }
}
