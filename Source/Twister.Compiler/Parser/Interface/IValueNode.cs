namespace Twister.Compiler.Parser.Interface
{
    public interface IValueNode<T> : INode
    {
        T Value { get; set; }
    }
}
