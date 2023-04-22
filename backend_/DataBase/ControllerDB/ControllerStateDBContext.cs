using backend_.Models.controller;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace backend_.DataBase.ControllerDB
{
    public class ControllerStateDBContext:DbContext
    {
        public DbSet<ControllerState> _context { get; set; }
        public ControllerStateDBContext() : base()
        {

        }
        public ControllerStateDBContext(DbContextOptions<ControllerStateDBContext> options) : base(options)
        {

        }

        public async Task<bool> AddState(string state)
        {
            _context.Add(new ControllerState() { Description = state, id = 0 });
            try
            {
                var res = await this.SaveChangesAsync();
            }catch(Exception e)
            {
                throw;
            }
            return true;

        }

        public async Task<ControllerState> GetState(int id)
        {
            return await _context.FirstAsync(x => x.id == id);
        }

        public async Task<bool> DeleteState(int id)
        {
            _context.Remove(_context.First(x => x.id == id));
            try
            {
                await this.SaveChangesAsync();
            }catch(Exception e)
            {
                throw;
            }
            return true;
        }

        public async Task<bool> PatchState(ControllerState state)
        {
            _context.Update(state);
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
}
