using PicPay.Api.Errors;
using PicPay.Tests.Extensions;

namespace PicPay.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    [TestCase(0)]
    [TestCase(-123)]
    public async Task Should_not_transfer_invalid_amount(long amount)
    {
        // Arrange
        var client = await _api.LoggedAsCustomer();

        // Act
        var response = await client.Transfer(amount, Guid.NewGuid());

        // Assert
        response.ShouldBeError(new InvalidTransferAmount());
    }

    [Test]
    public async Task Should_not_transfer_to_non_existent_wallet()
    {
        // Arrange
        var client = await _api.LoggedAsCustomer();

        // Act
        var response = await client.Transfer(123, Guid.NewGuid());

        // Assert
        response.ShouldBeError(new WalletNotFound());
    }

    [Test]
    public async Task Should_not_transfer_to_self_wallet()
    {
        // Arrange
        var client = await _api.LoggedAsCustomer();
        var wallet = await client.GetWallet();

        // Act
        var response = await client.Transfer(456, wallet.Id);

        // Assert
        response.ShouldBeError(new InvalidTargetWallet());
    }

    [Test]
    public async Task Should_not_transfer_without_balance()
    {
        // Arrange
        var sourceClient = await _api.LoggedAsCustomer();

        var targetClient = await _api.LoggedAsCustomer();
        var targetWallet = await targetClient.GetWallet();

        // Act
        var response = await sourceClient.Transfer(456, targetWallet.Id);

        // Assert
        response.ShouldBeError(new InsufficientWalletBalance());
    }

    [Test]
    public async Task Should_not_transfer_when_auth_service_is_down()
    {
        // Arrange
        var sourceClient = await _api.LoggedAsCustomer();
        var sourceWallet = await sourceClient.GetWallet();

        var admClient = await _api.LoggedAsAdm();
        await admClient.Deposit(420_00, sourceWallet.Id);

        var targetClient = await _api.LoggedAsCustomer();
        var targetWallet = await targetClient.GetWallet();

        // Act
        var response = await sourceClient.Transfer(374_58, targetWallet.Id);

        // Assert
        response.ShouldBeError(new AuthorizeServiceDown());
    }

    [Test]
    public async Task Should_not_transfer_when_auth_service_return_false()
    {
        // Arrange
        var sourceClient = await _api.LoggedAsCustomer();
        var sourceWallet = await sourceClient.GetWallet();

        var admClient = await _api.LoggedAsAdm();
        await admClient.Deposit(420_00, sourceWallet.Id);

        var targetClient = await _api.LoggedAsCustomer();
        var targetWallet = await targetClient.GetWallet();

        // Act
        var response = await sourceClient.Transfer(159_75, targetWallet.Id);

        // Assert
        response.ShouldBeError(new TransactionNotAuthorized());
    }
}
