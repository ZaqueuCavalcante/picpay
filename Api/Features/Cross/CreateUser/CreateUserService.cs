using PicPay.Api.Security;
using PicPay.Api.Features.Cross.CreateTransaction;

namespace PicPay.Api.Features.Cross.CreateUser;

public class CreateUserService(PicPayDbContext ctx, IPasswordHasher hasher) : IPicPayService
{
    public async Task<OneOf<CreateUserOut, PicPayError>> Create(CreateUserIn data)
    {
        if (!data.Email.IsValidEmail()) return new InvalidEmail();
        if (!data.Password.IsStrongPassword()) return new WeakPassword();

        await ctx.Database.BeginTransactionAsync();

        var document = data.Document.OnlyNumbers();
        var result = PicPayUser.New(data.Role, data.Name, document, data.Email);
        if (result.IsError()) return result.GetError();

        var user = result.GetSuccess();
        var passwordHash = hasher.Hash(user.Id, user.Email, data.Password);
        user.SetPasswordHash(passwordHash);

        ctx.Add(user);

        if (user.IsCustomer()) await CreateWelcomeBonus(10_00, user.Wallet);

        try
        {
            await ctx.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            if (ex.ToString().Contains("ix_users_email")) return new EmailAlreadyUsed();
            return new DocumentAlreadyUsed();
        }

        await ctx.Database.CommitTransactionAsync();

        return user.ToCreateOut();
    }

    public async Task CreateWelcomeBonus(long amount, Wallet targetWallet)
    {
        var userId = await ctx.Users.Where(u => u.Role == UserRole.Adm).Select(u => u.Id).FirstAsync();
        var sourceWallet = await ctx.Wallets.FromSql($"SELECT * FROM picpay.wallets WHERE user_id = {userId} FOR UPDATE").FirstAsync();

        sourceWallet.Take(amount);
        targetWallet.Put(amount);

        var transaction = new Transaction(sourceWallet.Id, targetWallet.Id, TransactionType.WelcomeBonus, amount);
        ctx.Add(transaction);
    }
}
