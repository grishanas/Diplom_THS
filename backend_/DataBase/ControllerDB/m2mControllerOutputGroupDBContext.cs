using backend_.Models.controllerGroup;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace backend_.DataBase.ControllerDB
{
    public class m2mControllerOutputGroupDBContext:DbContext
    {
        public DbSet<m2mControllerOutputGroup> Groups { get; set; }

        public m2mControllerOutputGroupDBContext() : base()
        {

        }
        public m2mControllerOutputGroupDBContext(DbContextOptions<m2mControllerOutputGroupDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<m2mControllerOutputGroup>()
                .HasKey(x => new { x.controllerOutputID, x.controllerID,x.controllerOutputGroupID });
        }


        public async Task<bool> Delete(m2mControllerOutputGroup group)
        {
            Groups.Remove(group);
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
        public async Task<bool> Add(m2mControllerOutputGroup group)
        {
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
