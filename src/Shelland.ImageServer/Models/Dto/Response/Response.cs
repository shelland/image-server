// Created on 16/02/2021 18:32 by Andrey Laserson

namespace Shelland.ImageServer.Models.Dto.Response;

/// <summary>
/// Base result wrapper
/// </summary>
/// <typeparam name="T"></typeparam>
public class Response<T>
{
    public Response(T data)
    {
        Data = data;
    }

    public T Data { get; set; }
}