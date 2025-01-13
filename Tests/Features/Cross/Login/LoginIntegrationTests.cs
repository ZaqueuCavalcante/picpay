using PicPay.Tests.Data;
using PicPay.Tests.Clients;
using PicPay.Tests.Extensions;

namespace PicPay.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_login_as_customer()
    {
        // Arrange
        var client = _api.GetClient();

        var cpf = "660.097.553-95";
        var email = Emails.New;
        var password = Guid.NewGuid().ToString();
        await client.CreateCustomer(cpf: cpf, email: email, password: password);

        // Act
        var response = await client.Login(email, password);

        // Assert
        response.GetSuccess().AccessToken.Should().StartWith("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.");
    }

    [Test]
    public async Task Should_login_as_merchant()
    {
        // Arrange
        var client = _api.GetClient();

        var cnpj = "14.208.727/0001-73";
        var email = Emails.New;
        var password = Guid.NewGuid().ToString();
        await client.CreateMerchant(cnpj: cnpj, email: email, password: password);

        // Act
        var response = await client.Login(email, password);

        // Assert
        response.GetSuccess().AccessToken.Should().StartWith("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.");
    }
}
