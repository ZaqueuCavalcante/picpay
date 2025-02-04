using PicPay.Tests.Data;
using PicPay.Api.Errors;
using PicPay.Tests.Clients;
using PicPay.Tests.Extensions;

namespace PicPay.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_not_login_when_user_not_exists()
    {
        // Arrange
        var client = _api.GetClient();

        var email = Emails.New;
        var password = "bfD43ae@8c46cb9fd18";

        // Act
        var response = await client.Login(email, password);

        // Assert
        response.ShouldBeError(new UserNotFound());
    }

    [Test]
    public async Task Should_not_login_with_wrong_password()
    {
        // Arrange
        var client = _api.GetClient();

        var cpf = "398.343.919-51";
        var email = Emails.New;
        var password = "bfD43ae@8c46cb9fd18";
        await client.CreateCustomer(cpf: cpf, email: email, password: password);

        // Act
        var response = await client.Login(email, "opa" + password);

        // Assert
        response.ShouldBeError(new WrongPassword());
    }

    [Test]
    public async Task Should_login_as_customer()
    {
        // Arrange
        var client = _api.GetClient();

        var cpf = "660.097.553-95";
        var email = Emails.New;
        var password = "bfD43ae@8c46cb9fd18";
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
        var password = "bfD43ae@8c46cb9fd18";
        await client.CreateMerchant(cnpj: cnpj, email: email, password: password);

        // Act
        var response = await client.Login(email, password);

        // Assert
        response.GetSuccess().AccessToken.Should().StartWith("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.");
    }
}
