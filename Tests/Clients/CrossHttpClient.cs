using PicPay.Web.Features.Cross.CreateCustomer;
using PicPay.Web.Features.Cross.CreateMerchant;

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
}
