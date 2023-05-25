using System.Collections.Generic;
using backend_.Connection.ControllerConnection;
using System.Collections.Concurrent;
using backend_.DataBase.ControllerDB;
using backend_.Models.controller;
using backend_.Connection.ControllerConnection.OmronController;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Text;

namespace backend_.Connection
{

    public interface IControllerManufactory
    {
        public IControllerConnection Create(UserController controller);
        public IControllerConnection Create();

        public static ControllerVersion GetControllerName { get; }
        public List<State> states { get; }
    }



    public class OmronManufactory : IControllerManufactory
    {
        public IControllerConnection Create(UserController userController)
        {
            return new OmronConnectionController(userController.IpAddress, userController.IpPort);
        }

        public IControllerConnection Create()
        {
            return new OmronConnectionController();
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
                        
                        foreach (var command in controller.outputs)
                        {
                            controllerConnection.AddCommand(command.id.ToString(), command.Query!=null?command.Query.query:null);

                            var ControllerCommand = controllerConnection.GetCommand(command.id.ToString());
                            var CommandState = ControllerCommand.GetAllowedState().FirstOrDefault(x=>x.description==command.outputState.description);
                            if(CommandState != null)
                            {
                                ControllerCommand.SetState(CommandState);
                                ControllerCommand.SetAnswerListener(this.AnswerListener);
                            }
                            else
                            {
                                Console.WriteLine("Exception");
                            }
                        }
                        var states = controllerConnection.GetAllowedState();
                        var ControllerState = states.FirstOrDefault(x => x.description == controller.ControllerState.Description);
                        if(ControllerState!=null)
                        {
                            controllerConnection.SetState(ControllerState);
                        }
                        controllerConnection.Start();
                    }
                }
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Initialize();
            
        }


        public bool SetOutputState(string state,UInt32 address,int id)
        {
            _taskManager.TryGetValue(address, out var controller);
            if(controller==null)
                return false;
            var command = controller.GetCommand(id.ToString());
            if (command == null)
                return false;
            var states = command.GetAllowedState();
            var commandState = states.FirstOrDefault(x => x.description == state);
            if (commandState == null)
                return false;
            command.SetState(commandState);
            return true;
        }

        public List<State> GetAllowedState(string ControllerName,string controllerVersion)
        {

            var NamesAndVersions = ControllerFactory.some;
            var value = NamesAndVersions.FirstOrDefault(x => x.Key.Name == ControllerName && x.Key.version.Contains(controllerVersion));
            var states = value.Value.states;
            return states;
        }
        public List<State> GetAllowedOutputState(string ControllerName, string controllerVersion)
        {
            var NamesAndVersions = ControllerFactory.some;
            var controller = NamesAndVersions.FirstOrDefault(x => x.Key.Name == ControllerName && x.Key.version.Contains(controllerVersion));
            var states = controller.Value.Create().GetAllowedState();

            return states;
        }

        private class OutValue
        {
            public UInt32 value { get; set; }
            public UInt32 controllerAddress { get; set; }
            public int controllerOutputId { get; set; }
            public DateTime DateTime { get; set; }

            public OutValue(OutputValue value)
            {
                this.value = (UInt32)(value.value[0] + ((int)value.value[1] << 8) + ((int)value.value[2] << 16) + ((int)value.value[3] << 24));
                controllerAddress = value.controllerAddress;
                controllerOutputId = value.controllerOutputId;
                DateTime = value.DateTime;
            }
        }
        private async Task AnswerListener(OutputValue value)
        {

            var res = new OutValue(value);

            await hub.Clients
                .Group(new backend_.Controllers.ValueControllers.ValueControll.OutputId() { ip = value.controllerAddress, outputId = value.controllerOutputId }
                    .ToString())
                .SendAsync("Receive", JsonConvert.SerializeObject(res));
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<ControllerDBContext>();
                dbContext.AddOutputValue(value);
            }
        }

        private readonly IHubContext<backend_.Controllers.ValueControllers.ValueControll> hub;

        public ConnectionController(IServiceScopeFactory serviceScopeFactory, IHubContext<backend_.Controllers.ValueControllers.ValueControll> hubContext)
        {
            _serviceScopeFactory = serviceScopeFactory;
            hub = hubContext;

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


        public void Dispose()
        {

        }


    }
}
