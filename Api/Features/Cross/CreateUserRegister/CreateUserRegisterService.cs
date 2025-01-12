using PicPay.Api.Security;

namespace PicPay.Api.Features.Cross.CreateUserRegister;

public class CreateUserRegisterService(PicPayDbContext ctx, IPasswordHasher hasher) : IPicPayService
{
    public async Task<OneOf<CreateUserRegisterOut, PicPayError>> Create(CreateUserRegisterIn data)
    {
        // Email invalido
        // Senha fraca
        // Documento ja usado
        // Email ja usado

        var document = data.Document.OnlyNumbers();

        var result = PicPayUser.New(data.Type, data.Name, document, data.Email);

        if (result.IsError()) return result.GetError();

        var user = result.GetSuccess();

        var passwordHash = hasher.Hash(user.Id, user.Email, data.Password);
        user.SetPasswordHash(passwordHash);

        ctx.Add(user);
        await ctx.SaveChangesAsync();

        return user.ToCreateOut();
    }
}
