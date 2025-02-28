using Microsoft.AspNetCore.Components;

namespace Application.Source.Helpers
{
    public abstract class Widget : ComponentBase
    {
        protected override void OnInitialized()
        {
            Build();
        }

        public virtual void Build() { }

        public virtual void Render()
        {
            Build();
            StateHasChanged();
        }

        protected virtual void Rendered() { }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                Rendered();
            }
        }
    }
}
