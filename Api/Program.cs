var builder = WebApplication.CreateBuilder(args);
builder.Host.AddLoggingConfigs();
builder.Services.AddSettingsConfigs();
builder.Services.AddServicesConfigs();
builder.Services.AddAuthenticationConfigs();
builder.Services.AddAuthorizationConfigs();
builder.Services.AddHttpConfigs();
builder.Services.AddEfCoreConfigs();
builder.Services.AddOpenApi();

var app = builder.Build();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseControllers();
app.UseLogging();

app.Run();
