using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend_.DataBase.UserDB;
using backend_.Connection;
using Microsoft.AspNetCore.Authorization;
using backend_.AuthorizationLogic;
using backend_.Connection.UserConnection;
using backend_.DataBase.ControllerDB;
using backend_.Models.controller;

namespace backend_.Controllers.ValueControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValueController : ControllerBase
    {
        private readonly AuthorizationLogic.Authorization _authorization;
        private readonly UserConnectionController _userConnect;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly Connection.ConnectionController _connectionController;

        public ValueController(IConfiguration configuration,UserConnectionController userConnect, IServiceScopeFactory serviceScopeFactory,Connection.ConnectionController connectionController)
        {
            _authorization = new AuthorizationLogic.Authorization(configuration);
            _userConnect = userConnect;
            _serviceScopeFactory = serviceScopeFactory;
            _connectionController = connectionController;
        }

        [HttpGet("StartListen")]
        [Authorize]
        public async Task<IResult>? StartListen()
        {
            var userCookie = Request.Cookies.ToList();
            var userID = _authorization.ValidateJWT(userCookie);
            if (userID == null)
                return Results.Problem();

            Response.ContentType = "text/plain";

            var Outputs = await _userConnect.AddUser((int)userID, Response.BodyWriter,Request.BodyReader);
            foreach (var item in Outputs)
            {
                _userConnect.userConnction.TryGetValue((UInt32)userID, out var User);
                _connectionController.AddUserToControllerOutput(item.controllerAddress, item.id, User);
            }
            while(true)
            {
                Thread.Sleep(1000);
            }
            return null;
            //return Results.Ok();
        }

        [HttpPost("StopListen")]
        [Authorize]
        public async Task StopListen()
        {

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
