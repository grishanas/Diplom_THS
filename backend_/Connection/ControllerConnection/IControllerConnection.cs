using backend_.Connection;
using System.Net;

namespace backend_.Connection.ControllerConnection
{
    public delegate void Command(byte[] controllerData);

    public class locker
    {
        public bool IsRun
        {
            get;set;
        } 

        public locker()
        {
            this.IsRun = false;          
        }
    }
    public interface IControllerConnection
    {
        public UInt32 id { get; }

        public locker IsRun { get; set; }
        public bool AddCommand(string OutputId, string? comand);
        public bool DeleteComand(string OutputId);

        public IControllerCommand GetCommand(string OutputId);

        public List<string> AllowCommand { get;}

        public Task Start();
        public void Stop();

        public static List<string> GetVersion { get; }
        public static string GetName { get; }

    }
}
