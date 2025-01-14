namespace PicPay.Web.Features.Cross.Transfer;

public class TransferClient(HttpClient http) : IPicPayClient
{
    public async Task<OneOf<TransferOut, ErrorOut>> Transfer(long amount, Guid walletId)
    {
        var data = new TransferIn(amount, walletId);

        var response = await http.PostAsJsonAsync("/customer/transfers", data);

        return await response.Resolve<TransferOut>();
    }
}
