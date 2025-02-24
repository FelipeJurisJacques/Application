using Microsoft.AspNetCore.Components;

namespace Application.Source.Helpers
{
    public class Widget: ComponentBase
    {
        public void Render()
        {
            StateHasChanged();
        }
    }
}
