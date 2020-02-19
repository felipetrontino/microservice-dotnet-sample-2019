using Framework.Core.Helpers;
using Framework.Web.Common;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Framework.Web.Extensions
{
    public static class HttpExtensions
    {
        public static StringContent ToStringContent(this object value)
        {
            return new StringContent(value?.JsonSerialize(), Encoding.UTF8, HttpWebNames.JsonContentType);
        }

        public static FormUrlEncodedContent ToFormUrlEncodedContent(this object value)
        {
            var values = new Dictionary<string, string>();

            if (value == null) return new FormUrlEncodedContent(values);

            var properties = value.GetType().GetProperties();

            foreach (var prop in properties)
            {
                values.Add(prop.Name, prop.GetValue(value)?.ToString());
            }

            return new FormUrlEncodedContent(values);
        }
    }
}