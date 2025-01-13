using Microsoft.AspNetCore.Authorization;

namespace PicPay.Api.Extensions;

public class AuthCustomerAttribute : AuthorizeAttribute
{
	public AuthCustomerAttribute()
	{
		Roles = UserRole.Customer.ToString();
		AuthenticationSchemes = AuthenticationConfigs.BearerScheme;
	}
}
