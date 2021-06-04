using Newtonsoft.Json;

namespace CleanTemplate.Application.Notifications
{
    public class NotificationError : Notification
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public ErrorCategory Category { get; protected set; }


        /// <summary>
        /// 
        /// </summary>
        public NotificationError()
            : base(NotificationType.Error)
        {
            Category = ErrorCategory.NotSpecified;
        }

        /// <summary>
        /// 
        /// </summary>
        public NotificationError(ErrorCategory category) 
            : base(NotificationType.Error) 
        {
            Category = category;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public NotificationError(
            int code, 
            string message, 
            ErrorCategory category = ErrorCategory.NotSpecified) 
            : base(NotificationType.Error, code, message) 
        {
            Category = category;
        }

        public override bool Equals(object obj)
        {
            var objCompare = obj as NotificationError;
            if (objCompare == null)
                return false;
            return objCompare.Code == Code &&
                   objCompare.Message == Message;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static NotificationError SpecifiedIdDoesNotExist()
        {
            return new NotificationError(-1, "Specified id does not exist.", ErrorCategory.EntityNotFound);
        }
    }
}
