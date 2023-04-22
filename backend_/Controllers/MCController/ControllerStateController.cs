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

        private readonly ControllerStateDBContext _dbContext;

        public ControllerStateController(ControllerStateDBContext dbContext)
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

        [HttpGet("GetController/{id}")]
        public async Task<IResult> GetController([FromRoute] int id)
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


        [HttpPost("AddController")]
        public async Task<IResult> AddController([FromBody] string controller)
        {
            try
            {
                var result = await _dbContext.AddState(controller);
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
        public async Task<IResult> DeleteController([FromRoute] int id)
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
