using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Linq;

namespace WineFinder.Shared.Extensions
{
    public static class HttpRequestHeadersExtensions
    {
        public static string GetValue(this HttpRequestHeaders headers, string parameterName)
        {
            if(headers.TryGetValues(parameterName, out IEnumerable<string> key))
            {
                var keyValue = key.FirstOrDefault();
                if (!string.IsNullOrEmpty(keyValue)) return keyValue;
            }
            return null;
        }
    }
}
