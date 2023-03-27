namespace backend_.Connection.ControllerConnection
{
    public interface IOutputListener
    {
        public void SetAnswerListener(Command Delegate);
        public void DeleteAnswerListener(Command Delegate);
    }
}
