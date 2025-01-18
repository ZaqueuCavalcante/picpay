using System.Text.Json;
using System.Text.Json.Serialization;

public static class HttpClientExtensions
{
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };
    
    public static async Task<T> GetFromJsonAsync<T>(this HttpClient httpClient, string requestUri)
    {
        var response = await httpClient.GetStringAsync(requestUri);
        return JsonSerializer.Deserialize<T>(response, Options)!;
    }
}
