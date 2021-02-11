// Created on 10/02/2021 11:55 by Andrey Laserson

using Ardalis.GuardClauses;
using Shelland.ImageServer.Core.Infrastructure.Exceptions;
using Shelland.ImageServer.Core.Models.Enums;

namespace Shelland.ImageServer.Core.Infrastructure.Extensions
{
    public static class GuardClauseExtensions
    {
        public static IGuardClause PositiveCondition(this IGuardClause clause, bool condition)
        {
            if (condition)
            {
                throw new AppFlowException(AppFlowExceptionType.InvalidParameters);
            }

            return clause;
        }
    }
}