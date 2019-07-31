#if NETSTANDARD2_0

namespace System.Collections.Generic
{
    internal static class StackExtensions
    {
        public static bool TryPop<T>(this Stack<T> stack, out T result)
        {
            if (stack.Count > 0)
            {
                result = stack.Pop();
                return true;
            }

#pragma warning disable CS8653 // A default expression introduces a null value for a type parameter.
            result = default;
#pragma warning restore CS8653 // A default expression introduces a null value for a type parameter.
            return false;
        }
    }
}

#endif
