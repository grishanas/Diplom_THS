using Microsoft.AspNetCore.Mvc;
using backend_.model;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.IdentityModel.Tokens;

namespace backend_.Controllers.Authorization
{

    [Route("/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public AuthorizationController(IConfiguration configuration)
        {
            _configuration=configuration;
        }
        /// <summary>
        /// call to the database controller, if controller return null then this user does not exist
        /// </summary>
        /// <returns></returns>
        /// 
        private UsersAndRole GetUsersAndRoles(UserLogin userLogin)
        {
            var _users = new UsersAndRole();
            _users.UserRoles = new List<UserRole>();
            var tm= new UserRole();
            tm.ur_description = "dsa";
            _users.UserRoles.Add(tm);

            tm = new UserRole();
            tm.ur_description = "sa";
            _users.UserRoles.Add(tm);

            if (_users == null)
                throw new Exception();

            return _users;
        }

        private string? GenerateJwtToken(UsersAndRole user)
        {
            string encodedJWT;
            try
            {
                var claims = new List<Claim>
                {
                new Claim(ClaimTypes.Name,user.Users[0].u_login)
                };
                user.UserRoles.ForEach((role) => { claims.Add(new Claim(ClaimTypes.Role, role.ur_description)); });

                var jwt = new JwtSecurityToken(claims: claims, expires: DateTime.UtcNow.AddHours(1));
                encodedJWT = new JwtSecurityTokenHandler().WriteToken(jwt);
            }
            catch(Exception e)
            {
                throw e;
            };

            return encodedJWT!=null ? encodedJWT.ToString(): throw new Exception();
        }


        [HttpPost("/login")]
        [AllowAnonymous]
        public IResult GenToken([FromBody] UserLogin userLogin)
        {

            UsersAndRole User = new UsersAndRole();
            try
            {
                User = GetUsersAndRoles(userLogin);
            }
            catch (Exception E)
            {
                return Results.Unauthorized();
            }

            GenerateJwtToken(User);
            return Results.Ok();

        }

    }
}
