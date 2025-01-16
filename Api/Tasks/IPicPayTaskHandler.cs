namespace PicPay.Api.Tasks;

public interface IPicPayTaskHandler<T> where T : IPicPayTask
{
    Task Handle(T task);
}
