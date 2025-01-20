using PicPay.Api.Security;

namespace PicPay.Api.Features.Cross.CreateUser;

public class CreateUserService(PicPayDbContext ctx, IPasswordHasher hasher) : IPicPayService
{
    public async Task<OneOf<CreateUserOut, PicPayError>> Create(CreateUserIn data)
    {
        if (!data.Email.IsValidEmail()) return new InvalidEmail();

        if (!data.Password.IsStrongPassword()) return new WeakPassword();

        var document = data.Document.OnlyNumbers();

        var result = PicPayUser.New(data.Role, data.Name, document, data.Email);
        if (result.IsError()) return result.GetError();

        var user = result.GetSuccess();

        var passwordHash = hasher.Hash(user.Id, user.Email, data.Password);
        user.SetPasswordHash(passwordHash);

        ctx.Add(user);

        try
        {
            await ctx.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            if (ex.ToString().Contains("ix_users_email")) return new EmailAlreadyUsed();

            return new DocumentAlreadyUsed();
        }

        return user.ToCreateOut();
    }
}
