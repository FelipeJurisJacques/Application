using Microsoft.JSInterop;
using Application.Source.Utils.Observer;

namespace Application.Source.Core
{
    public class Display(IJSRuntime js)
    {
        private int _width = 0;
        private int _height = 0;
        private readonly IJSRuntime _js = js;
        private readonly Subject _subject = new OnResizeSubject();

        public int Width => _width;

        public int Height => _height;

        public Subject OnResize => _subject;

        public async void Initialize()
        {
            await Update();
            OnResize.Notify();
        }

        public async Task Update()
        {
            _width = await _js.InvokeAsync<int>("eval", "window.innerWidth");
            _height = await _js.InvokeAsync<int>("eval", "window.innerHeight");
        }

        public class OnResizeSubject : Subject { }
    }
}