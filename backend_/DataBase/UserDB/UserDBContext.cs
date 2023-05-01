using backend_.Models.UserModels;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


namespace backend_.DataBase.UserDB
{
    public class UserDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> Roles { get; set; }

        private DbSet<m2mUserRole> m2mUserRoles { get; set; }


        public UserDBContext() : base()
        {

        }
        public UserDBContext(DbContextOptions<UserDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasMany(r => r.users)
                .WithMany(u => u.userRoles)
                .UsingEntity<m2mUserRole>(
                    m2m => m2m.HasOne(m2m => m2m.user)
                    .WithMany(m2m => m2m.m2mUserRoles)
                    .HasForeignKey(u => u.idUser),
                    m2m => m2m.HasOne(m2m => m2m.userRole)
                    .WithMany(u => u.m2mUserRoles)
                    .HasForeignKey(x => x.idUserRole),
                    m2m=>
                    {
                        m2m.HasKey(x => new { x.idUserRole, x.idUser });
                        m2m.Property(x => x.idUser).HasColumnName("u_id");
                        m2m.Property(x => x.idUserRole).HasColumnName("ut_id");
                    }  
                );
        }


        public async Task<User>? Get(UserLogin userLogin)
        {
            return await Users.FirstOrDefaultAsync(x => x.password == userLogin.password && x.login == userLogin.login);
        }

        public async Task<List<User>> GetAll()
        {
            var users = await Users.ToListAsync();
            foreach(var item in users)
            {
                var roles =await m2mUserRoles.Where(x=>x.idUser==item.id)
                            .Join(Roles, x => x.idUserRole, R => R.id, (x, R) => R)
                            .ToListAsync();
                item.userRoles = roles;

            }
            return users;
        }
        public async Task<User> Get(int id)
        {
            var user = Users.First(x => x.id == id);
            user.m2mUserRoles = await m2mUserRoles.Where(x => x.idUser == id).ToListAsync();
            return user;
        }

        public async Task<UserRole> GetRole(int id)
        {
            return Roles.First(x => x.id == id);
        }
        public async Task<bool> AddUser(User user)
        {
            var tmp = user.userRoles;
            user.userRoles = new List<UserRole>();

            Users.Add(user);
            try
            {
                this.SaveChanges();
            }catch(Exception e)
            {
                return false;
            }
            tmp.ForEach((role) =>
            {
                this.AddNewRoleToUser(role.id, user.id);
            });
            return true;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var user=Users.FirstOrDefault(x => x.id == id);
            user.m2mUserRoles = await m2mUserRoles.Where(x => x.idUser == id).ToListAsync();
            Users.Remove(user);
            try
            {
                await this.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteUser(User user)
        {
            Users.Remove(user);
            try
            {
                await this.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }



        public async Task<bool> AddUserRole(UserRole role)
        {
            Roles.Add(role);
            try
            {
                await this.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteUserRole(int id)
        {
            Roles.Remove(Roles.FirstOrDefault(x => x.id == id));
            try
            {
                await this.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteUserRole(UserRole role)
        {
            Roles.Remove(role);
            try
            {
                await this.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }


        public bool AddNewRoleToUser(int userRoleID, int userID)
        {
            var user = Users.FirstOrDefault(x=>x.id==userID);
            if (user == null)
                return false;
            var role = Roles.FirstOrDefault(x=>x.id==userRoleID);
            if (role == null) return false;
            user.userRoles.Add(role);
            try
            {
                this.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> AddNewRoleToUser(UserRole userRole,User user)
        {
            user.userRoles.Add(userRole);
            try
            {
                await this.SaveChangesAsync();
            }catch(Exception e)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> AddNewRoleToUser(List<UserRole> userRoles, List<User> users)
        {
            foreach(var role in Roles)
            {
                foreach (var user in users)
                    user.userRoles.Add(role);
            }

            try
            {
                await this.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteUserRole(int userId,int RoleId)
        {
            var user = await this.Get(userId);
            if (user == null)
                return false;
            var role = await Roles.FirstOrDefaultAsync(x=>x.id==RoleId);
            if (role == null)
                return false;
            user.userRoles.Remove(role);
            try
            {
                this.SaveChanges();
            }catch(Exception e)
            {
                return false;
            }

            return true;
        }

    }
}
