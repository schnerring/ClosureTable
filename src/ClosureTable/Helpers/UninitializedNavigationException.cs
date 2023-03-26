namespace ClosureTable.Helpers;

public class UninitializedNavigationException : InvalidOperationException
{
    public UninitializedNavigationException(string navigationPropertyName)
        : base($"Navigation property {navigationPropertyName} is uninitialized")
    {
    }
}
