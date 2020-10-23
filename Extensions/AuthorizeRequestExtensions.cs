using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AuthWindows.Extensions
{
    public class AuthorizeRequestExtensions
    {
        private readonly RequestDelegate _next;

        public AuthorizeRequestExtensions(RequestDelegate next)
        {
            _next = next;
        }

        // IMyScopedService is injected into Invoke
        public async Task Invoke(HttpContext httpContext)
        {
            var claims = httpContext.Request.HttpContext.User.Claims;

            var IsAuthenticated = claims.Where(x => x.Type == "IsAuthenticated" && x.Value == "true").Any();

            if (!IsAuthenticated)
            {
                //httpContext.Response.Redirect("/Home/Authenticate");                
            }
            
            await _next(httpContext);
        }
    }
}