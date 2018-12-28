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

        char Advance();

        char Advance(int count);

        char Peek();

        char Peek(int count);

        bool IsAtEnd();

        void Reset();
    }
}
