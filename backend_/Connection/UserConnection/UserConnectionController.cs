using System.Collections.Concurrent;
using backend_.Models.UserModels;
using System.IO.Pipelines;
using backend_.DataBase.ControllerDB;
using backend_.DataBase.UserDB;
using backend_.Models.controllerGroup;
using backend_.Models.controller;

namespace backend_.Connection.UserConnection
{
    public class UserConnectionController
    {
        public ConcurrentDictionary<UInt32, UserConnection> userConnction { get; set; } = new ConcurrentDictionary<UInt32, UserConnection>();

        private readonly IServiceScopeFactory _serviceScopeFactory;
        public UserConnectionController(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }


        private void connect(byte[] data)
        {

        }

        public async Task<List<ControllerOutput>> AddUser(int userID,PipeWriter writer,PipeReader reader)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var UserDB = scope.ServiceProvider.GetService<UserDBContext>();
                var user = await UserDB.Get(userID);

                if (user.userRoles.FirstOrDefault(x => x.description == "Admin") != null)
                {
                    var userConnection=new UserConnection(writer,reader){ user = user};
                    userConnction.TryAdd((UInt32)user.id, userConnection);
                    var outputs = new List<ControllerOutput>();

                    var ControllerDB = scope.ServiceProvider.GetService<ControllerDBContext>();
                    var controllers = await ControllerDB.GetAllControllers();
                    controllers.ForEach(x =>
                    {
                        outputs.AddRange(x.outputs);
                    });
                    return outputs;
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

                    return new List<ControllerOutput>();
                }

            }
        }

        public void DeleteUaser(int userID)
        {

        }



    }
}
