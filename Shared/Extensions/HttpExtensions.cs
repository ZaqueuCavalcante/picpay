using System.Net;
using Newtonsoft.Json;
using OneOf;

namespace PicPay.Shared;

public static class HttpExtensions
{
    public static async Task<ErrorOut> ToError(this HttpResponseMessage httpResponse)
    {
        var responseAsString = await httpResponse.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<ErrorOut>(responseAsString)!;
    }

    public static async Task<T> DeserializeTo<T>(this HttpResponseMessage httpResponse)
    {
        var responseAsString = await httpResponse.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(responseAsString)!;
    }

    public static async Task<OneOf<T, ErrorOut>> Resolve<T>(this HttpResponseMessage httpResponse)
    {
        if (httpResponse.IsSuccessStatusCode)
            return await httpResponse.DeserializeTo<T>();

        if (httpResponse.StatusCode == HttpStatusCode.Forbidden)
            return new ForbiddenErrorOut();

        return await httpResponse.ToError();
    }
}
