// Created on 08/02/2021 16:41 by Andrey Laserson

using System;

namespace Shelland.ImageServer.Core.Models.Base;

/// <summary>
/// Base model for all entities
/// </summary>
public class BaseModel<TKey> where TKey : struct
{
    /// <summary>
    /// Entity unique identifier
    /// </summary>
    public TKey Id { get; set; }

    /// <summary>
    /// Entity create date
    /// </summary>
    public DateTime CreateDateUtc { get; set; }
}