using backend_.Models.controller;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using backend_.Models.controllerGroup;

namespace backend_.DataBase.ControllerDB
{
    public class ControllerDBContext:DbContext
    {
        public DbSet<Controller> controllers { get; set; }
        public DbSet<ControllerName> controllersName { get; set; }
        public DbSet<ControllerState> controllerStates { get; set; }
        public DbSet<ControllerGroup> controllerGroups { get; set; }
        public DbSet<m2mControllerControllerGroup> m2mControllers { get; set; }
        public DbSet<ControllerOutput> outputs { get; set; }
        public DbSet<OutputValue> outputValues { get; set; }
        public DbSet<OutputState> outputStates { get; set; }
        public DbSet<OutputRange> outputRanges { get; set; }
        public DbSet<ControllerQuery> controllerQueries { get; set; }
        public DbSet<ControllerOutputGroup> controllerOutputGroups { get; set; }
        public DbSet<m2mControllerOutputGroup> m2mControllersOutputGroups { get; set; }

        public ControllerDBContext() : base()
        {

        }
        public ControllerDBContext(DbContextOptions<ControllerDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Controller>()
                .HasKey(x => x.IpAddress);
            modelBuilder.Entity<Controller>()
                .HasOne(x => x.controllerName)
                .WithMany(x => x.controllers)
                .HasConstraintName("mcn_id");
            modelBuilder.Entity<Controller>()
                .HasOne(x => x.ControllerState)
                .WithMany(x => x.controllers)
                .HasConstraintName("mc_s_id");
            modelBuilder.Entity<Controller>()
                .HasMany(x => x.controllerGroups)
                .WithMany(x => x.controllers)
                .UsingEntity<m2mControllerControllerGroup>
                (
                    x => x.HasOne(x => x.ControllerGroup)
                        .WithMany(x => x.m2mControllers)
                        .HasForeignKey(x => x.ControllerGroupID),
                    x => x.HasOne(x => x.Controller)
                        .WithMany(x => x.m2mControllerGroups)
                        .HasForeignKey(x => x.ControllerID),
                    x => {
                        x.HasKey(x => new { x.ControllerGroupID, x.ControllerID });
                        x.Property(x => x.ControllerID).HasColumnName("mc_addres");
                        x.Property(x => x.ControllerGroupID).HasColumnName("mc_g_id");
                        }
                );
            modelBuilder.Entity<ControllerState>()
                .HasMany(x => x.controllers)
                .WithOne(x => x.ControllerState);
            modelBuilder.Entity<ControllerName>()
                .HasMany(x => x.controllers)
                .WithOne(x => x.controllerName);
            modelBuilder.Entity<ControllerState>().HasKey(x => x.id).HasName("mc_s_id");
            modelBuilder.Entity<Controller>()
                .HasMany(x => x.outputs)
                .WithOne(x => x.controller)
                .HasForeignKey(x=>x.controllerAddress)
                .HasConstraintName("mc_address");


            modelBuilder.Entity<m2mControllerOutputGroup>()
                .HasKey(x => new { x.controllerOutputGroupID, x.controllerOutputID, x.controllerID });
            modelBuilder.Entity<ControllerOutput>()
                .HasMany(x => x.outputGroups)
                .WithMany(x => x.outputs)
                .UsingEntity<m2mControllerOutputGroup>(
                x => x.HasOne(x => x.controllerOutputGroup)
                    .WithMany(x => x.m2mControllers),
                x => x.HasOne(x => x.controllerOutput)
                    .WithMany(x => x.m2mOutputGroups),
                x =>
                {
                    x.Property(x => x.controllerID).HasColumnName("mc_address");
                    x.Property(x => x.controllerOutputID).HasColumnName("id");
                    x.Property(x => x.controllerOutputGroupID).HasColumnName("mco_g_id");
                    x.HasKey(x => new { x.controllerOutputGroupID, x.controllerID,  x.controllerOutputID });
                });
            modelBuilder.Entity<ControllerOutput>()
                 .HasMany(x => x.outputValues) 
                 .WithOne(x => x.controllerOutput);
            modelBuilder.Entity<OutputValue>()
                .HasKey(x => new { x.controllerOutputId, x.controllerAddress, x.DateTime });
           
        }

        public async Task<ControllerOutput> GetControllerOutput(UInt32 address,int id)
        {
            var output = await outputs.FirstOrDefaultAsync(x => x.controllerAddress == address && x.id == id);
            output.range = await outputRanges.FirstOrDefaultAsync(x => x.RangeId == output.RangeId);
            output.outputState = await outputStates.FirstOrDefaultAsync(x => x.id == output.outputStateId);
            output.Query = await controllerQueries.FirstOrDefaultAsync(x => x.id == output.QueryId);
            output.outputGroups = await m2mControllersOutputGroups
                .Where(x => x.controllerID == output.controllerAddress && x.controllerOutputID == output.id)
                .Join(controllerOutputGroups, x => x.controllerOutputGroupID, R => R.id, (x, R) => R)
                .ToListAsync();
            return output;
        }

