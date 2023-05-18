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

        public AuthorizationController(UserDBContext dbContezxt, IConfiguration config)
        {
            this.authorization = new AuthorizationLogic.Authorization(dbContezxt, config);
            _dbContezxt = dbContezxt;
        }

        [HttpPost("Login")]
        public async Task<IResult> LogIn([FromBody] UserLogin userLogin)
        {
            try
            {
                string Role;
                var UserToken = await this.authorization.UserAuthentication(userLogin);
                HttpContext.Response.Cookies.Append("some.Text",UserToken.JWT,
                    new CookieOptions
                    {
                        MaxAge = TimeSpan.FromMinutes(100)
                    });
                return Results.Ok(UserToken.Role);
            }
            catch(Exception e)
            {
                Response.StatusCode = 400;
                return Results.Problem();
            }
        }

        [HttpPost("LogOut")]
        public async Task<IResult> LogOut()
        {

            try
            {
                var token = authorization.LogOutUser();
                HttpContext.Response.Cookies.Append("some.Text", token.JWT,
                    new CookieOptions
                    {
                        MaxAge = TimeSpan.FromMinutes(0)
                    });
                return Results.Ok();
            }
            catch (Exception e)
            {
                Response.StatusCode = 400;
                return Results.Problem();
            }
           
            return Results.Problem();
            
        }


    }
}
