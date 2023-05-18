using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend_.DataBase.ControllerDB;
using backend_.Models.controller;

namespace backend_.Controllers.MCController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControllerNameController : ControllerBase
    {
        private readonly ControllerDBContext _dbContext;
        private readonly Connection.ConnectionController _controllerConnectionController;

        public ControllerNameController(ControllerDBContext dbContext, Connection.ConnectionController controllerConnectionController)
        {
            _dbContext = dbContext;
            _controllerConnectionController = controllerConnectionController;
        }

        public class dict
        {
            public string Name { get; set; }
            public List<string> Values { get; set; }
        }
        [HttpGet("AllowedControllerName")]
        public async Task<IResult> GetAllowedControllerName()
        {
            try
            {
                var controllers = _controllerConnectionController.GetAllowedController();
                var controll = new List<dict>();

                foreach (var controllerName in controllers)
                {
                    controll.Add(new dict() { Name= controllerName.Key,Values = controllerName.Value });
                }
                return Results.Json(controll);
            }
            catch(Exception e)
            {
                return Results.Problem();
            }
        }


        [HttpGet("GetAll")]
        public async Task<IResult> GetAllControllers()
        {
            try
            {
                return Results.Json(_dbContext.controllersName.ToList());
            }
            catch (Exception e)
            {
                return Results.Problem();
            }
        }

        [HttpGet("Get/{id}")]
        public async Task<IResult> GetController([FromRoute] int id)
        {
            try
            {
                return Results.Json(await _dbContext.GetControllerName(id));
            }
            catch (Exception e)
            {
                return Results.Problem();
            }

        }


        public class NameAndVersion
        {
            public string name { get; set; }
            public string version { get; set; }
        }

        [HttpPost("Add")]
        public async Task<IResult> AddController([FromBody] NameAndVersion nameAndVersion)
        {
            try
            {
                var result = await _dbContext.AddControllerName(nameAndVersion.name, nameAndVersion.version);
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

        public class ID
        {
            public int id { get; set; }
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IResult> DeleteController([FromBody] ID id)
        {
            try
            {
                await _dbContext.DeleteControllerName(id.id);
                return Results.Ok();
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        }
    }
}
