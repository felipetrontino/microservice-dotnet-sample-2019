using Framework.Web.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Framework.Web.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalErrorMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalErrorMiddleware>();
        }
    }
}