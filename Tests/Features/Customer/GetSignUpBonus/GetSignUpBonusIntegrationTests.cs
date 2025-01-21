using PicPay.Tests.Extensions;

namespace PicPay.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Customer_should_get_sign_up_bonus()
    {
        // Arrange
        var client = await _api.LoggedAsCustomer();

        // Act
        var response = await client.GetSignUpBonus();

        // Assert
        response.ShouldBeSuccess();

        var wallet = await client.GetWallet();
        wallet.Balance.Should().Be(10_00);
    }

    [Test]
    public async Task Customer_get_sign_up_bonus_operation_should_be_idempotent()
    {
        // Arrange
        var client = await _api.LoggedAsCustomer();

        // Act
        var bonus01 = await client.GetSignUpBonus();
        var bonus02 = await client.GetSignUpBonus();
        var bonus03 = await client.GetSignUpBonus();

        // Assert
        bonus01.ShouldBeSuccess();
        bonus02.ShouldBeSuccess();
        bonus03.ShouldBeSuccess();

        var wallet = await client.GetWallet();
        wallet.Balance.Should().Be(10_00);
    }

    [Test]
    public async Task Customer_get_sign_up_bonus_operation_should_be_idempotent_parallel_requests()
    {
        // Arrange
        var client = await _api.LoggedAsCustomer();

        // Act
        var bonus01 = client.GetSignUpBonus();
        var bonus02 = client.GetSignUpBonus();
        var bonus03 = client.GetSignUpBonus();

        var bonuses = await Task.WhenAll(bonus01, bonus02, bonus03);

        // Assert
        bonuses[0].ShouldBeSuccess();
        bonuses[1].ShouldBeSuccess();
        bonuses[2].ShouldBeSuccess();

        var wallet = await client.GetWallet();
        wallet.Balance.Should().Be(10_00);
    }
}
