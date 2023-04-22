using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend_.AuthorizationLogic;
using backend_.Models.UserModels;
using backend_.DataBase.UserDB;

namespace backend_.Controllers.Authorization
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly backend_.AuthorizationLogic.Authorization authorization;
        private readonly UserDBContext _dbContezxt;

        public AuthorizationController(UserDBContext dbContezxt)
        {
            this.authorization = new AuthorizationLogic.Authorization(dbContezxt);
            _dbContezxt = dbContezxt;
        }

        [HttpPost("Login")]
        public async Task<IResult> LogIn([FromBody] UserLogin userLogin)
        {
            try
            {
                var user = await this.authorization.UserAuthentication(userLogin);
                return Results.Ok();
            }
            catch(Exception e)
            {
                Response.StatusCode = 400;
                return Results.Problem();
            }
        }

        [HttpOptions("Login")]
        public async Task<IResult> LogOption()
        {
            var head = Request.Headers;
           
            return Results.Problem();
            
        }
    }
}
