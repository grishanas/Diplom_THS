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

        [HttpGet("GetControllersOutputGroups")]
        public async Task<IResult> GetControllersOutputGroups()
        {
            try
            {
                //var res = _dbContext.
            }catch(Exception e)
            {
                return Results.Problem();
            }
            return Results.Ok();
        }


        [HttpGet("GetControllerGroup")]
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

        [HttpGet("getControllerGroup/{id}")]
        public async Task<IResult> Get([FromQuery] int id)
        {
            try
            {
                return Results.Ok(await _dbContext.Get(id));

            }catch(Exception e)
            {
                return Results.Problem();

            }
        }

        [HttpGet("GetOutputGroups")]
        public async Task<IResult> GetOutputGroups()
        {
            try
            {
                var res = _dbContext.outputGroups.ToList();
                return Results.Ok(res);
            }catch(Exception e)
            {
                return Results.Problem();
            }
            return Results.Problem();
        }
        


        [HttpPost("AddOutputGroup")]
        public async Task<IResult> AddOutput([FromBody] string GroupDescription)
        {
            try
            {
                await _dbContext.AddOutputGroup(new ControllerOutputGroupUser() { description= GroupDescription});
                return Results.Ok();
            }catch(Exception E)
            {
                return Results.Problem();
            }
            return Results.Problem();


        }

        [HttpPost("AddControllerGroup")]
        public async Task<IResult> Add([FromBody] ControllerGroupUser controllerGroup)
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

        [HttpDelete("DeleteControllerGroup")]
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

        [HttpDelete("DeleteOutputGroup")]
        public async Task<IResult> DeleteOutputGroup([FromBody] int id)
        {
            try
            {
                await _dbContext.DeleteOutputGroup(id);
                return Results.Ok();
            }catch(Exception e)
            {
                return Results.Problem();
            }
            return Results.Problem();
        }

    }
}
