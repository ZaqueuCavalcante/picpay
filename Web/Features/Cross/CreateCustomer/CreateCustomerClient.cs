namespace PicPay.Web.Features.Cross.CreateCustomer;

public class CreateCustomerClient(HttpClient http) : IPicPayClient
{
    public async Task<OneOf<CreateCustomerOut, ErrorOut>> Create(
        string name,
        string document,
        string email,
        string password)
    {
        var data = new CreateCustomerIn(name, document, email, password);

        var response = await http.PostAsJsonAsync("/customers", data);

        return await response.Resolve<CreateCustomerOut>();
    }
}
