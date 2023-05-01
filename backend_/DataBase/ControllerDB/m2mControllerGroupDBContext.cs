using backend_.Models.controllerGroup;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


namespace backend_.DataBase.ControllerDB
{
    public class m2mControllerGroupDBContext:DbContext
    {
        public DbSet<m2mControllerControllerGroup> Groups { get; set; }

        public m2mControllerGroupDBContext() : base()
        {

        }
        public m2mControllerGroupDBContext(DbContextOptions<m2mControllerGroupDBContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<m2mControllerControllerGroup>()
                .HasKey(x => new { x.ControllerGroupID, x.ControllerID });
        }

        public async Task<bool> Delet(int controllerId, int GroupId)
        {
            var group = await Groups.FirstOrDefaultAsync(x => x.ControllerID == controllerId && x.ControllerGroupID == GroupId);
            Groups.Remove(group);
            try
            {
                this.SaveChanges();
            }catch(Exception e)
            {
                throw;
            }
            return true;
        }
        public async Task<bool> Add(int controllerId, int GroupId)
        {
            var group = new m2mControllerControllerGroup() { ControllerID = (UInt32)controllerId,ControllerGroupID = GroupId };
            Groups.Add(group);
            try
            {
                this.SaveChanges();
            }
            catch (Exception e)
            {
                throw;
            }
            return true;
        }
    }
}
