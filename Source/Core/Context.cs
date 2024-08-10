using Microsoft.JSInterop;

namespace Application.Source.Core
{
    public class Context
    {
        private readonly Theme _theme;
        private readonly Console _console;
        private readonly Display _display;

        public Context(IJSRuntime js)
        {
            _theme = new(js);
            _console = new(js);
            _display = new(js);
            js.InvokeVoidAsync(
                "eventsInterop.initialize",
                DotNetObjectReference.Create(this)
            );
        }

        public Theme Theme => _theme;

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
                    Console.Log(type);
                    break;
            }
        }
    }
}