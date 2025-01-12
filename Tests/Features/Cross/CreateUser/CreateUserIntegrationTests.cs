using PicPay.Api.Errors;
using System.Net.Http.Json;
using PicPay.Tests.Extensions;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_not_create_user_with_invalid_document()
    {
        // Arrange
        var client = _api.CreateClient();

        var data = new CreateUserIn()
        {
            Type = UserType.Customer,
            Name = "João da Silva",
            Document = "1234",
            Email = "joaodasilva@gmail.com",
            Password = "bfD43ae8c46cb9fd18"
        };

        // Act
        var response = await client.PostAsJsonAsync("/users", data);

        // Assert
        await response.AssertBadRequest(new InvalidDocument());
    }
}
