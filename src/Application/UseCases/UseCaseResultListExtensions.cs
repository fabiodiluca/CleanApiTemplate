using System;
using System.Collections.Generic;

namespace CleanTemplate.Application.UseCases
{
    public static class UseCaseResultListExtensions
    {
        public static void AddSpecifiedIdDoesNotExist<T>(this List<UseCaseResult<T>> resultList)
        {
            var errorResult = (UseCaseResult<T>) Activator.CreateInstance(
                typeof(UseCaseResult<T>),
                Notifications.NotificationError.SpecifiedIdDoesNotExist()
            );
            resultList.Add(errorResult);
        }
    }
}
