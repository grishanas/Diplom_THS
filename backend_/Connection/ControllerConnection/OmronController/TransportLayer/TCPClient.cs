using System.Net;
using System.Net.Sockets;

namespace backend_.Connection.ControllerConnection.OmronController.TransportLayer
{
    public class TCPClient:IControllerConnect
    {
        public IPEndPoint _iPEndPoint { get; private set; }
        private TcpClient client;
        public int bufferSize 
        {
            get { return bufferSize; }
            private set 
            {
                this.bufferSize = value; this.buffer = new byte[value]; 
            } 
        }

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

        public async Task<byte[]> ReadData()
        {
            var buffer = new byte[bufferSize];
            var responce = new byte[bufferSize];
            int bytes = 0;
            using (var networkStream = client.GetStream())
            {
                do
                {
                    bytes = await networkStream.ReadAsync(buffer);
                } while (bytes > 0);
                responce = buffer; 

            }
            return responce;
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
