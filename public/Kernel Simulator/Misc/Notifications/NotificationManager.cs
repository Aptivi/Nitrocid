
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Extensification.StringExts;
using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Misc.Splash;
using KS.Misc.Text;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;
using KS.Kernel.Events;

namespace KS.Misc.Notifications
{
    /// <summary>
    /// Notifications module
    /// </summary>
    public static class NotificationManager
    {

        internal static string notifyUpperLeftCornerChar = "╔";
        internal static string notifyUpperRightCornerChar = "╗";
        internal static string notifyLowerLeftCornerChar = "╚";
        internal static string notifyLowerRightCornerChar = "╝";
        internal static string notifyUpperFrameChar = "═";
        internal static string notifyLowerFrameChar = "═";
        internal static string notifyLeftFrameChar = "║";
        internal static string notifyRightFrameChar = "║";
        internal static KernelThread NotifThread = new("Notification Thread", false, NotifListen);
        private static readonly List<Notification> notifRecents = new();

        /// <summary>
        /// Recent notifications
        /// </summary>
        public static List<Notification> NotifRecents => notifRecents;
        /// <summary>
        /// Upper left corner character for the notfication box
        /// </summary>
        public static string NotifyUpperLeftCornerChar
        {
            get => notifyUpperLeftCornerChar;
            set => notifyUpperLeftCornerChar = string.IsNullOrEmpty(value) ? "╔" : value[0].ToString();
        }
        /// <summary>
        /// Upper right corner character for the notfication box
        /// </summary>
        public static string NotifyUpperRightCornerChar
        {
            get => notifyUpperRightCornerChar;
            set => notifyUpperRightCornerChar = string.IsNullOrEmpty(value) ? "╗" : value[0].ToString();
        }
        /// <summary>
        /// Lower left corner character for the notfication box
        /// </summary>
        public static string NotifyLowerLeftCornerChar
        {
            get => notifyLowerLeftCornerChar;
            set => notifyLowerLeftCornerChar = string.IsNullOrEmpty(value) ? "╚" : value[0].ToString();
        }
        /// <summary>
        /// Lower right corner character for the notfication box
        /// </summary>
        public static string NotifyLowerRightCornerChar
        {
            get => notifyLowerRightCornerChar;
            set => notifyLowerRightCornerChar = string.IsNullOrEmpty(value) ? "╝" : value[0].ToString();
        }
        /// <summary>
        /// Upper frame character for the notfication box
        /// </summary>
        public static string NotifyUpperFrameChar
        {
            get => notifyUpperFrameChar;
            set => notifyUpperFrameChar = string.IsNullOrEmpty(value) ? "═" : value[0].ToString();
        }
        /// <summary>
        /// Lower frame character for the notfication box
        /// </summary>
        public static string NotifyLowerFrameChar
        {
            get => notifyLowerFrameChar;
            set => notifyLowerFrameChar = string.IsNullOrEmpty(value) ? "═" : value[0].ToString();
        }
        /// <summary>
        /// Left frame character for the notfication box
        /// </summary>
        public static string NotifyLeftFrameChar
        {
            get => notifyLeftFrameChar;
            set => notifyLeftFrameChar = string.IsNullOrEmpty(value) ? "║" : value[0].ToString();
        }
        /// <summary>
        /// Right frame character for the notfication box
        /// </summary>
        public static string NotifyRightFrameChar
        {
            get => notifyRightFrameChar;
            set => notifyRightFrameChar = string.IsNullOrEmpty(value) ? "║" : value[0].ToString();
        }
        /// <summary>
        /// Don't disturb, meaning don't show any notification when this mode is on
        /// </summary>
        public static bool DoNotDisturb { 
            get => Flags.DoNotDisturb;
            set => Flags.DoNotDisturb = value;
        }

