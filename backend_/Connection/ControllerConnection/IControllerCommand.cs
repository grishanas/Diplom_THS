namespace backend_.Connection.ControllerConnection
{
    public interface IControllerCommand
    {
        public void DeleteAnswerListener(Command comand);   
        public void SetAnswerListener(Command comand);

        public Dictionary<string, string> GetAllowedCommand();

    }

    public interface IControllerCommandImplementation:IControllerCommand
    {
        public void SetCommand(string commandCode);
        public Task<bool> ExecuteCommand();
    }
}
