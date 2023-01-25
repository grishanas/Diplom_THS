using Microsoft.EntityFrameworkCore;
using backend_.model;

namespace backend_.DataBase
{
    public class UserContext: DbContext
    {
        public DbSet<Users> Users { get; set; }

        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

    }
}
