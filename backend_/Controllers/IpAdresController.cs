using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend_.model;
using backend_.DataBase;

namespace backend_.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class IpAdresController : ControllerBase
    {

        [HttpGet("{id}")]
        public IpAdres Get(int id)
        {
            var dbConnection = new IpAdresDataBase();
            var ipAdres = dbConnection.GetIpAdres(id);

            return ipAdres;
        }

        [HttpPost]
        public bool AddIp([FromBody] IpAdres adres)
        {
            var dbConnection = new IpAdresDataBase();
            return dbConnection.AddIpAdres(adres);
        }

        [HttpGet]
        public IEnumerable<IpAdres> Get()
        {

            var dbConnection = new IpAdresDataBase();
            var ipAdres = dbConnection.GetIpAdreses();

            return ipAdres;
        }

        [HttpDelete("{id}")]
        public bool Delete( int id)
        {
            var dbConnection = new IpAdresDataBase();
            return dbConnection.DeleteAdres(id);
        }

        [HttpPatch]
        public bool Patch([FromBody] IpAdres adres)
        {
            var dbConnection = new IpAdresDataBase();
            return dbConnection.PutchAdres(adres);
        }

    }



}
