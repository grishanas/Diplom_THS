using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend_.Models.controller;
using backend_.DataBase.ControllerDB;
using Microsoft.AspNetCore.Authorization;

namespace backend_.Controllers.MCController
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ControllerOutputController: ControllerBase
    {

        private readonly ControllerDBContext _controllerDB;
        private readonly Connection.ConnectionController controllers;
        public ControllerOutputController(ControllerDBContext dbContext, Connection.ConnectionController controller)
        {
            _controllerDB = dbContext;
            this.controllers = controller;
        }

        public class ControllerOutputGroupId
        {
            public UInt32 IpAddress { get; set; }
            public int OutputId { get; set; }
            
            public int GroupId { get; set; }
        }


        [HttpPost("AddControllerOutputToGroup")]
        public async Task<IResult> AddControllerOutputToGroup([FromBody] ControllerOutputGroupId controllerOutputGroup)
        {
            try
            {
                var res =await _controllerDB.AddControllerOutputToGroup(controllerOutputGroup.IpAddress, controllerOutputGroup.OutputId, controllerOutputGroup.GroupId);
                if(res)
                return Results.Ok();
                else
                    return Results.Problem();
            }
            catch (Exception e)
            {
                return Results.Problem();
            }
        }

        [HttpDelete("DeleteControllerOutputFromGroup")]
        public async Task<IResult> DeleteControllerOutputFromGroup([FromBody] ControllerOutputGroupId controllerOutputGroup)
        {
            try
            {
                var res = await _controllerDB.DeleteControllerOutputFromGroup(controllerOutputGroup.IpAddress, controllerOutputGroup.OutputId, controllerOutputGroup.GroupId);
                if (res)
                    return Results.Ok();
                else
                    return Results.Problem();
                return Results.Ok();
            }catch(Exception e)
            {
                return Results.Problem();
            }
        }

        [HttpPost("AddRange")]
        public async Task<IResult> AddRange([FromBody] OutputRange range)
        {
            try
            {
                var res = await _controllerDB.AddOutputRange(range);
                if (res)
                    return Results.Ok();
            }
            catch (Exception e)
            {
                return Results.Problem();
            }
            return Results.Problem();
        }

        [HttpPost("AddRangeToOutput")]
        public async Task<IResult> AddRangeToOutput()
        {
            try
            {

            }
            catch (Exception e)
            {
                return Results.Problem();
            }
            return Results.Problem();
        }

        [HttpGet("GetOutputRanges")]
        public async Task<IResult> GetRange()
        {
            try
            {
                var res = _controllerDB.outputRanges.ToList();
                return Results.Json(res);

            }catch(Exception e)
            {
                return Results.Problem();
            }
            return Results.Problem();
        }


        public class QeuryAndOutput
        {
            public string qeury { get; set; }
            public int OutputId { get; set; }
            public UInt32 address { get; set; } 
        }
        [HttpPost("AddQeuryToOutput")]
        public async Task<IResult> AddQeuryToOutput([FromBody] QeuryAndOutput qeury)
        {
            try
            {
                var dbCommand = await _controllerDB.GetControllerOutput(qeury.address, qeury.OutputId);
                if(dbCommand==null)
                    return Results.Problem();
                var command = controllers.SetCommand(qeury.address,qeury.OutputId,qeury.qeury);
                if(!command)
                    return Results.Problem();
                var res = await _controllerDB.AddQeuryToController(qeury.address,qeury.OutputId,qeury.qeury);
                if(!res)
                {
                    controllers.RemoveCommand(qeury.address, qeury.OutputId);
                }
                    
                return Results.Ok();
            }
            catch (Exception e)
            {
                return Results.Problem();
            }
            return Results.Problem();
        }

        public class OutputStateID
        {
            public int outputId { get; set; }
            public UInt32 address { get; set;}

            public string stateDescription { get; set; }
        }
        [HttpPost("SetOutputState")]
        public async Task<IResult> SetOutputState([FromBody] OutputStateID outputState)
        {
            try
            {
                var stateSet = controllers.SetOutputState(outputState.stateDescription, (UInt32)outputState.address, outputState.outputId);
                if(stateSet)
                {
                    var output = await _controllerDB.GetControllerOutput((UInt32)outputState.address, outputState.outputId);
                    var state = await _controllerDB.GetOutputState(outputState.stateDescription);
                    if(state==null)
                    {
                        await _controllerDB.AddOutputState(outputState.stateDescription);
                        state = await _controllerDB.GetOutputState(outputState.stateDescription);
                    }
                    output.m2mOutputGroups = null;
                    output.outputGroups = null;
                    output.outputState = state;
                    _controllerDB.outputs.Update(output);
                    try
                    {
                        _controllerDB.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        return Results.BadRequest();
                    }
                    return Results.Ok();
                }
                else
                {
                    return Results.BadRequest();
                }

            }catch(Exception e)
            {
                return Results.Problem();
            }
            return Results.Problem();

        }

        [HttpGet("GetAllAllowQeurys")]
        public async Task<IResult> GetAllAllowQeurys([FromQuery] UInt32 address)
        {
            try
            {
                var item = controllers.GetAllCommands((UInt32)address);
                return Results.Json(item,statusCode:StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return Results.Problem();
            }
            return Results.Problem();
        }

        public class ControllerOutputI
        {
            public int id { get; set; }

            public int controllerAddress { get; set; }
            public string name { get; set; }
            public string description { get; set; }

            public string state { get; set; }
        }

        [HttpPost("Output")]
        public async Task<IResult> AddOutput([FromBody] ControllerOutputI output)
        {
            try
            {
                var res = await _controllerDB.AddControllerOutput(new ControllerOutput() { id = output.id,controllerAddress= (UInt32)output.controllerAddress,name=output.name,description=output.description,outputState= new OutputState() { description=output.state} }) ;
                if (res)
                {
                    res = controllers.SetCommand((UInt32)output.controllerAddress, output.id, null);     
                }
                else
                {
                    return Results.Problem();
                }
                if (res)
                {   
                    return Results.Ok();
                }
                else
                {
                    await _controllerDB.DeleteControllerOutput((UInt32)output.controllerAddress, output.id);
                    return Results.Problem();
                }


            }catch(Exception e)
            {
                return Results.Problem();
            }
            return Results.Problem();
        }

        [HttpGet("Output/{id}")]
        public async Task<IResult> GetOutput([FromQuery] UInt32 adrress, [FromRoute] int id)
        {
            try
            {
                var res = await _controllerDB.GetControllerOutputs(adrress);
                var output=  res.FirstOrDefault(x=>x.id== id);                
                output.outputState.controllers = null;
                return Results.Json(output, statusCode:200);
            }
            catch(Exception e)
            {
                return Results.Problem();
            }
            return Results.Problem();
        }

        [HttpGet("Outputs")]
        public async Task<IResult> GetOutputs([FromQuery] int adrress)
        {
            try
            {
                var res = await _controllerDB.GetControllerOutputs((UInt32)adrress);
                foreach(var item in res)
                {
                    item.outputState.controllers = null;
                }
                return Results.Json(res, statusCode: 200);
            }
            catch (Exception e)
            {
                return Results.Problem();
            }
            return Results.Problem();
        }
    }
}
