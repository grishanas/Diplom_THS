using backend_.Models.controller;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace backend_.DataBase.ControllerDB
{
    public class ControllerDBContext:DbContext
    {
        public DbSet<Controller> controllers { get; set; }
        public ControllerDBContext() : base()
        {

        }
        public ControllerDBContext(DbContextOptions<ControllerDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Controller>().HasKey(x => x.IpAddress);
        }

        public async Task<Controller> GetController(int address)
        {
            return await controllers.FirstAsync(x=>x.IpAddress== address);
        }

        public async Task<bool> AddController(UserController userController)
        {
            var controller = new Controller(userController);
            controllers.Add(controller);
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

        public async Task<bool> DeleteController(UserController userController)
        {
            controllers.Remove(controllers.First(x=>x.IpAddress== userController.IpAddress));
            try
            {
                this.SaveChanges();
            }catch(Exception e)
            {
                throw;
            }
            return true;
        }

        public async Task<bool> DeleteController(int id)
        {
            controllers.Remove(controllers.First(x => x.IpAddress == id));
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
