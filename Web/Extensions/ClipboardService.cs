using Microsoft.JSInterop;

namespace PicPay.Web.Extensions;

public class ClipboardService(IJSRuntime JsInterop)
{
    public async Task CopyToClipboard(string text)
	{
		await JsInterop.InvokeVoidAsync("navigator.clipboard.writeText", text);
	}
}
