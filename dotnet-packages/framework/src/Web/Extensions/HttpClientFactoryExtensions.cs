using Framework.Web.Handlers;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace Framework.Web.Extensions
{
    public static class HttpClientFactoryExtensions
    {
        public const string ServiceInternalName = "api-service-internal";

        public static IHttpClientBuilder AddHttpClientServiceInternal(this IServiceCollection services)
        {
            return services.AddHttpClient(ServiceInternalName)
                .AddHttpMessageHandler<HeaderHandler>();
        }

        public static HttpClient CreateHttpClientServiceInternal(this IHttpClientFactory factory)
        {
            return factory.CreateClient(ServiceInternalName);
        }
    }
}