using PicPay.Tests.Extensions;

namespace PicPay.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_send_transfer_notification_with_success()
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
    }

    [Test]
    public async Task Should_try_send_transfer_notification_with_error()
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
    }
}
