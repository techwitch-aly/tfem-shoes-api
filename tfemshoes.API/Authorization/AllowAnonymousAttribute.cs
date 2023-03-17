namespace tfemshoes.API.Authorization
{
    /// <summary>
    /// Attribute for declaring a controller method allows anonymous access
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AllowAnonymousAttribute : Attribute
    {
    }
}
