using Microsoft.JSInterop;
using Application.Source.Utils.Observer;

namespace Application.Source.Core
{
    public class Theme
    {
        private Themes _theme;
        private readonly IJSRuntime _js;
        private OnChangeSubject _onChange;

        public Theme(IJSRuntime js)
        {
            _js = js;
            _theme = Themes.DARK;
            _onChange = new(this, js);
        }

        public Themes Current
        {
            get { return _theme; }
            set
            {
                _theme = value;
                switch (value)
                {
                    case Themes.LIGHT:
                        _js.InvokeVoidAsync("localStorage.setItem", "theme", "light");
                        break;
                    case Themes.HIGH_CONTRAST:
                        _js.InvokeVoidAsync("localStorage.setItem", "theme", "high_contrast");
                        break;
                    case Themes.DARK:
                    default:
                        _js.InvokeVoidAsync("localStorage.setItem", "theme", "dark");
                        break;
                }
            }
        }

        public OnChangeSubject OnChange => _onChange;

        public class OnChangeSubject : Subject
        {
            private readonly Theme _theme;
            private readonly IJSRuntime _js;

            public OnChangeSubject(Theme theme, IJSRuntime js)
            {
                _js = js;
                _theme = theme;
                _update();
            }

            private async Task _update()
            {
                string value = await _js.InvokeAsync<string>("localStorage.getItem", "theme");
                switch (value)
                {
                    case "light":
                        _theme._theme = Themes.LIGHT;
                        break;
                    case "high_contrast":
                        _theme._theme = Themes.HIGH_CONTRAST;
                        break;
                    case "dark":
                    default:
                        _theme._theme = Themes.DARK;
                        break;
                }
                base.Notify();
            }
        }
    }

    public enum Themes
    {
        DARK,
        LIGHT,
        HIGH_CONTRAST,
    }
}
