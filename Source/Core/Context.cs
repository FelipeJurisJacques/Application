using Microsoft.JSInterop;

namespace Application.Source.Core
{
    public class Context(IJSRuntime js)
    {
        private readonly IJSRuntime _js = js;
        private readonly Themes _themes = new(js);
        private readonly Console _console = new(js);
        private readonly Display _display = new(js);

        public Themes Themes => _themes;

        public Console Console => _console;

        public Display Display => _display;

        public async void Initialize(){
            await _js.InvokeVoidAsync(
                "eventsInterop.initialize",
                DotNetObjectReference.Create(this)
            );
            Themes.Initialize();
            Display.Initialize();
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