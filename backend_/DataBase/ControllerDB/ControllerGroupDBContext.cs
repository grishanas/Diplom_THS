using backend_.Models.controllerGroup;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace backend_.DataBase.ControllerDB
{
    public class ControllerGroupDBContext:DbContext
    {
        public DbSet<ControllerGroup> controllerGroups { get; set; }

        public ControllerGroupDBContext() : base()
        {

        }
        public ControllerGroupDBContext(DbContextOptions<ControllerGroupDBContext> options) : base(options)
        {

        }

        public async Task<List<ControllerGroup>> GetAll()
        {
            return await controllerGroups.ToListAsync();
        }

        public async Task<ControllerGroup> Get(int id)
        {
            return await controllerGroups.FirstAsync(x=>x.id==id);
        }

        public async Task<bool> Add(ControllerGroup controllerGroup)
        {
            controllerGroups.Add(controllerGroup);
            try
            {
                this.SaveChanges();
            }catch(Exception e)
            {
                throw;
            }
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            controllerGroups.Remove(controllerGroups.First(x => x.id == id));
            try
            {
                this.SaveChanges();
            }catch(Exception e)
            {
                throw;
            }
            return true;
        }
    }

    public class  ControllerOutputGroupDBContext:DbContext
    {
        public DbSet<ControllerOutputGroup> controllerOutputGroups { get; set; }

        public ControllerOutputGroupDBContext() : base()
        {

        }
        public ControllerOutputGroupDBContext(DbContextOptions<ControllerOutputGroupDBContext> options) : base(options)
        {

        }


        public async Task<List<ControllerOutputGroup>> GetAll()
        {
            return await controllerOutputGroups.ToListAsync();
        }

        public async Task<ControllerOutputGroup> Get(int id)
        {
            return await controllerOutputGroups.FirstAsync(x => x.id == id);
        }

        public async Task<bool> Add(ControllerOutputGroup controllerGroup)
        {
            controllerOutputGroups.Add(controllerGroup);
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

        public async Task<bool> Delete(int id)
        {
            controllerOutputGroups.Remove(controllerOutputGroups.First(x => x.id == id));
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
