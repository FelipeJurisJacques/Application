using Microsoft.JSInterop;
using Application.Source.Utils.Observer;

namespace Application.Source.Core
{
    public class Themes(IJSRuntime js)
    {
        private readonly Subject subject = new OnChangeSubject(js);
        private Theme _theme = new("default", ThemeType.HIGH_CONTRAST, "wite", "black");

        public Theme Theme
        {
            get { return _theme; }
            set { _theme = value; }
        }

        public Subject OnChange => subject;

        public class OnChangeSubject(IJSRuntime js) : Subject
        {
            private readonly IJSRuntime _js = js;

            public async Task<string> GetCurrentThemeName()
            {
                return await _js.InvokeAsync<string>("localStorage.getItem", "theme");
            }
        }
    }
}
