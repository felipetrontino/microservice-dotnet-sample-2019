using Framework.Web.Extensions;
using System.Net;
using System.Net.Http;

namespace Framework.Test.Mock.Web
{
    public static class HttpResponseMessageMock
    {
        public static HttpResponseMessage GetSuccess(object responseValue = null)
        {
            var ret = new HttpResponseMessage(HttpStatusCode.OK);

            if (responseValue == null)
                responseValue = string.Empty;

            ret.Content = responseValue.ToStringContent();

            return ret;
        }

        public static HttpResponseMessage GetError(object responseValue = null)
        {
            var ret = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            if (responseValue == null)
                responseValue = string.Empty;

            ret.Content = responseValue.ToStringContent();

            return ret;
        }
    }
}