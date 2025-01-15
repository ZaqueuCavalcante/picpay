using PicPay.Auth;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

builder.WebHost.UseUrls("http://*:5004");

app.MapGet("/api/v2/authorize", async (long? amount) =>
{
    if (amount != null && amount.Value == 374_58) await Task.Delay(5_000);

    bool authorize = false;

    authorize = (amount != null && new long[] { 159_75 }.Contains(amount.Value)) ? false : new Random().NextDouble() > 0.5;

    return new AuthorizeOut
    {
        Status = authorize ? "success" : "fail",
        Data = new() { Authorization = authorize },
    };
});

app.Run();

public partial class Program;
