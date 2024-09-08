using Microsoft.JSInterop;

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
            var connection = new IConnection(this, name);
            _connections.Add(connection);
            return connection;
        }
    }
}