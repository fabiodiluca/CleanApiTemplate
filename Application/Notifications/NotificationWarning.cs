namespace CleanTemplate.Application.Notifications
{
    public class NotificationWarning : Notification
    {
        /// <summary>
        /// 
        /// </summary>
        public NotificationWarning() : base(NotificationType.Error) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public NotificationWarning(int code, string message) : base(NotificationType.Error, code, message) { }
    }
}
