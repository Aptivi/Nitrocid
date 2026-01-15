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

using Newtonsoft.Json;
using Nitrocid.Base.ConsoleBase;
using Nitrocid.Base.Kernel.Threading.Performance;
using Nitrocid.Base.Misc.Notifications;
using Nitrocid.Base.Shell.Shells.Hex;
using Nitrocid.Base.Shell.Shells.Text;
using Terminaux.Inputs;
using Terminaux.Reader;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Textify.Data.Figlet;

namespace Nitrocid.Base.Kernel.Configuration.Instances
{
    /// <summary>
    /// Main kernel configuration instance
    /// </summary>
    public partial class KernelMainConfig : BaseKernelConfig
    {
        [JsonIgnore]
        private string defaultFigletFontName = "speed";

        /// <summary>
        /// Enables eyecandy on startup
        /// </summary>
        public bool StartScroll { get; set; } = true;
        /// <summary>
        /// The time and date will be longer, showing full month names, etc.
        /// </summary>
        public bool LongTimeDate { get; set; } = true;
        /// <summary>
        /// Turns on or off the text editor autosave feature
        /// </summary>
        public bool TextEditAutoSaveFlag { get; set; } = true;
        /// <summary>
        /// If autosave is enabled, the text file will be saved for each "n" seconds
        /// </summary>
        public int TextEditAutoSaveInterval
        {
            get => TextEditShellCommon.autoSaveInterval;
            set => TextEditShellCommon.autoSaveInterval = value < 0 ? 60 : value;
        }
        /// <summary>
        /// Turns on or off the hex editor autosave feature
        /// </summary>
        public bool HexEditAutoSaveFlag { get; set; } = true;
        /// <summary>
        /// If autosave is enabled, the binary file will be saved for each "n" seconds
        /// </summary>
        public int HexEditAutoSaveInterval
        {
            get => HexEditShellCommon.autoSaveInterval;
            set => HexEditShellCommon.autoSaveInterval = value < 0 ? 60 : value;
        }
        /// <summary>
        /// Covers the notification with the border
        /// </summary>
        public bool DrawBorderNotification { get; set; } = true;
        /// <summary>
        /// A character that resembles the upper left corner. Be sure to only input one character
        /// </summary>
        public char NotifyUpperLeftCornerChar
        {
            get => NotificationManager.notifyUpperLeftCornerChar;
            set => NotificationManager.notifyUpperLeftCornerChar = value;
        }
        /// <summary>
        /// A character that resembles the upper right corner. Be sure to only input one character
        /// </summary>
        public char NotifyUpperRightCornerChar
        {
            get => NotificationManager.notifyUpperRightCornerChar;
            set => NotificationManager.notifyUpperRightCornerChar = value;
        }
        /// <summary>
        /// A character that resembles the lower left corner. Be sure to only input one character
        /// </summary>
        public char NotifyLowerLeftCornerChar
        {
            get => NotificationManager.notifyLowerLeftCornerChar;
            set => NotificationManager.notifyLowerLeftCornerChar = value;
        }
        /// <summary>
        /// A character that resembles the lower right corner. Be sure to only input one character
        /// </summary>
        public char NotifyLowerRightCornerChar
        {
            get => NotificationManager.notifyLowerRightCornerChar;
            set => NotificationManager.notifyLowerRightCornerChar = value;
        }
        /// <summary>
        /// A character that resembles the upper frame. Be sure to only input one character
        /// </summary>
        public char NotifyUpperFrameChar
        {
            get => NotificationManager.notifyUpperFrameChar;
            set => NotificationManager.notifyUpperFrameChar = value;
        }
        /// <summary>
        /// A character that resembles the lower frame. Be sure to only input one character
        /// </summary>
        public char NotifyLowerFrameChar
        {
            get => NotificationManager.notifyLowerFrameChar;
            set => NotificationManager.notifyLowerFrameChar = value;
        }
        /// <summary>
        /// A character that resembles the left frame. Be sure to only input one character
        /// </summary>
        public char NotifyLeftFrameChar
        {
            get => NotificationManager.notifyLeftFrameChar;
            set => NotificationManager.notifyLeftFrameChar = value;
        }
        /// <summary>
        /// A character that resembles the right frame. Be sure to only input one character
        /// </summary>
        public char NotifyRightFrameChar
        {
            get => NotificationManager.notifyRightFrameChar;
            set => NotificationManager.notifyRightFrameChar = value;
        }
        /// <summary>
        /// Each login, it will show the latest RSS headline from the selected headline URL
        /// </summary>
        public bool ShowHeadlineOnLogin { get; set; }
        /// <summary>
        /// RSS headline URL to be used when showing the latest headline. This is usually your favorite feed
        /// </summary>
        public string RssHeadlineUrl { get; set; } = "https://www.techrepublic.com/rssfeeds/articles/";
        /// <summary>
        /// Shows the commands count in the command list, controlled by the three count show switches for different kinds of commands.
        /// </summary>
        public bool ShowCommandsCount { get; set; }
        /// <summary>
        /// Show the shell commands count on help
        /// </summary>
        public bool ShowShellCommandsCount { get; set; } = true;
        /// <summary>
        /// Show the aliases count on help
        /// </summary>
        public bool ShowShellAliasesCount { get; set; } = true;
        /// <summary>
        /// Show the unified commands count on help
        /// </summary>
        public bool ShowUnifiedCommandsCount { get; set; } = true;
        /// <summary>
        /// Show the addon commands count on help
        /// </summary>
        public bool ShowAddonCommandsCount { get; set; } = true;
        /// <summary>
        /// A character that masks the password. Leave blank for more security
        /// </summary>
        public string CurrentMask
        {
            get => Input.PasswordMaskChar.ToString();
            set => Input.PasswordMaskChar = string.IsNullOrEmpty(value) ? '*' : value[0];
        }
        /// <summary>
        /// A character that masks the password. Leave blank for more security
        /// </summary>
        public string CurrentMaskReader
        {
            get => TermReader.GlobalReaderSettings.PasswordMaskChar.ToString();
            set => TermReader.GlobalReaderSettings.PasswordMaskChar = string.IsNullOrEmpty(value) ? '*' : value[0];
        }
        /// <summary>
        /// Whether the input history is enabled or not. If enabled, you can access recently typed commands using the up or down arrow keys.
        /// </summary>
        public bool InputHistoryEnabled
        {
            get => TermReader.GlobalReaderSettings.HistoryEnabled;
            set => TermReader.GlobalReaderSettings.HistoryEnabled = value;
        }
        /// <summary>
        /// Enables the scroll bar in selection screens
        /// </summary>
        public bool EnableScrollBarInSelection { get; set; } = true;
        /// <summary>
        /// If Do Not Disturb is enabled, all notifications received will be suppressed from the display. This means that you won't be able to see any notification to help you focus.
        /// </summary>
        public bool DoNotDisturb
        {
            get => NotificationManager.dnd;
            set => NotificationManager.dnd = value;
        }
        /// <summary>
        /// A character that resembles the upper left corner. Be sure to only input one character.
        /// </summary>
        public char BorderUpperLeftCornerChar
        {
            get => BorderSettings.GlobalSettings.BorderUpperLeftCornerChar;
            set => BorderSettings.GlobalSettings.BorderUpperLeftCornerChar = value;
        }
        /// <summary>
        /// A character that resembles the upper right corner. Be sure to only input one character.
        /// </summary>
        public char BorderUpperRightCornerChar
        {
            get => BorderSettings.GlobalSettings.BorderUpperRightCornerChar;
            set => BorderSettings.GlobalSettings.BorderUpperRightCornerChar = value;
        }
        /// <summary>
        /// A character that resembles the lower left corner. Be sure to only input one character.
        /// </summary>
        public char BorderLowerLeftCornerChar
        {
            get => BorderSettings.GlobalSettings.BorderLowerLeftCornerChar;
            set => BorderSettings.GlobalSettings.BorderLowerLeftCornerChar = value;
        }
        /// <summary>
        /// A character that resembles the lower right corner. Be sure to only input one character.
        /// </summary>
        public char BorderLowerRightCornerChar
        {
            get => BorderSettings.GlobalSettings.BorderLowerRightCornerChar;
            set => BorderSettings.GlobalSettings.BorderLowerRightCornerChar = value;
        }
        /// <summary>
        /// A character that resembles the upper frame. Be sure to only input one character.
        /// </summary>
        public char BorderUpperFrameChar
        {
            get => BorderSettings.GlobalSettings.BorderUpperFrameChar;
            set => BorderSettings.GlobalSettings.BorderUpperFrameChar = value;
        }
        /// <summary>
        /// A character that resembles the lower frame. Be sure to only input one character.
        /// </summary>
        public char BorderLowerFrameChar
        {
            get => BorderSettings.GlobalSettings.BorderLowerFrameChar;
            set => BorderSettings.GlobalSettings.BorderLowerFrameChar = value;
        }
        /// <summary>
        /// A character that resembles the left frame. Be sure to only input one character.
        /// </summary>
        public char BorderLeftFrameChar
        {
            get => BorderSettings.GlobalSettings.BorderLeftFrameChar;
            set => BorderSettings.GlobalSettings.BorderLeftFrameChar = value;
        }
        /// <summary>
        /// A character that resembles the right frame. Be sure to only input one character.
        /// </summary>
        public char BorderRightFrameChar
        {
            get => BorderSettings.GlobalSettings.BorderRightFrameChar;
            set => BorderSettings.GlobalSettings.BorderRightFrameChar = value;
        }
        /// <summary>
        /// Censor private information that may be printed to the debug logs.
        /// </summary>
        public bool DebugCensorPrivateInfo { get; set; }
        /// <summary>
        /// Shows all new notifications as asterisks. This option is ignored in notifications with progress bar.
        /// </summary>
        public bool NotifyDisplayAsAsterisk { get; set; }
        /// <summary>
        /// Whether to show the file size in the status
        /// </summary>
        public bool IfmShowFileSize { get; set; }
        /// <summary>
        /// If enabled, uses the classic header style in the settings app. Otherwise, the new one.
        /// </summary>
        public bool ClassicSettingsHeaderStyle { get; set; }
        /// <summary>
        /// Specifies the default figlet font name
        /// </summary>
        public string DefaultFigletFontName
        {
            get => defaultFigletFontName;
            set => defaultFigletFontName = FigletTools.GetFigletFonts().ContainsKey(value) ? value : "speed";
        }
        /// <summary>
        /// Whether to update the CPU usage or not
        /// </summary>
        public bool CpuUsageDebugEnabled
        {
            get => CpuUsageDebug.usageUpdateEnabled;
            set
            {
                CpuUsageDebug.usageUpdateEnabled = value;
                CpuUsageDebug.RunCpuUsageDebugger();
            }
        }
        /// <summary>
        /// The interval in which the CPU usage is printed
        /// </summary>
        public int CpuUsageUpdateInterval
        {
            get => CpuUsageDebug.usageIntervalUpdatePeriod;
            set => CpuUsageDebug.usageIntervalUpdatePeriod = value >= 1000 ? value : 1000;
        }
        /// <summary>
        /// Whether to initialize the mouse support for the kernel or not, essentially enabling all mods to handle the mouse pointer
        /// </summary>
        public bool InitializeCursorHandler
        {
            get => ConsolePointerHandler.enableHandler;
            set
            {
                if (value)
                {
                    ConsolePointerHandler.enableHandler = true;
                    ConsolePointerHandler.StartHandler();
                }
                else
                {
                    ConsolePointerHandler.StopHandler();
                    ConsolePointerHandler.enableHandler = false;
                }
            }
        }
        /// <summary>
        /// Whether to also enable the movement events or not, improving the user experience of some interactive applications
        /// </summary>
        public bool HandleCursorMovement
        {
            get => Input.EnableMovementEvents;
            set => Input.EnableMovementEvents = value;
        }
    }
}
