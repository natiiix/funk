using System;
using System.Collections.Generic;

using StringEnum = Funk.BetterEnumerator<char>;

namespace Funk
{
    public static class Lexer
    {
        public static List<Token> Tokenize(string code)
        {
            if (code.Length == 0)
            {
                return new List<Token>();
            }

            List<Token> tokens = new List<Token>();
            StringEnum codeEnum = new StringEnum(code);

            do
            {
                char c = codeEnum.Current;

                // Whitespace
                if (char.IsWhiteSpace(c))
                {
                    continue;
                }
                // Symbol
                else if (char.IsLetter(c) || c == '_')
                {
                    tokens.Add(new Token(TokenType.Symbol, ReadWhile(codeEnum, x => char.IsLetterOrDigit(x) || x == '_')));
                }
                // Number
                else if (char.IsDigit(c) || c == '-')
                {
                    tokens.Add(new Token(TokenType.Number, ReadWhile(codeEnum, x => char.IsDigit(x))));
                }
                // Open parenthesis
                else if (c == '(')
                {
                    tokens.Add(new Token(TokenType.OpenParenthesis));
                }
                // Close parenthesis
                else if (c == ')')
                {
                    tokens.Add(new Token(TokenType.CloseParenthesis));
                }
                // Invalid character
                else
                {
                    throw new FatalException($"Unexpected character: \"{c}\"");
                }
            }
            while (codeEnum.MoveNext());

            return tokens;
        }

        private static string ReadWhile(StringEnum codeEnum, Predicate<char> condition)
        {
            string str = codeEnum.Current.ToString();

            while (codeEnum.NextAvailable && condition(codeEnum.Next))
            {
                str += codeEnum.Next;
                codeEnum.MoveNext();
            }

            return str;
        }
    }
}
