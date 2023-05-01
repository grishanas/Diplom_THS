using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend_.Models.controller;
using backend_.DataBase.ControllerDB;
using backend_.Connection;
using backend_.Connection.ControllerConnection;

namespace backend_.Controllers.MCController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControllerController : ControllerBase
    {
        private readonly ControllerDBContext _dbContext;
        private readonly Connection.ConnectionController controllers;

        public ControllerController(ControllerDBContext dbContext, Connection.ConnectionController controllers)
        {
            _dbContext = dbContext;
            this.controllers = controllers;
        }

        [HttpGet("GetAllowedControllers")]
        public async Task<IResult> GetAllAllowControllers()
        {
            var result = ControllerFactory.some.Keys;
            return Results.Json(result);
        }

        [HttpGet("GetAll")]
        public async Task<IResult> GetAllControllers()
        {
            try
            {
                var res = await _dbContext.GetAllControllers();
                foreach(var controller in res)
                {
                    controller.controllerName.controllers = null;
                    controller.ControllerState.controllers = null;
                    foreach(var item in controller.outputs)
                    {
                        item.outputState.controllers = null;
                        item.controller = null;
                     
                    }
                }
                return Results.Json(res);
            }
            catch (Exception e)
            {
                return Results.Problem();
            }
        }

        [HttpGet("GetController/{id}")]
        public async Task<IResult> GetController([FromRoute] UInt32 id)
        {
            try
            {
                return Results.Json(await _dbContext.GetController(id));
            }
            catch (Exception e)
            {
                return Results.Problem();
            }

        }

        public class ControllerAndGroup
        {
            public UInt32 id { get; set; }
            public int group { get; set; }
        }

        [HttpPost("AddControllerToGroup")]
        public async Task<IResult> AddControllerToGroup([FromBody] ControllerAndGroup controller)
        {
            try
            {
                var res = await _dbContext.AddControllerToGroup(controller.group, controller.id);
                if (res)
                    return Results.Ok();

                return Results.Problem();
            }
            catch(Exception e)
            {
                return Results.Problem();
            }
        }

        [HttpPost("DeleteControllerFromGroup")]
        public async Task<IResult> DeleteControllerFromGroup([FromBody] ControllerAndGroup controller)
        {
            try
            {
                var res = await _dbContext.DeleteControllerFromGroup(controller.group, controller.id);
                if (res)
                    return Results.Ok();

                return Results.Problem();
            }
            catch (Exception e)
            {
                return Results.Problem();
            }
        }

        [HttpPost("StopController/{id}")]
        public async Task<IResult> StopController([FromRoute] UInt32 id)
        {
            try
            {
                var controller = await _dbContext.GetController(id);

            }
            catch (Exception e)
            {
                return Results.Problem();
            }
            return Results.Ok();
        }

        [HttpPost("StartController/{id}")]
        public async Task<IResult> StartController([FromRoute] UInt32 id)
        {
            try
            {
                controllers.Start(id);

            }
            catch(Exception e)
            {
                return Results.Problem();
            }
            return Results.Ok();
        }

        private IControllerConnection AddControll(UserController controller)
        {
            if (controller.Name == null)
                return null;
            var controllerName = controller.controllerName;
            if (controllerName == null)
                return null;
            var newController = ControllerFactory.Create(controller);
            this.controllers.AddController(newController);

            return newController;
        }

        private void DeleteController(UInt32 ipAddress)
        {
            controllers.RemoveController(ipAddress);
        }


        [HttpPost("AddController")]
        public async Task<IResult> AddController([FromBody] UserController controller)
        {
            try
            {
                var res = AddControll(controller);
                if(res== null)
                    return Results.Problem();
                var result = await _dbContext.AddController(controller);
                if (result)
                {
                    return Results.Ok();
                }
                else
                {
                    controllers.RemoveController(res.id);
                    return Results.Problem();
                }
            }
            catch(Exception e)
            {
                return Results.Problem();
            }
        }



        [HttpDelete("DeleteController/{id}")]
        public async Task<IResult> DeleteControllerID([FromRoute] UInt32 ip)
        {
            try
            {
                var res = _dbContext.GetController(ip);
                await _dbContext.DeleteController(ip);
                return Results.Ok();
            }
            catch (Exception e)
            {
                return Results.Problem();
            }
        }

        [HttpDelete("DeleteController")]
        public async Task<IResult> DeleteController([FromBody] UserController controller)
        {
            try
            {
                
                await _dbContext.DeleteController(controller);
                return Results.Ok();
            }
            catch(Exception e)
            {
                return Results.Problem();
            }
        }

    }
}
