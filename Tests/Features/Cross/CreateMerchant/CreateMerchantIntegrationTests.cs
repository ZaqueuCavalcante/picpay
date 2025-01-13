using PicPay.Api.Errors;
using PicPay.Tests.Clients;
using PicPay.Tests.Extensions;

namespace PicPay.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_not_create_merchant_with_invalid_document()
    {
        // Arrange
        var client = _api.GetClient();

        // Act
        var response = await client.CreateMerchant(cnpj: "1234");

        // Assert
        response.ShouldBeError(new InvalidDocument());
    }
}
