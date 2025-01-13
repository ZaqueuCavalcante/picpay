using PicPay.Api.Features.Cross.CreateUser;

namespace PicPay.Api.Features.Cross.CreateMerchant;

public class CreateMerchantService(CreateUserService service) : IPicPayService
{
    public async Task<OneOf<CreateMerchantOut, PicPayError>> Create(CreateMerchantIn data)
    {
        var result = await service.Create(data.ToCreateUserIn());

        return result.IsSuccess() ? result.GetSuccess().ToCreateMerchantOut() : result.GetError();
    }
}
