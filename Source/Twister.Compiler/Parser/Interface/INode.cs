namespace Twister.Compiler.Parser.Interface
{
    public interface INode
    {
        INode Left { get; set; }

        INode Right { get; set; }
    }
}
