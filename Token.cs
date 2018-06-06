namespace Funk
{
    public class Token
    {
        public readonly TokenType Type;
        public readonly string Value;

        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}
