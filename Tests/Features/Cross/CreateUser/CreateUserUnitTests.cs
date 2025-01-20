using PicPay.Api.Errors;
using PicPay.Tests.Data;
using PicPay.Tests.Extensions;
using PicPay.Api.Features.Cross.CreateUser;

namespace PicPay.Tests.Features.Cross.CreateUser;

public class CreateUserUnitTests
{
    [Test]
    [TestCaseSource(typeof(Documents), nameof(Documents.InvalidCpfs))]
    public void Should_not_create_customer_with_invalid_cpf(string cpf)
    {
        // Arrange
        var result = PicPayUser.New(UserRole.Customer, "Zezinho", cpf, "zezinho@gmail.com");

        // Act / Assert
        result.ShouldBeError(new InvalidDocument());
    }

    [Test]
    [TestCaseSource(typeof(Documents), nameof(Documents.ValidCpfs))]
    public void Should_create_customer_with_valid_cpf(string cpf)
    {
        // Arrange
        var result = PicPayUser.New(UserRole.Customer, "Zezinho", cpf, "zezinho@gmail.com");

        // Act / Assert
        result.ShouldBeSuccess();
    }

    [Test]
    [TestCaseSource(typeof(Documents), nameof(Documents.InvalidCnpjs))]
    public void Should_not_create_merchant_with_invalid_cnpj(string cnpj)
    {
        // Arrange
        var result = PicPayUser.New(UserRole.Merchant, "Zezinho", cnpj, "zezinho@gmail.com");

        // Act / Assert
        result.ShouldBeError(new InvalidDocument());
    }

    [Test]
    [TestCaseSource(typeof(Documents), nameof(Documents.ValidCnpjs))]
    public void Should_create_merchant_with_valid_cnpj(string cnpj)
    {
        // Arrange
        var result = PicPayUser.New(UserRole.Merchant, "Zezinho", cnpj, "zezinho@gmail.com");

        // Act / Assert
        result.ShouldBeSuccess();
    }
}
