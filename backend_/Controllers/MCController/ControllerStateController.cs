using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend_.DataBase.ControllerDB;
using backend_.Models.controller;

namespace backend_.Controllers.MCController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControllerStateController : ControllerBase
    {
        private readonly ControllerDBContext _dbContext;
        private readonly Connection.ConnectionController _connectionController;

        public ControllerStateController(ControllerDBContext dbContext, Connection.ConnectionController connectionController)
        {
            _dbContext = dbContext;
            _connectionController = connectionController;
        }


        public class ControllerName
        {
            public string Name { get; set; }
            public string Version { get; set; }
        }

        [HttpGet("GetAllowedControllerState")]
        public async Task<IResult> GetAllowedControllerState([FromQuery] ControllerName controllerName)
        {
            try
            {
                var states = _connectionController.GetAllowedState(controllerName.Name, controllerName.Version);
                return Results.Json(states);
            }catch(Exception e)
            {
                return Results.Problem();
            }
        }

        [HttpGet("GetAll")]
        public async Task<IResult> GetAllControllersState()
        {
            try
            {
                return Results.Json(_dbContext.controllerStates.ToList());
            }
            catch (Exception e)
            {
                return Results.Problem();
            }
        }

        [HttpGet("GetController/{id}")]
        public async Task<IResult> GetControllerState([FromRoute] int id)
        {
            try
            {
                return Results.Json(await _dbContext.GetState(id));
            }
            catch (Exception e)
            {
                return Results.Problem();
            }

        }


        public class UserControllerState
        {
            public string description { get; set; }
        }

        [HttpPost("Add")]
        public async Task<IResult> AddControllerState([FromBody] UserControllerState controller)
        {
            try
            {
                var result = await _dbContext.AddState(controller.description);
                if (result)
                    return Results.Ok();
                else
                    return Results.Problem();
            }
            catch (Exception e)
            {
                return Results.Problem();
            }
        }

        [HttpDelete("DeleteController/{id}")]
        public async Task<IResult> DeleteControllerState([FromRoute] int id)
        {
            try
            {
                await _dbContext.DeleteState(id);
                return Results.Ok();
            }
            catch (Exception e)
            {
                return Results.Problem();
            }
        }
    }
}
