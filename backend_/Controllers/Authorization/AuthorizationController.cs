using Microsoft.AspNetCore.Mvc;
using backend_.model;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using backend_.DataBase;

namespace backend_.Controllers.Authorization
{

    [Route("/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {

        /// <summary>
        /// call to the database controller, if controller return null then this user does not exist
        /// </summary>
        /// <returns></returns>
        private UserAndRole GetUsersAndRoles(UserLogin userLogin)
        {
            UserContext userContext = new UserContext();
            var _users = userContext.GetUsers();

            return _users;
        }


        [HttpPost("/login")]
        
        public IResult GenToken([FromBody] UserLogin userLogin)
        {
            UserAndRole User = new UserAndRole();
            try
            {
                User = GetUsersAndRoles(userLogin);
            }
            catch (Exception E)
            {
                return Results.Json(E.Message);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,userLogin.login),
            };
            User.UserRoles.ForEach((role) => { claims.Add(new Claim(ClaimTypes.Role, role.ur_description)); });

            var jwt = new JwtSecurityToken(claims: claims);
            var encodedJWT = new JwtSecurityTokenHandler().WriteToken(jwt); 


            
            return Results.Json(encodedJWT);

        }

    }
}
