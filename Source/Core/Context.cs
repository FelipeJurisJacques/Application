using Microsoft.JSInterop;

namespace Application.Source.Core
{
    public class Context
    {
        private readonly IJSRuntime _js;
        private readonly Display _display;
        private readonly Console _console;

        public Context(IJSRuntime js)
        {
            _js = js;
            _display = new(js);
            _console = new(js);
            _js.InvokeVoidAsync(
                "eventsInterop.initialize",
                DotNetObjectReference.Create(this)
            );
        }

        public Console Console => _console;

        public Display Display => _display;

        [JSInvokable]
        public void OnEvent(string type)
        {
            switch (type)
            {
                case "resize":
                    _display.OnResize.Notify();
                    break;
                default:
                    _js.InvokeVoidAsync("console.log", type);
                    break;
            }
        }
    }
}