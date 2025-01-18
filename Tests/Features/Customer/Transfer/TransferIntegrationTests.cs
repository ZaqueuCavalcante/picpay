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
        var response = await client.Transfer(12_00, Guid.NewGuid());

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
        var response = await client.Transfer(34_00, wallet.Id);

        // Assert
        response.ShouldBeError(new InvalidTargetTransferWallet());
    }

    [Test]
    public async Task Should_not_transfer_to_adm_wallet()
    {
        // Arrange
        var sourceClient = await _api.LoggedAsCustomer();

        var admClient = await _api.LoggedAsAdm();
        var targetWallet = await admClient.GetWallet();

        // Act
        var response = await sourceClient.Transfer(56_00, targetWallet.Id);

        // Assert
        response.ShouldBeError(new InvalidTargetTransferWallet());
    }

    [Test]
    public async Task Should_not_transfer_without_balance()
    {
        // Arrange
        var sourceClient = await _api.LoggedAsCustomer();

        var targetClient = await _api.LoggedAsMerchant();
        var targetWallet = await targetClient.GetWallet();

        // Act
        var response = await sourceClient.Transfer(78_00, targetWallet.Id);

        // Assert
        response.ShouldBeError(new InsufficientWalletBalance());
    }

    [Test]
    public async Task Should_not_transfer_when_auth_service_is_down()
    {
        // Arrange
        var sourceClient = await _api.LoggedAsCustomer(420_00);

        var targetClient = await _api.LoggedAsCustomer();
        var targetWallet = await targetClient.GetWallet();

        // Act
        var response = await sourceClient.Transfer(100_00, targetWallet.Id);

        // Assert
        response.ShouldBeError(new AuthorizeServiceDown());
    }

    [Test]
    public async Task Should_not_transfer_when_auth_service_return_false()
    {
        // Arrange
        var sourceClient = await _api.LoggedAsCustomer(420_00);

        var targetClient = await _api.LoggedAsMerchant();
        var targetWallet = await targetClient.GetWallet();

        // Act
        var response = await sourceClient.Transfer(200_00, targetWallet.Id);

        // Assert
        response.ShouldBeError(new TransactionNotAuthorized());
    }

    [Test]
    public async Task Should_not_transfer_more_than_source_wallet_balance_in_parallel_requests()
    {
        // Arrange
        var sourceClient = await _api.LoggedAsCustomer(420_00);
        var sourceWalletBefore = await sourceClient.GetWallet();

        var targetClient = await _api.LoggedAsCustomer();
        var targetWalletBefore = await targetClient.GetWallet();

        // Act
        var transfer01 = sourceClient.Transfer(400_00, targetWalletBefore.Id);
        var transfer02 = sourceClient.Transfer(300_00, targetWalletBefore.Id);

        var transfers = await Task.WhenAll(transfer01, transfer02);

        // Assert
        await using var ctx = _api.GetDbContext();

        var error = transfers.Single(t => t.IsError());
        error.ShouldBeError(new InsufficientWalletBalance());

        var success = transfers.Single(t => t.IsSuccess()).GetSuccess();
        var transaction = await ctx.Transactions.FirstAsync(t => t.Id == success.TransactionId);

        var sourceWalletAfter = await sourceClient.GetWallet();
        var targetWalletAfter = await targetClient.GetWallet();

        sourceWalletAfter.Balance.Should().Be(sourceWalletBefore.Balance - transaction.Amount);
        targetWalletAfter.Balance.Should().Be(targetWalletBefore.Balance + transaction.Amount);
    }

    [Test]
    public async Task Should_assert_correct_wallet_balances_in_same_source_transfer_parallel_requests()
    {
        // Arrange
        var sourceClient = await _api.LoggedAsCustomer(420_00);

        var targetClient = await _api.LoggedAsMerchant();
        var targetWalletBefore = await targetClient.GetWallet();

        // Act
        var transfer01 = sourceClient.Transfer(250_00, targetWalletBefore.Id);
        var transfer02 = sourceClient.Transfer(150_00, targetWalletBefore.Id);

        var transfers = await Task.WhenAll(transfer01, transfer02);

        // Assert
        transfers.Should().AllSatisfy(t => t.IsSuccess());

        var sourceWalletAfter = await sourceClient.GetWallet();
        var targetWalletAfter = await targetClient.GetWallet();

        sourceWalletAfter.Balance.Should().Be(20_00);
        targetWalletAfter.Balance.Should().Be(400_00);
    }

    [Test]
    public async Task Should_assert_correct_wallet_balances_in_different_sources_transfer_parallel_requests()
    {
        // Arrange
        var sourceClientA = await _api.LoggedAsCustomer(400_00);
        var sourceClientB = await _api.LoggedAsCustomer(450_00);

        var targetClient = await _api.LoggedAsMerchant();
        var targetWalletBefore = await targetClient.GetWallet();

        // Act
        var transfer01 = sourceClientA.Transfer(250_00, targetWalletBefore.Id);
        var transfer02 = sourceClientB.Transfer(350_00, targetWalletBefore.Id);

        var transfers = await Task.WhenAll(transfer01, transfer02);

        // Assert
        transfers.Should().AllSatisfy(t => t.IsSuccess());

        var sourceWalletAAfter = await sourceClientA.GetWallet();
        var sourceWalletBAfter = await sourceClientB.GetWallet();
        var targetWalletAfter = await targetClient.GetWallet();

        sourceWalletAAfter.Balance.Should().Be(150_00);
        sourceWalletBAfter.Balance.Should().Be(100_00);
        targetWalletAfter.Balance.Should().Be(600_00);
    }

    [Test]
    public async Task Should_assert_correct_source_and_target_wallet_balances_in_cross_transfer_parallel_requests()
    {
        // Arrange
        var clientA = await _api.LoggedAsCustomer(400_00);
        var clientAWalletBefore = await clientA.GetWallet();

        var clientB = await _api.LoggedAsCustomer(100_00);
        var clientBWalletBefore = await clientB.GetWallet();

        // Act
        var transfer01 = clientA.Transfer(60_00, clientBWalletBefore.Id);
        var transfer02 = clientB.Transfer(20_00, clientAWalletBefore.Id);

        var transfers = await Task.WhenAll(transfer01, transfer02);

        // Assert
        transfers.Should().AllSatisfy(t => t.IsSuccess());

        var clientAWalletAfter = await clientA.GetWallet();
        var clientBWalletAfter = await clientB.GetWallet();

        clientAWalletAfter.Balance.Should().Be(360_00);
        clientBWalletAfter.Balance.Should().Be(140_00);
    }
}
