using Microsoft.JSInterop;
using Application.Source.Utils.Observer;

namespace Application.Source.Core
{
    public class Display
    {
        private int _width;
        private int _height;
        private readonly IJSRuntime _js;
        private readonly OnResizeSubject _onResize;

        public Display(IJSRuntime jsRuntime)
        {
            _js = jsRuntime;
            _width = 0;
            _height = 0;
            _onResize = new(this);
        }

        public int Width => _width;

        public int Height => _height;

        public OnResizeSubject OnResize => _onResize;

        public class OnResizeSubject : Subject
        {
            private Display _display;

            public OnResizeSubject(Display display)
            {
                _display = display;
                _update();
            }

            public override void Notify()
            {
                _update();
            }

            private async Task _update()
            {
                _display._width = await _display._js.InvokeAsync<int>("eval", "window.innerWidth");
                _display._height = await _display._js.InvokeAsync<int>("eval", "window.innerHeight");
                base.Notify();
            }
        }
    }
}