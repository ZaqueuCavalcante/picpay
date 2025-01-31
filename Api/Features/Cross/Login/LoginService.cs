using PicPay.Api.Security;
using PicPay.Api.Features.Cross.GenerateJWT;

namespace PicPay.Api.Features.Cross.Login;

public class LoginService(PicPayDbContext ctx, IPasswordHasher hasher, GenerateJWTService service) : IPicPayService
{
    public async Task<OneOf<LoginOut, PicPayError>> Login(LoginIn data)
    {
        var user = await ctx.Users.FirstOrDefaultAsync(u => u.Email == data.Email);
        if (user == null) return new UserNotFound();

        var success = hasher.Verify(user.Id, user.Email, data.Password, user.PasswordHash);
        if (!success) return new WrongPassword();

        return new LoginOut { AccessToken = service.Generate(user) };
    }
}
