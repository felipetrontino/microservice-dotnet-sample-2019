using System;
using System.Net.Http;

namespace Framework.Test.Mock.Web
{
    public class HttpClientStub : HttpClient
    {
        public HttpClientStub(HttpMessageHandler handler)
            : base(handler)
        {
            BaseAddress = new Uri("");
        }

        public static HttpClient Create(HttpResponseMessage response) => new HttpClientStub(HttpMessageHandlerStub.Create(x => response));

        public static HttpClient Create(Exception ex) => new HttpClientStub(HttpMessageHandlerStub.Create(x => { throw ex; }));
    }
}