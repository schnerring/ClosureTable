namespace ClosureTable.NET.Abstractions;

public interface ISelfReferencingEntity<out TKey> where TKey : notnull
{
    TKey Id { get; }
}
