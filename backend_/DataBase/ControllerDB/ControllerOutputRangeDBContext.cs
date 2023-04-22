using backend_.Models.controller;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using backend_.Models.controllerGroup;

namespace backend_.DataBase.ControllerDB
{
    public class ControllerOutputRangeDBContext:DbContext
    {
        public DbSet<OutputRange> _context { get; set; }

        public ControllerOutputRangeDBContext() : base()
        {

        }
        public ControllerOutputRangeDBContext(DbContextOptions<ControllerOutputRangeDBContext> options) : base(options)
        {

        }

        public async Task<bool> Add(OutputRange range)
        {
            range.RangeId = 0;
            _context.Add(range);
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

        public async Task<OutputRange> Get(int id)
        {
            return await _context.FirstAsync(x => x.RangeId == id);
        }

        public async Task<bool> Delete(int id)
        {
            _context.Remove(_context.First(x => x.RangeId == id));
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

        public async Task<bool> PatchState(OutputRange range)
        {
            _context.Update(range);
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
