using backend_.Models.controller;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using backend_.Models.controllerGroup;

namespace backend_.DataBase.ControllerDB
{
    public class ControllerOutputValueDBContext:DbContext
    {
        public DbSet<OutputValue> _context { get; set; }

        public ControllerOutputValueDBContext() : base()
        {

        }
        public ControllerOutputValueDBContext(DbContextOptions<ControllerOutputValueDBContext> options) : base(options)
        {

        }

        public async Task<bool> Add(OutputValue outputValue)
        {
            _context.Add(outputValue);
            try
            {

            }catch(Exception e)
            {
                throw;
            }
            return true;
        }

        
    }
}
