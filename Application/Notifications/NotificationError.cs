namespace CleanTemplate.Application.Notifications
{
    public class NotificationError : Notification
    {
        /// <summary>
        /// 
        /// </summary>
        public NotificationError() : base(NotificationType.Error) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public NotificationError(int code, string message) : base(NotificationType.Error, code, message) { }

    }
}
