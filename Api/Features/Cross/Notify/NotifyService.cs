using Polly;
using Polly.Retry;
using Polly.Timeout;

namespace PicPay.Api.Features.Cross.Notify;

public class NotifyService(NotifySettings settings, IHttpClientFactory factory) : IPicPayService
{
    public async Task<OneOf<bool, PicPayError>> Notify(NotificationIn notification)
    {
        var client = factory.CreateClient();
        client.BaseAddress = new Uri(settings.Url);

        var pipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                UseJitter = true,
                BackoffType = DelayBackoffType.Linear,
                MaxRetryAttempts = settings.MaxRetryAttempts,
                Delay = TimeSpan.FromSeconds(settings.Delay),
                ShouldHandle = new PredicateBuilder().Handle<Exception>(),
            })
            .AddTimeout(new TimeoutStrategyOptions { Timeout = TimeSpan.FromSeconds(settings.Timeout) })
            .Build();

        try
        {
            var response = await pipeline.ExecuteAsync(async cancellationToken =>
            {
                var response = await client.PostAsJsonAsync($"api/v1/notify", notification, cancellationToken);
                response.EnsureSuccessStatusCode();
                return response;
            });

            if (!response.IsSuccessStatusCode) return new NotifyServiceDown();

            return true;
        }
        catch (Exception)
        {
            return new NotifyServiceDown();
        }
    }
}
