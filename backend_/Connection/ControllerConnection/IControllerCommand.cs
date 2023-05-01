using backend_.Models.controller;
namespace backend_.Connection.ControllerConnection
{
    public interface IControllerCommand
    {
        public void DeleteAnswerListener(Command comand);   
        public void SetAnswerListener(Command comand);

        public List<string> GetAllowedCommand();

        public locker IsRun { get; set; }

    }

    public interface IControllerCommandImplementation:IControllerCommand
    {
        public void SetCommand(string commandCode);
        public Task<bool> ExecuteCommand();
    }
}
