using Newtonsoft.Json;

namespace CleanTemplate.Application.Notifications
{
    public class Notification
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public NotificationType Type { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Notification(NotificationType type) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public Notification(NotificationType type, int code, string message)
        {
            this.Type = type;
            this.Code = code;
            this.Message = message;
        }
    }
}
