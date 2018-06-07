namespace Funk
{
    public class Token
    {
        public readonly TokenType Type;
        public readonly string Value;

        public Token(TokenType type, string value = "")
        {
            Type = type;
            Value = value;
        }

        public override string ToString()
        {
            string str = $"Token({Type}";

            if (Value.Length > 0)
            {
                str += $", \"{Value}\"";
            }

            str += ")";

            return str;
        }
    }
}
