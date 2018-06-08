namespace Funk
{
    public class UnexpectedTokenException : System.Exception
    {
        public UnexpectedTokenException(Token token) : base($"Unexpected token: {token}") { }
    }
}
