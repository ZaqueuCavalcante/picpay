namespace PicPay.Api.Errors;

public abstract class PicPayError
{
    public abstract string Code { get; set; }
    public abstract string Message { get; set; }

    public SwaggerExample<ErrorOut> ToExampleErrorOut()
    {
        return SwaggerExample.Create(Message, new ErrorOut { Code = Code, Message = Message });
    }
}
