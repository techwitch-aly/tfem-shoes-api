using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using tfemshoes.Domain.Entities;

namespace tfemshoes.API.Authorization
{
    /// <summary>
    /// Attribute for indicating that a controller method is a protected route
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        /// <summary>
        /// Logic to run when attribute is authorizing a request
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
            {
                return;
            }

            var user = (User?)context.HttpContext.Items["User"];
            if (user == null)
            {
                // not logged in - 401
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };

                // set 'WWW-Authenticate' header
                context.HttpContext.Response.Headers["WWW-Authenticate"] = "Basic realm=\"\", charset=\"UTF-8\"";
            }
        }
    }
}
