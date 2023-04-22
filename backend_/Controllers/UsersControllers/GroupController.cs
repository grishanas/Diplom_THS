using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend_.Models.UserModels;
using backend_.DataBase.UserDB;

namespace backend_.Controllers.UsersControllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        [HttpPost("AddControllerGroupRole")]
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
        [HttpPost("AddOutputGroupRole")]
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

        [HttpDelete("AddControllerGroupRole")]
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
        [HttpDelete("AddOutputGroupRole")]
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
