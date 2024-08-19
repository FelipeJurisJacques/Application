using Microsoft.JSInterop;
using Application.Source.Utils.Observer;

namespace Application.Source.Core
{
    public class Themes(IJSRuntime js) : List<Theme>()
    {
        private int current = 0;
        private readonly IJSRuntime js = js;
        private readonly Subject subject = new OnChangeSubject();

        public Theme Theme => this[current];

        public Subject OnChange => subject;

        public async void Initialize()
        {
            await GetCurrentThemeName();
        }

        public async Task<bool> SetCurrentThemeName(string name)
        {
            for (var i = 0; i < Count; i++)
            {
                if (name == this[i].Name)
                {
                    if (i != current)
                    {
                        current = i;
                        subject.Notify();
                        await js.InvokeVoidAsync("localStorage.setItem", "theme", name);
                    }
                    return true;
                }
            }
            return false;
        }

        public async Task<string> GetCurrentThemeName()
        {
            var name = await js.InvokeAsync<string>("localStorage.getItem", "theme");
            if (name != "" && name != Theme.Name)
            {
                if (await SetCurrentThemeName(name))
                {
                    return name;
                }
            }
            return "";
        }

        public class OnChangeSubject : Subject { }
    }
}
