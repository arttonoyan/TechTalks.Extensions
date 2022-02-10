using System.Collections.Generic;
using System.Text;

namespace TechTalks.FixerIo.Client.Recommended
{
    public static class HttpQueryBuilder
    {
        public static string Build(params KeyValuePair<string, string>[] requestParams)
        {
            if (requestParams != null && requestParams.Length > 0)
            {
                var builder = new StringBuilder();
                foreach (var (key, value) in requestParams)
                {
                    builder.Append(key).Append('=').Append(value).Append('&');
                }
                return builder.ToString().TrimEnd('&');
            }
            return "";
        }

        public static string Build(KeyValuePair<string, string> requestParam, string query) =>
            $"{requestParam.Key}={requestParam.Value}&{query}";

        public static string BuildRequest(string path, string query) =>
            $"{path}?{query}";

        public static string BuildRequest(string path, KeyValuePair<string, string> requestParam, string query) =>
            BuildRequest(path, Build(requestParam, query));
    }
}
