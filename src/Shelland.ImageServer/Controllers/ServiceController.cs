// Created on 22/02/2021 20:34 by Andrey Laserson

using Microsoft.AspNetCore.Mvc;

namespace Shelland.ImageServer.Controllers;

public class ServiceController : BaseAppController
{
    public IActionResult Health()
    {
        return Ok();
    }
}