using Twister.Compiler.Parser.Enum;
using Twister.Compiler.Parser.Interface;

namespace Twister.Compiler.Parser.Node
{
    public class IdentifierNode : IValueNode<string>
    {
        public IdentifierNode(string identifier)
        { Value = identifier; }

        public string Value { get; }

        public NodeKind Kind => NodeKind.Identifier;
    }
}
