using PicPay.Api.Errors;
using PicPay.Tests.Data;
using PicPay.Tests.Clients;
using PicPay.Tests.Extensions;

namespace PicPay.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_not_create_merchant_with_invalid_cnpj()
    {
        // Arrange
        var client = _api.GetClient();

        // Act
        var response = await client.CreateMerchant(cnpj: "55.774.025/0001-35");

        // Assert
        response.ShouldBeError(new InvalidDocument());
    }

    [Test]
    public async Task Should_not_create_merchant_with_valid_cpf()
    {
        // Arrange
        var client = _api.GetClient();

        // Act
        var response = await client.CreateMerchant(cnpj: "174.606.156-17");

        // Assert
        response.ShouldBeError(new InvalidDocument());
    }

    [Test]
    public async Task Should_not_create_merchant_with_invalid_email()
    {
        // Arrange
        var client = _api.GetClient();
        var email = Emails.Invalid().PickRandom().First().ToString()!;

        // Act
        var response = await client.CreateMerchant(email: email);

        // Assert
        response.ShouldBeError(new InvalidEmail());
    }

    [Test]
    public async Task Should_not_create_merchant_with_weak_password()
    {
        // Arrange
        var client = _api.GetClient();
        var password = Passwords.Weak().PickRandom().First().ToString()!;

        // Act
        var response = await client.CreateMerchant(password: password);

        // Assert
        response.ShouldBeError(new WeakPassword());
    }

    [Test]
    public async Task Should_not_create_merchant_with_duplicated_cnpj()
    {
        // Arrange
        var client = _api.GetClient();
        var cnpj = Documents.GetRandomCnpj();

        // Act
        var firstResponse = await client.CreateMerchant(cnpj: cnpj, email: Emails.New);
        var secondResponse = await client.CreateMerchant(cnpj: cnpj, email: Emails.New);

        // Assert
        firstResponse.ShouldBeSuccess();
        secondResponse.ShouldBeError(new DocumentAlreadyUsed());
    }

    [Test]
    public async Task Should_not_create_merchant_with_duplicated_email()
    {
        // Arrange
        var client = _api.GetClient();
        var email = Emails.New;

        // Act
        var firstResponse = await client.CreateMerchant(cnpj: Documents.GetRandomCnpj(), email: email);
        var secondResponse = await client.CreateMerchant(cnpj: Documents.GetRandomCnpj(), email: email);

        // Assert
        firstResponse.ShouldBeSuccess();
        secondResponse.ShouldBeError(new EmailAlreadyUsed());
    }

    [Test]
    public async Task Should_not_create_merchant_with_duplicated_cnpj_and_email()
    {
        // Arrange
        var client = _api.GetClient();
        var cnpj = Documents.GetRandomCnpj();
        var email = Emails.New;

        // Act
        var firstResponse = await client.CreateMerchant(cnpj: cnpj, email: email);
        var secondResponse = await client.CreateMerchant(cnpj: cnpj, email: email);

        // Assert
        firstResponse.ShouldBeSuccess();
        secondResponse.ShouldBeError(new DocumentAlreadyUsed());
    }

    [Test]
    public async Task Should_not_create_merchant_with_duplicated_cnpj_and_email_parallel_requests()
    {
        // Arrange
        var client = _api.GetClient();
        var cnpj = Documents.GetRandomCnpj();
        var email = Emails.New;

        // Act
        var first = client.CreateMerchant(cnpj: cnpj, email: email);
        var second = client.CreateMerchant(cnpj: cnpj, email: email);

        var responses = await Task.WhenAll(first, second);

        // Assert
        var error = responses.Single(t => t.IsError());
        error.ShouldBeError(new DocumentAlreadyUsed());

        CreateMerchantOut customer = responses.Single(t => t.IsSuccess()).GetSuccess();
        customer.Cnpj.Should().Be(cnpj);
        customer.Email.Should().Be(email);
    }
}
