// Created on 08/02/2021 16:59 by Andrey Laserson

namespace Shelland.ImageServer.DataAccess.Abstract.Context;

/// <summary>
/// Application DB context
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IAppDbContext<out T>
{
    T Database { get; }
}