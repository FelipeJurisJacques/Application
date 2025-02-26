using Microsoft.JSInterop;

namespace Application.Source.Singletons
{
    public class Agent
    {
        private readonly IJSRuntime js;
        private IJSObjectReference? handler;
        private readonly DotNetObjectReference<Agent> helper;

        public Agent(IJSRuntime reference)
        {
            js = reference;
            handler = null;
            helper = DotNetObjectReference.Create(this);
        }

        public async void Initialize()
        {
            try
            {
                handler = await js.InvokeAsync<IJSObjectReference>("initializeAgent", helper);
            }
            catch (Exception)
            {
                await js.InvokeVoidAsync("console.error", "function initializeAgent() non callable in JS");
            }
        }

        public void Dispose()
        {
            helper.Dispose();
        }
    }
}