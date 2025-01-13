namespace PicPay.Web.Features.Cross.Login;

public class LoginClient(HttpClient http) : IPicPayClient
{
    public async Task<OneOf<LoginOut, ErrorOut>> Login(string email, string password)
    {
        var data = new LoginIn(email, password);

        var response = await http.PostAsJsonAsync("/login", data);

        return await response.Resolve<LoginOut>();
    }
}