        public async Task<bool> AddQeuryToController(UInt32 address,int id,string Qeury)
        {
            var output = await GetControllerOutput(address, id);
            output.Query = new ControllerQuery() { id = 0, query = Qeury };
            outputs.Update(output);
            try
            {
                this.SaveChanges();
            }catch(Exception e)
            {
                throw;
            }
            return true;
        }

        public async Task<List<ControllerOutput>?> GetControllerOutputs(UInt32 address)
        {
            var ControllerOutputs = await outputs.Where(x => x.controllerAddress == address).ToListAsync();
            for(int i=0;i< ControllerOutputs.Count;i++)
            {
                ControllerOutputs[i] = await GetControllerOutput(ControllerOutputs[i].controllerAddress, ControllerOutputs[i].id);
                if(ControllerOutputs[i].QueryId!=null)
                    ControllerOutputs[i].Query = await GetQeury((int)ControllerOutputs[i].QueryId);
                ControllerOutputs[i].outputState = await GetOutputState((int)ControllerOutputs[i].outputStateId);
                if (ControllerOutputs[i].RangeId != null)
                    ControllerOutputs[i].range = await GetOutputRange((int)ControllerOutputs[i].RangeId); 
            }
            return ControllerOutputs;

        }

        public async Task<bool> AddOutputRange(OutputRange range)
        {
            this.outputRanges.Add(range);
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

        public async Task<OutputRange> GetOutputRange(int id)
        {
            var res = await outputRanges.FirstOrDefaultAsync(x => x.RangeId == id);
            return res;
        }

        public async Task<List<OutputState>> GetOutputStates()
        {
            var res = outputStates.ToList();
            return res;
        }
        public async Task<OutputState?> GetOutputState(int id)
        {
            var res = await outputStates.FirstOrDefaultAsync(x => x.id == id);
            return res;
        }

        public async Task<bool> AddOutputState(string description)
        {
            this.outputStates.Add(new OutputState() { description=description});
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

        public async Task<ControllerQuery?> GetQeury(int id)
        {
            var res = await controllerQueries.FirstOrDefaultAsync(x => x.id == id);
            return res;
        }

        public async Task<bool> AddOutputQeury(ControllerQuery controllerQuery)
        {
            this.controllerQueries.Add(controllerQuery);
            try
            {
                this.SaveChanges();
            }catch(Exception e)
            {
                throw;
            }
            return true;
        }

        public async Task<bool> AddControllerOutput(ControllerOutput output)
        {
            var controller = await GetController(output.controllerAddress);
            if (controller == null)
                return false;
            controller.m2mControllerGroups = null;
            controller.controllerGroups = null;
            output.id = 0;
            outputs.Add(output);

/*            controller.outputs.Add(output);
            controllers.Update(controller);*/
            try
            {
                this.SaveChanges();
            }catch(Exception e)
            {
                throw;
            }
            return true;
        }

        public async Task<List<Controller>> GetAllControllers()
        {
            var res = await controllers.ToListAsync();
            foreach(var item in res)
            {
                item.controllerGroups = await GetControllerGroups(item.IpAddress);
                item.controllerName = await GetControllerName((int)item.controllerNameId);
                item.ControllerState = await GetState(item.ControllerStateId);
                item.outputs = await GetControllerOutputs(item.IpAddress);   
            }
            return res;
        
        }


        public async Task<Controller?> GetController(UInt32 address)
        {
            var res = await controllers.FirstOrDefaultAsync(x=>x.IpAddress== address);
            if (res == null)
                return null;
            res.controllerGroups = await GetControllerGroups(res.IpAddress);
            res.outputs = await GetControllerOutputs(res.IpAddress);
            return res;
        }

        public async Task<bool> AddControllerName(UInt32 address,int nameId)
        {
            var controller = await GetController(address);
            var controllerName = await controllersName.FirstOrDefaultAsync(x => x.id == nameId);
            if(controllerName == null)
                return false;
            controller.controllerName = controllerName;
            this.controllers.Update(controller);
            try
            {
                this.SaveChanges();
            }
            catch(Exception e)
            {
                throw e;
            }
            return true;
        }

        private Task<ControllerGroup?> GetControllerGroup(int id)
        {
            return controllerGroups.FirstOrDefaultAsync(x => x.id == id);
        }
        public async Task<List<ControllerGroup>> GetControllerGroups(UInt32 address)
        {
            var group = await m2mControllers.Where(x => x.ControllerID == address)
                .Join(controllerGroups, x => x.ControllerGroupID, R=>R.id,(x,R)=>R ).ToListAsync();
            return group;
        }

        public async Task<bool> AddControllerToGroup(int GroupID,UInt32 address)
        {
            var controller= await GetController(address);
            var Groups = await GetControllerGroups(address);
            if (Groups.FirstOrDefault(x => x.id == GroupID) != null)
                return false;
            var newGroup= await GetControllerGroup(GroupID);
            if (newGroup == null)
                return false;
            controller.controllerGroups = Groups;
            Groups.Add(newGroup);
            controllers.Update(controller);
            try
            {
                this.SaveChanges();
            }catch(Exception e)
            {
                throw;
            }
            return true;
        }

        public async Task<bool> DeleteControllerFromGroup(int GroupID, UInt32 address)
        {
            var controller=await GetController(address);
            var Groups = await GetControllerGroups(address);
            var group = Groups.FirstOrDefault(x => x.id == GroupID);
            if (group == null)
                return false;
            Groups.Remove(group);
            controller.controllerGroups = Groups;
            controllers.Update(controller);
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

        public async Task<bool> AddControllerState(UInt32 address,int stateID)
        {
            var controller= await GetController(address);
            var state = await this.controllerStates.FirstOrDefaultAsync(x => x.id == stateID);
            if (state == null)
                return false;
            controller.ControllerState = state;
            this.controllers.Update(controller);
            try
            {
                this.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
            return true;

        }

        public async Task<bool> AddController(UserController userController)
        {
            var controller = new Controller(userController);
            controller.controllerName = await this.controllersName
                .FirstOrDefaultAsync(x => x.name==userController.controllerName.Name&&
                x.version==userController.controllerName.version);
            if(controller.controllerName==null)
            {
                await this.AddControllerName(userController.controllerName.Name, userController.controllerName.version);
                controller.controllerName = this.controllersName
                    .First(x => x.name == userController.controllerName.Name &&
                    x.version == userController.controllerName.version);
            }
            controller.ControllerState = this.controllerStates.FirstOrDefault(x => x.id == userController.ControllerState);
            if(controller.ControllerState==null)
                return false;
            controllers.Add(controller);
            try
            {
                this.SaveChanges();
            }
            catch(Exception e)
            {
                throw;
            }
/*            await AddControllerName(controller.IpAddress, (int)userController.controllerName);
            await AddControllerState(controller.IpAddress, (int)userController.ControllerState);*/
            return true;
        }

        public async Task<bool> DeleteController(UserController userController)
        {
            controllers.Remove(controllers.First(x=>x.IpAddress== userController.IpAddress));
            try
            {
                this.SaveChanges();
            }catch(Exception e)
            {
                throw;
            }
            return true;
        }

        public async Task<bool> DeleteController(UInt32 id)
        {
            controllers.Remove(controllers.First(x => x.IpAddress == id));
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

        public async Task<bool> AddState(string state)
        {
            controllerStates.Add(new ControllerState() { Description = state, id = 0 });
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

        public async Task<ControllerState> GetState(int id)
        {
            return await controllerStates.FirstAsync(x => x.id == id);
        }

        public async Task<bool> DeleteState(int id)
        {
            controllerStates.Remove(controllerStates.First(x => x.id == id));
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

        public async Task<bool> PatchState(ControllerState state)
        {
            controllerStates.Update(state);
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


        private async Task<List<Controller>?> GetControllersAsync(ControllerName name)
        {
            try
            {
                var _controllers = controllers.Where(x => x.controllerName == name).ToList();
                return _controllers;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<ControllerName> GetControllerName(int id)
        {
            return controllersName.First(x => x.id == id);
        }

        public async Task<bool> AddControllerName(string name, string version)
        {
            controllersName.Add(new ControllerName() { name = name, version = version, id = 0 });
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


        public async Task<bool> DeleteControllerName(int id)
        {
            var ControllerName = await this.GetControllerName(id);
            ControllerName.controllers = await this.GetControllersAsync(ControllerName);
            if (ControllerName.controllers.Count != 0)
                throw new Exception("нельзя удалить название контроллера с привязанными контроллерами");



            controllersName.Remove(ControllerName);
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

        public async Task<bool> PatchControllerName(ControllerName state)
        {
            var ControllerName = await this.GetControllerName(state.id);
            ControllerName.controllers = await this.GetControllersAsync(ControllerName);
            ControllerName.name = state.name;
            ControllerName.version = state.version;
            controllersName.Update(ControllerName);
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
