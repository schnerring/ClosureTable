# ClosureTable

Store self-referencing hierarchies with .NET and Entity Framework.

## Getting Started

## Examples

```goat
roots:  A    B    C    D    E    F    G    H    I
                                                |
                                           .--+-+-+--.
                                          |   |   |   |
                                          J   M   N   O
                                          |
                                          K
                                          |
                                          L
```

### Root / Parent

```csharp
context.TestEntities.Roots<MyEntity, int>() // [A..I]
```

### Ancestors

Let `var i = context.TestEntities.First(e => e.Name == "I")`:

| Property                                                    | Value   |
| ----------------------------------------------------------- | ------- |
| `i.Ancestors`                                               | `[I]`   |
| `i.Ancestors.Count`                                         | `1`     |
| `i.Ancestors.Any()`                                         | `true`  |
| `i.AncestorsWithoutSelf`                                    | `[]`    |
| `i.AncestorsWithoutSelf.Count`                              | `0`     |
| `i.AncestorsWithoutSelf.Any()`                              | `false` |
| `context.HasAncestors<TestEntity, Guid>(i.Id)`              | `false` |
| `await context.HasAncestorsAsync<TestEntity, Guid>(i.Id)`   | `false` |
| `context.AncestorsCount<TestEntity, Guid>(i.Id)`            | `0`     |
| `await context.AncestorsCountAsync<TestEntity, Guid>(i.Id)` | `0`     |

Let `var l = context.TestEntities.First(e => e.Name == "L")`:

| Property                                                    | Value          |
| ----------------------------------------------------------- | -------------- |
| `l.Ancestors`                                               | `[I, J, K, L]` |
| `l.Ancestors.Count`                                         | `4`            |
| `l.Ancestors.Any()`                                         | `true`         |
| `l.AncestorsWithoutSelf`                                    | `[I, J, K]`    |
| `l.AncestorsWithoutSelf.Count`                              | `3`            |
| `l.AncestorsWithoutSelf.Any()`                              | `true`         |
| `context.HasAncestors<TestEntity, Guid>(l.Id)`              | `true`         |
| `await context.HasAncestorsAsync<TestEntity, Guid>(l.Id)`   | `true`         |
| `context.AncestorsCount<TestEntity, Guid>(l.Id)`            | `3`            |
| `await context.AncestorsCountAsync<TestEntity, Guid>(l.Id)` | `3`            |

### Descendants

Let `var i = context.TestEntities.First(e => e.Name == "I")`:

| Property                                                      | Value                   |
| ------------------------------------------------------------- | ----------------------- |
| `i.Descendants`                                               | `[I, J, K, L, M, N, O]` |
| `i.Descendants.Count`                                         | `7`                     |
| `i.Descendants.Any()`                                         | `true`                  |
| `i.DescendantsWithoutSelf`                                    | `[J, K, L, M, N, O]`    |
| `i.DescendantsWithoutSelf.Count`                              | `6`                     |
| `i.DescendantsWithoutSelf.Any()`                              | `true`                  |
| `context.HasDescendants<TestEntity, Guid>(i.Id)`              | `true`                  |
| `await context.HasDescendantsAsync<TestEntity, Guid>(i.Id)`   | `true`                  |
| `context.DescendantsCount<TestEntity, Guid>(i.Id)`            | `6`                     |
| `await context.DescendantsCountAsync<TestEntity, Guid>(i.Id)` | `6`                     |

Let `var l = context.TestEntities.First(e => e.Name == "L")`:

| Property                                                      | Value   |
| ------------------------------------------------------------- | ------- |
| `l.Descendants`                                               | `[L]`   |
| `l.Descendants.Count`                                         | `1`     |
| `l.Descendants.Any()`                                         | `true`  |
| `l.DescendantsWithoutSelf`                                    | `[]`    |
| `l.DescendantsWithoutSelf.Count`                              | `0`     |
| `l.DescendantsWithoutSelf.Any()`                              | `false` |
| `context.HasDescendants<TestEntity, Guid>(l.Id)`              | `false` |
| `await context.HasDescendantsAsync<TestEntity, Guid>(l.Id)`   | `false` |
| `context.DescendantsCount<TestEntity, Guid>(l.Id)`            | `0`     |
| `await context.DescendantsCountAsync<TestEntity, Guid>(l.Id)` | `0`     |

## See Also

### Curiously Recurring Template Pattern

- [Modern C# Techniques, Part 1: Curiously Recurring Generic Pattern](https://blog.stephencleary.com/2022/09/modern-csharp-techniques-1-curiously-recurring-generic-pattern.html)
  by Stephen Cleary
- [Curiouser and curiouser](https://ericlippert.com/2011/02/02/curiouser-and-curiouser/)
  by Eric Lippert
