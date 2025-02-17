using Microsoft.JSInterop;

namespace Application.Source.Utils
{
    public class Interoperability
    {
        private readonly IJSRuntime? js;
        private readonly IJSObjectReference? handler;

        protected Interoperability(IJSRuntime reference)
        {
            js = reference;
            handler = null;
        }

        public async ValueTask<T> GetAsync<T>(string identifier)
        {
            return await InvokeAsync<T>("callGetterAttribute", identifier);
        }

        public async ValueTask SetAsync(string identifier, object value)
        {
            await InvokeVoidAsync("callSetterAttribute", identifier, value);
        }

        public async ValueTask InvokeVoidAsync(string identifier, params object[] args)
        {
            if (js is not null)
            {
                await js.InvokeVoidAsync(identifier, args);
            }
            if (handler is not null)
            {
                await handler.InvokeVoidAsync(identifier, args);
            }
            throw new InvalidOperationException("IJSRuntime is not available");
        }

        public async ValueTask<T> InvokeAsync<T>(string identifier, params object[] args)
        {
            if (js is not null)
            {
                return await js.InvokeAsync<T>(identifier, args);
            }
            if (handler is not null)
            {
                await handler.InvokeAsync<T>(identifier, args);
            }
            throw new InvalidOperationException("IJSRuntime is not available");
        }
    }
}