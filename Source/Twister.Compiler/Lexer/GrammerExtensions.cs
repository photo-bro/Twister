using System.Linq;

namespace Twister.Compiler.Lexer
{
    public static class GrammerExtensions
    {
        public static char[] TwisterNumericChars =
        {
            '0', '1', '2','3','4','5','6','7','8','9','.','u','U'
        };

        public static bool IsTwisterNumericChar(this char c)
        {
            return TwisterNumericChars.Contains(c);
        }

        public static bool IsTwisterIdentifierOrKeywordChar(this char c)
        {
            return char.IsLetterOrDigit(c) || c == '_';
        }

    }
}
