// https://github.com/domn1995/dunet/blob/f5cbbc350e347bafc9dacb7db66aea33a5c96f15/src/Dunet/UnionAttribute.cs

using System;

namespace Dunet
{
    /// <summary>
    /// Enables dunet union source generation for the decorated partial record.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class UnionAttribute : Attribute { }
}
