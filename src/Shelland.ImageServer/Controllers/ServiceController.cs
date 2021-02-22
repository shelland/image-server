// Created on 22/02/2021 20:34 by Andrey Laserson

using Microsoft.AspNetCore.Mvc;

namespace Shelland.ImageServer.Controllers
{
    [Route("service")]
    public class ServiceController : BaseAppController
    {
        [HttpGet("health")]
        public IActionResult Index()
        {
            return Json(new
            {
                ok = true
            });
        }
    }
}