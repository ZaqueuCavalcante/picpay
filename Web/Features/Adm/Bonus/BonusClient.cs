namespace PicPay.Web.Features.Cross.Bonus;

public class BonusClient(HttpClient http) : IPicPayClient
{
    public async Task<OneOf<BonusOut, ErrorOut>> Bonus(long amount, Guid walletId)
    {
        var data = new BonusIn(amount, walletId);

        var response = await http.PostAsJsonAsync("/adm/bonus", data);

        return await response.Resolve<BonusOut>();
    }
}
