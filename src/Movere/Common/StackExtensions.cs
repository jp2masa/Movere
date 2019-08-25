#if NETSTANDARD2_0

using System.Runtime.CompilerServices;

namespace System.Collections.Generic
{
    internal static class StackExtensions
    {
        public static bool TryPop<T>(this Stack<T> stack, [MaybeNullWhenFalse] out T result)
        {
            if (stack.Count > 0)
            {
                result = stack.Pop();
                return true;
            }

            result = default!;
            return false;
        }
    }
}

#endif
