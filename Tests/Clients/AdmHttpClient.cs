namespace PicPay.Tests.Clients;

public class AdmHttpClient(HttpClient http)
{
    public readonly HttpClient Http = http;

    // public async Task<List<UserOut>> GetUsers()
    // {
    //     var client = new GetUsersClient(Cross);
    //     return await client.Get();
    // }
}
