using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SampleSecureApi.Areas.Identity.Data;
using SampleSecureApi.Data;
using System.Collections.Generic;

namespace SampleSecureApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ValuesController : ControllerBase
    {
        [HttpGet("getFruits")]
        [AllowAnonymous]
        public ActionResult GetFruits()
        {
            List<string> mylist = new List<string>() { "apples", "bananas" };
            return Ok(mylist);
        }

        [HttpGet("getFruitsAuthenticated")]
        public ActionResult GetFruitsAuthenticated()
        {
            List<string> mylist = new List<string>() { "organic apples", "organic bananas" };
            return Ok(mylist);
        }
    }
}
