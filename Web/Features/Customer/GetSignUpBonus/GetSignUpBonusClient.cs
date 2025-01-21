namespace PicPay.Web.Features.Cross.GetSignUpBonus;

public class GetSignUpBonusClient(HttpClient http) : IPicPayClient
{
    public async Task<OneOf<SuccessOut, ErrorOut>> GetSignUpBonus()
    {
        var response = await http.PostAsJsonAsync("/customer/sign-up-bonus", new {});

        return await response.Resolve<SuccessOut>();
    }
}
