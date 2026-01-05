//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Colors.Transformation;
using Nitrocid.Base.Files;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Base.Structures;
using Terminaux.Writer.CyclicWriters.Simple;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Splash;
using Nitrocid.Base.Kernel.Threading;
using Nitrocid.Base.Misc.Audio;
using Nitrocid.Base.Misc.Screensaver;
using Nitrocid.Base.Files.Paths;
using Nitrocid.Base.Kernel.Events;
using Nitrocid.Base.Kernel.Power;
using Terminaux.Base.Buffered;

namespace Nitrocid.Base.Misc.Notifications
{
    /// <summary>
    /// Notifications module
    /// </summary>
    public static class NotificationManager
    {

        internal static char notifyUpperLeftCornerChar = '╭';
        internal static char notifyUpperRightCornerChar = '╮';
        internal static char notifyLowerLeftCornerChar = '╰';
        internal static char notifyLowerRightCornerChar = '╯';
        internal static char notifyUpperFrameChar = '─';
        internal static char notifyLowerFrameChar = '─';
        internal static char notifyLeftFrameChar = '│';
        internal static char notifyRightFrameChar = '│';
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
                    SpinWait.SpinUntil(() => sent || dismissing || PowerManager.KernelShutdown);
                    if (PowerManager.KernelShutdown)
                        continue;
                    if (dismissing)
                    {
                        dismissing = false;
                        OldNotificationsList = [.. NotifRecents];
                        continue;
                    }
                    lock (NotifRecents)
                    {
                        NewNotificationsList = NotifRecents.Except(OldNotificationsList).ToList();
                    }
                    if (NewNotificationsList.Count > 0)
                    {
                        // Update the old notifications list
                        DebugWriter.WriteDebug(DebugLevel.W, "Notifications received! Recents count was {0}, Old count was {1}", vars: [NotifRecents.Count, OldNotificationsList.Count]);
                        OldNotificationsList = [.. NotifRecents];
                        sent = false;
                        EventsManager.FireEvent(EventType.NotificationsReceived, NewNotificationsList);

                        // Iterate through new notifications. If we're on the booting stage, ensure that the notifications are only queued until the
                        // kernel has finished booting.
                        while (!SplashReport.KernelBooted)
                            SpinWait.SpinUntil(() => SplashReport.KernelBooted);
                        foreach (Notification NewNotification in NewNotificationsList)
                        {
                            EventsManager.FireEvent(EventType.NotificationReceived, NewNotification);

                            // If do not disturb is enabled, don't show. However, fire event for other notifications
                            if (Config.MainConfig.DoNotDisturb)
                                continue;

                            // Make a dynamic global overlay and set it
                            var notificationOverlay = new ScreenPart();

                            // Only show the notification if we're on the GUI mode
                            if (ScreenTools.IsOnScreen)
                            {
                                notificationOverlay.AddDynamicText(() =>
                                {
                                    // Select how to display the notification
                                    bool useSimplified = Config.MainConfig.NotifyDisplayAsAsterisk && NewNotification.Type == NotificationType.Normal;

                                    // Populate title and description
                                    string Title, Desc;
                                    DebugWriter.WriteDebug(DebugLevel.I, "Title: {0}", vars: [NewNotification.Title]);
                                    DebugWriter.WriteDebug(DebugLevel.I, "Desc: {0}", vars: [NewNotification.Desc]);
                                    Title = useSimplified ? "*" : NewNotification.Title.Truncate(38);
                                    Desc = useSimplified ? "" : NewNotification.Desc.Truncate(38);
                                    DebugWriter.WriteDebug(DebugLevel.I, "Truncated title: {0}", vars: [Title]);
                                    DebugWriter.WriteDebug(DebugLevel.I, "Truncated desc: {0}", vars: [Desc]);
                                    DebugWriter.WriteDebug(DebugLevel.I, "Truncated title length: {0}", vars: [Title.Length]);
                                    DebugWriter.WriteDebug(DebugLevel.I, "Truncated desc length: {0}", vars: [Desc.Length]);

                                    // Set the border color
                                    DebugWriter.WriteDebug(DebugLevel.I, "Priority: {0}", vars: [NewNotification.Priority]);
                                    var NotifyBorderColor = ThemeColorsTools.GetColor("LowPriorityBorderColor");
                                    var NotifyTitleColor = ThemeColorsTools.GetColor("NotificationTitleColor");
                                    var NotifyDescColor = ThemeColorsTools.GetColor("NotificationDescriptionColor");
                                    var NotifyProgressColor = ThemeColorsTools.GetColor("NotificationProgressColor");
                                    var NotifyProgressFailureColor = ThemeColorsTools.GetColor("NotificationFailureColor");
                                    var NotifyProgressSuccessColor = ThemeColorsTools.GetColor(ThemeColorType.Success);
                                    switch (NewNotification.Priority)
                                    {
                                        case NotificationPriority.Medium:
                                            NotifyBorderColor = ThemeColorsTools.GetColor("MediumPriorityBorderColor");
                                            break;
                                        case NotificationPriority.High:
                                            NotifyBorderColor = ThemeColorsTools.GetColor("HighPriorityBorderColor");
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
                                    int notifTopAgnostic = 0;
                                    int notifLeft = useSimplified ? ConsoleWrapper.WindowWidth - 1 : notifLeftAgnostic + 3;
                                    int notifTop = useSimplified ? 0 : notifTopAgnostic;
                                    int notifTitleTop = notifTopAgnostic + 1;
                                    int notifDescTop = notifTopAgnostic + 2;
                                    int notifTipTop = notifTopAgnostic + 3;
                                    int notifWipeTop = notifTopAgnostic + 4;
                                    int notifWidth = ConsoleWrapper.WindowWidth - 1 - notifLeft;

                                    // Make a string builder for our buffer
                                    var printBuffer = new StringBuilder();
                                    var textColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
                                    var background = ThemeColorsTools.GetColor(ThemeColorType.Background);

                                    // Optionally, draw a border
                                    if (Config.MainConfig.DrawBorderNotification && !useSimplified)
                                    {
                                        // Prepare the variables
                                        char CurrentNotifyUpperLeftCornerChar = Config.MainConfig.NotifyUpperLeftCornerChar;
                                        char CurrentNotifyUpperRightCornerChar = Config.MainConfig.NotifyUpperRightCornerChar;
                                        char CurrentNotifyLowerLeftCornerChar = Config.MainConfig.NotifyLowerLeftCornerChar;
                                        char CurrentNotifyLowerRightCornerChar = Config.MainConfig.NotifyLowerRightCornerChar;
                                        char CurrentNotifyUpperFrameChar = Config.MainConfig.NotifyUpperFrameChar;
                                        char CurrentNotifyLowerFrameChar = Config.MainConfig.NotifyLowerFrameChar;
                                        char CurrentNotifyLeftFrameChar = Config.MainConfig.NotifyLeftFrameChar;
                                        char CurrentNotifyRightFrameChar = Config.MainConfig.NotifyRightFrameChar;

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
                                        var borderSettings = new BorderSettings()
                                        {
                                            BorderUpperLeftCornerChar = CurrentNotifyUpperLeftCornerChar,
                                            BorderLowerLeftCornerChar = CurrentNotifyLowerLeftCornerChar,
                                            BorderUpperRightCornerChar = CurrentNotifyUpperRightCornerChar,
                                            BorderLowerRightCornerChar = CurrentNotifyLowerRightCornerChar,
                                            BorderUpperFrameChar = CurrentNotifyUpperFrameChar,
                                            BorderLowerFrameChar = CurrentNotifyLowerFrameChar,
                                            BorderLeftFrameChar = CurrentNotifyLeftFrameChar,
                                            BorderRightFrameChar = CurrentNotifyRightFrameChar,
                                        };
                                        var border = new Border()
                                        {
                                            Left = notifLeft - 1,
                                            Top = notifTopAgnostic,
                                            Width = notifWidth,
                                            Height = 3,
                                            Color = NotifyBorderColor,
                                            BackgroundColor = background,
                                            Settings = borderSettings
                                        };
                                        printBuffer.Append(border.Render());
                                    }

                                    // Write notification to console
                                    if (useSimplified)
                                    {
                                        // Simplified way
                                        DebugWriter.WriteDebug(DebugLevel.I, "Where to store: ({0}, {1})", vars: [notifLeft, notifTop]);
                                        printBuffer.Append(TextWriterWhereColor.RenderWhereColorBack(Title, notifLeft, notifTop, NotifyBorderColor, background));
                                    }
                                    else
                                    {
                                        // Normal way
                                        DebugWriter.WriteDebug(DebugLevel.I, "Where to store: ({0}, {1}), Title top: {2}, Desc top: {3}, Wipe top: {4}, Tip top: {5}", vars: [notifLeft, notifTop, notifTitleTop, notifDescTop, notifWipeTop, notifTipTop]);
                                        printBuffer.Append(TextWriterWhereColor.RenderWhereColorBack(Title + new string(' ', notifWidth - ConsoleChar.EstimateCellWidth(Title)), notifLeft, notifTitleTop, NotifyTitleColor, background));
                                        printBuffer.Append(TextWriterWhereColor.RenderWhereColorBack(Desc + new string(' ', notifWidth - ConsoleChar.EstimateCellWidth(Desc)), notifLeft, notifDescTop, NotifyDescColor, background));
                                    }

                                    // Show progress
                                    if (NewNotification.Type == NotificationType.Progress)
                                    {
                                        // Some variables
                                        bool indeterminate = NewNotification.ProgressIndeterminate;
                                        string renderedProgressTitle = Title.Truncate(36);
                                        string renderedProgressTitleSuccess = $"{Title} ({LanguageTools.GetLocalized("NKS_MISC_NOTIFICATIONS_PROGSUCCESS")})".Truncate(36);
                                        string renderedProgressTitleFailure = $"{Title} ({LanguageTools.GetLocalized("NKS_MISC_NOTIFICATIONS_PROGFAILURE")})".Truncate(36);

                                        // Loop until the progress is finished
                                        var progress = new SimpleProgress(NewNotification.Progress, 100)
                                        {
                                            Indeterminate = indeterminate,
                                            Width = 38,
                                            ProgressActiveForegroundColor = NotifyProgressColor,
                                            ProgressForegroundColor = TransformationTools.GetDarkBackground(NotifyProgressColor),
                                        };

                                        // Now, check to see if the progress failed or succeeded, or if still progressing
                                        if (NewNotification.ProgressState == NotificationProgressState.Failure)
                                            printBuffer.Append(TextWriterWhereColor.RenderWhereColorBack(renderedProgressTitleFailure, notifLeft, notifTitleTop, NotifyProgressFailureColor, background));
                                        else if (NewNotification.ProgressState == NotificationProgressState.Success)
                                            printBuffer.Append(TextWriterWhereColor.RenderWhereColorBack(renderedProgressTitleSuccess, notifLeft, notifTitleTop, NotifyProgressSuccessColor, background));
                                        else
                                        {
                                            // Change the title according to the current progress percentage
                                            DebugWriter.WriteDebug(DebugLevel.I, "Where to store progress: {0},{1}", vars: [notifLeft, notifWipeTop]);
                                            DebugWriter.WriteDebug(DebugLevel.I, "Progress: {0}", vars: [NewNotification.Progress]);

                                            // Write the title, the description, and the progress
                                            printBuffer.Append(TextWriterWhereColor.RenderWhereColorBack(Title + new string(' ', notifWidth - ConsoleChar.EstimateCellWidth(Title)), notifLeft, notifTitleTop, NotifyTitleColor, background));
                                            printBuffer.Append(TextWriterWhereColor.RenderWhereColorBack(Desc + new string(' ', notifWidth - ConsoleChar.EstimateCellWidth(Desc)), notifLeft, notifDescTop, NotifyDescColor, background));

                                            // For indeterminate progresses, flash the box inside the progress bar
                                            progress.Position = NewNotification.Progress;
                                            printBuffer.Append(RendererTools.RenderRenderable(progress, new Coordinate(notifLeft, notifTipTop)));
                                        }
                                    }

                                    // Return our buffer
                                    return printBuffer.ToString();
                                });
                            }

                            // Beep according to priority
                            int BeepTimes = (int)NewNotification.Priority;
                            if (NewNotification.Priority == NotificationPriority.Custom)
                                BeepTimes = NewNotification.CustomBeepTimes;
                            for (int i = 1; i <= BeepTimes; i++)
                                ConsoleWrapper.Beep();

                            // Play audio cues if enabled
                            switch (NewNotification.Priority)
                            {
                                case NotificationPriority.Low:
                                    AudioCuesTools.PlayAudioCue(AudioCueType.NotificationLow);
                                    break;
                                case NotificationPriority.Medium:
                                    AudioCuesTools.PlayAudioCue(AudioCueType.NotificationMedium);
                                    break;
                                case NotificationPriority.High:
                                    AudioCuesTools.PlayAudioCue(AudioCueType.NotificationHigh);
                                    break;
                            }

                            // Render the screen, if it exists
                            Screen.GlobalOverlayPart = notificationOverlay;
                            if (ScreenTools.IsOnScreen && !ScreensaverManager.InSaver)
                            {
                                if (NewNotification.Type != NotificationType.Progress)
                                {
                                    if (ScreenTools.IsOnScreen)
                                        ScreenTools.Render();
                                }
                                else
                                {
                                    while (NewNotification.ProgressState == NotificationProgressState.Progressing)
                                    {
                                        if (ScreenTools.IsOnScreen)
                                            ScreenTools.Render();
                                        Thread.Sleep(NewNotification.ProgressIndeterminate ? 250 : 1);
                                    }
                                }
                            }
                            else
                            {
                                // Provide visual feedback
                                ConsoleMisc.Flash();
                            }

                            // Wait 5 seconds and reset overlay
                            SpinWait.SpinUntil(() => sent, 5000);
                            Screen.GlobalOverlayPart = null;
                            if (ScreenTools.IsOnScreen && !ScreensaverManager.InSaver)
                                ScreenTools.Render();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Shutting down notification thread because of {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }

        /// <summary>
        /// Sends notification
        /// </summary>
        /// <param name="notif">Instance of notification holder</param>
        public static void NotifySend(Notification notif)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "List contains this notification? {0}", vars: [NotifRecents.Contains(notif)]);
            if (!NotifRecents.Contains(notif))
            {
                lock (NotifRecents)
                {
                    NotifRecents.Add(notif);
                    sent = true;
                    EventsManager.FireEvent(EventType.NotificationSent, notif);
                }
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
                lock (NotifRecents)
                {
                    NotifRecents.RemoveAt(ind);
                    DebugWriter.WriteDebug(DebugLevel.I, "Removed index {0} from notification list", vars: [ind]);
                    EventsManager.FireEvent(EventType.NotificationDismissed);
                    dismissing = true;
                }
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Error trying to dismiss notification: {0}", vars: [ex.Message]);
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
                FilesystemTools.GetNumberedFileName(
                    Path.GetDirectoryName(PathsManagement.GetKernelPath(KernelPathType.NotificationRecents)),
                    PathsManagement.GetKernelPath(KernelPathType.NotificationRecents)
                );
            string serialized = JsonConvert.SerializeObject(NotifRecents, Formatting.Indented);
            FilesystemTools.WriteContentsText(recentsPath, serialized);
        }

    }
}
