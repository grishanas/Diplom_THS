using backend_.Connection;

namespace backend_.Connection.ControllerConnection
{
    public delegate void Command(byte[] controllerData);
    public interface IControllerConnection
    {

        public void SetOutPutGroup(string ControllerAddress,string OutputId, string Group,Command command);
        public void DeleteOutPutGroup(string ControllerAddress, string OutputId, string Group, Command command);

        public void AddComand(byte[] comand);
        public void DeleteComand(byte[] comand);

    }
}
