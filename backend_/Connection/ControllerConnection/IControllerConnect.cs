namespace backend_.Connection.ControllerConnection
{
    public interface IControllerConnect
    {
        public bool Connect();
        public void Disconect();
        public Task<byte[]> ReadData(int length);

        public Task WriteData(byte[] data, int length);

        public void SetIpAddress(UInt32 ipAddress, int port);
    }
}
