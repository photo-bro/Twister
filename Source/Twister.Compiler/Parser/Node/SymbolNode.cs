using System;
using Twister.Compiler.Parser.Enum;
using Twister.Compiler.Parser.Interface;
using Twister.Compiler.Parser.Primitive;
using Twister.Compiler.Parser.Symbol;

namespace Twister.Compiler.Parser.Node
{
    public class SymbolNode : IValueNode<TwisterPrimitive>
    {
        public SymbolNode(string identifier, IScope scope)
        {
            Identifier = identifier;
            Scope = scope;
        }

        public TwisterPrimitive Value
        {
            get
            {
                var sym = Scope.GetSymbol(Identifier);
                return sym.GetPrimitiveValue();
            }
        }

        public NodeKind Kind => NodeKind.Symbol;


        public string Identifier { get; private set; }

        public IScope Scope { get; private set; }
    }
}
