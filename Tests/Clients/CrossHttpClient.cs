using PicPay.Web.Features.Cross.CreateUser;

namespace PicPay.Tests.Clients;

public static class CrossHttpClient
{
    public static async Task<OneOf<CreateUserOut, ErrorOut>> CreateUser(
        this HttpClient http,
        UserType type = UserType.Customer,
        string name = "João da Silva",
        string document = "833.779.773-84",
        string email = "joao.da.silva@gmail.com",
        string password = "bfD43ae8c46cb9fd18"
    ) {
        var client = new CreateUserClient(http);
        return await client.Create(type, name, document, email, password);
    }
}
