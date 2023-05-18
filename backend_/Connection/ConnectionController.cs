using System.Collections.Generic;
using backend_.Connection.ControllerConnection;
using backend_.Connection.UserConnection;
using System.Collections.Concurrent;
using backend_.DataBase.ControllerDB;
using backend_.Models.controller;
using backend_.Connection.ControllerConnection.OmronController;

namespace backend_.Connection
{

    public interface IControllerManufactory
    {
        public IControllerConnection Create(UserController controller);
        public static ControllerVersion GetControllerName { get; }
        public List<State> states { get; }
    }



    public class OmronManufactory : IControllerManufactory
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

        public List<State> states { get; } = OmronConnectionController.AllowedState;
    }

    public class ControllerVersion
    {
        public string Name { get; set; }
        public List<string> version { get; set; }
    }

    public static class ControllerFactory
    {
        public static Dictionary<ControllerVersion, IControllerManufactory> some { get; } = new Dictionary<ControllerVersion, IControllerManufactory>()
        {
            { OmronManufactory.GetControllerName, new OmronManufactory() },
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



    public class ConnectionController : BackgroundService,IDisposable
    {
        private ConcurrentDictionary<IOwnerConnection, List<IControllerCommand>> _userConnections = new ConcurrentDictionary<IOwnerConnection, List<IControllerCommand>>();
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

                            var ControllerCommand = controllerConnection.GetCommand(command.id.ToString());
                            if (command.outputState.description == "RUN")
                            {
                                
                                lock(ControllerCommand.IsRun)
                                {
                                    ControllerCommand.IsRun.IsRun = true;
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


        public bool SetOutputState(State state,UInt32 address,int id)
        {
            _taskManager.TryGetValue(address, out var controller);
            var command = controller.GetCommand(id.ToString());
            lock(command.IsRun)
            {
                command.IsRun = state;
            }
            return true;
        }

        public List<State> GetAllowedState(string ControllerName,string controllerVersion)
        {

            var NamesAndVersions = ControllerFactory.some;
            var value = NamesAndVersions.FirstOrDefault(x => x.Key.Name == ControllerName && x.Key.version.Contains(controllerVersion));

            var states = value.Value.states;
            return states;
        }

        public ConnectionController(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
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

        public List<string> GetAllCommands(UInt32 address)
        {
            var controller = _taskManager.Where(x=>x.Value.id== address).FirstOrDefault();
            var comand = controller.Value.AllowedCommand;
            return comand;
        }

        public Dictionary<string,List<string>> GetAllowedController()
        {
            var version = ControllerFactory.some.ToList();
            var Dict = new Dictionary<string, List<string>>();
            foreach( var item in version)
            {
                Dict.Add(item.Key.Name, item.Key.version);
            }
            return Dict;
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

        public void AddUserToControllerOutput(UInt32 address,int id, IOwnerConnection userConnection)
        {
            _taskManager.TryGetValue(address,out var Controller);
            var command = Controller.GetCommand(id.ToString());
            command.SetAnswerListener(userConnection.listener);
            _userConnections.TryGetValue(userConnection, out var ListenCommand);
            List<IControllerCommand> newListCommand;
            if (ListenCommand == null)
            {
                newListCommand = new List<IControllerCommand>();
                _userConnections.TryAdd(userConnection, newListCommand);
                lock (newListCommand)
                {
                    newListCommand.Add(command);
                }
            }
            else
            {
                lock(ListenCommand)
                {
                    ListenCommand.Add(command);
                }
            }

        }

        public void DeleteUserFromControllerOutput(UInt32 address,int id,IOwnerConnection ownerConnection)
        {

        }

        public void DeleteUserFromControllerOutputs(IOwnerConnection ownerConnection)
        {
            _userConnections.TryRemove(ownerConnection, out var ListenCommand);
            if (ListenCommand.Count == 0)
                return;
            lock(ListenCommand)
            {
                foreach (var listenCommand in ListenCommand)
                    listenCommand.DeleteAnswerListener(ownerConnection.listener);
            }

        }

        public void Dispose()
        {

        }


    }
}
