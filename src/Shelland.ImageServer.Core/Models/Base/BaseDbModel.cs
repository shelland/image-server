// Created on 08/02/2021 16:41 by Andrey Laserson

using System;

namespace Shelland.ImageServer.Core.Models.Base
{
    /// <summary>
    /// Base model for database entities
    /// </summary>
    public class BaseDbModel
    {
        /// <summary>
        /// Entity unique identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Entity create date
        /// </summary>
        public DateTimeOffset CreateDate { get; set; }
    }
}