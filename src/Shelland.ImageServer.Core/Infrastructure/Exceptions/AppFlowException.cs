// Created on 10/02/2021 16:22 by Andrey Laserson

using System;
using Shelland.ImageServer.Core.Models.Enums;

namespace Shelland.ImageServer.Core.Infrastructure.Exceptions
{
    public class AppFlowException : Exception
    {
        public AppFlowException(AppFlowExceptionType type)
        {
            this.Type = type;
        }

        public AppFlowException(AppFlowExceptionType type, string? parameter)
        {
            this.Type = type;
            this.Parameter = parameter;
        }

        public string? Parameter { get; set; }

        public AppFlowExceptionType? Type { get; set; }
    }
}