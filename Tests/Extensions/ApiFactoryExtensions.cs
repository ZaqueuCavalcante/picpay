using PicPay.Tests.Data;
using PicPay.Api.Database;
using PicPay.Tests.Clients;
using PicPay.Api.Features.Cross.CreateUser;
using Microsoft.Extensions.DependencyInjection;

namespace PicPay.Tests.Extensions;

public static class ApiFactoryExtensions
{
    public static HttpClient GetClient(this ApiFactory factory)
    {
        return factory.CreateClient();
    }

    public static PicPayDbContext GetDbContext(this ApiFactory factory)
    {
        var scope = factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<PicPayDbContext>();
    }

    public static T GetService<T>(this ApiFactory factory) where T : notnull
    {
        var scope = factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<T>();
    }

    public static async Task RegisterAdm(this ApiFactory factory)
    {
        await using var ctx = factory.GetDbContext();
        var service = factory.GetService<CreateUserService>();

        var userIn = new CreateUserIn(
            UserRole.Adm,
            "PicPay",
            "22.896.431/0001-10",
            "admilson@picpay.com",
            "efd46375c2A74fe6b@cfc7d20f67e23ab"
        );

        await service.Create(userIn);
    }

    public static async Task<AdmHttpClient> LoggedAsAdm(this ApiFactory factory)
    {
        var client = factory.GetClient();

        await client.Login("admilson@picpay.com", "efd46375c2A74fe6b@cfc7d20f67e23ab");

        return new(client);
    }

    public static async Task<CustomerHttpClient> LoggedAsCustomer(this ApiFactory factory)
    {
        var client = factory.GetClient();

        var cpf = Documents.GetRandomCpf();
        var name = Person.GetRandomName();
        var email = Emails.New;
        var password = "bfD43ae@8c46cb9fd18";
        CreateCustomerOut customer = await client.CreateCustomer(cpf: cpf, name: name, email: email, password: password);

        await client.Login(email, password);

        return new(client) { UserName = name, WalletId = customer.WalletId };
    }

    public static async Task<MerchantHttpClient> LoggedAsMerchant(this ApiFactory factory)
    {
        var client = factory.GetClient();

        var cnpj = Documents.GetRandomCnpj();
        var name = Company.GetRandomName();
        var email = Emails.New;
        var password = "bfD43ae@8c46cb9fd18";
        CreateMerchantOut merchant = await client.CreateMerchant(cnpj: cnpj, name: name, email: email, password: password);

        await client.Login(email, password);

        return new(client) { WalletId = merchant.WalletId };
    }
}
