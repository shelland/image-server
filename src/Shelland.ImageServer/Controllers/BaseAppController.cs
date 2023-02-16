// Created on 08/02/2021 15:59 by Andrey Laserson

using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shelland.ImageServer.Models.Dto.Response;

namespace Shelland.ImageServer.Controllers;

[ApiController]
[Route("[controller]")]
public class BaseAppController : ControllerBase
{
    /// <summary>
    /// Returns a default uploaded file
    /// </summary>
    /// <returns></returns>
    [NonAction]
    public IFormFile? GetDefaultFile()
    {
        var files = Request.Form.Files;
        return files.Any() ? files[0] : null;
    }

    public IActionResult Ok<T>(T data) => base.Ok(new Response<T>(data));

    public IActionResult OkOrNotFound<T>(T? data) => data != null ? 
        Ok(data) : 
        NotFound();

    /// <summary>
    /// IP address
    /// </summary>
    public string? IpAddress => Request.HttpContext.Connection.RemoteIpAddress?.ToString();
}