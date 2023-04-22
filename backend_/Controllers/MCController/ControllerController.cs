using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend_.Models.controller;
using backend_.DataBase.ControllerDB;

namespace backend_.Controllers.MCController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControllerController : ControllerBase
    {
        private readonly ControllerDBContext _dbContext;
        private readonly ControllerNameDBContext _controllerName;
        private readonly Connection.ConnectionController controllers; 

        public ControllerController(ControllerDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("GetAll")]
        public async Task<IResult> GetAllControllers()
        {
            try
            {
                return Results.Json(_dbContext.controllers.ToList());
            }
            catch (Exception e)
            {
                return Results.Problem();
            }
        }

        [HttpGet("GetController/{id}")]
        public async Task<IResult> GetController([FromRoute] int id)
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
        private async void AddController(backend_.Models.controller.Controller controller)
        {
            if (controller.Name == null)
                return;
            var controllerName = await this._controllerName.Get(int.Parse(controller.Name));
            if (controllerName == null)
                return;
            switch (controllerName.name)
            {
                case "Omron":
                    {

                        break;
                    }

            }


        }


        [HttpPost("AddController")]
        public async Task<IResult> AddController([FromBody] UserController controller)
        {
            try
            {
                var result = await _dbContext.AddController(controller);
                if (result)
                {
                    var controll = await _dbContext.GetController(controller.IpAddress);
                    AddController(controll);


                    return Results.Ok();
                }
                else
                    return Results.Problem();
            }
            catch(Exception e)
            {
                return Results.Problem();
            }
        }



        [HttpDelete("DeleteController/{id}")]
        public async Task<IResult> DeleteController([FromRoute] int ip)
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
