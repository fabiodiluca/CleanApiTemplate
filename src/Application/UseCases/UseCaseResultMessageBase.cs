using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using CleanTemplate.Application.Notifications;
using FluentValidation.Results;

namespace CleanTemplate.Application.UseCases
{
    public abstract class UseCaseResultMessageBase
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<NotificationError> Errors { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<NotificationWarning> Warnings { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notificacoes"></param>
        protected UseCaseResultMessageBase(IEnumerable<NotificationError> notificacoes)
        {
            if (notificacoes != null && !notificacoes.Any())
                return;

            Errors = notificacoes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notificacoes"></param>
        protected UseCaseResultMessageBase(IEnumerable<NotificationWarning> notificacoes)
        {
            if (notificacoes != null && !notificacoes.Any())
                return;

            Warnings = notificacoes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationResult"></param>
        protected UseCaseResultMessageBase(ValidationResult validationResult)
        {
            var ErrorsList = new List<NotificationError>();
            foreach (var error in validationResult.Errors)
            {
                int ErrorCode;
                Int32.TryParse(error.ErrorCode, out ErrorCode);
                var errorResponse = new NotificationError(ErrorCode, error.ErrorMessage, ErrorCategory.EntityValidation);
                ErrorsList.Add(errorResponse);
            }
            if (ErrorsList.Any())
                this.Errors = ErrorsList;
        }

        /// <summary>
        /// 
        /// </summary>
        protected UseCaseResultMessageBase() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="errorMessage"></param>
        public void AddError(int errorCode, string errorMessage)
        {
            if (this.Errors == null)
                this.Errors = new List<NotificationError>();

            this.Errors.ToList().Add(new NotificationError(errorCode, errorMessage));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool AnyErrors()
        {
            return (Errors != null && !Errors.Any());
        }

        public bool AnyValidationErrors()
        {
            return (Errors != null && !Errors.Where(x => x.Category == ErrorCategory.EntityValidation).Any());
        }

        public bool AnyNotFoundErrors()
        {
            return (Errors != null && !Errors.Where(x => x.Category == ErrorCategory.NotSpecified).Any());
        }
    }
}
