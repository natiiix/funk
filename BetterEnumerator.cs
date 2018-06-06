using System.Collections.Generic;
using System.Linq;

namespace Funk
{
    public class BetterEnumerator<T>
    {
        private readonly IEnumerable<T> enumerable;
        private readonly int count;
        private int index;

        public bool PreviousAvailable { get => index - 1 >= 0; }
        public T Previous { get => enumerable.ElementAt(index - 1); }

        public bool CurrentAvailable { get => index >= 0; }
        public T Current { get => enumerable.ElementAt(index); }

        public bool NextAvailable { get => index + 1 < count; }
        public T Next { get => enumerable.ElementAt(index + 1); }

        public BetterEnumerator(IEnumerable<T> enumerable)
        {
            this.enumerable = enumerable;
            count = enumerable.Count();
            Reset();
        }

        public void Reset() => index = 0;

        public bool MovePrevious()
        {
            bool available = PreviousAvailable;

            if (available)
            {
                index--;
            }

            return available;
        }

        public bool MoveNext()
        {
            bool available = NextAvailable;

            if (available)
            {
                index++;
            }

            return available;
        }
    }
}
