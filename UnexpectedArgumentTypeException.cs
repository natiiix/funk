namespace Funk
{
    public class UnexpectedArgumentTypeException : FatalException
    {
        public UnexpectedArgumentTypeException(string functionName) : base($"Unexpected argument type passed to function \"{functionName}\"") { }
    }
}
