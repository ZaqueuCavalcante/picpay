namespace PicPay.Web.Features.Cross.CreateUser;

public class CreateUserClient(HttpClient http) : IPicPayClient
{
    public async Task<OneOf<CreateUserOut, ErrorOut>> Create(
        UserType type,
        string name,
        string document,
        string email,
        string password)
    {
        var data = new CreateUserIn(type, name, document, email, password);

        var response = await http.PostAsJsonAsync("/users", data);

        return await response.Resolve<CreateUserOut>();
    }
}
