using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Template.Api.IntegrationTests.Helpers;

namespace Template.Api.IntegrationTests.Extensions
{
    public static class HttpExtensions
    {
        public static async Task<T> DeserializeResponse<T>(this HttpResponseMessage response,
            JsonSerializerOptions options = null)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var content =
                JsonSerializer.Deserialize<T>(responseContent, options ?? TestOptions.GetJsonSerializerOptions());
            return content;
        }

        public static StringContent SerializeRequest<T>(this T request, JsonSerializerOptions options = null)
        {
            return new StringContent(
                JsonSerializer.Serialize(request, options ?? TestOptions.GetJsonSerializerOptions()),
                Encoding.UTF8, "application/json");
        }
    }
}
