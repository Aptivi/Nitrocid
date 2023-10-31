//
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
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
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.Kernel.Debugging;
using KS.Misc.Splash;
using KS.Misc.Text;
using KS.Kernel.Events;
using KS.Kernel.Configuration;
using KS.Kernel.Threading;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Misc.Screensaver;
using Terminaux.Colors;
using KS.Kernel.Power;
using KS.Users.Login;
using KS.Languages;

namespace KS.Misc.Notifications
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
        private static readonly List<Notification> notifRecents = new();

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

                            // Write notification to console
                            int notifLeftAgnostic = ConsoleWrapper.WindowWidth - 42;
                            int notifTopAgnostic = 1;
                            int notifLeft = useSimplified ? ConsoleWrapper.WindowWidth - 3 : notifLeftAgnostic;
                            int notifTop = useSimplified ? 1 : notifTopAgnostic;
                            int notifTitleTop = notifTopAgnostic + 1;
                            int notifDescTop = notifTopAgnostic + 2;
                            int notifTipTop = notifTopAgnostic + 3;
                            int notifWipeTop = notifTopAgnostic + 4;
                            string clear = ConsoleExtensions.GetClearLineToRightSequence();
                            if (useSimplified)
                            {
                                // Simplified way
                                DebugWriter.WriteDebug(DebugLevel.I, "Where to store: ({0}, {1})", notifLeft, notifTop);
                                TextWriterWhereColor.WriteWhereColor(Title, notifLeft, notifTop, true, NotifyBorderColor);
                            }
                            else
                            {
                                // Normal way
                                DebugWriter.WriteDebug(DebugLevel.I, "Where to store: ({0}, {1}), Title top: {2}, Desc top: {3}, Wipe top: {4}, Tip top: {5}", notifLeft, notifTop, notifTitleTop, notifDescTop, notifWipeTop, notifTipTop);
                                TextWriterWhereColor.WriteWhereColor(Title + clear, notifLeft, notifTitleTop, true, NotifyTitleColor);
                                TextWriterWhereColor.WriteWhereColor(Desc + clear, notifLeft, notifDescTop, true, NotifyDescColor);
                            }

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
                                TextWriterWhereColor.WriteWhereKernelColor(clear, notifLeftAgnostic - 1, notifTopAgnostic, true, KernelColorType.NeutralText);
                                TextWriterWhereColor.WriteWhereKernelColor(clear, notifLeftAgnostic - 1, notifWipeTop, true, KernelColorType.NeutralText);
                                TextWriterWhereColor.WriteWhereColor(CurrentNotifyUpperLeftCornerChar + new string(CurrentNotifyUpperFrameChar, 38) + CurrentNotifyUpperRightCornerChar, notifLeftAgnostic - 1, notifTopAgnostic, true, NotifyBorderColor);
                                TextWriterWhereColor.WriteWhereColor(CurrentNotifyLeftFrameChar.ToString(), notifLeftAgnostic - 1, notifTitleTop, true, NotifyBorderColor);
                                TextWriterWhereColor.WriteWhereColor(CurrentNotifyLeftFrameChar.ToString(), notifLeftAgnostic - 1, notifDescTop, true, NotifyBorderColor);
                                TextWriterWhereColor.WriteWhereColor(CurrentNotifyLeftFrameChar.ToString(), notifLeftAgnostic - 1, notifTipTop, true, NotifyBorderColor);
                                TextWriterWhereColor.WriteWhereColor(CurrentNotifyRightFrameChar.ToString(), ConsoleWrapper.WindowWidth - 4, notifTitleTop, true, NotifyBorderColor);
                                TextWriterWhereColor.WriteWhereColor(CurrentNotifyRightFrameChar.ToString(), ConsoleWrapper.WindowWidth - 4, notifDescTop, true, NotifyBorderColor);
                                TextWriterWhereColor.WriteWhereColor(CurrentNotifyRightFrameChar.ToString(), ConsoleWrapper.WindowWidth - 4, notifTipTop, true, NotifyBorderColor);
                                TextWriterWhereColor.WriteWhereColor(CurrentNotifyLowerLeftCornerChar + new string(CurrentNotifyLowerFrameChar, 38) + CurrentNotifyLowerRightCornerChar, notifLeftAgnostic - 1, notifWipeTop, true, NotifyBorderColor);
                            }

                            // Beep according to priority
                            int BeepTimes = (int)NewNotification.Priority;
                            if (NewNotification.Priority == NotificationPriority.Custom)
                                BeepTimes = NewNotification.CustomBeepTimes;
                            for (int i = 1; i <= BeepTimes; i++)
                                ConsoleWrapper.Beep();

                            // Show progress
                            if (NewNotification.Type == NotificationType.Progress)
                            {
                                int indeterminateStep = 0;
                                while (NewNotification.Progress < 100 && NewNotification.ProgressState == NotificationProgressState.Progressing)
                                {
                                    string ProgressTitle =
                                        !NewNotification.ProgressIndeterminate ?
                                        Title + $" ({NewNotification.Progress}%) " :
                                        Title + " (...%) ";
                                    ProgressTitle = ProgressTitle.Truncate(36);
                                    DebugWriter.WriteDebug(DebugLevel.I, "Where to store progress: {0},{1}", notifLeftAgnostic, notifWipeTop);
                                    DebugWriter.WriteDebug(DebugLevel.I, "Progress: {0}", NewNotification.Progress);

                                    // Write the title, the description, and the progress
                                    TextWriterWhereColor.WriteWhereColor(ProgressTitle, notifLeftAgnostic, notifTitleTop, true, NotifyTitleColor);
                                    TextWriterWhereColor.WriteWhereColor(Desc, notifLeftAgnostic, notifDescTop, true, NotifyDescColor);

                                    // For indeterminate progress, flash the box inside the progress bar
                                    ProgressBarColor.WriteProgress(NewNotification.ProgressIndeterminate ? 100 * indeterminateStep : NewNotification.Progress, notifLeftAgnostic, notifTipTop, notifLeftAgnostic, 6, NotifyProgressColor, NotifyBorderColor, KernelColorTools.GetColor(KernelColorType.Background), DrawBorderNotification);
                                    indeterminateStep++;
                                    if (indeterminateStep > 1)
                                        indeterminateStep = 0;
                                    Thread.Sleep(1);
                                    if (NewNotification.ProgressState == NotificationProgressState.Failure)
                                        TextWriterWhereColor.WriteWhereColor(ProgressTitle, notifLeftAgnostic, notifTitleTop, true, NotifyProgressFailureColor);
                                    else if (NewNotification.ProgressState == NotificationProgressState.Success)
                                        TextWriterWhereColor.WriteWhereColor((ProgressTitle + Translate.DoTranslation("Success")).Truncate(36), notifLeftAgnostic, notifTitleTop, true, NotifyProgressSuccessColor);
                                }
                            }

                            // Clear the area
                            SpinWait.SpinUntil(() => sent, 5000);
                            int width = ConsoleWrapper.WindowWidth - (DrawBorderNotification ? 43 : 42);
                            if (useSimplified)
                                TextWriterWhereColor.WriteWhere(" ", notifLeft, notifTop, true);
                            else
                            {
                                if (DrawBorderNotification)
                                {
                                    TextWriterWhereColor.WriteWhereKernelColor(clear, width, notifTopAgnostic, true, KernelColorType.NeutralText);
                                    TextWriterWhereColor.WriteWhereKernelColor(clear, width, notifWipeTop, true, KernelColorType.NeutralText);
                                }
                                TextWriterWhereColor.WriteWhereKernelColor(clear, width, notifTitleTop, true, KernelColorType.NeutralText);
                                TextWriterWhereColor.WriteWhereKernelColor(clear, width, notifDescTop, true, KernelColorType.NeutralText);
                                TextWriterWhereColor.WriteWhereKernelColor(clear, width, notifTipTop, true, KernelColorType.NeutralText);
                                if (NewNotification.Type == NotificationType.Progress)
                                    TextWriterWhereColor.WriteWhereKernelColor(clear, width, notifWipeTop + 1, true, KernelColorType.NeutralText);
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

    }
}
