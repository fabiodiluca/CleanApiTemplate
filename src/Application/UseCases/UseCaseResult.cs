using CleanTemplate.Application.Notifications;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Net;

namespace CleanTemplate.Application.UseCases
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public class UseCaseResult<TData> : UseCaseResultMessageBase
    {
        /// <summary>
        /// 
        /// </summary>
        public TData Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notification"></param>
        public UseCaseResult(NotificationError notification) : base(new List<NotificationError>() { notification }) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notifications"></param>
        public UseCaseResult(IEnumerable<NotificationError> notifications) : base(notifications) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationResult"></param>
        public UseCaseResult(ValidationResult validationResult) : base(validationResult) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public UseCaseResult(TData data)
        {
            if (data != null)
            {
                this.Data = data;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public UseCaseResult() { }
    }
}
