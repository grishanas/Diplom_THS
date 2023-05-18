using backend_.Connection;
using System.Net;
using backend_.Models.controller;

namespace backend_.Connection.ControllerConnection
{

    public delegate void CommandListener(OutputValue controllerData);


    public class State
    {
        public bool IsRun
        {
            get; set;
        }

        public string description { get; set; }
        public State()
        {
            this.IsRun = false;          
        }
    }
    public interface IControllerConnection
    {
        public UInt32 id { get; }



        public State IsRun { get; set; }
        public bool AddCommand(string OutputId, string? comand);
        public bool DeleteComand(string OutputId);

        public IControllerCommand GetCommand(string OutputId);

        public List<string> AllowedCommand { get;}

        public Task Start();
        public void Stop();

        public static List<string> GetVersion { get; }
        public static string GetName { get; }
        public static List<State> AllowedState { get; }

    }
}
