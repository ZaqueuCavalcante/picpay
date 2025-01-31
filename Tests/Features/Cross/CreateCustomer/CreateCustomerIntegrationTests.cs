using PicPay.Api.Errors;
using PicPay.Tests.Data;
using PicPay.Tests.Clients;
using PicPay.Tests.Extensions;

namespace PicPay.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_create_customer()
    {
        // Arrange
        var client = _api.GetClient();
        var name = "Edisberlson Gomes dos Santos";
        var cpf = Documents.GetRandomCpf();
        var email = Emails.New;

        // Act
        var response = await client.CreateCustomer(name: name, cpf: cpf, email: email);

        // Assert
        var customer = response.ShouldBeSuccess();
        customer.Id.Should().NotBeEmpty();
        customer.Name.Should().Be(name);
        customer.Cpf.Should().Be(cpf);
        customer.Email.Should().Be(email);
    }

    [Test]
    public async Task Should_not_create_customer_with_invalid_cpf()
    {
        // Arrange
        var client = _api.GetClient();

        // Act
        var response = await client.CreateCustomer(cpf: "171.275.980-97");

        // Assert
        response.ShouldBeError(new InvalidDocument());
    }

    [Test]
    public async Task Should_not_create_customer_with_valid_cnpj()
    {
        // Arrange
        var client = _api.GetClient();

        // Act
        var response = await client.CreateCustomer(cpf: "75.462.479/0001-87");

        // Assert
        response.ShouldBeError(new InvalidDocument());
    }

    [Test]
    public async Task Should_not_create_customer_with_invalid_email()
    {
        // Arrange
        var client = _api.GetClient();
        var email = Emails.Invalid().PickRandom().First().ToString()!;

        // Act
        var response = await client.CreateCustomer(email: email);

        // Assert
        response.ShouldBeError(new InvalidEmail());
    }

    [Test]
    public async Task Should_not_create_customer_with_weak_password()
    {
        // Arrange
        var client = _api.GetClient();
        var password = Passwords.Weak().PickRandom().First().ToString()!;

        // Act
        var response = await client.CreateCustomer(password: password);

        // Assert
        response.ShouldBeError(new WeakPassword());
    }

    [Test]
    public async Task Should_not_create_customer_with_duplicated_cpf()
    {
        // Arrange
        var client = _api.GetClient();
        var cpf = Documents.GetRandomCpf();

        // Act
        var firstResponse = await client.CreateCustomer(cpf: cpf, email: Emails.New);
        var secondResponse = await client.CreateCustomer(cpf: cpf, email: Emails.New);

        // Assert
        firstResponse.ShouldBeSuccess();
        secondResponse.ShouldBeError(new DocumentAlreadyUsed());
    }

    [Test]
    public async Task Should_not_create_customer_with_duplicated_email()
    {
        // Arrange
        var client = _api.GetClient();
        var email = Emails.New;

        // Act
        var firstResponse = await client.CreateCustomer(cpf: Documents.GetRandomCpf(), email: email);
        var secondResponse = await client.CreateCustomer(cpf: Documents.GetRandomCpf(), email: email);

        // Assert
        firstResponse.ShouldBeSuccess();
        secondResponse.ShouldBeError(new EmailAlreadyUsed());
    }

    [Test]
    public async Task Should_not_create_customer_with_duplicated_cpf_and_email()
    {
        // Arrange
        var client = _api.GetClient();
        var cpf = Documents.GetRandomCpf();
        var email = Emails.New;

        // Act
        var firstResponse = await client.CreateCustomer(cpf: cpf, email: email);
        var secondResponse = await client.CreateCustomer(cpf: cpf, email: email);

        // Assert
        firstResponse.ShouldBeSuccess();
        secondResponse.ShouldBeError(new DocumentAlreadyUsed());
    }

    [Test]
    public async Task Should_not_create_customer_with_duplicated_cpf_and_email_in_parallel_requests()
    {
        // Arrange
        var client01 = _api.GetClient();
        var client02 = _api.GetClient();
        var cpf = Documents.GetRandomCpf();
        var email = Emails.New;

        // Act
        var first = client01.CreateCustomer(cpf: cpf, email: email);
        var second = client02.CreateCustomer(cpf: cpf, email: email);

        var responses = await Task.WhenAll(first, second);

        // Assert
        var error = responses.Single(t => t.IsError());
        error.ShouldBeError(new DocumentAlreadyUsed());

        CreateCustomerOut customer = responses.Single(t => t.IsSuccess()).GetSuccess();
        customer.Cpf.Should().Be(cpf);
        customer.Email.Should().Be(email);
    }

    [Test]
    public async Task Should_give_welcome_bonus_to_customer()
    {
        // Arrange
        var client = await _api.LoggedAsCustomer();

        // Act
        var wallet = await client.GetWallet();

        // Assert
        wallet.Balance.Should().Be(10_00);
    }

    [Test]
    public async Task Should_assert_correct_wallet_balances_on_creating_two_customers_in_parallel_requests()
    {
        // Arrange
        var client01 = _api.GetClient();
        var client02 = _api.GetClient();

        // Act
        var first = client01.CreateCustomer(cpf: Documents.GetRandomCpf(), email: Emails.New);
        var second = client02.CreateCustomer(cpf: Documents.GetRandomCpf(), email: Emails.New);

        var responses = await Task.WhenAll(first, second);

        // Assert
        responses.Should().AllSatisfy(t => t.IsSuccess());

        await using var ctx = _api.GetDbContext();
        var sum = await ctx.Wallets.SumAsync(w => w.Balance);
        sum.Should().Be(0);
    }

    [Test]
    public async Task Should_send_welcome_bonus_created_notification_with_success()
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
}
