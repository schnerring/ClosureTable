namespace ClosureTable.NET.Helpers;

public static class Extensions
{
    public static T AssertNavigationLoaded<T>(this T? @this, string navigationName) where T : class
    {
        return @this ?? throw new UninitializedNavigationException(navigationName);
    }
}
