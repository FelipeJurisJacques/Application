using Microsoft.JSInterop;

namespace Application.Source.Core.Storage.IndexedDb
{
    public class Connections(IJSRuntime js)
    {
        private readonly IJSRuntime _js = js;
        private readonly List<Connection> _connections = [];

        public Connection Open(string name)
        {
            foreach (var conn in _connections)
            {
                if (name == conn.Name)
                {
                    return conn;
                }
            }
            var connection = new Connection(_js, name);
            _connections.Add(connection);
            return connection;
        }
    }
}