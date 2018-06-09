namespace Funk
{
    public class UnexpectedNumberOfArgumentsException : FatalException
    {
        public UnexpectedNumberOfArgumentsException(string functionName, string expected, int received) :
            base($"Unexpected number of arguments provided to function \"{functionName}\" (Expected: {expected}, Received: {received})")
        { }

        public UnexpectedNumberOfArgumentsException(string functionName, int expected, int received) :
            this(functionName, expected.ToString(), received)
        { }
    }
}
