using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using backend_.Models.network;

namespace backend_.DataBase.network
{
    public class DBSubNetworkContext: DbContext
    {
        private DbSet<SubNetwork> _context;

        public DBSubNetworkContext() : base()
        {

        }
        public DBSubNetworkContext(DbContextOptions<DBNetworkContext> options) : base(options)
        {

        }

    }

}
