using PicPay.Api.Errors;
using PicPay.Tests.Extensions;

namespace PicPay.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Customer_should_transfer_to_another_customer()
    {
        // Arrange
        var sourceClient = await _api.LoggedAsCustomer();

        var targetClient = await _api.LoggedAsCustomer();
        var targetWalletBefore = await targetClient.GetWallet();

        // Act
        var response = await sourceClient.Transfer(3_25, targetWalletBefore.Id);

        // Assert
        var transfer = response.ShouldBeSuccess();
        transfer.TransactionId.Should().NotBeEmpty();

        var sourceWalletAfter = await sourceClient.GetWallet();
        var targetWalletAfter = await targetClient.GetWallet();

        sourceWalletAfter.Balance.Should().Be(6_75);
        targetWalletAfter.Balance.Should().Be(13_25);

        await using var ctx = _api.GetDbContext();
        var transaction = await ctx.Transactions.FirstAsync(t => t.Id == transfer.TransactionId);
        transaction.SourceWalletId.Should().Be(sourceWalletAfter.Id);
        transaction.TargetWalletId.Should().Be(targetWalletAfter.Id);
        transaction.Type.Should().Be(TransactionType.Transfer);
        transaction.Amount.Should().Be(3_25);
        transaction.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Test]
    public async Task Customer_should_transfer_to_merchant()
    {
        // Arrange
        var sourceClient = await _api.LoggedAsCustomer();

        var targetClient = await _api.LoggedAsMerchant();
        var targetWalletBefore = await targetClient.GetWallet();

        // Act
        var response = await sourceClient.Transfer(6_80, targetWalletBefore.Id);

        // Assert
        var transfer = response.ShouldBeSuccess();
        transfer.TransactionId.Should().NotBeEmpty();

        var sourceWalletAfter = await sourceClient.GetWallet();
        var targetWalletAfter = await targetClient.GetWallet();

        sourceWalletAfter.Balance.Should().Be(3_20);
        targetWalletAfter.Balance.Should().Be(6_80);

        await using var ctx = _api.GetDbContext();
        var transaction = await ctx.Transactions.FirstAsync(t => t.Id == transfer.TransactionId);
        transaction.SourceWalletId.Should().Be(sourceWalletAfter.Id);
        transaction.TargetWalletId.Should().Be(targetWalletAfter.Id);
        transaction.Type.Should().Be(TransactionType.Transfer);
        transaction.Amount.Should().Be(6_80);
        transaction.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

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
        var response = await client.Transfer(2_00, Guid.NewGuid());

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
        response.ShouldBeError(new InvalidTargetWallet());
    }

    [Test]
    public async Task Should_not_transfer_to_adm_wallet()
    {
        // Arrange
        var sourceClient = await _api.LoggedAsCustomer();

        var admClient = await _api.LoggedAsAdm();
        var targetWallet = await admClient.GetWallet();

        // Act
        var response = await sourceClient.Transfer(6_00, targetWallet.Id);

        // Assert
        response.ShouldBeError(new InvalidTargetWallet());
    }

    [Test]
    public async Task Should_not_transfer_without_balance()
    {
        // Arrange
        var sourceClient = await _api.LoggedAsCustomer();

        var targetClient = await _api.LoggedAsMerchant();
        var targetWallet = await targetClient.GetWallet();

        // Act
        var response = await sourceClient.Transfer(18_00, targetWallet.Id);

        // Assert
        response.ShouldBeError(new InsufficientWalletBalance());
    }

    [Test]
    public async Task Should_not_transfer_when_auth_service_return_false()
    {
        // Arrange
        var sourceClient = await _api.LoggedAsCustomer();

        var targetClient = await _api.LoggedAsMerchant();
        var targetWallet = await targetClient.GetWallet();

        // Act
        var response = await sourceClient.Transfer(6_66, targetWallet.Id);

        // Assert
        response.ShouldBeError(new TransactionNotAuthorized());
    }

    [Test]
    public async Task Should_not_transfer_when_auth_service_is_down()
    {
        // Arrange
        var sourceClient = await _api.LoggedAsCustomer();

        var targetClient = await _api.LoggedAsCustomer();
        var targetWallet = await targetClient.GetWallet();

        // Act
        var response = await sourceClient.Transfer(5_04, targetWallet.Id);

        // Assert
        response.ShouldBeError(new AuthorizeServiceDown());
    }

    [Test]
    public async Task Should_not_transfer_more_than_source_wallet_balance_in_parallel_requests()
    {
        // Arrange
        var sourceClient = await _api.LoggedAsCustomer();
        var sourceWalletBefore = await sourceClient.GetWallet();

        var targetClient = await _api.LoggedAsCustomer();
        var targetWalletBefore = await targetClient.GetWallet();

        // Act
        var transfer01 = sourceClient.Transfer(8_00, targetWalletBefore.Id);
        var transfer02 = sourceClient.Transfer(8_00, targetWalletBefore.Id);

        var transfers = await Task.WhenAll(transfer01, transfer02);

        // Assert
        var error = transfers.Single(t => t.IsError());
        error.ShouldBeError(new InsufficientWalletBalance());

        var success = transfers.Single(t => t.IsSuccess()).GetSuccess();

        var sourceWalletAfter = await sourceClient.GetWallet();
        var targetWalletAfter = await targetClient.GetWallet();

        sourceWalletAfter.Balance.Should().Be(sourceWalletBefore.Balance - 8_00);
        targetWalletAfter.Balance.Should().Be(targetWalletBefore.Balance + 8_00);
    }

    [Test]
    public async Task Should_assert_correct_wallet_balances_in_same_source_transfer_in_parallel_requests()
    {
        // Arrange
        var sourceClient = await _api.LoggedAsCustomer();

        var targetClient = await _api.LoggedAsMerchant();
        var targetWalletBefore = await targetClient.GetWallet();

        // Act
        var transfer01 = sourceClient.Transfer(4_00, targetWalletBefore.Id);
        var transfer02 = sourceClient.Transfer(4_00, targetWalletBefore.Id);

        var transfers = await Task.WhenAll(transfer01, transfer02);

        // Assert
        transfers.Should().AllSatisfy(t => t.IsSuccess());

        var sourceWalletAfter = await sourceClient.GetWallet();
        var targetWalletAfter = await targetClient.GetWallet();

        sourceWalletAfter.Balance.Should().Be(2_00);
        targetWalletAfter.Balance.Should().Be(8_00);
    }

    [Test]
    public async Task Should_assert_correct_wallet_balances_in_different_sources_transfer_in_parallel_requests()
    {
        // Arrange
        var sourceClientA = await _api.LoggedAsCustomer();
        var sourceClientB = await _api.LoggedAsCustomer();

        var targetClient = await _api.LoggedAsMerchant();
        var targetWalletBefore = await targetClient.GetWallet();
        await (await _api.LoggedAsCustomer()).Transfer(5_00, targetWalletBefore.Id);

        // Act
        var transfer01 = sourceClientA.Transfer(8_00, targetWalletBefore.Id);
        var transfer02 = sourceClientB.Transfer(8_00, targetWalletBefore.Id);

        var transfers = await Task.WhenAll(transfer01, transfer02);

        // Assert
        transfers.Should().AllSatisfy(t => t.IsSuccess());

        var sourceWalletAAfter = await sourceClientA.GetWallet();
        var sourceWalletBAfter = await sourceClientB.GetWallet();
        var targetWalletAfter = await targetClient.GetWallet();

        sourceWalletAAfter.Balance.Should().Be(2_00);
        sourceWalletBAfter.Balance.Should().Be(2_00);
        targetWalletAfter.Balance.Should().Be(21_00);
    }

    [Test]
    public async Task Should_assert_correct_wallet_balances_when_customer_is_source_and_target_at_same_time()
    {
        // Arrange
        var sourceClientA = await _api.LoggedAsCustomer();
        var sourceClientB = await _api.LoggedAsCustomer();

        var targetClient = await _api.LoggedAsMerchant();
        var targetWalletBefore = await targetClient.GetWallet();
        await sourceClientA.Transfer(5_00, targetWalletBefore.Id);
        await sourceClientB.Transfer(2_00, targetWalletBefore.Id);
        await (await _api.LoggedAsCustomer()).Transfer(3_00, targetWalletBefore.Id);

        var sourceWalletABefore = await sourceClientA.GetWallet();

        // Act
        var transfer01 = sourceClientA.Transfer(2_00, targetWalletBefore.Id);
        var transfer02 = sourceClientB.Transfer(6_00, sourceWalletABefore.Id);

        var transfers = await Task.WhenAll(transfer01, transfer02);

        // Assert
        transfers.Should().AllSatisfy(t => t.IsSuccess());

        var sourceWalletAAfter = await sourceClientA.GetWallet();
        var sourceWalletBAfter = await sourceClientB.GetWallet();
        var targetWalletAfter = await targetClient.GetWallet();

        sourceWalletAAfter.Balance.Should().Be(9_00);
        sourceWalletBAfter.Balance.Should().Be(2_00);
        targetWalletAfter.Balance.Should().Be(12_00);
    }

    [Test]
    public async Task Should_assert_correct_source_and_target_wallet_balances_in_cross_transfer_parallel_requests()
    {
        // Arrange
        var clientA = await _api.LoggedAsCustomer();
        var clientAWalletBefore = await clientA.GetWallet();

        var clientB = await _api.LoggedAsCustomer();
        var clientBWalletBefore = await clientB.GetWallet();

        // Act
        var transfer01 = clientA.Transfer(6_00, clientBWalletBefore.Id);
        var transfer02 = clientB.Transfer(2_00, clientAWalletBefore.Id);

        var transfers = await Task.WhenAll(transfer01, transfer02);

        // Assert
        transfers.Should().AllSatisfy(t => t.IsSuccess());

        var clientAWalletAfter = await clientA.GetWallet();
        var clientBWalletAfter = await clientB.GetWallet();

        clientAWalletAfter.Balance.Should().Be(6_00);
        clientBWalletAfter.Balance.Should().Be(14_00);
    }

    [Test]
    public async Task Should_send_transfer_created_notification_to_customer_with_success()
    {
        // Arrange
        var sourceClient = await _api.LoggedAsCustomer();

        var targetClient = await _api.LoggedAsCustomer();
        var targetWalletBefore = await targetClient.GetWallet();

        await sourceClient.Transfer(4_20, targetWalletBefore.Id);

        // Act
        await _worker.ProcessAll();

        // Assert
        var notifications = await targetClient.GetNotifications();
        notifications.Should().HaveCount(2);
        notifications.First().Status.Should().Be(NotificationStatus.Success);
        notifications.First().Message.Should().Be($"Você recebeu uma transferência de R$ 4,20 de {sourceClient.UserName}");
    }

    [Test]
    public async Task Should_send_transfer_created_notification_to_merchant_with_success()
    {
        // Arrange
        var sourceClient = await _api.LoggedAsCustomer();

        var targetClient = await _api.LoggedAsMerchant();
        var targetWalletBefore = await targetClient.GetWallet();

        await sourceClient.Transfer(2_20, targetWalletBefore.Id);

        // Act
        await _worker.ProcessAll();

        // Assert
        var notifications = await targetClient.GetNotifications();
        notifications.Should().ContainSingle();
        notifications.First().Status.Should().Be(NotificationStatus.Success);
        notifications.First().Message.Should().Be($"Você recebeu uma transferência de R$ 2,20 de {sourceClient.UserName}");
    }

    [Test]
    public async Task Should_try_send_transfer_created_notification_with_error_on_notification_service()
    {
        // Arrange
        var sourceClient = await _api.LoggedAsCustomer();

        var targetClient = await _api.LoggedAsMerchant();
        var targetWalletBefore = await targetClient.GetWallet();

        await sourceClient.Transfer(1_23, targetWalletBefore.Id);

        // Act
        await _worker.ProcessAll();

        // Assert
        var notifications = await targetClient.GetNotifications();
        notifications.Should().ContainSingle();
        notifications.First().Status.Should().Be(NotificationStatus.Failed);
        notifications.First().Message.Should().Be($"Você recebeu uma transferência de R$ 1,23 de {sourceClient.UserName}");
    }
}
