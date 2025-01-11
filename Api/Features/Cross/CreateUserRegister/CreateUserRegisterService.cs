namespace PicPay.Api.Features.Cross.CreateUserRegister;

public class CreateUserRegisterService(PicPayDbContext ctx) : IPicPayService
{
    public async Task<CreateUserRegisterOut> Create(CreateUserRegisterIn data)
    {
        var user = new PicPayUser(data.Type, data.Name, data.Document, data.Email, data.Password);

        ctx.Add(user);
        await ctx.SaveChangesAsync();

        return user.ToCreateOut();
    }
}
