using Microsoft.AspNetCore.Identity;

namespace PicPay.Api.Security;

public class PasswordHasher(IPasswordHasher<IdentityUser> passwordHasher) : IPasswordHasher
{
    public string Hash(Guid id, string email, string password)
    {
        return passwordHasher.HashPassword(
            new IdentityUser()
            {
                Email = email,
                UserName = email,
                Id = id.ToString(),
            },
            password);
    }

    public bool Verify(Guid id, string email, string password, string hash)
    {
        return passwordHasher.VerifyHashedPassword(
            new IdentityUser()
            {
                Email = email,
                UserName = email,
                Id = id.ToString(),
            },
            hash,
            password) == PasswordVerificationResult.Success;
    }
}
