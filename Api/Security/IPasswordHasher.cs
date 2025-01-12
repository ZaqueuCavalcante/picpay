namespace PicPay.Api.Security;

public interface IPasswordHasher
{
    string Hash(Guid id, string email, string password);
    bool Verify(Guid id, string email, string password, string hash);
}
