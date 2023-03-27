using System.Net;
using System.Net.Sockets;
using System.Collections.Concurrent;

namespace backend_.Connection.ControllerConnection.OmronController.TransportLayer
{
    public class TCPServer
    {
        private ConcurrentDictionary<string,TCPClient> clients = new ConcurrentDictionary<string, TCPClient>();
        public TCPServer()
        {
            
        }

        public async Task RemoveClient(string endPoint)
        {
            TCPClient tCPClient = null;
            if (clients.TryRemove(endPoint, out tCPClient))
            {
                tCPClient.Disconect();
            }
        }

        public async Task AddClient(IPEndPoint endpoint,TCPClient client)
        {
            clients.TryAdd(endpoint.ToString(), client);
        }
    }
}
