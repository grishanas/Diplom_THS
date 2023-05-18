using backend_.DataBase.UserDB;
using backend_.Models.UserModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace backend_.AuthorizationLogic
{
    public class Authorization
    {
        private readonly UserDBContext _dbContext;
        private readonly IConfiguration _config;
        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public Authorization(UserDBContext dbContext, IConfiguration config)
        {
            _dbContext = dbContext;
            _config = config;
        }

        public Authorization(IConfiguration config)
        {
            _config = config;
        }

        private string GetRandomString(int length)
        {
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            
        }


        public class RoleJWT
        {
            public string JWT { get; set; }
            public string Role { get; set; }
        }
        public async Task<RoleJWT> UserAuthentication(UserLogin userLogin)
        {
            
            var user = await _dbContext.Get(userLogin);
            if (user == null)
                throw new Exception("");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var Role = new RoleJWT();
            var claims = new List<Claim>();
            claims.Add(new Claim("userId", user.id.ToString()));
            claims.Add(new Claim("userLogin", user.login.ToString()));
            if(user.userRoles.Where(x=>x.description=="Admin")!=null)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                Role.Role = "Admin";
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "User"));
                Role.Role = "User";
            }
            /*            foreach(var item in user.userRoles)
                        {
                            claims.Add(new Claim("User Role", item.id.ToString()));
                        }*/

            var jwt = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(100),
                signingCredentials: credentials
                );
            Role.JWT = new JwtSecurityTokenHandler().WriteToken(jwt);
            return Role;
        }

        public RoleJWT LogOutUser()
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var roleJWT = new RoleJWT();
            var jwt = new JwtSecurityToken(
                expires: DateTime.UtcNow.AddMilliseconds(0),
                signingCredentials: credentials);
            roleJWT.JWT = new JwtSecurityTokenHandler().WriteToken(jwt);
            
            return roleJWT;
        }

        public int? ValidateJWT(List<KeyValuePair<string,string>> cookies)
        {
            var jwt = cookies.Where(x => x.Key == "some.Text").FirstOrDefault();
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = (Encoding.UTF8.GetBytes(_config["JWT:SecretKey"]));
            try
            {
                tokenHandler.ValidateToken(jwt.Value, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(securityKey),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                }, out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "userId").Value);
                return userId;
            }
            catch (Exception e)
            {
                return null;
            }




        }
    }

    
}
