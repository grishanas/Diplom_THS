using System.Net;
using System.Net.Sockets;

namespace backend_.Connection.ControllerConnection.OmronController.TransportLayer
{
    public class TCPClient:IControllerConnect
    {
        public IPEndPoint _iPEndPoint { get; private set; }
        private TcpClient client;

        public override string ToString()
        {
            return _iPEndPoint.ToString();
        }

        public byte[] buffer { get;private set; }

        public TCPClient(IPEndPoint iPEndPoint)
        {
            _iPEndPoint = iPEndPoint;
        }
        public TCPClient()
        {
            _iPEndPoint = new IPEndPoint(0, 0);
        }

        public void SetIpAddress(string ipAddress, int port)
        {
            this._iPEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
        }

        public void SetIpAddress(UInt32 ipAddress, int port)
        {
            this._iPEndPoint = new IPEndPoint(new IPAddress(ipAddress), port);
        }

        public void Disconect()
        {
            client.Close();

        }

        public bool Connect()
        {
            client = new TcpClient();
            client.Connect(_iPEndPoint);
            return client.Connected;
        }

        public async Task<byte[]> ReadData(int lengthData)
        {
            var responce = new byte[lengthData];
            using (var networkStream = client.GetStream())
            {

                await networkStream.ReadAsync(buffer, 0, lengthData);

            }
            return responce;
        }

        public async Task WriteData(byte[] buffer,int length)
        {
            using(var networkStream = client.GetStream())
            {
                await networkStream.WriteAsync(buffer, 0, length);
            }

        }


    }
}
