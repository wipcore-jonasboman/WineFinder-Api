using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WineFinder.Shared.Extensions;
using WineFinder.Shared.Models;
using WineFinder.Shared.Services;

namespace MinaViner.Controllers
{
    [RoutePrefix("api/GetMeSomeWine")]
    public class WinesApiController : ApiController
    {
        [HttpPost,HttpOptions]
        [Route("")]
        public IHttpActionResult GetWines(WineRequestData data)
        {
            var handle = Request.Headers.GetValue("X-Authorization-User");
            var key = Request.Headers.GetValue("X-Authorization-Key");

            if (string.IsNullOrEmpty(handle) || string.IsNullOrEmpty(key))
                return BadRequest("Error in credentials - check the login form");

            try
            {
                var wineService = new WineService(handle, key);
                var wines = wineService.Get(data);
                if (!wines.Any())
                {
                    return NotFound();
                }

                return Ok<List<WineItem>>(wines);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        }
    }
}
