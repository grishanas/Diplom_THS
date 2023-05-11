using backend_.Models.UserModels;
using backend_.Connection.ControllerConnection;

namespace backend_.Connection.UserConnection
{
    public class UserConnection:IUserConnection,IDisposable
    {
        public User user { get; set; }
        public UInt32 address { get; }
        public int OutputId { get; }

        public CommandListener listener { get; }

        

        public UserConnection()
        {
            listener = connect;
        }

        private void connect(byte[] data)
        {

        }




        public void Dispose()
        {

        }
    }
}
