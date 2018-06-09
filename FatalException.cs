using System;

namespace Funk
{
    public class FatalException : Exception
    {
        public FatalException(string message) : base(message) { }
    }
}
