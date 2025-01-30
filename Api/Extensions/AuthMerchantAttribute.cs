using Microsoft.AspNetCore.Authorization;

namespace PicPay.Api.Extensions;

public class AuthMerchantAttribute : AuthorizeAttribute
{
	public AuthMerchantAttribute()
	{
		Roles = UserRole.Merchant.ToString();
		AuthenticationSchemes = AuthenticationConfigs.BearerScheme;
	}
}
