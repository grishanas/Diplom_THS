using System.Collections.Concurrent;
using backend_.Models.UserModels;
using System.IO.Pipelines;
using backend_.DataBase.ControllerDB;
using backend_.DataBase.UserDB;
using backend_.Models.controllerGroup;

namespace backend_.Connection.UserConnection
{
    public class UserConnectionController
    {
        public ConcurrentDictionary<PipeWriter, List<UserConnection>> userConnction { get; set; } = new ConcurrentDictionary<PipeWriter, List<UserConnection>>();

        private readonly IServiceScopeFactory _serviceScopeFactory;
        public UserConnectionController(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async void AddUser(int userID,PipeWriter writer)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var UserDB = scope.ServiceProvider.GetService<UserDBContext>();
                var user = await UserDB.Get(userID);

                var RoleDB = scope.ServiceProvider.GetService<ControllerGroupDBContext>();

                List<Models.controllerGroup.ControllerOutputGroupUser> Groups = new List<Models.controllerGroup.ControllerOutputGroupUser>();
                foreach(var item in user.userRoles)
                {
                    var OutputRoles = await RoleDB.GetOutputGroupsWithRole(item.id);
                    Groups.AddRange(OutputRoles);
                }

                var ControllerDB = scope.ServiceProvider.GetService<ControllerDBContext>();
                foreach (var item in Groups)
                {
                    var output = await ControllerDB.GetControolerOutputs(item.id);
                }


            }
        }

        public void DeleteUaser(int userID)
        {

        }



    }
}
