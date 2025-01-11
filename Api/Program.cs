var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Funciona muito bem");
app.MapGet("/opa", () => "Opa, tudo bem?");

app.Run();
