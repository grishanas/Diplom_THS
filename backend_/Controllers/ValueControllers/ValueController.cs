using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend_.DataBase.UserDB;
using backend_.Connection;
using Microsoft.AspNetCore.Authorization;
using backend_.AuthorizationLogic;
using backend_.DataBase.ControllerDB;
using backend_.Models.controller;
using Microsoft.AspNetCore.Authorization;

namespace backend_.Controllers.ValueControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ValueController : ControllerBase
    {
        private readonly AuthorizationLogic.Authorization _authorization;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly Connection.ConnectionController _connectionController;

        public ValueController(IConfiguration configuration,IServiceScopeFactory serviceScopeFactory,Connection.ConnectionController connectionController)
        {
            _authorization = new AuthorizationLogic.Authorization(configuration);
            _serviceScopeFactory = serviceScopeFactory;
            _connectionController = connectionController;
        }




        public class OutputId
        {
            public UInt32 address { get; set; }
            public int outputId { get; set; }
        }
        public class ValueId: OutputId
        {
            public DateTime startTime { get; set; }
            public DateTime endTime { get; set; }
        }


        private async Task<ControllerOutput> GetOutput(UInt32 address, int outputId)
        {
            return new ControllerOutput();
        }
        [HttpGet("AllowedOutput")]
        [Authorize]
        public async Task<IResult> GetAllowedOutput()
        {
            try
            {
                var userCookie = Request.Cookies.ToList();
                var userID = _authorization.ValidateJWT(userCookie);
                if (userID == null)
                    return Results.Problem();

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var UserDB = scope.ServiceProvider.GetService<UserDBContext>();
                    var user = await UserDB.Get((int)userID);
                    if(user.userRoles.FirstOrDefault(x => x.description == "Admin") != null)
                    {
                        var ControllerDB = scope.ServiceProvider.GetService<ControllerDBContext>();

                        var controllers = await ControllerDB.GetAllControllers();

                        var Outputs = new List<ControllerOutput>();
                        foreach(var controller in controllers)
                        {
                            controller.controllerName.controllers = null;
                            controller.ControllerState.controllers = null;

                            Outputs.AddRange(controller.outputs);
                        }

                        foreach (var output in Outputs)
                        {
                            output.controller.outputs = null;
                            output.outputState.controllers = null;
                        }

                        return Results.Ok(Outputs);



                    }
                    else
                    {

                        var OutputGroupsDB = scope.ServiceProvider.GetService<GroupDBContext>();
                        var outputsGroups = new List<backend_.Models.controllerGroup.ControllerOutputGroupUser>();
                        foreach(var Role in user.userRoles)
                        {
                            outputsGroups.AddRange(await OutputGroupsDB.GetOutputGroups(Role.id));
                        }
                        var controllerDB = scope.ServiceProvider.GetService<ControllerDBContext>();
                        var Outputs = new List<ControllerOutput>();

                        foreach (var Group in outputsGroups)
                        {
                            Outputs.AddRange(await controllerDB.GetControllerOutputsWithOutputGroup(Group.id));
                        }
                        return Results.Ok(Outputs);
                    }
                }
            }
            catch(Exception e)
            {
                return Results.Problem();
            }
            return Results.Problem();
        }

        [HttpPost("GetValueWithTime")]
        [Authorize]
        public async Task<IResult> GetValueWithTime([FromBody] ValueId valueId)
        {

            try
            {
                var userCookie = Request.Cookies.ToList();
                var userID = _authorization.ValidateJWT(userCookie);
                if (userID == null)
                    return Results.Problem();

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var UserDB = scope.ServiceProvider.GetService<UserDBContext>();
                    var user = await UserDB.Get((int)userID);


                    if (user.userRoles.FirstOrDefault(x => x.description == "Admin") != null)
                    {
                        var ControllerDB = scope.ServiceProvider.GetService<ControllerDBContext>();

                        var values = ControllerDB.GetValues(valueId.address, valueId.outputId, valueId.startTime, valueId.endTime);

                        return Results.Ok(values);
                    }
                    else
                    {
                        var RoleDB = scope.ServiceProvider.GetService<ControllerGroupDBContext>();

                        List<Models.controllerGroup.ControllerOutputGroupUser> Groups = new List<Models.controllerGroup.ControllerOutputGroupUser>();
                        foreach (var item in user.userRoles)
                        {
                            var OutputRoles = await RoleDB.GetOutputGroupsWithRole(item.id);
                            Groups.AddRange(OutputRoles);
                        }

                        var ControllerDB = scope.ServiceProvider.GetService<ControllerDBContext>();
                        foreach (var item in Groups)
                        {
                            var output = await ControllerDB.GetControolerOutputs(item.id);
                        }
                    }


                }



            }
            catch(Exception e)
            {
                return Results.Problem();
            }
            return Results.Problem();


        }
    }
}
