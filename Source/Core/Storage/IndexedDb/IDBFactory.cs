using Microsoft.JSInterop;

namespace Application.Source.Core.Storage.IndexedDb
{
    public class IDBFactory(IJSRuntime js)
    {
        internal readonly IJSRuntime JS = js;

        public IDBOpenDBRequest Open(string name)
        {
            return new IDBOpenDBRequest(this, name);
        }

        public IDBOpenDBRequest Open(string name, int version)
        {
            return new IDBOpenDBRequest(this, name, version);
        }

        public class IDBOpenDBRequest
        {
            internal int? Version = null;
            internal readonly string Name;
            internal readonly IDBFactory Parent;
            internal readonly TaskCompletionSource<IJSObjectReference> Request;

            public IDBOpenDBRequest(IDBFactory parent, string name)
            {
                Name = name;
                Parent = parent;
                Version = null;
                Request = new();
                Start();
            }

            public IDBOpenDBRequest(IDBFactory parent, string name, int version)
            {
                Name = name;
                Parent = parent;
                Request = new();
                Version = version;
                Start();
            }

            private async void Start()
            {
                if (Version == null)
                {
                    Request.SetResult(await Parent.JS.InvokeAsync<IJSObjectReference>(
                        "window.indexedDB.open",
                        Name
                    ));
                }
                else
                {
                    Request.SetResult(await Parent.JS.InvokeAsync<IJSObjectReference>(
                        "window.indexedDB.open",
                        Name,
                        Version
                    ));
                }
            }

            [JSInvokable]
            public async Task OnEvent(string type)
            {
                await Parent.JS.InvokeVoidAsync(
                    "window.console.log",
                    type
                );
            }

            public class IDBUpgrade
            {
                internal readonly IDBOpenDBRequest Parent;

                public IDBUpgrade(IDBOpenDBRequest parent)
                {
                    Parent = parent;
                }
            }
        }
    }
}