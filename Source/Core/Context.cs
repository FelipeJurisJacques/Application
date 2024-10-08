using Microsoft.JSInterop;
using Application.Source.Core.Storage.IndexedDb;

namespace Application.Source.Core
{
    public class Context
    {
        private readonly IJSRuntime _js;
        private readonly Themes _themes;
        private readonly Console _console;
        private readonly Display _display;
        private readonly Connections _connections;

        public Context(IJSRuntime js)
        {
            _js = js;
            _themes = new(js);
            _console = new(js);
            _display = new(js);
            _connections = new(js);
            Initialize();
        }

        public Themes Themes => _themes;

        public Console Console => _console;

        public Display Display => _display;

        public Connections IndexedDb => _connections;

        public async void Initialize()
        {
            await _js.InvokeVoidAsync(
                "window.interop.initialize",
                DotNetObjectReference.Create(this)
            );
            Themes.Initialize();
        }

        [JSInvokable]
        public void OnEvent(string type)
        {
            switch (type)
            {
                case "resize":
                    Display.OnResize.Notify();
                    break;
                default:
                    Console.Log(type);
                    break;
            }
        }
    }
}