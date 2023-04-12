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

Let `var l = context.TestEntities.First(e => e.Name == "L")`:

| Property                       | Value          |
| ------------------------------ | -------------- |
| `l.Ancestors`                  | `[I, J, K, L]` |
| `l.Ancestors.Count`            | `4`            |
| `l.Ancestors.Any()`            | `true`         |
| `l.AncestorsWithoutSelf`       | `[I, J, K]`    |
| `l.AncestorsWithoutSelf.Count` | `3`            |
| `l.AncestorsWithoutSelf.Any()` | `true`         |

Let `var i = context.TestEntities.First(e => e.Name == "I")`:

| Property                       | Value   |
| ------------------------------ | ------- |
| `i.Ancestors`                  | `[I]`   |
| `i.Ancestors.Count`            | `1`     |
| `i.Ancestors.Any()`            | `true`  |
| `i.AncestorsWithoutSelf`       | `[]`    |
| `i.AncestorsWithoutSelf.Count` | `0`     |
| `i.AncestorsWithoutSelf.Any()` | `false` |

### Descendants

Let `var l = context.TestEntities.First(e => e.Name == "L")`:

| Property                         | Value   |
| -------------------------------- | ------- |
| `l.Descendants`                  | `[L]`   |
| `l.Descendants.Count`            | `1`     |
| `l.Descendants.Any()`            | `true`  |
| `l.DescendantsWithoutSelf`       | `[]`    |
| `l.DescendantsWithoutSelf.Count` | `0`     |
| `l.DescendantsWithoutSelf.Any()` | `false` |

Let `var i = context.TestEntities.First(e => e.Name == "I")`:

| Property                         | Value                   |
| -------------------------------- | ----------------------- |
| `i.Descendants`                  | `[I, J, K, L, M, N, O]` |
| `i.Descendants.Count`            | `7`                     |
| `i.Descendants.Any()`            | `true`                  |
| `i.DescendantsWithoutSelf`       | `[J, K, L, M, N, O]`    |
| `i.DescendantsWithoutSelf.Count` | `6`                     |
| `i.DescendantsWithoutSelf.Any()` | `true`                  |

## See Also

### Curiously Recurring Template Pattern

- [Modern C# Techniques, Part 1: Curiously Recurring Generic Pattern](https://blog.stephencleary.com/2022/09/modern-csharp-techniques-1-curiously-recurring-generic-pattern.html)
  by Stephen Cleary
- [Curiouser and curiouser](https://ericlippert.com/2011/02/02/curiouser-and-curiouser/)
  by Eric Lippert
