using System.Net;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace PicPay.Web.Auth;

public class PicPayDelegatingHandler(ILocalStorageService storage, NavigationManager nav, PicPayAuthStateProvider auth) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        var token = await storage.GetItemAsync("AccessToken");

        if (token != null)
        {
            request.Headers.Add("Authorization", $"Bearer {token}");
        }

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await storage.RemoveItemAsync("AccessToken");
            auth.MarkUserAsLoggedOut();
            if (!nav.Uri.Equals("/"))
                nav.NavigateTo("/", forceLoad: true);
        }

        return response;
    }
}
