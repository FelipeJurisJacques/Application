using Microsoft.JSInterop;
using Application.Source.Utils.Observer;

namespace Application.Source.Core
{
    public class Themes(IJSRuntime js) : List<Theme>()
    {
        private int current = 0;
        private readonly Subject subject = new OnChangeSubject(js);

        public Theme Theme => this[current];

        public string Current
        {
            get
            {
                return Theme.Name;
            }
            set
            {
                for (var i = 0; i < Count; i++)
                {
                    if (value == this[i].Name)
                    {
                        current = i;
                        break;
                    }
                }
            }
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
