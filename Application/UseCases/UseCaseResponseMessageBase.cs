using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using CleanTemplate.Application.Notifications;
using FluentValidation.Results;

namespace CleanTemplate.Application.UseCases
{
    public abstract class UseCaseResponseMessageBase
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<NotificationError> Errors { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<NotificationWarning> Warnings { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        protected int? _httpStatusToOverride { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notificacao"></param>
        protected UseCaseResponseMessageBase(Notification notificacao)
        {
            if (notificacao == null)
                return;

            ResponseMessageBase(new List<Notification> { notificacao });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notificacoes"></param>
        protected UseCaseResponseMessageBase(IEnumerable<Notification> notificacoes)
        {
            if (notificacoes != null && !notificacoes.Any())
                return;

            ResponseMessageBase(notificacoes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notificacoes"></param>
        /// <param name="tipo"></param>
        private void ResponseMessageBase(IEnumerable<Notification> notificacoes)
        {
            var tipo = notificacoes.FirstOrDefault().Type;

            if (tipo.Equals(NotificationType.Error))
                Errors = (IEnumerable<NotificationError>)notificacoes;

            else if (tipo.Equals(NotificationType.Warning))
                Warnings = (IEnumerable<NotificationWarning>)notificacoes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationResult"></param>
        protected UseCaseResponseMessageBase(ValidationResult validationResult)
        {
            var ErrorsList = new List<NotificationError>();
            foreach (var error in validationResult.Errors)
            {
                int ErrorCode = -1;
                Int32.TryParse(error.ErrorCode, out ErrorCode);
                var errorResponse = new NotificationError(ErrorCode, error.ErrorMessage);
                ErrorsList.Add(errorResponse);
            }
            if (ErrorsList.Any())
                this.Errors = ErrorsList;
        }

        /// <summary>
        /// 
        /// </summary>
        protected UseCaseResponseMessageBase() { }

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
        public bool IsValid()
        {
            return (Errors == null || !Errors.Any()) &&
                (Warnings == null || !Warnings.Any());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int? GetHttpStatusToOverride()
        {
            return _httpStatusToOverride;
        }
    }
}
