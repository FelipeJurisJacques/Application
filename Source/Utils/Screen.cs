using Microsoft.JSInterop;
using Application.Source.Helpers;

namespace Application.Source.Utils
{
    public class Screen
    {
        private int width;
        private int height;
        private readonly IJSRuntime js;
        private readonly List<Widget> handler;
        private readonly DotNetObjectReference<Screen> helper;

        public Screen(IJSRuntime runtime)
        {
            js = runtime;
            width = 1024;
            height = 768;
            handler = [];
            helper = DotNetObjectReference.Create(this);
            Initialize();
        }

        public int Width { get => width; }

        public int Height { get => height; }

        public void Subscribe(Widget widget)
        {
            handler.Add(widget);
        }

        public void Unsubscribe(Widget widget)
        {
            handler.Remove(widget);
        }

        [JSInvokable]
        public void OnResize(int width, int height)
        {
            this.width = width;
            this.height = height;
            foreach (var widget in handler)
            {
                widget.Render();
            }
        }

        public void Dispose()
        {
            helper.Dispose();
        }

        private async void Initialize()
        {
            try
            {
                await js.InvokeVoidAsync("listenResize", helper);
            }
            catch (Exception error)
            {
                await js.InvokeVoidAsync("console.error", "function initialize() non callable in JS");
            }
        }
    }
}