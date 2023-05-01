using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend_.Models.controller;
using backend_.DataBase.ControllerDB;

namespace backend_.Controllers.MCController
{
    public class ControllerOutputController
    {

        private readonly ControllerDBContext _controllerDB;
        private readonly Connection.ConnectionController controllers;
        public ControllerOutputController(ControllerDBContext dbContext, Connection.ConnectionController controller)
        {
            _controllerDB = dbContext;
            this.controllers = controller;
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

        public class SetOutputStateClass
        {
            public int outputId { get; set; }
            public UInt32 address { get; set;}

            public int stateId { get; set; }
        }
        [HttpPost("SetOutputState")]
        public async Task<IResult> SetOutputState([FromBody] SetOutputStateClass outputState)
        {
            try
            {

            }catch(Exception e)
            {
                return Results.Problem();
            }
            return Results.Ok();

        }

        [HttpGet("GetAllAllowQeurys")]
        public async Task<IResult> GetAllAllowQeurys([FromQuery] UInt32 address)
        {
            try
            {
                var item = await controllers.GetAllCommands((UInt32)address);
                return Results.Ok(item);
            }
            catch (Exception e)
            {
                return Results.Problem();
            }
            return Results.Problem();
        }

        [HttpPost("AddOutput")]
        public async Task<IResult> AddOutput([FromBody]ControllerOutput output)
        {
            try
            {
                var res= await _controllerDB.AddControllerOutput(output);
                if (res)
                    return Results.Ok();

            }catch(Exception e)
            {
                return Results.Problem();
            }
            return Results.Problem();
        }

        [HttpGet("GetAllOutput/{id}")]
        public async Task<IResult> GetAllOutput([FromQuery] UInt32 adrress)
        {
            try
            {
                var res = await _controllerDB.GetControllerOutputs(adrress);
            }
            catch(Exception e)
            {
                return Results.Problem();
            }
            return Results.Problem();
        }


        [HttpPost("AddOutputState")]
        public async Task<IResult> AddOutputState([FromBody] string description)
        {
            try
            {
                await _controllerDB.AddOutputState(description);
            }catch(Exception e)
            {
                return Results.Problem();
            }
            return Results.Ok();
        }

        [HttpGet("GetOutputStates")]
        public async Task<IResult> GetOutputStates()
        {
            try
            {
                var res = await _controllerDB.GetOutputStates();
                return Results.Ok(res);
            }catch(Exception e)
            {
                return Results.Problem();
            }
            return Results.Ok();
        }
    }
}
