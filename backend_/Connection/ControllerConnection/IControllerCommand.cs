using backend_.Models.controller;
namespace backend_.Connection.ControllerConnection
{
    public interface IControllerCommand
    {
        public int id { get; set; }
        public int Address { get; set; }
        public void DeleteAnswerListener(CommandListener comand);   
        public void SetAnswerListener(CommandListener comand);

        public List<string> GetAllowedCommand();

        public State IsRun { get; set; }

        public void SetState(State state);

        public static List<State> AllowedState { get; }

    }

    public interface IControllerCommandImplementation:IControllerCommand
    {
        public void SetCommand(string commandCode);
        public Task<bool> ExecuteCommand();
    }
}
