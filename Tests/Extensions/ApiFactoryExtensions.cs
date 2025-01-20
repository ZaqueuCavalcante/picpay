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
            "Admilson",
            "22.896.431/0001-10",
            "admilson@picpay.com",
            "efd46375c2a74fe6bcfc7d20f67e23ab"
        );

        await service.Create(userIn);
    }

    public static async Task<AdmHttpClient> LoggedAsAdm(this ApiFactory factory)
    {
        var client = factory.GetClient();

        await client.Login("admilson@picpay.com", "efd46375c2a74fe6bcfc7d20f67e23ab");

        return new(client);
    }

    public static async Task<CustomerHttpClient> LoggedAsCustomer(this ApiFactory factory, long? balance = 0)
    {
        var client = factory.GetClient();

        var cpf = Documents.GetRandomCpf();
        var name = Person.GetRandomName();
        var email = Emails.New;
        var password = Guid.NewGuid().ToString();
        await client.CreateCustomer(cpf: cpf, name: name, email: email, password: password);

        await client.Login(email, password);

        if (balance != null)
        {
            var wallet = await client.GetWallet();
            var admClient = await factory.LoggedAsAdm();
            await admClient.Bonus(balance.Value, wallet.Id);
        }

        return new(client);
    }

    public static async Task<MerchantHttpClient> LoggedAsMerchant(this ApiFactory factory, long? balance = 0)
    {
        var client = factory.GetClient();

        var cnpj = Documents.GetRandomCnpj();
        var name = Company.GetRandomName();
        var email = Emails.New;
        var password = Guid.NewGuid().ToString();
        await client.CreateMerchant(cnpj: cnpj, name: name, email: email, password: password);

        await client.Login(email, password);

        if (balance != null)
        {
            var wallet = await client.GetWallet();
            var admClient = await factory.LoggedAsAdm();
            await admClient.Bonus(balance.Value, wallet.Id);
        }

        return new(client);
    }
}
