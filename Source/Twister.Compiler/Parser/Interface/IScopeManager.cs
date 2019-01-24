namespace Twister.Compiler.Parser.Interface
{
    public interface IScopeManager
    {
        IScope ActiveScope { get; }

        int TotalDepth { get; }

        IScope NewScope();

        void RemoveBottomScope();
    }
}
