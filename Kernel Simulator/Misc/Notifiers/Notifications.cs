
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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
using KS.Misc.Splash;
using KS.Misc.Text;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;

namespace KS.Misc.Notifications
{
    public static class Notifications
    {

        // Variables
        public static List<Notification> NotifRecents = new();
        public static KernelThread NotifThread = new("Notification Thread", false, NotifListen);
        public static string NotifyUpperLeftCornerChar = "╔";
        public static string NotifyUpperRightCornerChar = "╗";
        public static string NotifyLowerLeftCornerChar = "╚";
        public static string NotifyLowerRightCornerChar = "╝";
        public static string NotifyUpperFrameChar = "═";
        public static string NotifyLowerFrameChar = "═";
        public static string NotifyLeftFrameChar = "║";
        public static string NotifyRightFrameChar = "║";

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
                        DebugWriter.Wdbg(DebugLevel.W, "Notifications received! Recents count was {0}, Old count was {1}", NotifRecents.Count, OldNotificationsList.Count);
                        OldNotificationsList = new List<Notification>(NotifRecents);
                        Kernel.Kernel.KernelEventManager.RaiseNotificationsReceived(NewNotificationsList);

                        // Iterate through new notifications. If we're on the booting stage, ensure that the notifications are only queued until the
                        // kernel has finished booting.
                        while (!SplashReport.KernelBooted)
                            Thread.Sleep(100);
                        foreach (Notification NewNotification in NewNotificationsList)
                        {
                            Kernel.Kernel.KernelEventManager.RaiseNotificationReceived(NewNotification);

                            // Populate title and description
                            string Title, Desc;
                            DebugWriter.Wdbg(DebugLevel.I, "Title: {0}", NewNotification.Title);
                            DebugWriter.Wdbg(DebugLevel.I, "Desc: {0}", NewNotification.Desc);
                            Title = NewNotification.Title.Truncate(36);
                            Desc = NewNotification.Desc.Truncate(36);
                            DebugWriter.Wdbg(DebugLevel.I, "Truncated title: {0}", Title);
                            DebugWriter.Wdbg(DebugLevel.I, "Truncated desc: {0}", Desc);
                            DebugWriter.Wdbg(DebugLevel.I, "Truncated title length: {0}", Title.Length);
                            DebugWriter.Wdbg(DebugLevel.I, "Truncated desc length: {0}", Desc.Length);

                            // Set the border color
                            DebugWriter.Wdbg(DebugLevel.I, "Priority: {0}", NewNotification.Priority);
                            var NotifyBorderColor = ColorTools.LowPriorityBorderColor;
                            var NotifyTitleColor = ColorTools.NotificationTitleColor;
                            var NotifyDescColor = ColorTools.NotificationDescriptionColor;
                            var NotifyProgressColor = ColorTools.NotificationProgressColor;
                            var NotifyProgressFailureColor = ColorTools.NotificationFailureColor;
                            switch (NewNotification.Priority)
                            {
                                case NotifPriority.Medium:
                                    {
                                        NotifyBorderColor = ColorTools.MediumPriorityBorderColor;
                                        break;
                                    }
                                case NotifPriority.High:
                                    {
                                        NotifyBorderColor = ColorTools.HighPriorityBorderColor;
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
                            DebugWriter.Wdbg(DebugLevel.I, "Where to store: ({0}, {1}), Title top: {2}, Desc top: {3}", Console.WindowWidth - 40, Console.WindowTop, Console.WindowTop + 1, Console.WindowTop + 2);
                            TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", Console.WindowWidth - 40, Console.WindowTop, true, ColorTools.ColTypes.Neutral);
                            TextWriterWhereColor.WriteWhere(Title + Convert.ToString(CharManager.GetEsc()) + "[0K", Console.WindowWidth - 40, Console.WindowTop + 1, true, NotifyTitleColor);
                            TextWriterWhereColor.WriteWhere(Desc + Convert.ToString(CharManager.GetEsc()) + "[0K", Console.WindowWidth - 40, Console.WindowTop + 2, true, NotifyDescColor);
                            TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", Console.WindowWidth - 40, Console.WindowTop + 3, true, ColorTools.ColTypes.Neutral);

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
                                TextWriterWhereColor.WriteWhere(CurrentNotifyUpperLeftCornerChar + CurrentNotifyUpperFrameChar.Repeat(38L) + CurrentNotifyUpperRightCornerChar, Console.WindowWidth - 41, Console.WindowTop, true, NotifyBorderColor);
                                TextWriterWhereColor.WriteWhere(CurrentNotifyLeftFrameChar, Console.WindowWidth - 41, Console.WindowTop + 1, true, NotifyBorderColor);
                                TextWriterWhereColor.WriteWhere(CurrentNotifyLeftFrameChar, Console.WindowWidth - 41, Console.WindowTop + 2, true, NotifyBorderColor);
                                TextWriterWhereColor.WriteWhere(CurrentNotifyLeftFrameChar, Console.WindowWidth - 41, Console.WindowTop + 3, true, NotifyBorderColor);
                                TextWriterWhereColor.WriteWhere(CurrentNotifyRightFrameChar, Console.WindowWidth - 2, Console.WindowTop + 1, true, NotifyBorderColor);
                                TextWriterWhereColor.WriteWhere(CurrentNotifyRightFrameChar, Console.WindowWidth - 2, Console.WindowTop + 2, true, NotifyBorderColor);
                                TextWriterWhereColor.WriteWhere(CurrentNotifyRightFrameChar, Console.WindowWidth - 2, Console.WindowTop + 3, true, NotifyBorderColor);
                                TextWriterWhereColor.WriteWhere(CurrentNotifyLowerLeftCornerChar + CurrentNotifyLowerFrameChar.Repeat(38L) + CurrentNotifyLowerRightCornerChar, Console.WindowWidth - 41, Console.WindowTop + 4, true, NotifyBorderColor);
                            }

                            // Beep according to priority
                            int BeepTimes = (int)NewNotification.Priority;
                            if (NewNotification.Priority == NotifPriority.Custom)
                                BeepTimes = NewNotification.CustomBeepTimes;
                            for (int i = 1, loopTo = BeepTimes; i <= loopTo; i++)
                                Console.Beep();

                            // Show progress
                            if (NewNotification.Type == NotifType.Progress)
                            {
                                while (!(NewNotification.Progress >= 100 | NewNotification.ProgressFailed))
                                {
                                    string ProgressTitle = Title + " (" + NewNotification.Progress.ToString() + "%)";
                                    DebugWriter.Wdbg(DebugLevel.I, "Where to store progress: {0},{1}", Console.WindowWidth - 40, Console.WindowTop + 3);
                                    DebugWriter.Wdbg(DebugLevel.I, "Progress: {0}", NewNotification.Progress);
                                    TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", Console.WindowWidth - 40, Console.WindowTop, true, ColorTools.ColTypes.Neutral);
                                    TextWriterWhereColor.WriteWhere(ProgressTitle + Convert.ToString(CharManager.GetEsc()) + "[0K", Console.WindowWidth - 40, Console.WindowTop + 1, true, NotifyTitleColor, NewNotification.Progress);
                                    TextWriterWhereColor.WriteWhere(Desc + Convert.ToString(CharManager.GetEsc()) + "[0K", Console.WindowWidth - 40, Console.WindowTop + 2, true, NotifyDescColor);
                                    TextWriterWhereColor.WriteWhere("*".Repeat((long)Math.Round(NewNotification.Progress * 100 / 100d * (38d / 100d))), Console.WindowWidth - 40, Console.WindowTop + 3, true, NotifyProgressColor);
                                    Thread.Sleep(1);
                                    if (NewNotification.ProgressFailed)
                                        TextWriterWhereColor.WriteWhere(ProgressTitle + Convert.ToString(CharManager.GetEsc()) + "[0K", Console.WindowWidth - 40, Console.WindowTop + 1, true, NotifyProgressFailureColor, NewNotification.Progress);
                                }
                            }

                            // Clear the area
                            int TopTitleClear = Console.WindowTop + 1;
                            int TopDescClear = Console.WindowTop + 2;
                            int TopProgClear = Console.WindowTop + 3;
                            int TopOpenBorderClear = Console.WindowTop;
                            int TopCloseBorderClear = Console.WindowTop + 4;
                            Thread.Sleep(5000);
                            NotifClearArea(Console.WindowWidth - (Flags.DrawBorderNotification ? 41 : 40), TopTitleClear, TopDescClear, TopProgClear, TopOpenBorderClear, TopCloseBorderClear);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Shutting down botification thread because of {0}", ex.Message);
                DebugWriter.WStkTrc(ex);
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
                TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", Console.WindowWidth - 41, Console.WindowTop, true, ColorTools.ColTypes.Neutral);
                TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", Width, TopOpenBorder, true, ColorTools.ColTypes.Neutral);
                TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", Console.WindowWidth - 41, Console.WindowTop + 1, true, ColorTools.ColTypes.Neutral);
                TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", Width, TopTitle, true, ColorTools.ColTypes.Neutral);
                TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", Console.WindowWidth - 41, Console.WindowTop + 2, true, ColorTools.ColTypes.Neutral);
                TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", Width, TopDesc, true, ColorTools.ColTypes.Neutral);
                TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", Console.WindowWidth - 41, Console.WindowTop + 3, true, ColorTools.ColTypes.Neutral);
                TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", Width, TopProg, true, ColorTools.ColTypes.Neutral);
                TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", Console.WindowWidth - 41, Console.WindowTop + 4, true, ColorTools.ColTypes.Neutral);
                TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", Width, TopCloseBorder, true, ColorTools.ColTypes.Neutral);
            }
            else
            {
                TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", Console.WindowWidth - 40, Console.WindowTop + 1, true, ColorTools.ColTypes.Neutral);
                TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", Width, TopTitle, true, ColorTools.ColTypes.Neutral);
                TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", Console.WindowWidth - 40, Console.WindowTop + 2, true, ColorTools.ColTypes.Neutral);
                TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", Width, TopDesc, true, ColorTools.ColTypes.Neutral);
                TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", Console.WindowWidth - 40, Console.WindowTop + 3, true, ColorTools.ColTypes.Neutral);
                TextWriterWhereColor.WriteWhere(Convert.ToString(CharManager.GetEsc()) + "[0K", Width, TopProg, true, ColorTools.ColTypes.Neutral);
            }
        }

        /// <summary>
        /// Sends notification
        /// </summary>
        /// <param name="notif">Instance of notification holder</param>
        public static void NotifySend(Notification notif)
        {
            DebugWriter.Wdbg(DebugLevel.I, "List contains this notification? {0}", NotifRecents.Contains(notif));
            if (!NotifRecents.Contains(notif))
            {
                NotifRecents.Add(notif);
                Kernel.Kernel.KernelEventManager.RaiseNotificationSent(notif);
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
            Kernel.Kernel.KernelEventManager.RaiseNotificationsSent(notifs);
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
                DebugWriter.Wdbg(DebugLevel.I, "Removed index {0} from notification list", ind);
                Kernel.Kernel.KernelEventManager.RaiseNotificationDismissed();
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Error trying to dismiss notification: {0}", ex.Message);
                DebugWriter.WStkTrc(ex);
            }
            return false;
        }

    }
}
