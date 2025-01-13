using PicPay.Api.Errors;
using PicPay.Tests.Data;
using PicPay.Tests.Extensions;
using PicPay.Api.Features.Cross.CreateUser;

namespace PicPay.Tests.Features.Cross.CreateUser;

public class CreateUserUnitTests
{
    [Test]
    [TestCaseSource(typeof(Documents), nameof(Documents.InvalidCpfs))]
    public void Should_not_create_user_with_invalid_cpf(string cpf)
    {
        // Arrange
        var result = PicPayUser.New(UserType.Customer, "Zezinho", cpf, "zezinho@gmail.com");

        // Act / Assert
        result.ShouldBeError(new InvalidDocument());
    }

    [Test]
    [TestCaseSource(typeof(Documents), nameof(Documents.InvalidCnpjs))]
    public void Should_not_create_user_with_invalid_cnpj(string cnpj)
    {
        // Arrange
        var result = PicPayUser.New(UserType.Customer, "Zezinho", cnpj, "zezinho@gmail.com");

        // Act / Assert
        result.ShouldBeError(new InvalidDocument());
    }
}
