using backend_.Connection.ControllerConnection.OmronController.FinsCmd;
using backend_.Connection.ControllerConnection.OmronController.TransportLayer;
using System.Collections.Generic;
using System.Collections.Concurrent;
using backend_.Connection.ControllerConnection;
using System.Net;
using System.Threading;


namespace backend_.Connection.ControllerConnection.OmronController
{

    public class OmronConnectionController : IControllerConnection
    {
        public UInt32 id { get; }

        public static List<State> AllowedState { get; } = new List<State>()
        {
            new State(){IsRun = true,description = "RUN"},
            new State(){IsRun = false, description = "STOP"}
        };

        public static List<string> GetVersion { get; } = new List<string>()
        {
            "CJ2M-CPU33",
        };
        public static string GetName { get; } = "OMRON";

        private IControllerConnect connect;

        private ConcurrentDictionary<string,IControllerCommandImplementation> controllerCommand =
            new ConcurrentDictionary<string, IControllerCommandImplementation>();

        public List<string> AllowedCommand { get; private set; } = new List<string>();
        public State IsRun { get; set; }

        public OmronConnectionController(Models.controller.Controller controller)
        {
            id = controller.IpAddress;
            this.IsRun = new State();
        }


        public OmronConnectionController(UInt32 address,int port)
        {
            this.id = address;
            this.IsRun = new State();
            AllowedCommand = FinsComand.allowedCommand;

            connect = new TCPClient();
            connect.SetIpAddress(address, port);
        }


        public IControllerCommand? GetCommand(string OutputId)
        {
            controllerCommand.TryGetValue(OutputId, out var cmd);
            return cmd;
        }



        public async void Stop()
        {
            lock(this.IsRun)
            {
                this.IsRun.IsRun = false;
            }
        }

        public async Task Start()
        {
            lock(this.IsRun)
            {
                this.IsRun.IsRun = true;
            }
            while (this.IsRun.IsRun)
            {
                foreach(var cmd in controllerCommand)
                {
                    var res = await cmd.Value.ExecuteCommand();
                }
                
            }
        }



        #region FinsComand
        public bool AddCommand(string OutputId,string? command)
        {
            var FinsCommand = new FinsComand(this.id,int.Parse(OutputId));
            if(command != null)
                FinsCommand.SetCommand(command);
            FinsCommand.SetTransportLaeyr(this.connect);
            controllerCommand.TryAdd(OutputId, FinsCommand);
            return true;

        }
        public bool DeleteComand(string OutputId)
        {
            controllerCommand.TryRemove(OutputId,out var cmd);
            return true;
        }

        #endregion



    }
}
