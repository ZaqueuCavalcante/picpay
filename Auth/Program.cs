using PicPay.Auth;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

builder.WebHost.UseUrls("http://*:5004");

long[] fails = [159_75];

app.MapGet("/api/v2/authorize", async (long? amount) =>
{
    if (amount != null && amount.Value == 374_58) await Task.Delay(5_000);

    bool authorize = false;
    if (amount != null)
    {
        authorize = !fails.Contains(amount.Value);
    }
    else
    {
        authorize = new Random().NextDouble() > 0.5;
    }

    return new AuthorizeOut
    {
        Status = authorize ? "success" : "fail",
        Data = new() { Authorization = authorize },
    };
});

app.Run();

public partial class Program;
