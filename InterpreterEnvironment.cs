namespace Funk
{
    public class InterpreterEnvironment
    {
        public readonly bool HasParent;
        public readonly InterpreterEnvironment Parent;

        public InterpreterEnvironment()
        {
            HasParent = false;
            Parent = null;
        }

        public InterpreterEnvironment(InterpreterEnvironment parent)
        {
            HasParent = true;
            Parent = parent;
        }
    }
}
