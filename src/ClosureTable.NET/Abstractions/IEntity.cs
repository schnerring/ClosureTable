namespace ClosureTable.NET.Abstractions;

public interface IEntity<out TKey> where TKey : notnull
{
    TKey Id { get; }
}
