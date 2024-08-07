using System.Text.Json;

namespace Template.Api.IntegrationTests.Helpers
{
    public static class TestOptions
    {
        public static JsonSerializerOptions GetJsonSerializerOptions()
        {
            return new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }
    }
}
