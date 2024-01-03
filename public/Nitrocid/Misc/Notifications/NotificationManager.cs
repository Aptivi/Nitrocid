//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Terminaux.Colors;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Users.Login;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;
using Nitrocid.ConsoleBase;
using Nitrocid.Files.Operations;
using Nitrocid.Misc.Splash;
using Nitrocid.Languages;
using Nitrocid.ConsoleBase.Writers.FancyWriters;
using Nitrocid.Misc.Text;
using Nitrocid.ConsoleBase.Writers.ConsoleWriters;
using Nitrocid.Files.Paths;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Kernel.Events;
using Nitrocid.Files.Operations.Querying;
using Nitrocid.Kernel.Power;

namespace Nitrocid.Misc.Notifications
{
    /// <summary>
    /// Notifications module
    /// </summary>
    public static class NotificationManager
    {

        internal static char notifyUpperLeftCornerChar = '╔';
        internal static char notifyUpperRightCornerChar = '╗';
        internal static char notifyLowerLeftCornerChar = '╚';
        internal static char notifyLowerRightCornerChar = '╝';
        internal static char notifyUpperFrameChar = '═';
        internal static char notifyLowerFrameChar = '═';
        internal static char notifyLeftFrameChar = '║';
        internal static char notifyRightFrameChar = '║';
        internal static bool dnd;
        internal static KernelThread NotifThread = new("Notification Thread", false, NotifListen) { isCritical = true };
        private static bool sent = false;
        private static bool dismissing;
        private static readonly List<Notification> notifRecents = [];

        /// <summary>
        /// Recent notifications
        /// </summary>
        public static List<Notification> NotifRecents =>
            notifRecents;

        /// <summary>
        /// Upper left corner character for the notfication box
        /// </summary>
        public static char NotifyUpperLeftCornerChar =>
            Config.MainConfig.NotifyUpperLeftCornerChar;

        /// <summary>
        /// Upper right corner character for the notfication box
        /// </summary>
        public static char NotifyUpperRightCornerChar =>
            Config.MainConfig.NotifyUpperRightCornerChar;

        /// <summary>
        /// Lower left corner character for the notfication box
        /// </summary>
        public static char NotifyLowerLeftCornerChar =>
            Config.MainConfig.NotifyLowerLeftCornerChar;

        /// <summary>
        /// Lower right corner character for the notfication box
        /// </summary>
        public static char NotifyLowerRightCornerChar =>
            Config.MainConfig.NotifyLowerRightCornerChar;

        /// <summary>
        /// Upper frame character for the notfication box
        /// </summary>
        public static char NotifyUpperFrameChar =>
            Config.MainConfig.NotifyUpperFrameChar;

        /// <summary>
        /// Lower frame character for the notfication box
        /// </summary>
        public static char NotifyLowerFrameChar =>
            Config.MainConfig.NotifyLowerFrameChar;

        /// <summary>
        /// Left frame character for the notfication box
        /// </summary>
        public static char NotifyLeftFrameChar =>
            Config.MainConfig.NotifyLeftFrameChar;

        /// <summary>
        /// Right frame character for the notfication box
        /// </summary>
        public static char NotifyRightFrameChar =>
            Config.MainConfig.NotifyRightFrameChar;

        /// <summary>
        /// Don't disturb, meaning don't show any notification when this mode is on
        /// </summary>
        public static bool DoNotDisturb =>
            Config.MainConfig.DoNotDisturb;

        /// <summary>
        /// Shows all new notifications as asterisks. This option is ignored in notifications with progress bar.
        /// </summary>
        public static bool NotifyDisplayAsAsterisk =>
            Config.MainConfig.NotifyDisplayAsAsterisk;

        /// <summary>
        /// Draws the border around the notification
        /// </summary>
        public static bool DrawBorderNotification =>
            Config.MainConfig.DrawBorderNotification;

