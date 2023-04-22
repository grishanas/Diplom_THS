namespace backend_.Connection.ControllerConnection
{
    public interface IControllerConnect
    {
        public bool Connect();
        public Task<byte[]> ReadData(int length);

        public Task WriteData(byte[] data, int length);

        public void SetIpAddress(string ipAddress, int port);
    }
}
