using System;
using System.Collections.Generic;
using System.Linq;
using Twister.Compiler.Common.Interface;

namespace Twister.Compiler.Common
{
    public class Scanner<T> : IScanner<T>
    {
        private readonly Memory<T> _items;

        public Scanner(IEnumerable<T> items, T invalidItem)
        {
            _items = items.ToArray().AsMemory();
            InvalidItem = invalidItem;
        }

        public T InvalidItem { get; }

        public int Offset => Position - Base;

        public int Base { get; set; } = 0;

        public int Position { get; set; } = 0;

        public int SourceLength => _items.Length;

        public IEnumerable<T> CurrentWindow
        {
            get
            {
                if (Base > SourceLength)
                    return new[] { InvalidItem };

                if (Base + Offset > SourceLength)
                    return _items.Slice(Base).ToArray();

                return _items.Slice(Base, Offset).ToArray();
            }
        }

        public T Advance() => Advance(1);

        public T Advance(int count)
        {
            if (IsAtEnd())
                return InvalidItem;

            if (Position + count > SourceLength)
                return InvalidItem;

            if (count == 0)
                return _items.Span[Position];

            if (count < 0)
                throw new InvalidOperationException($"{nameof(Scanner<T>)}" +
                    $".{nameof(Scanner<T>)} can only advance forward");

            var currentSpan = _items.Slice(Position, count).Span;

            Position += count;

            return currentSpan[count - 1];
        }

        public bool IsAtEnd()
        {
            if (SourceLength == 0) return true;
            return Position >= SourceLength;
        }

        public T Peek() => Peek(1);

        public T Peek(int count)
        {
            if (Position + count > SourceLength)
                return InvalidItem;

            if (Position + count == 0)
                return _items.Span[0];

            if (Position + count < 1)
                return InvalidItem;

            return _items.Span[Position + count - 1];
        }

        public void Reset()
        {
            Base = 0;
            Position = 0;
        }
    }
}
