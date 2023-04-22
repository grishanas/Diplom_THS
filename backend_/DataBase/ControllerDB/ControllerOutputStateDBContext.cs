using backend_.Models.controller;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using backend_.Models.controllerGroup;

namespace backend_.DataBase.ControllerDB
{
    public class ControllerOutputStateDBContext:DbContext
    {
        public DbSet<OutputState> _context { get; set; }

        public ControllerOutputStateDBContext() : base()
        {

        }
        public ControllerOutputStateDBContext(DbContextOptions<ControllerOutputStateDBContext> options) : base(options)
        {

        }

        public async Task<bool> AddState(string state)
        {
            _context.Add(new OutputState() { description = state, id = 0 });
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

        public async Task<OutputState> GetState(int id)
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

        public async Task<bool> PatchState(OutputState state)
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
