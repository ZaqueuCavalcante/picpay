using PicPay.Auth;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/api/v2/authorize", (bool? response) =>
{
    var authorize = response ?? new Random().NextDouble() > 0.5;

    return new AuthorizeOut
    {
        Status = authorize ? "success" : "fail",
        Data = new() { Authorization = authorize },
    };
});

app.Run();
