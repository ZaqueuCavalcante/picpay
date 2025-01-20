using PicPay.Api.Errors;
using PicPay.Tests.Clients;
using PicPay.Tests.Extensions;

namespace PicPay.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_not_create_customer_with_invalid_cpf()
    {
        // Arrange
        var client = _api.GetClient();

        // Act
        var response = await client.CreateCustomer(cpf: "171.275.980-97");

        // Assert
        response.ShouldBeError(new InvalidDocument());
    }

    [Test]
    public async Task Should_not_create_customer_with_valid_cnpj()
    {
        // Arrange
        var client = _api.GetClient();

        // Act
        var response = await client.CreateCustomer(cpf: "75.462.479/0001-87");

        // Assert
        response.ShouldBeError(new InvalidDocument());
    }


    // Email invalido
    // Senha fraca (regras do syki)
    // Documento ja usado
    // Email ja usado

}