        /// <summary>
        /// Listens for notifications and notifies the user if one has been found
        /// </summary>
        private static void NotifListen()
        {
            try
            {
                var OldNotificationsList = new List<Notification>(NotifRecents);
                List<Notification> NewNotificationsList;
                while (!PowerManager.KernelShutdown)
                {
                    SpinWait.SpinUntil(() => NotifRecents.Except(OldNotificationsList).ToList().Count > 0 || dismissing);
                    if (dismissing)
                    {
                        dismissing = false;
                        OldNotificationsList = new List<Notification>(NotifRecents);
                        continue;
                    }
                    NewNotificationsList = NotifRecents.Except(OldNotificationsList).ToList();
                    if (NewNotificationsList.Count > 0 & !ScreensaverManager.InSaver)
                    {
                        // Update the old notifications list
                        DebugWriter.WriteDebug(DebugLevel.W, "Notifications received! Recents count was {0}, Old count was {1}", NotifRecents.Count, OldNotificationsList.Count);
                        OldNotificationsList = new List<Notification>(NotifRecents);
                        sent = false;
                        EventsManager.FireEvent(EventType.NotificationsReceived, NewNotificationsList);

                        // Iterate through new notifications. If we're on the booting stage, ensure that the notifications are only queued until the
                        // kernel has finished booting and that the user is signed in.
                        while (!SplashReport.KernelBooted || !Login.LoggedIn)
                            SpinWait.SpinUntil(() => SplashReport.KernelBooted && Login.LoggedIn);
                        foreach (Notification NewNotification in NewNotificationsList)
                        {
                            EventsManager.FireEvent(EventType.NotificationReceived, NewNotification);

                            // If do not disturb is enabled, don't show.
                            if (DoNotDisturb)
                                // However, fire event for other notifications
                                continue;

                            // Select how to display the notification
                            bool useSimplified = NotifyDisplayAsAsterisk && NewNotification.Type == NotificationType.Normal;

                            // Populate title and description
                            string Title, Desc;
                            DebugWriter.WriteDebug(DebugLevel.I, "Title: {0}", NewNotification.Title);
                            DebugWriter.WriteDebug(DebugLevel.I, "Desc: {0}", NewNotification.Desc);
                            Title = useSimplified ? "*" : NewNotification.Title.Truncate(36);
                            Desc = useSimplified ? "" : NewNotification.Desc.Truncate(36);
                            DebugWriter.WriteDebug(DebugLevel.I, "Truncated title: {0}", Title);
                            DebugWriter.WriteDebug(DebugLevel.I, "Truncated desc: {0}", Desc);
                            DebugWriter.WriteDebug(DebugLevel.I, "Truncated title length: {0}", Title.Length);
                            DebugWriter.WriteDebug(DebugLevel.I, "Truncated desc length: {0}", Desc.Length);

                            // Set the border color
                            DebugWriter.WriteDebug(DebugLevel.I, "Priority: {0}", NewNotification.Priority);
                            var NotifyBorderColor = KernelColorTools.GetColor(KernelColorType.LowPriorityBorder);
                            var NotifyTitleColor = KernelColorTools.GetColor(KernelColorType.NotificationTitle);
                            var NotifyDescColor = KernelColorTools.GetColor(KernelColorType.NotificationDescription);
                            var NotifyProgressColor = KernelColorTools.GetColor(KernelColorType.NotificationProgress);
                            var NotifyProgressFailureColor = KernelColorTools.GetColor(KernelColorType.NotificationFailure);
                            var NotifyProgressSuccessColor = KernelColorTools.GetColor(KernelColorType.Success);
                            switch (NewNotification.Priority)
                            {
                                case NotificationPriority.Medium:
                                    NotifyBorderColor = KernelColorTools.GetColor(KernelColorType.MediumPriorityBorder);
                                    break;
                                case NotificationPriority.High:
                                    NotifyBorderColor = KernelColorTools.GetColor(KernelColorType.HighPriorityBorder);
                                    break;
                                case NotificationPriority.Custom:
                                    NotifyBorderColor = NewNotification.CustomColor;
                                    NotifyTitleColor = NewNotification.CustomTitleColor;
                                    NotifyDescColor = NewNotification.CustomDescriptionColor;
                                    NotifyProgressColor = NewNotification.CustomProgressColor;
                                    NotifyProgressFailureColor = NewNotification.CustomProgressFailureColor;
                                    NotifyProgressSuccessColor = NewNotification.CustomProgressSuccessColor;
                                    break;
                            }

                            // Use the custom border color if available
                            if (NewNotification.NotificationBorderColor != Color.Empty)
                                NotifyBorderColor = NewNotification.NotificationBorderColor;

                            // Determine positions
                            int notifLeftAgnostic = ConsoleWrapper.WindowWidth - 42;
                            int notifTopAgnostic = 1;
                            int notifLeft = useSimplified ? ConsoleWrapper.WindowWidth - 3 : notifLeftAgnostic;
                            int notifTop = useSimplified ? 1 : notifTopAgnostic;
                            int notifTitleTop = notifTopAgnostic + 1;
                            int notifDescTop = notifTopAgnostic + 2;
                            int notifTipTop = notifTopAgnostic + 3;
                            int notifWipeTop = notifTopAgnostic + 4;
                            int notifWidth = ConsoleWrapper.WindowWidth - 4 - notifLeftAgnostic;

                            // Make a string builder for our buffer
                            var printBuffer = new StringBuilder();
                            var textColor = KernelColorTools.GetColor(KernelColorType.NeutralText);
                            var background = KernelColorTools.GetColor(KernelColorType.Background);

                            // Return to the original position
                            (int x, int y) = (ConsoleWrapper.CursorLeft, ConsoleWrapper.CursorTop);

                            // Optionally, draw a border
                            if (DrawBorderNotification && !useSimplified)
                            {
                                // Prepare the variables
                                char CurrentNotifyUpperLeftCornerChar = NotifyUpperLeftCornerChar;
                                char CurrentNotifyUpperRightCornerChar = NotifyUpperRightCornerChar;
                                char CurrentNotifyLowerLeftCornerChar = NotifyLowerLeftCornerChar;
                                char CurrentNotifyLowerRightCornerChar = NotifyLowerRightCornerChar;
                                char CurrentNotifyUpperFrameChar = NotifyUpperFrameChar;
                                char CurrentNotifyLowerFrameChar = NotifyLowerFrameChar;
                                char CurrentNotifyLeftFrameChar = NotifyLeftFrameChar;
                                char CurrentNotifyRightFrameChar = NotifyRightFrameChar;

                                // Get custom corner characters
                                if (NewNotification.Priority == NotificationPriority.Custom)
                                {
                                    CurrentNotifyUpperLeftCornerChar = NewNotification.CustomUpperLeftCornerChar;
                                    CurrentNotifyUpperRightCornerChar = NewNotification.CustomUpperRightCornerChar;
                                    CurrentNotifyLowerLeftCornerChar = NewNotification.CustomLowerLeftCornerChar;
                                    CurrentNotifyLowerRightCornerChar = NewNotification.CustomLowerRightCornerChar;
                                    CurrentNotifyUpperFrameChar = NewNotification.CustomUpperFrameChar;
                                    CurrentNotifyLowerFrameChar = NewNotification.CustomLowerFrameChar;
                                    CurrentNotifyLeftFrameChar = NewNotification.CustomLeftFrameChar;
                                    CurrentNotifyRightFrameChar = NewNotification.CustomRightFrameChar;
                                }

                                // Just draw the border!
                                printBuffer.Append(
                                    BorderColor.RenderBorder(
                                        notifLeftAgnostic - 1, notifTopAgnostic, notifWidth, 3,
                                        CurrentNotifyUpperLeftCornerChar, CurrentNotifyLowerLeftCornerChar,
                                        CurrentNotifyUpperRightCornerChar, CurrentNotifyLowerRightCornerChar,
                                        CurrentNotifyUpperFrameChar, CurrentNotifyLowerFrameChar,
                                        CurrentNotifyLeftFrameChar, CurrentNotifyRightFrameChar,
                                        NotifyBorderColor, background
                                    )
                                );
                            }

                            // Write notification to console
                            if (useSimplified)
                            {
                                // Simplified way
                                DebugWriter.WriteDebug(DebugLevel.I, "Where to store: ({0}, {1})", notifLeft, notifTop);
                                printBuffer.Append(TextWriterWhereColor.RenderWhere(Title, notifLeft, notifTop, NotifyBorderColor, background));
                            }
                            else
                            {
                                // Normal way
                                DebugWriter.WriteDebug(DebugLevel.I, "Where to store: ({0}, {1}), Title top: {2}, Desc top: {3}, Wipe top: {4}, Tip top: {5}", notifLeft, notifTop, notifTitleTop, notifDescTop, notifWipeTop, notifTipTop);
                                printBuffer.Append(TextWriterWhereColor.RenderWhere(Title + new string(' ', notifWidth - Title.Length), notifLeft, notifTitleTop, NotifyTitleColor, background));
                                printBuffer.Append(TextWriterWhereColor.RenderWhere(Desc + new string(' ', notifWidth - Desc.Length), notifLeft, notifDescTop, NotifyDescColor, background));
                            }

                            // Go to the original position and print
                            TextWriterColor.WritePlain(printBuffer.ToString(), false);
                            printBuffer.Clear();
                            ConsoleWrapper.SetCursorPosition(x, y);

                            // Beep according to priority
                            int BeepTimes = (int)NewNotification.Priority;
                            if (NewNotification.Priority == NotificationPriority.Custom)
                                BeepTimes = NewNotification.CustomBeepTimes;
                            for (int i = 1; i <= BeepTimes; i++)
                                ConsoleWrapper.Beep();

                            // Show progress
                            if (NewNotification.Type == NotificationType.Progress)
                            {
                                // Some variables
                                bool indeterminate = NewNotification.ProgressIndeterminate;
                                int indeterminateStep = 0;
                                string ProgressTitle = Title;
                                string renderedProgressTitle = ProgressTitle.Truncate(36);
                                string renderedProgressTitleSuccess = $"{ProgressTitle} ({Translate.DoTranslation("Success")})".Truncate(36);
                                string renderedProgressTitleFailure = $"{ProgressTitle} ({Translate.DoTranslation("Failure")})".Truncate(36);

                                // Loop until the progress is finished
                                while (NewNotification.ProgressState == NotificationProgressState.Progressing)
                                {
                                    // Change the title according to the current progress percentage
                                    ProgressTitle =
                                        !NewNotification.ProgressIndeterminate ?
                                        Title + $" ({NewNotification.Progress}%) " :
                                        Title + " (...%) ";
                                    renderedProgressTitle = ProgressTitle.Truncate(36);
                                    DebugWriter.WriteDebug(DebugLevel.I, "Where to store progress: {0},{1}", notifLeftAgnostic, notifWipeTop);
                                    DebugWriter.WriteDebug(DebugLevel.I, "Progress: {0}", NewNotification.Progress);

                                    // Write the title, the description, and the progress
                                    printBuffer.Append(TextWriterWhereColor.RenderWhere(renderedProgressTitle, notifLeftAgnostic, notifTitleTop, NotifyTitleColor, background));
                                    printBuffer.Append(TextWriterWhereColor.RenderWhere(Desc, notifLeftAgnostic, notifDescTop, NotifyDescColor, background));

                                    // For indeterminate progresses, flash the box inside the progress bar
                                    ProgressBarColor.WriteProgress(indeterminate ? 100 * indeterminateStep : NewNotification.Progress, notifLeftAgnostic, notifTipTop, notifLeftAgnostic, 6, NotifyProgressColor, NotifyBorderColor, KernelColorTools.GetColor(KernelColorType.Background), DrawBorderNotification);
                                    indeterminateStep++;
                                    if (indeterminateStep > 1)
                                        indeterminateStep = 0;
                                    Thread.Sleep(indeterminate ? 250 : 1);

                                    // Print the buffer
                                    TextWriterColor.WritePlain(printBuffer.ToString(), false);
                                    printBuffer.Clear();
                                    ConsoleWrapper.SetCursorPosition(x, y);
                                }

                                // Now, check to see if the progress failed or succeeded
                                if (NewNotification.ProgressState == NotificationProgressState.Failure)
                                    printBuffer.Append(TextWriterWhereColor.RenderWhere(renderedProgressTitleFailure, notifLeftAgnostic, notifTitleTop, NotifyProgressFailureColor, background));
                                else if (NewNotification.ProgressState == NotificationProgressState.Success)
                                    printBuffer.Append(TextWriterWhereColor.RenderWhere(renderedProgressTitleSuccess, notifLeftAgnostic, notifTitleTop, NotifyProgressSuccessColor, background));

                                // Print the buffer
                                TextWriterColor.WritePlain(printBuffer.ToString(), false);
                                printBuffer.Clear();
                                ConsoleWrapper.SetCursorPosition(x, y);
                            }

                            // Clear the area
                            SpinWait.SpinUntil(() => sent, 5000);
                            int left = ConsoleWrapper.WindowWidth - (DrawBorderNotification ? 43 : 42);
                            int width = DrawBorderNotification ? 43 : 42;
                            if (useSimplified)
                                TextWriterWhereColor.WriteWhere(" ", notifLeft, notifTop, true);
                            else
                            {
                                // Clear the area
                                string spaces = new(' ', width);
                                printBuffer.Append(
                                    TextWriterWhereColor.RenderWhere(spaces, left, notifTitleTop, textColor, background) +
                                    TextWriterWhereColor.RenderWhere(spaces, left, notifDescTop, textColor, background) +
                                    TextWriterWhereColor.RenderWhere(spaces, left, notifTipTop, textColor, background)
                                );

                                // Also, clear the border area
                                if (DrawBorderNotification)
                                {
                                    printBuffer.Append(
                                        TextWriterWhereColor.RenderWhere(spaces, left, notifTopAgnostic, textColor, background) +
                                        TextWriterWhereColor.RenderWhere(spaces, left, notifWipeTop, textColor, background)
                                    );
                                }

                                // Also, clear the progress area
                                if (NewNotification.Type == NotificationType.Progress)
                                    printBuffer.Append(
                                        TextWriterWhereColor.RenderWhere(spaces, left, notifWipeTop + 1, textColor, background)
                                    );

                                // Render it
                                TextWriterColor.WritePlain(printBuffer.ToString(), false);
                                printBuffer.Clear();
                                ConsoleWrapper.SetCursorPosition(x, y);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Shutting down notification thread because of {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }

        /// <summary>
        /// Sends notification
        /// </summary>
        /// <param name="notif">Instance of notification holder</param>
        public static void NotifySend(Notification notif)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "List contains this notification? {0}", NotifRecents.Contains(notif));
            if (!NotifRecents.Contains(notif))
            {
                NotifRecents.Add(notif);
                sent = true;
                EventsManager.FireEvent(EventType.NotificationSent, notif);
            }
        }

        /// <summary>
        /// Sends notifications
        /// </summary>
        /// <param name="notifs">Instances of notification holder</param>
        public static void NotifySendRange(List<Notification> notifs)
        {
            foreach (Notification notif in notifs)
                NotifySend(notif);
            EventsManager.FireEvent(EventType.NotificationsSent, notifs);
        }

        /// <summary>
        /// Dismisses a notification
        /// </summary>
        /// <param name="ind">Index of notification</param>
        public static bool NotifDismiss(int ind)
        {
            try
            {
                NotifRecents.RemoveAt(ind);
                DebugWriter.WriteDebug(DebugLevel.I, "Removed index {0} from notification list", ind);
                EventsManager.FireEvent(EventType.NotificationDismissed);
                dismissing = true;
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Error trying to dismiss notification: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return false;
        }

        /// <summary>
        /// Dismisses all notifications
        /// </summary>
        public static bool NotifDismissAll()
        {
            bool successful = true;
            for (int i = NotifRecents.Count - 1; i >= 0; i--)
            {
                if (!NotifDismiss(i))
                    successful = false;

            }
            return successful;
        }

        /// <summary>
        /// Saves recent notifications to the APPDATA folder
        /// </summary>
        public static void SaveRecents()
        {
            string recentsPath =
                Getting.GetNumberedFileName(
                    Path.GetDirectoryName(PathsManagement.GetKernelPath(KernelPathType.NotificationRecents)),
                    PathsManagement.GetKernelPath(KernelPathType.NotificationRecents)
                );
            string serialized = JsonConvert.SerializeObject(NotifRecents, Formatting.Indented);
            Writing.WriteContentsText(recentsPath, serialized);
        }

    }
}
