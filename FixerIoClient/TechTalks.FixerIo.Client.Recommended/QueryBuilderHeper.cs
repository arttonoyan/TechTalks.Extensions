using System.Collections.Generic;
using System.Text;

namespace TechTalks.FixerIo.Client.Recommended
{
    public static class QueryBuilderHeper
    {
        public static string BuildQuery(params KeyValuePair<string, string>[] requestParams)
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
    }
}
