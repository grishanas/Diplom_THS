using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace backend_.Connection.ControllerConnection.OmronController.TransportLayer
{
    public class TCPClient:IControllerConnect
    {
        private IPEndPoint _iPEndPoint = null;
        private Socket _socket = null;



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
            var tmp = new byte[4];
            tmp[0] = (byte)(((int)ipAddress >> 24) & 0xff);
            tmp[1] = (byte)(((int)ipAddress >> 16) & 0xff);
            tmp[2] = (byte)(((int)ipAddress >> 8) & 0xff);
            tmp[3] = (byte)(((int)ipAddress >> 0) & 0xff);

            this._iPEndPoint = new IPEndPoint(new IPAddress(tmp), port);
        }

        public void Disconect()
        {
           

        }

        private Int32 _timeout = 2000;

        public bool Connect()
        {
            this._socket = new Socket(_iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            this._socket.SendTimeout = this._timeout;
            this._socket.ReceiveTimeout = this._timeout;

            // try to connect
            //
            this._socket.Connect(this._iPEndPoint);

            return this.Connected;
        }


        public bool Connected
        {
            get { return (this._socket == null) ? false : this._socket.Connected; }
        }


        public async Task<byte[]> ReadData(int lengthData)
        {
            byte[] buffer = new byte[lengthData];
            if (!this.Connected)
            {
                throw new Exception("Socket is not connected.");
            }
            int bytesRecv = 0;
            try
            {
                bytesRecv = this._socket.Receive(buffer, lengthData, SocketFlags.None);
            }
            catch(Exception e)
            {
                Console.WriteLine(buffer);
                return new byte[lengthData];
            }

            // check the number of bytes received
            //
            if (bytesRecv != lengthData)
            {
                string msg = string.Format("Receiving error. (Expected: {0}  Received: {1})"
                                            , lengthData, bytesRecv);
                throw new Exception(msg);
            }

            return buffer;
        }

        public async Task WriteData(byte[] buffer,int length)
        {
            if (!this.Connected)
            {
                throw new Exception("Socket is not connected.");
            }

            // sends the command
            //
            int bytesSent = this._socket.Send(buffer, length, SocketFlags.None);

            // it checks the number of bytes sent
            //
            if (bytesSent != length)
            {
                string msg = string.Format("Sending error. (Expected bytes: {0}  Sent: {1})"
                                            , length, bytesSent);
                throw new Exception(msg);
            }
        }

    }

}

