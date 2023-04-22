using backend_.Models.controller;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace backend_.DataBase.ControllerDB
{
    public class ControllerNameDBContext:DbContext
    {
        public DbSet<ControllerName> _context { get; set; }
        public ControllerNameDBContext() : base()
        {

        }
        public ControllerNameDBContext(DbContextOptions<ControllerNameDBContext> options) : base(options)
        {

        }

        public async Task<ControllerName> Get(int id)
        {
            return _context.First(x => x.id == id);
        }

        public async Task<bool> Add(string name,string version)
        {
            _context.Add(new ControllerName() { name = name, version = version, id = 0 });
            try
            {
                var res = await this.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw;
            }
            return true;

        }

        public async Task<bool> Delete(int id)
        {
            _context.Remove(_context.First(x => x.id == id));
            try
            {
                await this.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw;
            }
            return true;
        }

        public async Task<bool> Patch(ControllerName state)
        {
            _context.Update(state);
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
