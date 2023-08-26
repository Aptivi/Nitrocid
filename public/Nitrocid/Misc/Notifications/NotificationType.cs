namespace KS.Misc.Notifications
{
    /// <summary>
    /// Notification type
    /// </summary>
    public enum NotificationType
    {
        /// <summary>
        /// Normal notification.
        /// </summary>
        Normal = 1,
        /// <summary>
        /// A notification with the progress bar. Use if you're going to notify the user while your mod is doing something.
        /// </summary>
        Progress
    }
}
