using backend_.Connection.ControllerConnection;
using backend_.DataBase.ControllerDB;
using backend_.Models.controller;

namespace backend_.Connection.UserConnection
{
    public class DataBaseConnection:IOwnerConnection,IDisposable
    {
        public CommandListener listener { get; }
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private IServiceScope _serviceScope;
        private ControllerDBContext controllerDB;
        private Controller controller;

        DataBaseConnection(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _serviceScope = _serviceScopeFactory.CreateScope();
            controllerDB = _serviceScope.ServiceProvider.GetService<ControllerDBContext>();
            listener = WriteDataToDataBase;
        }

        private void WriteDataToDataBase(OutputValue value)
        {
            
            controllerDB.AddOutputValue(value);
        }

        public void Dispose()
        {
            var connection = _serviceScope.ServiceProvider.GetService<ConnectionController>();
            connection.DeleteUserFromControllerOutputs(this);
            connection.Dispose();
            _serviceScope.Dispose();
        }
    }
}
