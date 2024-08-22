using Microsoft.JSInterop;
using Application.Source.Utils.Observer;

namespace Application.Source.Core
{
    public class Display
    {
        private int _width;
        private int _height;
        private readonly IJSRuntime _js;
        private readonly Subject _subject;

        public Display(IJSRuntime js)
        {
            _js = js;
            _width = 0;
            _height = 0;
            _subject = new OnResizeSubject(this);
            _subject.Notify();
        }

        public int Width => _width;

        public int Height => _height;

        public Subject OnResize => _subject;

        private async Task Update()
        {
            _width = await _js.InvokeAsync<int>("eval", "window.innerWidth");
            _height = await _js.InvokeAsync<int>("eval", "window.innerHeight");
        }

        public class OnResizeSubject(Display display) : Subject
        {
            private readonly Display display = display;

            public override void Notify()
            {
                Update();
            }

            private async void Update()
            {
                await display.Update();
                base.Notify();
            }
        }
    }
}