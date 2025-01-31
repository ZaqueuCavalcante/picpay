using PicPay.Tests.Extensions;

namespace PicPay.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Customer_should_get_welcome_bonus_created_notification()
    {
        // Arrange
        var client = await _api.LoggedAsCustomer();

        // Act
        await _worker.ProcessAll();

        // Assert
        var notifications = await client.GetNotifications();
        notifications.Should().ContainSingle();
        notifications.First().Status.Should().Be(NotificationStatus.Success);
        notifications.First().Message.Should().Be("Bônus de Boas-Vindas no valor de R$ 10,00");
    }

    [Test]
    public async Task Customer_should_get_transfer_created_notification()
    {
        // Arrange
        var sourceClient = await _api.LoggedAsCustomer();

        var targetClient = await _api.LoggedAsCustomer();
        var targetWalletBefore = await targetClient.GetWallet();

        // Act
        await sourceClient.Transfer(6_80, targetWalletBefore.Id);
        await _worker.ProcessAll();

        // Assert
        var notifications = await targetClient.GetNotifications();
        notifications.Should().HaveCount(2);
        notifications.First().Status.Should().Be(NotificationStatus.Success);
        notifications.First().Message.Should().Be($"Você recebeu uma transferência de R$ 6,80 de {sourceClient.UserName}");
        notifications.Last().Status.Should().Be(NotificationStatus.Success);
        notifications.Last().Message.Should().Be("Bônus de Boas-Vindas no valor de R$ 10,00");
    }

    [Test]
    public async Task Merchant_should_get_transfer_created_notification()
    {
        // Arrange
        var sourceClient = await _api.LoggedAsCustomer();

        var targetClient = await _api.LoggedAsMerchant();
        var targetWalletBefore = await targetClient.GetWallet();

        // Act
        await sourceClient.Transfer(6_80, targetWalletBefore.Id);
        await _worker.ProcessAll();

        // Assert
        var notifications = await targetClient.GetNotifications();
        notifications.Should().ContainSingle();
        notifications.First().Status.Should().Be(NotificationStatus.Success);
        notifications.First().Message.Should().Be($"Você recebeu uma transferência de R$ 6,80 de {sourceClient.UserName}");
    }

    [Test]
    public async Task Merchant_should_get_second_transfer_created_notification()
    {
        // Arrange
        var sourceClient = await _api.LoggedAsCustomer();

        var targetClient = await _api.LoggedAsMerchant();
        var targetWalletBefore = await targetClient.GetWallet();

        // Act
        await sourceClient.Transfer(6_80, targetWalletBefore.Id);
        await sourceClient.Transfer(1_25, targetWalletBefore.Id);
        await _worker.ProcessAll();

        // Assert
        var notifications = await targetClient.GetNotifications();
        notifications.Should().HaveCount(2);
        notifications.First().Status.Should().Be(NotificationStatus.Success);
        notifications.First().Message.Should().Be($"Você recebeu uma transferência de R$ 1,25 de {sourceClient.UserName}");
        notifications.Last().Status.Should().Be(NotificationStatus.Success);
        notifications.Last().Message.Should().Be($"Você recebeu uma transferência de R$ 6,80 de {sourceClient.UserName}");
    }
}
