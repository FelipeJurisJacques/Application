using Microsoft.AspNetCore.Components;

namespace Application.Source.Helpers
{
    public abstract class Widget: ComponentBase
    {
        public virtual void Render()
        {
            StateHasChanged();
        }
    }
}
