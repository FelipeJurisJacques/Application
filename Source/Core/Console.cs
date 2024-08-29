using Microsoft.JSInterop;

namespace Application.Source.Core
{
    public class Console(IJSRuntime js)
    {
        private readonly IJSRuntime js = js;

        public void Log(string message)
        {
            js.InvokeVoidAsync("console.log", message);
        }

        public void Error(string error)
        {
            js.InvokeVoidAsync("console.error", error);
        }

        public void Error(Exception error)
        {
            if (error.StackTrace == null)
            {
                Error(error.Message);
            }
            else
            {
                Error(error.Message + error.StackTrace.ToString());
            }
        }
    }
}
