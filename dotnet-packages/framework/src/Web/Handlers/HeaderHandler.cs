using Framework.Core.Common;
using Framework.Web.Common;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Web.Handlers
{
    public class HeaderHandler : DelegatingHandler
    {
        private readonly IUserAccessor _userAccessor;
        private readonly ITokenAccessor _tokenAccessor;

        public HeaderHandler(IUserAccessor userAccessor = null, ITokenAccessor tokenAccessor = null)
        {
            _userAccessor = userAccessor;
            _tokenAccessor = tokenAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_userAccessor != null && !request.Headers.Contains(HttpHeaderNames.UserName))
                request.Headers.Add(HttpHeaderNames.UserName, _userAccessor.UserName);

            if (_tokenAccessor != null && !request.Headers.Contains(HttpHeaderNames.Authorization))
                request.Headers.Add(HttpHeaderNames.Authorization, $"Bearer {_tokenAccessor.Token}");

            var response = await base.SendAsync(request, cancellationToken);

            return response;
        }
    }
}