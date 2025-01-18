using PicPay.Web.Features.Cross.Login;
using PicPay.Web.Features.Cross.GetWallet;
using PicPay.Web.Features.Cross.CreateCustomer;
using PicPay.Web.Features.Cross.CreateMerchant;
using PicPay.Web.Features.Cross.GetNotifications;

namespace PicPay.Tests.Clients;

public static class CrossHttpClient
{
    public static async Task<OneOf<CreateCustomerOut, ErrorOut>> CreateCustomer(
        this HttpClient http,
        string name = "João da Silva",
        string cpf = "833.779.773-84",
        string email = "joao.da.silva@gmail.com",
        string password = "bfD43ae8c46cb9fd18"
    ) {
        var client = new CreateCustomerClient(http);
        return await client.Create(name, cpf, email, password);
    }

    public static async Task<OneOf<CreateMerchantOut, ErrorOut>> CreateMerchant(
        this HttpClient http,
        string name = "Gilbirdelson Lanches",
        string cnpj = "55.774.025/0001-34",
        string email = "gilbirdelson.lanches@gmail.com",
        string password = "dc9ab8a5960b44edbcd71ba5ec1a0f"
    ) {
        var client = new CreateMerchantClient(http);
        return await client.Create(name, cnpj, email, password);
    }

    public static async Task<OneOf<LoginOut, ErrorOut>> Login(this HttpClient http, string email, string password)
    {
        var client = new LoginClient(http);
        var response = await client.Login(email, password);

        http.Logout();
        http.AddAuthToken(response.IsSuccess() ? response.GetSuccess().AccessToken : "");

        return response;
    }

    public static async Task<GetWalletOut> GetWallet(this HttpClient http)
    {
        var client = new GetWalletClient(http);
        return await client.Get();
    }

    public static async Task<List<GetNotificationOut>> GetNotifications(this HttpClient http)
    {
        var client = new GetNotificationsClient(http);
        return await client.Get();
    }

    public static void Logout(this HttpClient client)
    {
        client.DefaultRequestHeaders.Remove("Authorization");
    }

    public static void AddAuthToken(this HttpClient client, string token)
    {
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
    }
}
