using Microsoft.AspNetCore.Authorization;

namespace PicPay.Api.Extensions;

public class AuthAdmAttribute : AuthorizeAttribute
{
	public AuthAdmAttribute()
	{
		Roles = UserRole.Adm.ToString();
		AuthenticationSchemes = AuthenticationConfigs.BearerScheme;
	}
}
