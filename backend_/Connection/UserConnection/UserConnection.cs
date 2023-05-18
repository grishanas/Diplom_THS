using backend_.Models.UserModels;
using backend_.Connection.ControllerConnection;
using System.IO.Pipelines;
using System.Collections.Concurrent;
using backend_.Models.controller;
using Newtonsoft.Json;
using System.Text;

namespace backend_.Connection.UserConnection
{
    public class UserConnection:IUserConnection,IDisposable
    {
        public User user { get; set; }
        public UInt32 address { get; }
        public int OutputId { get; }

        public CommandListener listener { get; set; }

        public Stream Stream { get; set; }

        private List<OutputValue> Values = new List<OutputValue>();

        private PipeWriter writer { get; set; }
        private PipeReader reader { get; set; }

        
        private async void streamWriter()
        {
            while(true)
            {
                var tmp = new List<OutputValue>();
                lock (Values)
                {
                    if(Values.Count == 0)
                        Monitor.Wait(Values);
                    tmp.AddRange(Values);
                    Values.Clear();
                }
                var value = JsonConvert.SerializeObject(tmp);
                await writer.WriteAsync(Encoding.UTF8.GetBytes(value));
                await writer.FlushAsync();
            }
        }


        private void AddValue(OutputValue value)
        {
            lock(Values)
            {
                Values.Add(value);
                Monitor.PulseAll(Values);
            }
        }
        public UserConnection(PipeWriter writer, PipeReader reader)
        {
            this.writer = writer;
            this.reader = reader;
            listener = AddValue;
            Task.Run(()=>this.streamWriter());

        }

        public void Dispose()
        {

        }
    }
}
