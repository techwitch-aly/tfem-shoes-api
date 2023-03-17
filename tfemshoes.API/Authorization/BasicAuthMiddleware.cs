using System.Net.Http.Headers;
using System.Text;
using tfemshoes.Domain.Service;

namespace tfemshoes.API.Authorization
{
    /// <summary>
    /// Custom middleware for adding simple Basic Authentication for protected routes
    /// </summary>
    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Delegate constructor
        /// </summary>
        /// <param name="next">Next delegate to process the request</param>
        public BasicAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Method called by the host when executing the custom middleware
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userService"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context, IUserService userService, ILogger<BasicAuthMiddleware> logger)
        {
            try
            {
                var authenticationHeader = AuthenticationHeaderValue.Parse(context.Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authenticationHeader.Parameter ?? "");
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);
                var user = credentials[0];
                var password = credentials[1];

                var checkedUser = await userService.Authenticate(user, password);
                if (checkedUser != null)
                {
                    context.Items["User"] = checkedUser;
                }
            }
            catch (Exception ex)
            {
                // Empty catch, do nothing with auth calls fail or anything is wrong with auth info
                // user will not be attached to context and will only have access to anonymous routes
                logger.LogError("Failed to parse authentication from AuthenticationHeader. Exception {Message}", ex.Message);
            }

            await _next(context);
        }
    }
}
