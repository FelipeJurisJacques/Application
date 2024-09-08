using Microsoft.JSInterop;
using Application.Source.Core.Storage.IndexedDb;

namespace Application.Source.Core.Storage
{
    public class Connections(IJSRuntime js)
    {
        internal readonly IJSRuntime JS = js;
        private readonly List<IConnection> _connections = [];

        public IConnection GetConnection(string name)
        {
            foreach (var conn in _connections)
            {
                if (name == conn.Name)
                {
                    return conn;
                }
            }
            var connection = new Connection(this, name);
            _connections.Add(connection);
            return connection;
        }
    }
}