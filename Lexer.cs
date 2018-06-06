using System.Collections.Generic;

namespace Funk
{
    public static class Lexer
    {
        public static List<Token> Tokenize(string code)
        {
            List<Token> tokens = new List<Token>();

            BetterEnumerator<char> codeEnum = new BetterEnumerator<char>(code);

            // TODO

            return tokens;
        }
    }
}
