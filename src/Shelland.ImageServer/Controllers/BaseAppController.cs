// Created on 08/02/2021 15:59 by Andrey Laserson

using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Shelland.ImageServer.Controllers
{
    [ApiController]
    public class BaseAppController : Controller
    {
        /// <summary>
        /// Returns a default uploaded file
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public IFormFile GetDefaultFile()
        {
            var files = Request.Form.Files;
            return files.Any() ? files[0] : null;
        }
    }
}