namespace PicPay.Api.Features.Cross.Authorize;

public class AuthorizeService(AuthorizeSettings settings, IHttpClientFactory factory) : IPicPayService
{
    public async Task<OneOf<bool, PicPayError>> Authorize(long amount)
    {
        var client = factory.CreateClient();
        client.BaseAddress = new Uri(settings.Url);
        client.Timeout = TimeSpan.FromSeconds(settings.Timeout);

        try
        {
            var response = await client.GetAsync($"api/v2/authorize?amount={amount}");
            if (!response.IsSuccessStatusCode) return new AuthorizeServiceDown();

            var authorizeOut = await response.DeserializeTo<AuthorizeOut>() ?? new AuthorizeOut();
            return authorizeOut.Data.Authorization ? true : new TransactionNotAuthorized();
        }
        catch (Exception)
        {
            return new AuthorizeServiceDown();
        }
    }
}
