var builder = WebApplication.CreateBuilder(args);
builder.Host.AddLoggingConfigs();
builder.Services.AddSettingsConfigs();
builder.Services.AddServicesConfigs();
builder.Services.AddAuthenticationConfigs();
builder.Services.AddAuthorizationConfigs();
builder.Services.AddHttpConfigs();
builder.Services.AddEfCoreConfigs();
builder.Services.AddOpenApi();
builder.Services.AddDocsConfigs();

var app = builder.Build();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseControllers();
app.UseLogging();
app.UseSwagger();

var ctx = app.Services.GetRequiredService<PicPayDbContext>();
ctx.ResetDb();

app.Run();
