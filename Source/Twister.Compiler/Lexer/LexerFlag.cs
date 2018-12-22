using System;

namespace Twister.Compiler.Lexer
{
    [Flags]
    public enum LexerFlag
    {
        None = 0,
        AllowUnicode = None << 1
    }

    public static class LexerFlagExtensions
    {
        public static bool AllowUnicode(this LexerFlag flag) => (flag & LexerFlag.AllowUnicode) > 0;
    }
}
