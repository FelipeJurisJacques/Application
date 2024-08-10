using Microsoft.JSInterop;

namespace Application.Source.Core
{
    public class Console(IJSRuntime js)
    {
        private readonly IJSRuntime _js = js;

        public void Log(string message)
        {
            _js.InvokeVoidAsync("console.log", message);
        }
    }
}
