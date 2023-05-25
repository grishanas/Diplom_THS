using Microsoft.AspNetCore.SignalR;
using backend_.Models.controller;
using backend_.DataBase.UserDB;
using backend_.DataBase.ControllerDB;
using backend_.Connection.ControllerConnection;

namespace backend_.Controllers.ValueControllers
{
    public class ValueControll:Hub
    {

        public class OutputId
        {
            public UInt32 ip { get; set; }
            public int outputId { get; set; }
        }

        private IServiceScopeFactory _serviceScopeFactory { get; }
        private readonly AuthorizationLogic.Authorization _authorization;
        private IConfiguration config { get; }


        public ValueControll(IServiceScopeFactory serviceScopeFactory, IConfiguration config )//AuthorizationLogic.Authorization authorization)
        {

            _serviceScopeFactory = serviceScopeFactory;
            config = config;
            _authorization= new AuthorizationLogic.Authorization(config);


        }
        public async Task Send(OutputValue value)
        {


/*            await this.Clients.All.SendAsync("Receive", value);*/
        }

        private async void AddToGroup(string connectionId, OutputId output)
        {
            await Groups.AddToGroupAsync(connectionId, output.ToString());
        }
        public override async Task OnConnectedAsync()
        {

            Console.WriteLine(Context.ConnectionId);

            var userCookie = Context.GetHttpContext().Request.Cookies.ToList();
            var userID = _authorization.ValidateJWT(userCookie);

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var UserDB = scope.ServiceProvider.GetService<UserDBContext>();
                var user = await UserDB.Get((int)userID);

                if (user.userRoles.FirstOrDefault(x => x.description == "Admin") != null)
                {
                    var ControllerDB = scope.ServiceProvider.GetService<ControllerDBContext>();
                    var controllers = await ControllerDB.GetAllControllers();
                    controllers.ForEach(x =>
                    {
                        x.outputs.ForEach(item =>
                        {
                            this.AddToGroup(Context.ConnectionId, new OutputId() { ip = x.IpAddress, outputId = item.id });
                        }
                        ); 
                    });
                    //await Groups.AddToGroupAsync();
                }
                else
                {

                    var RoleDB = scope.ServiceProvider.GetService<ControllerGroupDBContext>();

                    List<Models.controllerGroup.ControllerOutputGroupUser> Groups = new List<Models.controllerGroup.ControllerOutputGroupUser>();
                    foreach (var item in user.userRoles)
                    {
                        var OutputRoles = await RoleDB.GetOutputGroupsWithRole(item.id);
                        Groups.AddRange(OutputRoles);
                    }

                    var ControllerDB = scope.ServiceProvider.GetService<ControllerDBContext>();
                    foreach (var item in Groups)
                    {
                        var output = await ControllerDB.GetControolerOutputs(item.id);
                    }

                    //return new List<ControllerOutput>();
                }
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
        
            await base.OnDisconnectedAsync(exception);
        }
    }
}
