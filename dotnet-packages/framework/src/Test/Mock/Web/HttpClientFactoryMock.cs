using NSubstitute;
using System;
using System.Net.Http;

namespace Framework.Test.Mock.Web
{
    public static class HttpClientFactoryMock
    {
        public static IHttpClientFactory CreateWithResponse(HttpResponseMessage response)
        {
            var mock = Substitute.For<IHttpClientFactory>();
            mock.CreateClient(Arg.Any<string>()).Returns(HttpClientStub.Create(response));

            return mock;
        }

        public static IHttpClientFactory CreateWithException(Exception exception)
        {
            var mock = Substitute.For<IHttpClientFactory>();
            mock.CreateClient(Arg.Any<string>()).Returns(HttpClientStub.Create(exception));

            return mock;
        }
    }
}