        /// <summary>
        /// Notification priority
        /// </summary>
        public enum NotifPriority
        {
            /// <summary>
            /// Low priority. One beep.
            /// </summary>
            Low = 1,
            /// <summary>
            /// Medium priority. Two beeps.
            /// </summary>
            Medium = 2,
            /// <summary>
            /// High priority. Three beeps.
            /// </summary>
            High = 3,
            /// <summary>
            /// Custom priority. Custom colors, beeps, etc.
            /// </summary>
            Custom = 4
        }

        /// <summary>
        /// Notification type
        /// </summary>
        public enum NotifType
        {
            /// <summary>
            /// Normal notification.
            /// </summary>
            Normal = 1,
            /// <summary>
            /// A notification with the progress bar. Use if you're going to notify the user while your mod is doing something.
            /// </summary>
            Progress = 2
        }

        /// <summary>
        /// Listens for notifications and notifies the user if one has been found
        /// </summary>
        private static void NotifListen()
        {
            try
            {
                var OldNotificationsList = new List<Notification>(NotifRecents);
                List<Notification> NewNotificationsList;
                while (!Flags.KernelShutdown)
                {
                    Thread.Sleep(100);
                    NewNotificationsList = NotifRecents.Except(OldNotificationsList).ToList();
                    if (NewNotificationsList.Count > 0 & !Screensaver.Screensaver.InSaver)
                    {
                        // Update the old notifications list
                        DebugWriter.WriteDebug(DebugLevel.W, "Notifications received! Recents count was {0}, Old count was {1}", NotifRecents.Count, OldNotificationsList.Count);
                        OldNotificationsList = new List<Notification>(NotifRecents);
                        EventsManager.FireEvent(EventType.NotificationsReceived, NewNotificationsList);

                        // Iterate through new notifications. If we're on the booting stage, ensure that the notifications are only queued until the
                        // kernel has finished booting.
                        while (!SplashReport.KernelBooted)
                            Thread.Sleep(100);
                        foreach (Notification NewNotification in NewNotificationsList)
                        {
                            EventsManager.FireEvent(EventType.NotificationReceived, NewNotification);

                            // If do not disturb is enabled, don't show.
                            if (DoNotDisturb)
                                // However, fire event for other notifications
                                continue;

                            // Populate title and description
                            string Title, Desc;
                            DebugWriter.WriteDebug(DebugLevel.I, "Title: {0}", NewNotification.Title);
                            DebugWriter.WriteDebug(DebugLevel.I, "Desc: {0}", NewNotification.Desc);
                            Title = NewNotification.Title.Truncate(36);
                            Desc = NewNotification.Desc.Truncate(36);
                            DebugWriter.WriteDebug(DebugLevel.I, "Truncated title: {0}", Title);
                            DebugWriter.WriteDebug(DebugLevel.I, "Truncated desc: {0}", Desc);
                            DebugWriter.WriteDebug(DebugLevel.I, "Truncated title length: {0}", Title.Length);
                            DebugWriter.WriteDebug(DebugLevel.I, "Truncated desc length: {0}", Desc.Length);

                            // Set the border color
                            DebugWriter.WriteDebug(DebugLevel.I, "Priority: {0}", NewNotification.Priority);
                            var NotifyBorderColor = ColorTools.GetColor(KernelColorType.LowPriorityBorder);
                            var NotifyTitleColor = ColorTools.GetColor(KernelColorType.NotificationTitle);
                            var NotifyDescColor = ColorTools.GetColor(KernelColorType.NotificationDescription);
                            var NotifyProgressColor = ColorTools.GetColor(KernelColorType.NotificationProgress);
                            var NotifyProgressFailureColor = ColorTools.GetColor(KernelColorType.NotificationFailure);
                            switch (NewNotification.Priority)
                            {
                                case NotifPriority.Medium:
                                    {
                                        NotifyBorderColor = ColorTools.GetColor(KernelColorType.MediumPriorityBorder);
                                        break;
                                    }
                                case NotifPriority.High:
                                    {
                                        NotifyBorderColor = ColorTools.GetColor(KernelColorType.HighPriorityBorder);
                                        break;
                                    }
                                case NotifPriority.Custom:
                                    {
                                        NotifyBorderColor = NewNotification.CustomColor;
                                        NotifyTitleColor = NewNotification.CustomTitleColor;
                                        NotifyDescColor = NewNotification.CustomDescriptionColor;
                                        NotifyProgressColor = NewNotification.CustomProgressColor;
                                        NotifyProgressFailureColor = NewNotification.CustomProgressFailureColor;
                                        break;
                                    }
                            }
                            if (NewNotification.NotificationBorderColor is not null)
                            {
                                NotifyBorderColor = NewNotification.NotificationBorderColor;
                            }

                            // Write notification to console
                            DebugWriter.WriteDebug(DebugLevel.I, "Where to store: ({0}, {1}), Title top: {2}, Desc top: {3}", ConsoleBase.ConsoleWrapper.WindowWidth - 40, ConsoleBase.ConsoleWrapper.WindowTop, ConsoleBase.ConsoleWrapper.WindowTop + 1, ConsoleBase.ConsoleWrapper.WindowTop + 2);
                            TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", ConsoleBase.ConsoleWrapper.WindowWidth - 40, ConsoleBase.ConsoleWrapper.WindowTop, true, KernelColorType.NeutralText);
                            TextWriterWhereColor.WriteWhere(Title + Convert.ToString(CharManager.GetEsc()) + "[0K", ConsoleBase.ConsoleWrapper.WindowWidth - 40, ConsoleBase.ConsoleWrapper.WindowTop + 1, true, NotifyTitleColor);
                            TextWriterWhereColor.WriteWhere(Desc + Convert.ToString(CharManager.GetEsc()) + "[0K", ConsoleBase.ConsoleWrapper.WindowWidth - 40, ConsoleBase.ConsoleWrapper.WindowTop + 2, true, NotifyDescColor);
                            TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", ConsoleBase.ConsoleWrapper.WindowWidth - 40, ConsoleBase.ConsoleWrapper.WindowTop + 3, true, KernelColorType.NeutralText);

                            // Optionally, draw a border
                            if (Flags.DrawBorderNotification)
                            {
                                // Prepare the variables
                                string CurrentNotifyUpperLeftCornerChar = NotifyUpperLeftCornerChar;
                                string CurrentNotifyUpperRightCornerChar = NotifyUpperRightCornerChar;
                                string CurrentNotifyLowerLeftCornerChar = NotifyLowerLeftCornerChar;
                                string CurrentNotifyLowerRightCornerChar = NotifyLowerRightCornerChar;
                                string CurrentNotifyUpperFrameChar = NotifyUpperFrameChar;
                                string CurrentNotifyLowerFrameChar = NotifyLowerFrameChar;
                                string CurrentNotifyLeftFrameChar = NotifyLeftFrameChar;
                                string CurrentNotifyRightFrameChar = NotifyRightFrameChar;

                                // Get custom corner characters
                                if (NewNotification.Priority == NotifPriority.Custom)
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
                                TextWriterWhereColor.WriteWhere(CurrentNotifyUpperLeftCornerChar + CurrentNotifyUpperFrameChar.Repeat(38L) + CurrentNotifyUpperRightCornerChar, ConsoleBase.ConsoleWrapper.WindowWidth - 41, ConsoleBase.ConsoleWrapper.WindowTop, true, NotifyBorderColor);
                                TextWriterWhereColor.WriteWhere(CurrentNotifyLeftFrameChar, ConsoleBase.ConsoleWrapper.WindowWidth - 41, ConsoleBase.ConsoleWrapper.WindowTop + 1, true, NotifyBorderColor);
                                TextWriterWhereColor.WriteWhere(CurrentNotifyLeftFrameChar, ConsoleBase.ConsoleWrapper.WindowWidth - 41, ConsoleBase.ConsoleWrapper.WindowTop + 2, true, NotifyBorderColor);
                                TextWriterWhereColor.WriteWhere(CurrentNotifyLeftFrameChar, ConsoleBase.ConsoleWrapper.WindowWidth - 41, ConsoleBase.ConsoleWrapper.WindowTop + 3, true, NotifyBorderColor);
                                TextWriterWhereColor.WriteWhere(CurrentNotifyRightFrameChar, ConsoleBase.ConsoleWrapper.WindowWidth - 2, ConsoleBase.ConsoleWrapper.WindowTop + 1, true, NotifyBorderColor);
                                TextWriterWhereColor.WriteWhere(CurrentNotifyRightFrameChar, ConsoleBase.ConsoleWrapper.WindowWidth - 2, ConsoleBase.ConsoleWrapper.WindowTop + 2, true, NotifyBorderColor);
                                TextWriterWhereColor.WriteWhere(CurrentNotifyRightFrameChar, ConsoleBase.ConsoleWrapper.WindowWidth - 2, ConsoleBase.ConsoleWrapper.WindowTop + 3, true, NotifyBorderColor);
                                TextWriterWhereColor.WriteWhere(CurrentNotifyLowerLeftCornerChar + CurrentNotifyLowerFrameChar.Repeat(38L) + CurrentNotifyLowerRightCornerChar, ConsoleBase.ConsoleWrapper.WindowWidth - 41, ConsoleBase.ConsoleWrapper.WindowTop + 4, true, NotifyBorderColor);
                            }

                            // Beep according to priority
                            int BeepTimes = (int)NewNotification.Priority;
                            if (NewNotification.Priority == NotifPriority.Custom)
                                BeepTimes = NewNotification.CustomBeepTimes;
                            for (int i = 1; i <= BeepTimes; i++)
                                ConsoleBase.ConsoleWrapper.Beep();

                            // Show progress
                            if (NewNotification.Type == NotifType.Progress)
                            {
                                while (!(NewNotification.Progress >= 100 | NewNotification.ProgressFailed))
                                {
                                    string ProgressTitle = Title + " (" + NewNotification.Progress.ToString() + "%)";
                                    DebugWriter.WriteDebug(DebugLevel.I, "Where to store progress: {0},{1}", ConsoleBase.ConsoleWrapper.WindowWidth - 40, ConsoleBase.ConsoleWrapper.WindowTop + 3);
                                    DebugWriter.WriteDebug(DebugLevel.I, "Progress: {0}", NewNotification.Progress);
                                    TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", ConsoleBase.ConsoleWrapper.WindowWidth - 40, ConsoleBase.ConsoleWrapper.WindowTop, true, KernelColorType.NeutralText);
                                    TextWriterWhereColor.WriteWhere(ProgressTitle + Convert.ToString(CharManager.GetEsc()) + "[0K", ConsoleBase.ConsoleWrapper.WindowWidth - 40, ConsoleBase.ConsoleWrapper.WindowTop + 1, true, NotifyTitleColor, NewNotification.Progress);
                                    TextWriterWhereColor.WriteWhere(Desc + Convert.ToString(CharManager.GetEsc()) + "[0K", ConsoleBase.ConsoleWrapper.WindowWidth - 40, ConsoleBase.ConsoleWrapper.WindowTop + 2, true, NotifyDescColor);
                                    ProgressBarColor.WriteProgress(NewNotification.Progress, ConsoleBase.ConsoleWrapper.WindowWidth - 40, ConsoleBase.ConsoleWrapper.WindowTop + 3, 36, 0, NotifyProgressColor, NotifyBorderColor, ColorTools.GetColor(KernelColorType.Background), Flags.DrawBorderNotification, true);
                                    Thread.Sleep(1);
                                    if (NewNotification.ProgressFailed)
                                        TextWriterWhereColor.WriteWhere(ProgressTitle + Convert.ToString(CharManager.GetEsc()) + "[0K", ConsoleBase.ConsoleWrapper.WindowWidth - 40, ConsoleBase.ConsoleWrapper.WindowTop + 1, true, NotifyProgressFailureColor, NewNotification.Progress);
                                }
                            }

                            // Clear the area
                            int TopTitleClear = ConsoleBase.ConsoleWrapper.WindowTop + 1;
                            int TopDescClear = ConsoleBase.ConsoleWrapper.WindowTop + 2;
                            int TopProgClear = ConsoleBase.ConsoleWrapper.WindowTop + 3;
                            int TopOpenBorderClear = ConsoleBase.ConsoleWrapper.WindowTop;
                            int TopCloseBorderClear = ConsoleBase.ConsoleWrapper.WindowTop + 4;
                            Thread.Sleep(5000);
                            NotifClearArea(ConsoleBase.ConsoleWrapper.WindowWidth - (Flags.DrawBorderNotification ? 41 : 40), TopTitleClear, TopDescClear, TopProgClear, TopOpenBorderClear, TopCloseBorderClear);
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
        /// Clears the area of the displayed notification
        /// </summary>
        /// <param name="Width">Console width</param>
        /// <param name="TopTitle">Vertical location of title</param>
        /// <param name="TopDesc">Vertical location of description</param>
        /// <param name="TopProg">Vertical location of progress</param>
        /// <param name="TopOpenBorder">Vertical location of open border</param>
        /// <param name="TopCloseBorder">Vertical location of close border</param>
        private static void NotifClearArea(int Width, int TopTitle, int TopDesc, int TopProg, int TopOpenBorder, int TopCloseBorder)
        {
            if (Flags.DrawBorderNotification)
            {
                TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", ConsoleBase.ConsoleWrapper.WindowWidth - 41, ConsoleBase.ConsoleWrapper.WindowTop, true, KernelColorType.NeutralText);
                TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", Width, TopOpenBorder, true, KernelColorType.NeutralText);
                TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", ConsoleBase.ConsoleWrapper.WindowWidth - 41, ConsoleBase.ConsoleWrapper.WindowTop + 1, true, KernelColorType.NeutralText);
                TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", Width, TopTitle, true, KernelColorType.NeutralText);
                TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", ConsoleBase.ConsoleWrapper.WindowWidth - 41, ConsoleBase.ConsoleWrapper.WindowTop + 2, true, KernelColorType.NeutralText);
                TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", Width, TopDesc, true, KernelColorType.NeutralText);
                TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", ConsoleBase.ConsoleWrapper.WindowWidth - 41, ConsoleBase.ConsoleWrapper.WindowTop + 3, true, KernelColorType.NeutralText);
                TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", Width, TopProg, true, KernelColorType.NeutralText);
                TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", ConsoleBase.ConsoleWrapper.WindowWidth - 41, ConsoleBase.ConsoleWrapper.WindowTop + 4, true, KernelColorType.NeutralText);
                TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", Width, TopCloseBorder, true, KernelColorType.NeutralText);
            }
            else
            {
                TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", ConsoleBase.ConsoleWrapper.WindowWidth - 40, ConsoleBase.ConsoleWrapper.WindowTop + 1, true, KernelColorType.NeutralText);
                TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", Width, TopTitle, true, KernelColorType.NeutralText);
                TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", ConsoleBase.ConsoleWrapper.WindowWidth - 40, ConsoleBase.ConsoleWrapper.WindowTop + 2, true, KernelColorType.NeutralText);
                TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", Width, TopDesc, true, KernelColorType.NeutralText);
                TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", ConsoleBase.ConsoleWrapper.WindowWidth - 40, ConsoleBase.ConsoleWrapper.WindowTop + 3, true, KernelColorType.NeutralText);
                TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", Width, TopProg, true, KernelColorType.NeutralText);
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
        /// Dismisses notification
        /// </summary>
        /// <param name="ind">Index of notification</param>
        public static bool NotifDismiss(int ind)
        {
            try
            {
                NotifRecents.RemoveAt(ind);
                DebugWriter.WriteDebug(DebugLevel.I, "Removed index {0} from notification list", ind);
                EventsManager.FireEvent(EventType.NotificationDismissed);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Error trying to dismiss notification: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return false;
        }

    }
}
