using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend_.model;

namespace backend_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NetworkController : ControllerBase
    {
        [HttpGet]
        public Network GetNetwork(int id)
        {
            return new Network();
        }

        //[HttpGet]
/*        public IEnumerable<Network> GetNetworks()
        {

        }

        [HttpDelete]
        public bool DeleteNetwork(int id)
        {

        }

        [HttpPatch]
        public bool UpdateNetwork([FromBody]Network network)
        {

        }

        [HttpPost]
        public bool AddNetwork([FromBody]Network network)
        {

        }*/


    }
}
