using backend_.Models.controllerGroup;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace backend_.DataBase.ControllerDB
{
    public class ControllerGroupDBContext:DbContext
    {
        public DbSet<ControllerGroupUser> controllerGroups { get; set; }
        public DbSet<m2mUserRoleControllerGroup> userRoleControllerGroups { get; set; }
        public DbSet<UserRole1> userRole1s { get; set; }

        public DbSet<ControllerOutputGroupUser> outputGroups { get; set; }
        public DbSet<m2mUserRoleControllerOutputGroup> m2mRoleOutputs { get; set; }

        public ControllerGroupDBContext() : base()
        {

        }
        public ControllerGroupDBContext(DbContextOptions<ControllerGroupDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ControllerGroupUser>()
                .HasMany(x => x.userRoles)
                .WithMany(x => x.controllerGroups)
                .UsingEntity<m2mUserRoleControllerGroup>(
                 x => x.HasOne(x => x.userRole)
                    .WithMany(x => x.m2mUserRoleControllerGroups)
                    .HasForeignKey(x => x.userRoleId),
                 x => x.HasOne(x => x.controllerGroup)
                    .WithMany(x => x.m2mUserRolesControllerGroups)
                    .HasForeignKey(x => x.controllerGroupId),
                 x => {
                     x.HasKey(x => new { x.controllerGroupId, x.userRoleId });
                     x.Property(x => x.controllerGroupId).HasColumnName("mc_g_id");
                     x.Property(x => x.userRoleId).HasColumnName("ut_id");
                     }
                );
            modelBuilder.Entity<ControllerOutputGroupUser>()
                .HasMany(x => x.userRoles)
                .WithMany(x => x.controllerOutputGroups)
                .UsingEntity<m2mUserRoleControllerOutputGroup>(
                 x => x.HasOne(x => x.userRole)
                    .WithMany(x => x.m2mUserRoleControllerOutputGroups)
                    .HasForeignKey(x => x.userRoleId),
                 x=>x.HasOne(x=>x.controllerOutputGroup)
                    .WithMany(x=>x.m2mUserRoleControllerOutputGroups)
                    .HasForeignKey(x=>x.controllerOutputGroupID),
                 x =>
                 {
                     x.HasKey(x => new { x.controllerOutputGroupID, x.userRoleId });
                     x.Property(x => x.controllerOutputGroupID).HasColumnName("mco_g_id");
                     x.Property(x => x.userRoleId).HasColumnName("ut_id");
                 }

                ) ;
            

        }


        public async Task<List<ControllerOutputGroupUser>> GetOutputGroupsWithRole(int RoleID)
        {
            var Groups = await m2mRoleOutputs
                .Where(x=>x.userRoleId==RoleID)
                .Join(outputGroups,x=>x.controllerOutputGroupID,r=>r.id,(x,r)=>r)
                .ToListAsync();
            return Groups;
        }

        public async Task<List<ControllerOutputGroupUser>> OutputsGroups()
        {
            var Groups = await outputGroups.ToListAsync();
            foreach(var group in Groups)
            {
                group.userRoles = await m2mRoleOutputs
                    .Where(x => x.controllerOutputGroupID == group.id)
                    .Join(userRole1s, x => x.userRoleId, R => R.id, (x, r) => r)
                    .ToListAsync();
            }
            return Groups;
        }

        public async Task<bool> AddOutputGroup(ControllerOutputGroupUser group)
        {
            group.id = 0;
            outputGroups.Add(group);
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
        public async Task<bool> DeleteOutputGroup(int id)
        {
            outputGroups.Remove(outputGroups.First(x => x.id == id));
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


        public async Task<List<ControllerGroupUser>> GetAll()
        {
            var allGroups = await controllerGroups.ToListAsync();
            foreach(var group in allGroups)
            {
                group.userRoles = await userRoleControllerGroups
                    .Where(x => x.controllerGroupId == group.id)
                    .Join(userRole1s, x => x.userRoleId, R => R.id, (x, r) => r)
                    .ToListAsync();
            }
            return allGroups; 
        }

        public async Task<ControllerGroupUser> Get(int id)
        {
            var group = await controllerGroups.FirstAsync(x=>x.id==id);
            group.userRoles = await userRoleControllerGroups
                    .Where(x => x.controllerGroupId == group.id)
                    .Join(userRole1s, x => x.userRoleId, R => R.id, (x, r) => r)
                    .ToListAsync();
            return group;
        }

        public async Task<bool> Add(ControllerGroupUser controllerGroup)
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
}
