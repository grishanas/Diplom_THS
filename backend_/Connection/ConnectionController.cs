using System.Collections.Generic;
using backend_.Connection.ControllerConnection;
using backend_.Connection.UserConnection;
using System.Collections.Concurrent;
using backend_.DataBase.ControllerDB;
using backend_.Models.controller;
using backend_.Connection.ControllerConnection.OmronController;

namespace backend_.Connection
{

    public interface IControllerFabricant
    {
        public IControllerConnection Create(UserController controller);

        public static ControllerVersion GetControllerName { get; }
    }

    public class Omron1 : IControllerFabricant
    {
        public IControllerConnection Create(UserController userController)
        {
            return new OmronConnectionController(userController.IpAddress, userController.IpPort);
        }

        public static ControllerVersion GetControllerName { get; } = new ControllerVersion()
        {
            Name = OmronConnectionController.GetName,
            version = OmronConnectionController.GetVersion
        };
    }

    public class ControllerVersion
    {
        public string Name { get; set; }
        public List<string> version { get; set; }
    }
    public static class ControllerFactory
    {
        public static Dictionary<ControllerVersion, IControllerFabricant> some { get; } = new Dictionary<ControllerVersion, IControllerFabricant>()
        {
            { Omron1.GetControllerName, new Omron1() },
        };

        public static IControllerConnection Create(UserController controller)
        {
            var controllers = some
                .Where(x => x.Key.Name == controller.controllerName.Name).ToList();
            if (controllers[0].Key.version.Contains(controller.controllerName.version))
            {
                return controllers[0].Value.Create(controller);
            }
            return null;
        }
    }

    public class NameAndVersion
    {
        public string name { get; set; }
        public List<string> version { get; set; } = new List<string>();
    }

    public class ConnectionController : BackgroundService
    {
        /* private ConcurrentDictionary<IControllerConnection> _controllers = new List<IControllerConnection>();*/
        private List<IUserConnection> _userConnections = new List<IUserConnection>();
        private ConcurrentDictionary<UInt32, IControllerConnection> _taskManager = new ConcurrentDictionary<UInt32, IControllerConnection>();
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private async Task Initialize()
        {
            ControllerDBContext controllerDB;
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                controllerDB = scope.ServiceProvider.GetService<ControllerDBContext>();

                var controllers = await controllerDB.GetAllControllers();
                foreach (var controller in controllers)
                {

                    var controllerConnection = ControllerFactory.Create(new UserController()
                    {
                        controllerName = new UserControllerName() { Name = controller.controllerName.name, version = controller.controllerName.version },
                        IpAddress = controller.IpAddress,
                        IpPort = controller.IpPort
                    });

                    var res = _taskManager.TryAdd(controller.IpAddress, controllerConnection);
                    if(res)
                    {
                        foreach(var command in controller.outputs)
                        {
                            controllerConnection.AddCommand(command.id.ToString(), command.Query!=null?command.Query.query:null);
                            if (command.outputState.description == "RUN")
                            {
                                var comand = controllerConnection.GetCommand(command.id.ToString());
                                lock(comand.IsRun)
                                {
                                    comand.IsRun.IsRun = true;
                                }
                            }
                        }
                    }
                    
                }
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Initialize();
            
        }

        public ConnectionController(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Execute()
        {
            
        }

        public void Start(UInt32 id)
        {
            _taskManager.TryGetValue(id, out var controller);
            if (controller == null)
                return;

            controller.Start();
        }
        public void Stop(UInt32 id)
        {
            _taskManager.TryGetValue(id, out var controller);
            if (controller == null)
                return;
            controller.Stop();
        }
        public bool AddController(IControllerConnection controller)
        {
            _taskManager.TryGetValue(controller.id, out var OldController);
            if (OldController != null)
                return false;
            _taskManager.TryAdd(controller.id, controller);
            return true;
        }

        public bool RemoveController(UInt32 id)
        {
            _taskManager.TryGetValue(id, out var Oldcontroller);
            if (Oldcontroller == null)
                return true;
            _taskManager.TryRemove(id, out Oldcontroller);
            if (Oldcontroller == null)
                return false;
            return true;
        }

        public async Task<List<string>> GetAllCommands(UInt32 address)
        {
            var controller = _taskManager.Where(x=>x.Value.id== address).FirstOrDefault();
            var comand = controller.Value.AllowCommand;
            return comand;
        }

        public bool RemoveCommand(UInt32 address,int id)
        {
            _taskManager.TryGetValue(address, out var Controller);
            if (Controller == null)
                return false;
            var res = Controller.DeleteComand(id.ToString());
            return res;
        }

        public bool SetCommand(UInt32 address,int id,string command)
        {
            _taskManager.TryGetValue(address, out var Controller);
            if (Controller == null)
                return false;
            var res = Controller.AddCommand(id.ToString(), command);
            if(res == false)
            {
                _taskManager.TryRemove(address, out var Oldcontroller);
            }
            return res;


        }

        public async Task AddUserToController(IControllerConnection controller, IUserConnection userConnection)
        {

        }
    }
}
