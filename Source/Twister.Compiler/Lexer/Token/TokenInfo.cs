﻿using Twister.Compiler.Lexer.Enum;
namespace Twister.Compiler.Lexer.Token
{
    public struct TokenInfo
    {
        public string Text { get; set; }

        public TokenKind TokenType { get; set; }

        public int SourceLineNumber { get; set; }
    }
}
