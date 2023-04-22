using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend_.Models.controllerGroup;
using backend_.DataBase.ControllerDB;

namespace backend_.Controllers.MCController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControllerGroupController : ControllerBase
    {

        private readonly ControllerGroupDBContext _dbContext;

        public ControllerGroupController(ControllerGroupDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet("GetAll")]
        public async Task<IResult> GetAll()
        {
            try
            {
                return Results.Json(await _dbContext.GetAll()); 

            }catch(Exception e)
            {
                return Results.Problem();
            }
        }

        [HttpGet("get/{id}")]
        public async Task<IResult> Get([FromQuery] int id)
        {
            try
            {
                return Results.Json(await _dbContext.Get(id));

            }catch(Exception e)
            {
                return Results.Problem();

            }
        }

        [HttpPost("Add")]
        public async Task<IResult> Add([FromBody] ControllerGroup controllerGroup)
        {
            try
            {
                var res = _dbContext.Add(controllerGroup);
                return Results.Ok();

            }
            catch (Exception e)
            {
                return Results.Problem();

            }
        }

        [HttpDelete("Delete")]
        public async Task<IResult> Delete([FromBody] int id)
        {
            try
            {
                var res = _dbContext.Delete(id);
                return Results.Ok();

            }
            catch (Exception e)
            {
                return Results.Problem();

            }
        }
    }
}
