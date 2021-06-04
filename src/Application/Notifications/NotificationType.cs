using CleanTemplate.Attributes;

namespace CleanTemplate.Application.Notifications
{
    public enum NotificationType
    {
        /// <summary>
        /// 
        /// </summary>
        [CodeDescription("1", "Error")]
        Error = 1,

        /// <summary>
        /// 
        /// </summary>
        [CodeDescription("2", "Warning")]
        Warning = 2
    }
}
