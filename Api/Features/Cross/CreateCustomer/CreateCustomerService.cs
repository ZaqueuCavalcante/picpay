using PicPay.Api.Features.Cross.CreateUser;

namespace PicPay.Api.Features.Cross.CreateCustomer;

public class CreateCustomerService(CreateUserService service) : IPicPayService
{
    public async Task<OneOf<CreateCustomerOut, PicPayError>> Create(CreateCustomerIn data)
    {
        var result = await service.Create(data.ToCreateUserIn());

        return result.IsSuccess() ? result.GetSuccess().ToCreateCustomerOut() : result.GetError();
    }
}
