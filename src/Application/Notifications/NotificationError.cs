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

        public override bool Equals(object obj)
        {
            var objCompare = obj as NotificationError;
            if (objCompare == null)
                return false;
            return  objCompare.Code == Code &&
                    objCompare.Message == Message &&
                    objCompare.Type == Type;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
