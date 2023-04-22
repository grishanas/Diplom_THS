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
        
        private ConcurrentDictionary<string, ConcurrentDictionary<string,IControllerCommandImplementation>> controllerCommand;
        private ConcurrentDictionary<string, TCPClient> Controllers;

        private locker IsRun = new locker();

        private class locker 
        {    
            public bool IsRun = false;
        }

        public IControllerCommand GetCommand(string ControllerAddress,string OutputId)
        {
            controllerCommand.TryGetValue(ControllerAddress, out var cmd);
            cmd.TryGetValue(OutputId, out var res);
            return res;
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
                foreach(var controller in this.Controllers)
                {
                    this.controllerCommand.TryGetValue(controller.Key, out var cmds);
                    foreach(var cmd in cmds)
                    {
                        var res = await cmd.Value.ExecuteCommand();
                    }
                }
            }
        }


        #region controller
        public async Task<bool> AddNewController(IPEndPoint ip)
        {

            this.Controllers.TryGetValue(ip.ToString(), out var controller);
            if (controller != null)
                return false;

            var TCPconnection = new TCPClient(ip);
            var result = this.Controllers.TryAdd(TCPconnection.ToString(), TCPconnection);
            if(result)
            {
                var listComand = new ConcurrentDictionary<string, IControllerCommandImplementation>();
                var res = this.controllerCommand.TryAdd(TCPconnection.ToString(), listComand);
            }
            return result;
        }

        public async Task<bool> AddNewController(string ip,int port)
        {
            var IPEnd = new IPEndPoint(IPAddress.Parse(ip), port);
            return await AddNewController(IPEnd);
        }

        public async Task<bool> DeleteController()
        {
            return false;
        }
        #endregion



        #region FinsComand
        public bool AddComand(string ControllerAddress, string OutputId,string command)
        {
            var FinsCommand = new FinsComand();
            FinsCommand.SetCommand(command);
            this.Controllers.TryGetValue(ControllerAddress, out var Controller);
            FinsCommand.SetTransportLaeyr(Controller);
            return true;

        }
        public bool DeleteComand(string ControllerAddress, string OutputId)
        {

            this.controllerCommand.TryGetValue(ControllerAddress, out var ComandOnController);
            ComandOnController.TryRemove(OutputId,out var cmd);
            return true;
        }

        #endregion



    }
}
