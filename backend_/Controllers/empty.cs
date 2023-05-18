using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using Microsoft.AspNetCore.Authorization;

namespace backend_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class empty : ControllerBase
    {

        public empty()
        {

        }
        [HttpGet]
        //[Authorize(Roles = ("Admin"))]
        public async Task Get()
        {
            Response.ContentType = "text/plain";
            StreamWriter sw;
            await using ((sw = new StreamWriter(Response.Body)).ConfigureAwait(false))
            {

                var random= new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                int i = 0;

                while (true)
                {
                    var item = new string(Enumerable.Repeat(chars, 10)
                        .Select(s => s[random.Next(s.Length)]).ToArray());
                    await sw.WriteLineAsync(item.ToString()).ConfigureAwait(false);
                    await sw.FlushAsync().ConfigureAwait(false);
                    //i++;
                    //if (i > 100)
                    //    break;
                    Thread.Sleep(100);
                }
            }

        }
    }
}
