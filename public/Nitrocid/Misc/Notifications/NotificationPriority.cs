namespace KS.Misc.Notifications
{
    /// <summary>
    /// Notification priority
    /// </summary>
    public enum NotificationPriority
    {
        /// <summary>
        /// Low priority. One beep.
        /// </summary>
        Low = 1,
        /// <summary>
        /// Medium priority. Two beeps.
        /// </summary>
        Medium,
        /// <summary>
        /// High priority. Three beeps.
        /// </summary>
        High,
        /// <summary>
        /// Custom priority. Custom colors, beeps, etc.
        /// </summary>
        Custom
    }
}
