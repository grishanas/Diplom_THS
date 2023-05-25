using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend_.Models.UserModels;
using backend_.DataBase.UserDB;
using Microsoft.AspNetCore.Authorization;

namespace backend_.Controllers.UsersControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class GroupController : ControllerBase
    {
        private readonly GroupDBContext _dbContext;

        public GroupController(GroupDBContext dbContext)
        {
            _dbContext = dbContext;
        }

     
        public class RoleAndGroup
        {
            public int roleId { get; set; }
            public int groupId { get; set; }
        }

        #region Post

        [HttpPost("ControllerGroupRole")]
        public async Task<IResult> AddControllerRole([FromBody] RoleAndGroup roleAndGroup)
        {
            try
            {
                await _dbContext.AddRoleControllerGroup(roleAndGroup.roleId,roleAndGroup.groupId);
                return Results.Ok();
            }
            catch(Exception e)
            {
                return Results.Problem();
            }
        }
        [HttpPost("OutputGroupRole")]
        public async Task<IResult> AddOutputRole([FromBody] RoleAndGroup roleAndGroup)
        {
            try
            {
                await _dbContext.AddRoleControllerOutputGroup(roleAndGroup.roleId, roleAndGroup.groupId);
                return Results.Ok();
            }
            catch (Exception e)
            {
                return Results.Problem();
            }
        }
        #endregion

        [HttpGet("OutputGroup")]
        public async Task<IResult> GetOutputGroup()
        {
            try
            {
                var res = await _dbContext.GetOutputGroups();
                return Results.Ok(res);
            }
            catch(Exception e)
            {
                return Results.Problem();
            }


        }

        [HttpGet("ControllerGroup")]
        public async Task<IResult> GetControllerGroup()
        {
            try
            {
                var res = await _dbContext.GetControllerGroups();
                foreach (var item in res)
                {
                    item.userRoles = null;
                }
                return Results.Ok(res);
            }
            catch (Exception e)
            {
                return Results.Problem();
            }

        }

        [HttpDelete("ControllerGroupRole")]
        public async Task<IResult> DeleteControllerRole([FromBody] RoleAndGroup roleAndGroup)
        {
            try
            {
                await _dbContext.DeleteRoleControllerGroup(roleAndGroup.roleId, roleAndGroup.groupId);
                return Results.Ok();
            }
            catch (Exception e)
            {
                return Results.Problem();
            }
        }
        [HttpDelete("OutputGroupRole")]
        public async Task<IResult> DeleteOutputRole([FromBody] RoleAndGroup roleAndGroup)
        {
            try
            {
                await _dbContext.DeleteRoleControllerOutputGroup(roleAndGroup.roleId, roleAndGroup.groupId);
                return Results.Ok();
            }
            catch (Exception e)
            {
                return Results.Problem();
            }
        }
    }
}
