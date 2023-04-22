using backend_.Connection;
using System.Net;

namespace backend_.Connection.ControllerConnection
{
    public delegate void Command(byte[] controllerData);
    public interface IControllerConnection
    {  
        public bool AddComand(string ControllerAddress, string OutputId, string comand);
        public bool DeleteComand(string ControllerAddress, string OutputId);

        public IControllerCommand GetCommand(string ControllerAddress,string OutputId);

        public Task Start();
        public void Stop();

    }
}
