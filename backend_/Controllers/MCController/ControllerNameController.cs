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
        private readonly ControllerNameDBContext _dbContext;

        public ControllerNameController(ControllerNameDBContext dbContext)
        {
            _dbContext = dbContext;
        }


        [HttpGet("GetAll")]
        public async Task<IResult> GetAllControllers()
        {
            try
            {
                return Results.Json(_dbContext._context.ToList());
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
                return Results.Json(await _dbContext.Get(id));
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
                var result = await _dbContext.Add(nameAndVersion.name, nameAndVersion.version);
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

        [HttpDelete("Delete/{id}")]
        public async Task<IResult> DeleteController([FromRoute] int id)
        {
            try
            {
                await _dbContext.Delete(id);
                return Results.Ok();
            }
            catch (Exception e)
            {
                return Results.Problem();
            }
        }
    }
}
