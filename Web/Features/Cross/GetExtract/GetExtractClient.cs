namespace PicPay.Web.Features.Cross.GetExtract;

public class GetExtractClient(HttpClient http) : IPicPayClient
{
    public async Task<List<GetExtractOut>> Get()
    {
        return await http.GetFromJsonAsync<List<GetExtractOut>>("/extract") ?? [];
    }
}
