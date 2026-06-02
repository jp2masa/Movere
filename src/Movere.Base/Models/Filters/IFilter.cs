namespace Movere.Models.Filters
{
    public interface IFilter<in T>
    {
        bool Matches(T value);
    }
}
