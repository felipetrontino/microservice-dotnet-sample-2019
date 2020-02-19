using Framework.Core.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Framework.Web.Common
{
    public class HttpContextInfoAccessor : IUserAccessor, ITokenAccessor
    {
        public HttpContextInfoAccessor(IHttpContextAccessor accessor = null)
        {
            if (accessor?.HttpContext == null) return;

            var header = accessor.HttpContext.Request.Headers;
            UserName = accessor.HttpContext.User.Identity.Name ?? GetHeader(header, HttpHeaderNames.UserName);
            Token = accessor.HttpContext.GetTokenAsync("access_token").Result;
        }

        public string UserName { get; }

        public string Token { get; }

        private static string GetHeader(IHeaderDictionary header, string name)
        {
            return header.ContainsKey(name) ? header[name].ToString() : null;
        }
    }
}