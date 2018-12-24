using System.Collections.Generic;
using System.Text;
using Twister.Compiler.Lexer.Interface;

namespace Twister.Compiler.Lexer.Token
{
    public static class TokenExtensions
    {
        public static string ToFormattedString(this IEnumerable<IToken> tokens)
        {
            var sb = new StringBuilder();
            var count = 0;
            foreach (var token in tokens)
                sb.AppendLine($"{count++}: {token}");
            return sb.ToString();
        }
    }
}
