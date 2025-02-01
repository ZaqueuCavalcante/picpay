using System.Security.Claims;

namespace PicPay.Api.Extensions;

public static class UserExtensions
{
    public static Guid Id(this ClaimsPrincipal user)
    {
        return Guid.Parse(user.FindFirstValue("sub")!);
    }

    public static Guid WalletId(this ClaimsPrincipal user)
    {
        return Guid.Parse(user.FindFirstValue("wid")!);
    }
}
