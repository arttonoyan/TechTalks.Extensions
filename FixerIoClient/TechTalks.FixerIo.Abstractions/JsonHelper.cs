using System.Text.Json;

namespace TechTalks.FixerIo
{
    public static class JsonHelper
    {
        public static JsonSerializerOptions DefaultOptions { get; } = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true
        };
    }
}
