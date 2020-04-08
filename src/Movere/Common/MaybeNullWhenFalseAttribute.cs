#if NETSTANDARD2_0

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    internal sealed class MaybeNullWhenFalseAttribute : Attribute
    {
    }
}

#endif
