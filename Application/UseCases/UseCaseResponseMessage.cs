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
    public class UseCaseResponseMessage<TData> : UseCaseResponseMessageBase
    {
        /// <summary>
        /// 
        /// </summary>
        public TData Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notifications"></param>
        public UseCaseResponseMessage(NotificationError notifications) : base(notifications) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notifications"></param>
        public UseCaseResponseMessage(IEnumerable<NotificationError> notifications) : base(notifications) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationResult"></param>
        public UseCaseResponseMessage(ValidationResult validationResult) : base(validationResult) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public UseCaseResponseMessage(TData data)
        {
            if (data != null)
            {
                this.Data = data;
            }
            else
            {
                this._httpStatusToOverride = (int)HttpStatusCode.NoContent;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public UseCaseResponseMessage() { }
    }
}
