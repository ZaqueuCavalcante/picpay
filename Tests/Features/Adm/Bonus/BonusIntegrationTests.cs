using PicPay.Api.Errors;
using PicPay.Tests.Extensions;

namespace PicPay.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    [TestCase(0)]
    [TestCase(-123)]
    public async Task Should_not_bonus_invalid_amount(long amount)
    {
        // Arrange
        var client = await _api.LoggedAsAdm();

        // Act
        var response = await client.Bonus(amount, Guid.NewGuid());

        // Assert
        response.ShouldBeError(new InvalidBonusAmount());
    }

    [Test]
    public async Task Should_not_bonus_to_non_existent_wallet()
    {
        // Arrange
        var client = await _api.LoggedAsAdm();

        // Act
        var response = await client.Bonus(123, Guid.NewGuid());

        // Assert
        response.ShouldBeError(new WalletNotFound());
    }

    [Test]
    public async Task Should_not_bonus_to_self_wallet()
    {
        // Arrange
        var client = await _api.LoggedAsAdm();
        var wallet = await client.GetWallet();

        // Act
        var response = await client.Bonus(456, wallet.Id);

        // Assert
        response.ShouldBeError(new InvalidTargetWallet());
    }

    [Test]
    public async Task Should_assert_correct_wallet_balances_in_same_target_bonus_parallel_requests()
    {
        // Arrange
        var admClient = await _api.LoggedAsAdm();

        var targetClient = await _api.LoggedAsMerchant();
        var targetWalletBefore = await targetClient.GetWallet();

        // Act
        var transfer01 = admClient.Bonus(250_00, targetWalletBefore.Id);
        var transfer02 = admClient.Bonus(150_00, targetWalletBefore.Id);

        var transfers = await Task.WhenAll(transfer01, transfer02);

        // Assert
        transfers.Should().AllSatisfy(t => t.IsSuccess());

        var targetWalletAfter = await targetClient.GetWallet();
        targetWalletAfter.Balance.Should().Be(400_00);
    }

    [Test]
    public async Task Should_assert_correct_wallet_balances_in_different_targets_bonus_parallel_requests()
    {
        // Arrange
        var admClient = await _api.LoggedAsAdm();

        var targetClientA = await _api.LoggedAsCustomer();
        var targetWalletABefore = await targetClientA.GetWallet();

        var targetClientB = await _api.LoggedAsMerchant();
        var targetWalletBBefore = await targetClientB.GetWallet();

        // Act
        var transfer01 = admClient.Bonus(200_00, targetWalletABefore.Id);
        var transfer02 = admClient.Bonus(350_00, targetWalletBBefore.Id);

        var transfers = await Task.WhenAll(transfer01, transfer02);

        // Assert
        transfers.Should().AllSatisfy(t => t.IsSuccess());

        var targetWalletAAfter = await targetClientA.GetWallet();
        var targetWalletBAfter = await targetClientB.GetWallet();

        targetWalletAAfter.Balance.Should().Be(200_00);
        targetWalletBAfter.Balance.Should().Be(350_00);
    }
}
