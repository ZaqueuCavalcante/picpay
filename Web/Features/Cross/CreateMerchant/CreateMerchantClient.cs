namespace PicPay.Web.Features.Cross.CreateMerchant;

public class CreateMerchantClient(HttpClient http) : IPicPayClient
{
    public async Task<OneOf<CreateMerchantOut, ErrorOut>> Create(
        string name,
        string cnpj,
        string email,
        string password)
    {
        var data = new CreateMerchantIn(name, cnpj, email, password);

        var response = await http.PostAsJsonAsync("/merchants", data);

        return await response.Resolve<CreateMerchantOut>();
    }
}
