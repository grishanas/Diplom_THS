using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using backend_.Models.network;

namespace backend_.DataBase.network
{
    public class DBNetworkContext:DbContext
    {
        private DbSet<Network> _contetx { get; set; }

        public DBNetworkContext() : base()
        {

        }
        public DBNetworkContext(DbContextOptions<DBNetworkContext> options) : base(options)
        {

        }


        public async Task<Network>? AddNetwork(Network net)
        {
            try
            {
                _contetx.Add(net);
                this.SaveChanges();
            }catch(Exception e)
            {
                throw new Exception("Errore in AddNetwork");
            }
            return net;
        }

        public async Task<ICollection<Network>>? GetNetworks()
        {
            try
            {
                return await _contetx.ToListAsync();
            }catch(Exception e)
            {
                throw;
            }
            return null; 
        }

        public async Task<Network>? UpdateNetwork(int id, Network network)
        {
            var OldNetwork = _contetx.FirstOrDefault(x=>x.Id==id);
            if (OldNetwork == null)
                throw new Exception();
            OldNetwork.Discription=network.Discription;
            OldNetwork.Name=network.Name;
            try
            {
                this.Update(OldNetwork);
                this.SaveChanges();
            }catch(Exception e)
            {
                throw new Exception();
            }
            return OldNetwork;

        }

        public async Task<bool>? DeleteNetwork(int id)
        {
            var DeleteNetwork= _contetx.FirstOrDefault(x=>x.Id==id) ?? throw new Exception();
            _contetx.Remove(DeleteNetwork);
            try
            {
                this.SaveChanges();
            }
            catch(Exception e)
            {
                throw;
            }

            return true;
        }

    }
}
