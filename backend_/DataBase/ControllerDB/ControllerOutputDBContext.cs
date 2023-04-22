using backend_.Models.controller;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace backend_.DataBase.ControllerDB
{
    public class ControllerOutputDBContext:DbContext
    {
        public DbSet<ControllerOutput> ControllerOutputs { get; set; }

        private ControllerDBContext ControllerDBContext { get; }

        public ControllerOutputDBContext(ControllerDBContext controllerDBContext) : base()
        {
            ControllerDBContext = controllerDBContext;
        }
        public ControllerOutputDBContext(DbContextOptions<ControllerOutputDBContext> options, ControllerDBContext controllerDBContext) : base(options)
        {
            ControllerDBContext = controllerDBContext;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ControllerOutput>()
                .HasKey(t => new { t.controllerAddress, t.id });
        }



    }
}
