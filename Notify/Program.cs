var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(options => options.MapControllers());

builder.WebHost.UseUrls("http://*:5005");

app.Run();

public partial class Program;
public class NotifyProgram : Program { }
