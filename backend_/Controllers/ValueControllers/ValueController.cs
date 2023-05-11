using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend_.DataBase.UserDB;
using backend_.Connection;
using Microsoft.AspNetCore.Authorization;
using backend_.AuthorizationLogic;
using backend_.Connection.UserConnection;

namespace backend_.Controllers.ValueControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValueController : ControllerBase
    {
        private readonly AuthorizationLogic.Authorization _authorization;
        private readonly UserConnectionController _userConnect;

        public ValueController(IConfiguration configuration,UserConnectionController userConnect)
        {
            _authorization = new AuthorizationLogic.Authorization(configuration);
            _userConnect = userConnect;
        }

        [HttpGet("StartListen")]
        [Authorize]
        public async Task<IResult> StartListen()
        {
            var userCookie = Request.Cookies.ToList();
            var userID = _authorization.ValidateJWT(userCookie);
            if (userID == null)
                return Results.Problem();


            _userConnect.AddUser((int)userID, Response.BodyWriter) ;




            return Results.Ok();

        }

        [HttpPost("StopListen")]
        [Authorize]
        public async Task StopListen()
        {

        }
    }
}
