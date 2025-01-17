using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace PicPay.Tests.Base;

public class NotifyFactory : WebApplicationFactory<NotifyProgram>
{
    private bool _disposed;
    private IHost? _kestrelServerHost;

    public string ServerAddress
    {
        get
        {
            EnsureServer();
            return ClientOptions.BaseAddress.ToString();
        }
    }

    public override IServiceProvider Services
    {
        get
        {
            EnsureServer();
            return _kestrelServerHost!.Services!;
        }
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.UseUrls("http://127.0.0.1:0");
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        // https://github.com/dotnet/aspnetcore/issues/33846#issuecomment-890483307
        var testServerHost = builder.Build();

        builder.ConfigureWebHost(p => p.UseKestrel());

        _kestrelServerHost = builder.Build();
        _kestrelServerHost.Start();

        var server = _kestrelServerHost.Services.GetRequiredService<IServer>();
        var addresses = server.Features.Get<IServerAddressesFeature>();

        ClientOptions.BaseAddress = addresses!.Addresses
            .Select(p => new Uri(p))
            .Last();
        
        testServerHost.Start();

        return testServerHost;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (!_disposed)
        {
            if (disposing)
            {
                _kestrelServerHost?.Dispose();
            }
            _disposed = true;
        }
    }

    private void EnsureServer()
    {
        // This forces WebApplicationFactory to bootstrap the server
        using var _ = CreateDefaultClient();
    }
}
