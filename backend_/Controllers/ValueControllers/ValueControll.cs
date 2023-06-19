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
                }
                else
                {

                    var OutputGroupsDB = scope.ServiceProvider.GetService<GroupDBContext>();
                    var outputsGroups = new List<backend_.Models.controllerGroup.ControllerOutputGroupUser>();
                    foreach (var Role in user.userRoles)
                    {
                        outputsGroups.AddRange(await OutputGroupsDB.GetOutputGroups(Role.id));
                    }
                    var controllerDB = scope.ServiceProvider.GetService<ControllerDBContext>();
                    var Outputs = new List<ControllerOutput>();

                    foreach (var Group in outputsGroups)
                    {
                        Outputs.AddRange(await controllerDB.GetControllerOutputsWithOutputGroup(Group.id));
                    }
                    Outputs.ForEach(item =>
                    {
                        this.AddToGroup(Context.ConnectionId, new OutputId() { ip = item.controllerAddress, outputId = item.id });
                    });
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
