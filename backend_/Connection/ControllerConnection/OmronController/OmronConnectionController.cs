using backend_.Connection.ControllerConnection.OmronController.FinsCmd;
using backend_.Connection.ControllerConnection.OmronController.TransportLayer;
using System.Collections.Generic;
using System.Collections.Concurrent;
using backend_.Connection.ControllerConnection;
using System.Net;


namespace backend_.Connection.ControllerConnection.OmronController
{
    public class OmronConnectionController : IControllerConnection
    {
        private ConcurrentDictionary<string, ConcurrentDictionary<string,FinsComand>> FinsComand;
        private ConcurrentDictionary<string, TCPClient> Controllers;


        public void SetOutPutGroup(string ControllerAddress, string OutputId, string Group, Command command)
        {
            var res = FinsComand[ControllerAddress][Group+ OutputId];
            res.SetAnswerListener(command);

        }
        public void DeleteOutPutGroup(string ControllerAddress, string OutputId, string Group, Command command)
        {
            var res = FinsComand[ControllerAddress][Group + OutputId];
            res.DeleteAnswerListener(command);
        }




        private async Task CallController(List<FinsComand> fins)
        {
            foreach (FinsComand finsComand in fins)
                await finsComand.Call();
        }


        public async Task StartCalling()
        {

            while(true)
            {
                
            }
        }


        public async Task<bool> AddNewController(IPEndPoint ip)
        {
            var TCPconnection = new TCPClient(ip);
            var result = this.Controllers.TryAdd(TCPconnection.ToString(), TCPconnection);
            if(result)
            {
                var listComand = new ConcurrentDictionary<string,FinsComand>();
                var res = this.FinsComand.TryAdd(TCPconnection.ToString(), listComand);
            }
            return result;
        }

        public async Task<bool> AddNewController(IPAddress ip,int port)
        {
            var IPEnd = new IPEndPoint(ip, port);
            return await AddNewController(IPEnd);
        }

        public async Task<bool> DeleteController()
        {
            return false;
        }



        #region Add FinsComand

        #endregion



    }
}
