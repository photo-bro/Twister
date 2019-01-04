using System.Collections.Generic;
namespace Twister.Compiler.Common.Interface
{
    public interface IScanner<T>
    {
        T InvalidItem { get; }

        int Offset { get; }

        int Base { get; set; }

        int Position { get; }

        int SourceLength { get; }

        IEnumerable<T> CurrentWindow { get; }

        T Advance();

        T Advance(int count);

        T Peek();

        T Peek(int count);

        bool IsAtEnd();

        void Reset();
    }
}
