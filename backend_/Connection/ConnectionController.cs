using System.Collections.Generic;
using backend_.Connection.ControllerConnection;
using backend_.Connection.UserConnection;
using System.Collections.Concurrent;

namespace backend_.Connection
{
    public class ConnectionController
    {
        private List<IControllerConnection> _controllers = new List<IControllerConnection>();
        private List<IUserConnection> _userConnections = new List<IUserConnection>();
        private Dictionary<IControllerConnection,Task> _taskManager = new Dictionary<IControllerConnection, Task>();


        public ConnectionController()
        {

        }

        public async Task AddController(IControllerConnection controller)
        {

            lock (_controllers)
            {
                _controllers.Add(controller);
            }

            lock(_taskManager)
            {

            }
        }   

        public async Task RemoveController(IControllerConnection controller)
        {

            lock (_controllers)
            {
                _controllers.Remove(controller);
            }
        }

        public async Task AddUserToController(IControllerConnection controller, IUserConnection userConnection)
        {

        }
    }
}
