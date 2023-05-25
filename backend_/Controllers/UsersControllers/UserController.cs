using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend_.Models.UserModels;
using backend_.DataBase.UserDB;
using Microsoft.AspNetCore.Authorization;


namespace backend_.Controllers.UsersControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserDBContext _dbContext;

        public UserController(UserDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Get

        [HttpGet("GetAllUsers")]
        public async Task<IResult> GetAll()
        {
            try
            {
                var res = await _dbContext.GetAll();
                return Results.Ok(res);
            }
            catch (Exception e)
            {
                return Results.Problem();
            }
        }


        [HttpGet("GetUser/{id}")]
        public async Task<IResult> Get([FromRoute] int id)
        {
            try
            {
                return Results.Ok(await _dbContext.Get(id));
            }
            catch (Exception e)
            {
                return Results.Problem();
            }

        }


        [HttpGet("GetAllRoles")]
        public async Task<IResult> GetAllRoles()
        {
            try
            {
                return Results.Ok(_dbContext.Roles.ToList());
            }
            catch (Exception e)
            {
                return Results.Problem();
            }
        }

        [HttpGet("GetUserRole/{id}")]
        public async Task<IResult> GetRole([FromRoute] int id)
        {
            try
            {
                return Results.Ok(await _dbContext.GetRole(id));
            } catch (Exception e)
            {

                return Results.Problem();
            }
        }

        #endregion

        #region Post
        [HttpPost("AddUser")]
        public async Task<IResult> AddUser([FromBody] User user)
        {
            try
            {
                var result = await _dbContext.AddUser(user);
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

        [HttpPost("AddRole")]
        public async Task<IResult> AddRole([FromBody] UserRole userRole)
        {
            try
            {
                var res = await _dbContext.AddUserRole(userRole);
                if (res)
                    return Results.Ok();
                else
                    return Results.Problem();
            }
            catch (Exception e)
            {
                return Results.Problem();
            }
        }


        public class UserAndRole
        {
            public int user { get; set; }
            public int userRole { get; set; }
        }
        [HttpPost("AddRoleToUser")]
        public async Task<IResult> AddRoleToUser([FromBody] UserAndRole userAndRole)
        {
            try
            {
                var res = _dbContext.AddNewRoleToUser(userAndRole.userRole, userAndRole.user);
                if (res)
                    return Results.Ok();
                else
                    return Results.Problem();
            } catch (Exception e)
            {
                return Results.Problem();
            }
        }
        #endregion


        #region Delete
        [HttpDelete("DeleteUser/{id}")]
        public async Task<IResult> DeleteUser([FromRoute] int id)
        {
            try
            {
                await _dbContext.DeleteUser(id);
                return Results.Ok();
            }
            catch (Exception e)
            {
                return Results.Problem();
            }
        }

        [HttpDelete("DeleteRole/{id}")]
        public async Task<IResult> DeleteRole([FromRoute] int id)
        {
            try
            {
                var res = await _dbContext.DeleteUserRole(id);
                if (res)
                    return Results.Ok();
                return Results.Problem();
            }
            catch (Exception e)
            {
                return Results.Problem();
            }
        }

        public class DeleteUserRole
        {
            public int UserId { get; set; }
            public int RoleId { get; set; }
        }
        [HttpDelete("DeleteUserRole")]
        public async Task<IResult> DeleteRoleofUser([FromBody] DeleteUserRole userRole )
        {
            try
            {
                await _dbContext.DeleteUserRole(userRole.UserId, userRole.RoleId);
                return Results.Ok();
            }
            catch(Exception e)
            {
                return Results.Problem();
            }
        }
        #endregion
    }
}
