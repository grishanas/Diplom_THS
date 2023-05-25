using backend_.Models.controllerGroup;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic; 

namespace backend_.DataBase.UserDB
{
    public class GroupDBContext:DbContext
    {
        public DbSet<UserRole1> userRoles { get; set; }
        public DbSet<ControllerGroupUser> controllerGroups { get; set; }
        public DbSet<ControllerOutputGroupUser> controllerOutputGroups { get; set; }

        public DbSet<m2mUserRoleControllerGroup> m2mRoles { get; set; }
        public DbSet<m2mUserRoleControllerOutputGroup> m2MUserRoleControllerOutputGroups { get; set; }


        public GroupDBContext() : base()
        {

        }
        public GroupDBContext(DbContextOptions<GroupDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<UserRole1>()
                .HasMany(x => x.controllerGroups)
                .WithMany(x => x.userRoles)
                .UsingEntity<m2mUserRoleControllerGroup>(
                x => x.HasOne(t=>t.controllerGroup)
                      .WithMany(t=>t.m2mUserRolesControllerGroups)
                      .HasForeignKey(t=>t.userRoleId),
                x=>x.HasOne(t=>t.userRole)
                    .WithMany(t=>t.m2mUserRoleControllerGroups)
                    .HasForeignKey(t=>t.controllerGroupId),
                x =>
                {
                    x.Property(x => x.controllerGroupId).HasColumnName("mc_g_id");
                    x.Property(x => x.userRoleId).HasColumnName("ut_id");
                    x.HasKey(x => new { x.controllerGroupId, x.userRoleId });
                    x.ToTable("m2m_mcg_ut");
                });
            modelBuilder.Entity<UserRole1>()
                .HasMany(x => x.controllerOutputGroups)
                .WithMany(x => x.userRoles)
                .UsingEntity<m2mUserRoleControllerOutputGroup>(
                x=>x.HasOne(x=>x.controllerOutputGroup)
                    .WithMany(x=>x.m2mUserRoleControllerOutputGroups)
                    .HasForeignKey(x=>x.controllerOutputGroupID),
                x=>x.HasOne(x=>x.userRole)
                    .WithMany(x=>x.m2mUserRoleControllerOutputGroups)
                    .HasForeignKey(x=>x.userRoleId),
                x =>
                {
                    x.Property(x => x.controllerOutputGroupID).HasColumnName("mco_g_id");
                    x.Property(x => x.userRoleId).HasColumnName("ut_id");
                    x.HasKey(x => new { x.controllerOutputGroupID, x.userRoleId });
                    x.ToTable("m2m_mcog_ut");
                });
        }

        public async Task<List<ControllerGroupUser>> GetControllerGroups()
        {
            var groups = await controllerGroups.ToListAsync();
            foreach (var item in groups)
            {
                item.userRoles = await m2mRoles.Where(x => x.controllerGroupId == item.id).Join(userRoles, x => x.userRoleId, r => r.id, (x, r) => r).ToListAsync();
            }
            return groups;
        }
        public async Task<List<ControllerOutputGroupUser>> GetOutputGroups()
        {
            var groups = await controllerOutputGroups.ToListAsync();
            foreach(var item in groups)
            {
                item.userRoles= await m2MUserRoleControllerOutputGroups.Where(x=>x.controllerOutputGroupID==item.id).Join(userRoles,x=>x.userRoleId,r=>r.id,(x,r)=>r).ToListAsync();
            }
            return groups;
        }
        public async Task<bool> AddRoleControllerGroup(int roleId,int controllerGroupId)
        {
            var role = await userRoles.FirstOrDefaultAsync(x => x.id == roleId);
            var group = await controllerGroups.FirstOrDefaultAsync(x => x.id == controllerGroupId);
            role.controllerGroups.Add(group);
            try
            {
                this.SaveChanges();
            }catch(Exception e)
            {
                throw;
            }
            return true;
        }
        public async Task<bool> DeleteRoleControllerGroup(int roleId, int controllerGroupId)
        {
            var role = await userRoles.FirstOrDefaultAsync(x => x.id == roleId);
            var group = await controllerGroups.FirstOrDefaultAsync(x => x.id == controllerGroupId);
            role.controllerGroups.Remove(group);
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

        public async Task<bool> AddRoleControllerOutputGroup(int roleId, int controllerOutputGroupId)
        {
            var role = await userRoles.FirstOrDefaultAsync(x => x.id == roleId);
            var group = await controllerOutputGroups.FirstOrDefaultAsync(x => x.id == controllerOutputGroupId);
            role.controllerOutputGroups.Add(group);
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
        public async Task<bool> DeleteRoleControllerOutputGroup(int roleId, int controllerOutputGroupId)
        {
            var role = await userRoles.FirstOrDefaultAsync(x => x.id == roleId);
            var group = await controllerOutputGroups.FirstOrDefaultAsync(x => x.id == controllerOutputGroupId);
            m2MUserRoleControllerOutputGroups.Remove(new m2mUserRoleControllerOutputGroup() { controllerOutputGroupID=group.id,userRoleId=role.id});
            role.controllerOutputGroups.Remove(group);
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
