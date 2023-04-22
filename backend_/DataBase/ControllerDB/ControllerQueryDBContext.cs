using backend_.Models.controller;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using backend_.Models.controllerGroup;

namespace backend_.DataBase.ControllerDB
{
    public class ControllerQueryDBContext:DbContext
    {
        public DbSet<ControllerQuery> _context { get; set; }

        public ControllerQueryDBContext() : base()
        {

        }
        public ControllerQueryDBContext(DbContextOptions<ControllerQueryDBContext> options) : base(options)
        {

        }

        public async Task<bool> Add(ControllerQuery controllerQuery)
        {
            _context.Add(controllerQuery);
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

        public async Task<ControllerQuery> GetState(int id)
        {
            return await _context.FirstAsync(x => x.id == id);
        }

        public async Task<bool> DeleteState(int id)
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

        public async Task<bool> PatchState(ControllerQuery state)
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
