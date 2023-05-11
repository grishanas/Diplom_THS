using backend_.Models.controller;
namespace backend_.Connection.ControllerConnection
{
    public interface IControllerCommand
    {
        public void DeleteAnswerListener(CommandListener comand);   
        public void SetAnswerListener(CommandListener comand);

        public List<string> GetAllowedCommand();

        public State IsRun { get; set; }

    }

    public interface IControllerCommandImplementation:IControllerCommand
    {
        public void SetCommand(string commandCode);
        public Task<bool> ExecuteCommand();
    }
}
