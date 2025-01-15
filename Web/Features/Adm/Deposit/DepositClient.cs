namespace PicPay.Web.Features.Cross.Deposit;

public class DepositClient(HttpClient http) : IPicPayClient
{
    public async Task<OneOf<DepositOut, ErrorOut>> Deposit(long amount, Guid walletId)
    {
        var data = new DepositIn(amount, walletId);

        var response = await http.PostAsJsonAsync("/adm/deposits", data);

        return await response.Resolve<DepositOut>();
    }
}
