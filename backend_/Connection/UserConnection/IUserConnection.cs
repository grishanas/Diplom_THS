using backend_.Connection;
using backend_.Models.UserModels;
using backend_.Models.controller;
using backend_.Connection.ControllerConnection;

namespace backend_.Connection.UserConnection
{

    public interface IOwnerConnection
    {
        public CommandListener listener { get; }
    }

    public interface IUserConnection: IOwnerConnection
    {
        public User user { get; set; }
    }

    
}
