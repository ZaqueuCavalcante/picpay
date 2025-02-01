using PicPay.Tests.Extensions;

namespace PicPay.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Customer_should_get_extract_with_welcome_bonus()
    {
        // Arrange
        var client = await _api.LoggedAsCustomer();

        // Act
        await _worker.ProcessAll();

        // Assert
        var transactions = await client.GetExtract();
        transactions.Should().ContainSingle();
        transactions.First().Id.Should().NotBeEmpty();
        transactions.First().Type.Should().Be(TransactionType.WelcomeBonus);
        transactions.First().Amount.Should().Be(10_00);
        transactions.First().Other.Should().Be("PicPay");
        transactions.First().CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Test]
    public async Task Source_customer_should_get_extract_with_welcome_bonus_and_transfer()
    {
        // Arrange
        var sourceClient = await _api.LoggedAsCustomer();
        var targetClient = await _api.LoggedAsCustomer();

        // Act
        await sourceClient.Transfer(6_80, targetClient.WalletId);
        await _worker.ProcessAll();

        // Assert
        var transactions = await sourceClient.GetExtract();
        transactions.Should().HaveCount(2);

        transactions.First().Id.Should().NotBeEmpty();
        transactions.First().Type.Should().Be(TransactionType.Transfer);
        transactions.First().Amount.Should().Be(-6_80);
        transactions.First().Other.Should().Be(targetClient.UserName);
        transactions.First().CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));

        transactions.Last().Id.Should().NotBeEmpty();
        transactions.Last().Type.Should().Be(TransactionType.WelcomeBonus);
        transactions.Last().Amount.Should().Be(10_00);
        transactions.Last().Other.Should().Be("PicPay");
        transactions.Last().CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Test]
    public async Task Target_customer_should_get_extract_with_welcome_bonus_and_transfer()
    {
        // Arrange
        var sourceClient = await _api.LoggedAsCustomer();
        var targetClient = await _api.LoggedAsCustomer();

        // Act
        await sourceClient.Transfer(6_80, targetClient.WalletId);
        await _worker.ProcessAll();

        // Assert
        var transactions = await targetClient.GetExtract();
        transactions.Should().HaveCount(2);

        transactions.First().Id.Should().NotBeEmpty();
        transactions.First().Type.Should().Be(TransactionType.Transfer);
        transactions.First().Amount.Should().Be(6_80);
        transactions.First().Other.Should().Be(sourceClient.UserName);
        transactions.First().CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));

        transactions.Last().Id.Should().NotBeEmpty();
        transactions.Last().Type.Should().Be(TransactionType.WelcomeBonus);
        transactions.Last().Amount.Should().Be(10_00);
        transactions.Last().Other.Should().Be("PicPay");
        transactions.Last().CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Test]
    public async Task Merchant_should_get_extract_with_one_transfer()
    {
        // Arrange
        var sourceClient = await _api.LoggedAsCustomer();
        var targetClient = await _api.LoggedAsMerchant();

        // Act
        await sourceClient.Transfer(6_80, targetClient.WalletId);
        await _worker.ProcessAll();

        // Assert
        var transactions = await targetClient.GetExtract();
        transactions.Should().ContainSingle();

        transactions.First().Id.Should().NotBeEmpty();
        transactions.First().Type.Should().Be(TransactionType.Transfer);
        transactions.First().Amount.Should().Be(6_80);
        transactions.First().Other.Should().Be(sourceClient.UserName);
        transactions.First().CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Test]
    public async Task Merchant_should_get_extract_with_two_transfers()
    {
        // Arrange
        var sourceClientA = await _api.LoggedAsCustomer();
        var sourceClientB = await _api.LoggedAsCustomer();
        var targetClient = await _api.LoggedAsMerchant();

        // Act
        await sourceClientA.Transfer(6_80, targetClient.WalletId);
        await sourceClientB.Transfer(4_90, targetClient.WalletId);
        await _worker.ProcessAll();

        // Assert
        var transactions = await targetClient.GetExtract();

        transactions.First().Id.Should().NotBeEmpty();
        transactions.First().Type.Should().Be(TransactionType.Transfer);
        transactions.First().Amount.Should().Be(4_90);
        transactions.First().Other.Should().Be(sourceClientB.UserName);
        transactions.First().CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));

        transactions.Last().Id.Should().NotBeEmpty();
        transactions.Last().Type.Should().Be(TransactionType.Transfer);
        transactions.Last().Amount.Should().Be(6_80);
        transactions.Last().Other.Should().Be(sourceClientA.UserName);
        transactions.Last().CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }
}
