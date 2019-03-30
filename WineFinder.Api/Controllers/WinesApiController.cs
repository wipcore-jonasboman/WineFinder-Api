using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WineFinder.Shared.Extensions;
using WineFinder.Shared.Models;
using WineFinder.Shared.Services;

namespace MinaViner.Controllers
{
    [RoutePrefix("api/GetMeSomeWine")]
    public class WinesApiController : ApiController
    {
        [HttpPost, HttpOptions]
        [Route("")]
        public IHttpActionResult GetWines(WineRequestData data)
        {
            var handle = Request.Headers.GetValue("X-Authorization-User");
            var key = Request.Headers.GetValue("X-Authorization-Key");

            if (string.IsNullOrEmpty(handle) || string.IsNullOrEmpty(key))
                return BadRequest("error_in_credentials");

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

        [HttpPost, HttpOptions]
        [Route("CreateSharedListKey")]
        public IHttpActionResult CreateSharedListKey(WineRequestData data)
        {
            var handle = Request.Headers.GetValue("X-Authorization-User");
            var key = Request.Headers.GetValue("X-Authorization-Key");

            if (string.IsNullOrEmpty(handle) || string.IsNullOrEmpty(key))
                return BadRequest("error_in_credentials");

            try
            {
                string listId = string.Empty;
                if (System.Web.HttpContext.Current.Session["listId"] != null)
                {
                    listId = System.Web.HttpContext.Current.Session["listId"].ToString();
                }
                else
                {
                    var wineService = new WineService(handle, key);
                    listId = wineService.CreateSharedList(data);
                }

                if (!string.IsNullOrWhiteSpace(listId))
                {
                    System.Web.HttpContext.Current.Session["listId"] = listId;
                    return Ok<string>(listId); 
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost, HttpOptions]
        [Route("GetSharedList/{listId}")]
        public IHttpActionResult GetSharedList(string listId)
        {
            if (string.IsNullOrEmpty(listId))
                return BadRequest("error_in_credentials");

            try
            {
                var wineService = new WineService();
                var wines = wineService.GetSharedWines(listId);
                if (wines == null)
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
