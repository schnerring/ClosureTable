# ClosureTable

Store self-referencing hierarchies with .NET and Entity Framework.

## Getting Started

## Examples

```goat
roots:  A    B    C    D    E    F    G    H    I
                                                |
                                             .--+--.
                                            |   |   |
                                            J   M   N
                                            |
                                            K
                                            |
                                            L
```

### Root / Parent

```csharp
context.Entities.Roots<MyEntity, int>() // [A..I]
```

### Ancestors

### Descendants

## See Also

### Curiously Recurring Template Pattern

- [Modern C# Techniques, Part 1: Curiously Recurring Generic Pattern](https://blog.stephencleary.com/2022/09/modern-csharp-techniques-1-curiously-recurring-generic-pattern.html) by Stephen Cleary
- [Curiouser and curiouser](https://ericlippert.com/2011/02/02/curiouser-and-curiouser/) by Eric Lippert
