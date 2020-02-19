using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace Framework.Core.Helpers
{
    public static class HashHelper
    {
        public static string Create(object obj)
        {
            var responseJson = JsonConvert.SerializeObject(obj);

            using var algorithm = MD5.Create();
            var hashBytes = algorithm.ComputeHash(Encoding.Default.GetBytes(responseJson));

            var sb = new StringBuilder();
            foreach (var b in hashBytes)
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}