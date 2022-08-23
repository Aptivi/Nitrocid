
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
using ColorSeq;
using Extensification.StringExts;
using KS.ConsoleBase.Themes;
using KS.Kernel;
using KS.Kernel.Configuration;
using KS.Languages;
using KS.Misc.Platform;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;

namespace KS.ConsoleBase.Colors
{
    public static class ColorTools
    {

        /// <summary>
        /// Enumeration for color types
        /// </summary>
        public enum ColTypes : int
        {
            /// <summary>
            /// Neutral text (for general purposes)
            /// </summary>
            Neutral,
            /// <summary>
            /// Input text
            /// </summary>
            Input,
            /// <summary>
            /// Continuable kernel panic text (usually sync'd with Warning)
            /// </summary>
            Continuable,
            /// <summary>
            /// Uncontinuable kernel panic text (usually sync'd with Error)
            /// </summary>
            Uncontinuable,
            /// <summary>
            /// Host name color
            /// </summary>
            HostName,
            /// <summary>
            /// User name color
            /// </summary>
            UserName,
            /// <summary>
            /// License color
            /// </summary>
            License,
            /// <summary>
            /// Gray color (for special purposes)
            /// </summary>
            Gray,
            /// <summary>
            /// List value text
            /// </summary>
            ListValue,
            /// <summary>
            /// List entry text
            /// </summary>
            ListEntry,
            /// <summary>
            /// Stage text
            /// </summary>
            Stage,
            /// <summary>
            /// Error text
            /// </summary>
            Error,
            /// <summary>
            /// Warning text
            /// </summary>
            Warning,
            /// <summary>
            /// Option text
            /// </summary>
            Option,
            /// <summary>
            /// Banner text
            /// </summary>
            Banner,
            /// <summary>
            /// Notification title text
            /// </summary>
            NotificationTitle,
            /// <summary>
            /// Notification description text
            /// </summary>
            NotificationDescription,
            /// <summary>
            /// Notification progress text
            /// </summary>
            NotificationProgress,
            /// <summary>
            /// Notification failure text
            /// </summary>
            NotificationFailure,
            /// <summary>
            /// Question text
            /// </summary>
            Question,
            /// <summary>
            /// Success text
            /// </summary>
            Success,
            /// <summary>
            /// User dollar sign on shell text
            /// </summary>
            UserDollarSign,
            /// <summary>
            /// Tip text
            /// </summary>
            Tip,
            /// <summary>
            /// Separator text
            /// </summary>
            SeparatorText,
            /// <summary>
            /// Separator color
            /// </summary>
            Separator,
            /// <summary>
            /// List title text
            /// </summary>
            ListTitle,
            /// <summary>
            /// Development warning text
            /// </summary>
            DevelopmentWarning,
            /// <summary>
            /// Stage time text
            /// </summary>
            StageTime,
            /// <summary>
            /// General progress text
            /// </summary>
            Progress,
            /// <summary>
            /// Back option text
            /// </summary>
            BackOption,
            /// <summary>
            /// Low priority notification border color
            /// </summary>
            LowPriorityBorder,
            /// <summary>
            /// Medium priority notification border color
            /// </summary>
            MediumPriorityBorder,
            /// <summary>
            /// High priority notification border color
            /// </summary>
            HighPriorityBorder,
            /// <summary>
            /// Table separator
            /// </summary>
            TableSeparator,
            /// <summary>
            /// Table header
            /// </summary>
            TableHeader,
            /// <summary>
            /// Table value
            /// </summary>
            TableValue,
            /// <summary>
            /// Selected option
            /// </summary>
            SelectedOption,
            /// <summary>
            /// Alternative option
            /// </summary>
            AlternativeOption
        }

        // Variables for colors used by previous versions of the kernel.
        public static Color InputColor = new Color((int)ConsoleColors.White);
        public static Color LicenseColor = new Color((int)ConsoleColors.White);
        public static Color ContKernelErrorColor = new Color((int)ConsoleColors.Yellow);
        public static Color UncontKernelErrorColor = new Color((int)ConsoleColors.Red);
        public static Color HostNameShellColor = new Color((int)ConsoleColors.DarkGreen);
        public static Color UserNameShellColor = new Color((int)ConsoleColors.Green);
        public static Color BackgroundColor = new Color((int)ConsoleColors.Black);
        public static Color NeutralTextColor = new Color((int)ConsoleColors.Gray);
        public static Color ListEntryColor = new Color((int)ConsoleColors.DarkYellow);
        public static Color ListValueColor = new Color((int)ConsoleColors.DarkGray);
        public static Color StageColor = new Color((int)ConsoleColors.Green);
        public static Color ErrorColor = new Color((int)ConsoleColors.Red);
        public static Color WarningColor = new Color((int)ConsoleColors.Yellow);
        public static Color OptionColor = new Color((int)ConsoleColors.DarkYellow);
        public static Color BannerColor = new Color((int)ConsoleColors.Green);
        public static Color NotificationTitleColor = new Color((int)ConsoleColors.White);
        public static Color NotificationDescriptionColor = new Color((int)ConsoleColors.Gray);
        public static Color NotificationProgressColor = new Color((int)ConsoleColors.DarkYellow);
        public static Color NotificationFailureColor = new Color((int)ConsoleColors.Red);
        public static Color QuestionColor = new Color((int)ConsoleColors.Yellow);
        public static Color SuccessColor = new Color((int)ConsoleColors.Green);
        public static Color UserDollarColor = new Color((int)ConsoleColors.Gray);
        public static Color TipColor = new Color((int)ConsoleColors.Gray);
        public static Color SeparatorTextColor = new Color((int)ConsoleColors.White);
        public static Color SeparatorColor = new Color((int)ConsoleColors.Gray);
        public static Color ListTitleColor = new Color((int)ConsoleColors.White);
        public static Color DevelopmentWarningColor = new Color((int)ConsoleColors.Yellow);
        public static Color StageTimeColor = new Color((int)ConsoleColors.Gray);
        public static Color ProgressColor = new Color((int)ConsoleColors.DarkYellow);
        public static Color BackOptionColor = new Color((int)ConsoleColors.DarkRed);
        public static Color LowPriorityBorderColor = new Color((int)ConsoleColors.White);
        public static Color MediumPriorityBorderColor = new Color((int)ConsoleColors.Yellow);
        public static Color HighPriorityBorderColor = new Color((int)ConsoleColors.Red);
        public static Color TableSeparatorColor = new Color((int)ConsoleColors.DarkGray);
        public static Color TableHeaderColor = new Color((int)ConsoleColors.White);
        public static Color TableValueColor = new Color((int)ConsoleColors.Gray);
        public static Color SelectedOptionColor = new Color((int)ConsoleColors.Yellow);
        public static Color AlternativeOptionColor = new Color((int)ConsoleColors.DarkGreen);

        /// <summary>
        /// Resets all colors to default
        /// </summary>
        public static void ResetColors()
        {
            DebugWriter.Wdbg(DebugLevel.I, "Resetting colors");
            var DefInfo = new ThemeInfo("_Default");
            InputColor = DefInfo.ThemeInputColor;
            LicenseColor = DefInfo.ThemeLicenseColor;
            ContKernelErrorColor = DefInfo.ThemeContKernelErrorColor;
            UncontKernelErrorColor = DefInfo.ThemeUncontKernelErrorColor;
            HostNameShellColor = DefInfo.ThemeHostNameShellColor;
            UserNameShellColor = DefInfo.ThemeUserNameShellColor;
            BackgroundColor = DefInfo.ThemeBackgroundColor;
            NeutralTextColor = DefInfo.ThemeNeutralTextColor;
            ListEntryColor = DefInfo.ThemeListEntryColor;
            ListValueColor = DefInfo.ThemeListValueColor;
            StageColor = DefInfo.ThemeStageColor;
            ErrorColor = DefInfo.ThemeErrorColor;
            WarningColor = DefInfo.ThemeWarningColor;
            OptionColor = DefInfo.ThemeOptionColor;
            BannerColor = DefInfo.ThemeBannerColor;
            NotificationTitleColor = DefInfo.ThemeNotificationTitleColor;
            NotificationDescriptionColor = DefInfo.ThemeNotificationDescriptionColor;
            NotificationProgressColor = DefInfo.ThemeNotificationProgressColor;
            NotificationFailureColor = DefInfo.ThemeNotificationFailureColor;
            QuestionColor = DefInfo.ThemeQuestionColor;
            SuccessColor = DefInfo.ThemeSuccessColor;
            UserDollarColor = DefInfo.ThemeUserDollarColor;
            TipColor = DefInfo.ThemeTipColor;
            SeparatorTextColor = DefInfo.ThemeSeparatorTextColor;
            SeparatorColor = DefInfo.ThemeSeparatorColor;
            ListTitleColor = DefInfo.ThemeListTitleColor;
            DevelopmentWarningColor = DefInfo.ThemeDevelopmentWarningColor;
            StageTimeColor = DefInfo.ThemeStageTimeColor;
            ProgressColor = DefInfo.ThemeProgressColor;
            BackOptionColor = DefInfo.ThemeBackOptionColor;
            LowPriorityBorderColor = DefInfo.ThemeLowPriorityBorderColor;
            MediumPriorityBorderColor = DefInfo.ThemeMediumPriorityBorderColor;
            HighPriorityBorderColor = DefInfo.ThemeHighPriorityBorderColor;
            TableSeparatorColor = DefInfo.ThemeTableSeparatorColor;
            TableHeaderColor = DefInfo.ThemeTableHeaderColor;
            TableValueColor = DefInfo.ThemeTableValueColor;
            SelectedOptionColor = DefInfo.ThemeSelectedOptionColor;
            AlternativeOptionColor = DefInfo.ThemeAlternativeOptionColor;
            LoadBack();

            // Raise event
            Kernel.Kernel.KernelEventManager.RaiseColorReset();
        }

        /// <summary>
        /// Loads the background
        /// </summary>
        public static void LoadBack()
        {
            try
            {
                DebugWriter.Wdbg(DebugLevel.I, "Filling background with background color");
                SetConsoleColor(BackgroundColor, true);
                Console.Clear();
            }
            catch (Exception ex)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Failed to set background: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Sets custom colors. It only works if colored shell is enabled.
        /// </summary>
        /// <param name="InputColor">Input color</param>
        /// <param name="LicenseColor">License color</param>
        /// <param name="ContKernelErrorColor">Continuable kernel error color</param>
        /// <param name="UncontKernelErrorColor">Uncontinuable kernel error color</param>
        /// <param name="HostNameShellColor">Host name color</param>
        /// <param name="UserNameShellColor">User name color</param>
        /// <param name="BackgroundColor">Background color</param>
        /// <param name="NeutralTextColor">Neutral text color</param>
        /// <param name="ListEntryColor">Command list color</param>
        /// <param name="ListValueColor">Command definition color</param>
        /// <param name="StageColor">Stage color</param>
        /// <param name="ErrorColor">Error color</param>
        /// <param name="WarningColor">Warning color</param>
        /// <param name="OptionColor">Option color</param>
        /// <param name="BannerColor">Banner color</param>
        /// <param name="NotificationTitleColor">Notification title color</param>
        /// <param name="NotificationDescriptionColor">Notification description color</param>
        /// <param name="NotificationProgressColor">Notification progress color</param>
        /// <param name="NotificationFailureColor">Notification failure color</param>
        /// <param name="QuestionColor">Question color</param>
        /// <param name="SuccessColor">Success text color</param>
        /// <param name="UserDollarColor">User dollar color</param>
        /// <param name="TipColor">Tip color</param>
        /// <param name="SeparatorTextColor">Separator text color</param>
        /// <param name="SeparatorColor">Separator color</param>
        /// <param name="ListTitleColor">List title color</param>
        /// <param name="DevelopmentWarningColor">Development warning color</param>
        /// <param name="StageTimeColor">Stage time color</param>
        /// <param name="ProgressColor">Progress color</param>
        /// <param name="BackOptionColor">Back option color</param>
        /// <param name="LowPriorityBorderColor">Low priority notification border color</param>
        /// <param name="MediumPriorityBorderColor">Medium priority notification border color</param>
        /// <param name="HighPriorityBorderColor">High priority notification border color</param>
        /// <param name="TableSeparatorColor">Table separator color</param>
        /// <param name="TableHeaderColor">Table header color</param>
        /// <param name="TableValueColor">Table value color</param>
        /// <param name="SelectedOptionColor">Selected option color</param>
        /// <param name="AlternativeOptionColor">Alternative option color</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TrySetColors(string InputColor, string LicenseColor, string ContKernelErrorColor, string UncontKernelErrorColor, string HostNameShellColor, string UserNameShellColor, string BackgroundColor, string NeutralTextColor, string ListEntryColor, string ListValueColor, string StageColor, string ErrorColor, string WarningColor, string OptionColor, string BannerColor, string NotificationTitleColor, string NotificationDescriptionColor, string NotificationProgressColor, string NotificationFailureColor, string QuestionColor, string SuccessColor, string UserDollarColor, string TipColor, string SeparatorTextColor, string SeparatorColor, string ListTitleColor, string DevelopmentWarningColor, string StageTimeColor, string ProgressColor, string BackOptionColor, string LowPriorityBorderColor, string MediumPriorityBorderColor, string HighPriorityBorderColor, string TableSeparatorColor, string TableHeaderColor, string TableValueColor, string SelectedOptionColor, string AlternativeOptionColor)
        {
            try
            {
                SetColors(InputColor, LicenseColor, ContKernelErrorColor, UncontKernelErrorColor, HostNameShellColor, UserNameShellColor, BackgroundColor, NeutralTextColor, ListEntryColor, ListValueColor, StageColor, ErrorColor, WarningColor, OptionColor, BannerColor, NotificationTitleColor, NotificationDescriptionColor, NotificationProgressColor, NotificationFailureColor, QuestionColor, SuccessColor, UserDollarColor, TipColor, SeparatorTextColor, SeparatorColor, ListTitleColor, DevelopmentWarningColor, StageTimeColor, ProgressColor, BackOptionColor, LowPriorityBorderColor, MediumPriorityBorderColor, HighPriorityBorderColor, TableSeparatorColor, TableHeaderColor, TableValueColor, SelectedOptionColor, AlternativeOptionColor);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Sets custom colors. It only works if colored shell is enabled.
        /// </summary>
        /// <param name="InputColor">Input color</param>
        /// <param name="LicenseColor">License color</param>
        /// <param name="ContKernelErrorColor">Continuable kernel error color</param>
        /// <param name="UncontKernelErrorColor">Uncontinuable kernel error color</param>
        /// <param name="HostNameShellColor">Host name color</param>
        /// <param name="UserNameShellColor">User name color</param>
        /// <param name="BackgroundColor">Background color</param>
        /// <param name="NeutralTextColor">Neutral text color</param>
        /// <param name="ListEntryColor">Command list color</param>
        /// <param name="ListValueColor">Command definition color</param>
        /// <param name="StageColor">Stage color</param>
        /// <param name="ErrorColor">Error color</param>
        /// <param name="WarningColor">Warning color</param>
        /// <param name="OptionColor">Option color</param>
        /// <param name="BannerColor">Banner color</param>
        /// <param name="NotificationTitleColor">Notification title color</param>
        /// <param name="NotificationDescriptionColor">Notification description color</param>
        /// <param name="NotificationProgressColor">Notification progress color</param>
        /// <param name="NotificationFailureColor">Notification failure color</param>
        /// <param name="QuestionColor">Question color</param>
        /// <param name="SuccessColor">Success text color</param>
        /// <param name="UserDollarColor">User dollar color</param>
        /// <param name="TipColor">Tip color</param>
        /// <param name="SeparatorTextColor">Separator text color</param>
        /// <param name="SeparatorColor">Separator color</param>
        /// <param name="ListTitleColor">List title color</param>
        /// <param name="DevelopmentWarningColor">Development warning color</param>
        /// <param name="StageTimeColor">Stage time color</param>
        /// <param name="ProgressColor">Progress color</param>
        /// <param name="BackOptionColor">Back option color</param>
        /// <param name="LowPriorityBorderColor">Low priority notification border color</param>
        /// <param name="MediumPriorityBorderColor">Medium priority notification border color</param>
        /// <param name="HighPriorityBorderColor">High priority notification border color</param>
        /// <param name="TableSeparatorColor">Table separator color</param>
        /// <param name="TableHeaderColor">Table header color</param>
        /// <param name="TableValueColor">Table value color</param>
        /// <param name="SelectedOptionColor">Selected option color</param>
        /// <param name="AlternativeOptionColor">Alternative option color</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="Exceptions.ColorException"></exception>
        public static void SetColors(string InputColor, string LicenseColor, string ContKernelErrorColor, string UncontKernelErrorColor, string HostNameShellColor, string UserNameShellColor, string BackgroundColor, string NeutralTextColor, string ListEntryColor, string ListValueColor, string StageColor, string ErrorColor, string WarningColor, string OptionColor, string BannerColor, string NotificationTitleColor, string NotificationDescriptionColor, string NotificationProgressColor, string NotificationFailureColor, string QuestionColor, string SuccessColor, string UserDollarColor, string TipColor, string SeparatorTextColor, string SeparatorColor, string ListTitleColor, string DevelopmentWarningColor, string StageTimeColor, string ProgressColor, string BackOptionColor, string LowPriorityBorderColor, string MediumPriorityBorderColor, string HighPriorityBorderColor, string TableSeparatorColor, string TableHeaderColor, string TableValueColor, string SelectedOptionColor, string AlternativeOptionColor)
        {
            // Check colors for null and set them to "def" if found
            if (string.IsNullOrEmpty(OptionColor))
                OptionColor = "def";
            if (string.IsNullOrEmpty(WarningColor))
                WarningColor = "def";
            if (string.IsNullOrEmpty(ErrorColor))
                ErrorColor = "def";
            if (string.IsNullOrEmpty(StageColor))
                StageColor = "def";
            if (string.IsNullOrEmpty(ListValueColor))
                ListValueColor = "def";
            if (string.IsNullOrEmpty(ListEntryColor))
                ListEntryColor = "def";
            if (string.IsNullOrEmpty(NeutralTextColor))
                NeutralTextColor = "def";
            if (string.IsNullOrEmpty(BackgroundColor))
                BackgroundColor = "def";
            if (string.IsNullOrEmpty(UserNameShellColor))
                UserNameShellColor = "def";
            if (string.IsNullOrEmpty(HostNameShellColor))
                HostNameShellColor = "def";
            if (string.IsNullOrEmpty(UncontKernelErrorColor))
                UncontKernelErrorColor = "def";
            if (string.IsNullOrEmpty(ContKernelErrorColor))
                ContKernelErrorColor = "def";
            if (string.IsNullOrEmpty(LicenseColor))
                LicenseColor = "def";
            if (string.IsNullOrEmpty(InputColor))
                InputColor = "def";
            if (string.IsNullOrEmpty(BannerColor))
                BannerColor = "def";
            if (string.IsNullOrEmpty(NotificationTitleColor))
                NotificationTitleColor = "def";
            if (string.IsNullOrEmpty(NotificationDescriptionColor))
                NotificationDescriptionColor = "def";
            if (string.IsNullOrEmpty(NotificationProgressColor))
                NotificationProgressColor = "def";
            if (string.IsNullOrEmpty(NotificationFailureColor))
                NotificationFailureColor = "def";
            if (string.IsNullOrEmpty(QuestionColor))
                QuestionColor = "def";
            if (string.IsNullOrEmpty(SuccessColor))
                SuccessColor = "def";
            if (string.IsNullOrEmpty(UserDollarColor))
                UserDollarColor = "def";
            if (string.IsNullOrEmpty(TipColor))
                TipColor = "def";
            if (string.IsNullOrEmpty(SeparatorTextColor))
                SeparatorTextColor = "def";
            if (string.IsNullOrEmpty(SeparatorColor))
                SeparatorColor = "def";
            if (string.IsNullOrEmpty(ListTitleColor))
                ListTitleColor = "def";
            if (string.IsNullOrEmpty(DevelopmentWarningColor))
                DevelopmentWarningColor = "def";
            if (string.IsNullOrEmpty(StageTimeColor))
                StageTimeColor = "def";
            if (string.IsNullOrEmpty(ProgressColor))
                ProgressColor = "def";
            if (string.IsNullOrEmpty(BackOptionColor))
                BackOptionColor = "def";
            if (string.IsNullOrEmpty(LowPriorityBorderColor))
                LowPriorityBorderColor = "def";
            if (string.IsNullOrEmpty(MediumPriorityBorderColor))
                MediumPriorityBorderColor = "def";
            if (string.IsNullOrEmpty(HighPriorityBorderColor))
                HighPriorityBorderColor = "def";
            if (string.IsNullOrEmpty(TableSeparatorColor))
                TableSeparatorColor = "def";
            if (string.IsNullOrEmpty(TableHeaderColor))
                TableHeaderColor = "def";
            if (string.IsNullOrEmpty(TableValueColor))
                TableValueColor = "def";
            if (string.IsNullOrEmpty(SelectedOptionColor))
                SelectedOptionColor = "def";
            if (string.IsNullOrEmpty(AlternativeOptionColor))
                AlternativeOptionColor = "def";

            // Set colors
            if (Shell.Shell.ColoredShell == true)
            {
                // Check for defaults
                // We use New Color() to parse entered color. This is to ensure that the kernel can use the correct VT sequence.
                if (InputColor == "def")
                    InputColor = new Color((int)ConsoleColors.White).PlainSequence;
                if (LicenseColor == "def")
                    LicenseColor = new Color((int)ConsoleColors.White).PlainSequence;
                if (ContKernelErrorColor == "def")
                    ContKernelErrorColor = new Color((int)ConsoleColors.Yellow).PlainSequence;
                if (UncontKernelErrorColor == "def")
                    UncontKernelErrorColor = new Color((int)ConsoleColors.Red).PlainSequence;
                if (HostNameShellColor == "def")
                    HostNameShellColor = new Color((int)ConsoleColors.DarkGreen).PlainSequence;
                if (UserNameShellColor == "def")
                    UserNameShellColor = new Color((int)ConsoleColors.Green).PlainSequence;
                if (NeutralTextColor == "def")
                    NeutralTextColor = new Color((int)ConsoleColors.Gray).PlainSequence;
                if (ListEntryColor == "def")
                    ListEntryColor = new Color((int)ConsoleColors.DarkYellow).PlainSequence;
                if (ListValueColor == "def")
                    ListValueColor = new Color((int)ConsoleColors.DarkGray).PlainSequence;
                if (StageColor == "def")
                    StageColor = new Color((int)ConsoleColors.Green).PlainSequence;
                if (ErrorColor == "def")
                    ErrorColor = new Color((int)ConsoleColors.Red).PlainSequence;
                if (WarningColor == "def")
                    WarningColor = new Color((int)ConsoleColors.Yellow).PlainSequence;
                if (OptionColor == "def")
                    OptionColor = new Color((int)ConsoleColors.DarkYellow).PlainSequence;
                if (BannerColor == "def")
                    OptionColor = new Color((int)ConsoleColors.Green).PlainSequence;
                if (NotificationTitleColor == "def")
                    NotificationTitleColor = new Color((int)ConsoleColors.White).PlainSequence;
                if (NotificationDescriptionColor == "def")
                    NotificationDescriptionColor = new Color((int)ConsoleColors.Gray).PlainSequence;
                if (NotificationProgressColor == "def")
                    NotificationProgressColor = new Color((int)ConsoleColors.DarkYellow).PlainSequence;
                if (NotificationFailureColor == "def")
                    NotificationFailureColor = new Color((int)ConsoleColors.Red).PlainSequence;
                if (QuestionColor == "def")
                    QuestionColor = new Color((int)ConsoleColors.Yellow).PlainSequence;
                if (SuccessColor == "def")
                    SuccessColor = new Color((int)ConsoleColors.Green).PlainSequence;
                if (UserDollarColor == "def")
                    UserDollarColor = new Color((int)ConsoleColors.Gray).PlainSequence;
                if (TipColor == "def")
                    TipColor = new Color((int)ConsoleColors.Gray).PlainSequence;
                if (SeparatorTextColor == "def")
                    SeparatorTextColor = new Color((int)ConsoleColors.White).PlainSequence;
                if (SeparatorColor == "def")
                    SeparatorColor = new Color((int)ConsoleColors.Gray).PlainSequence;
                if (ListTitleColor == "def")
                    ListTitleColor = new Color((int)ConsoleColors.White).PlainSequence;
                if (DevelopmentWarningColor == "def")
                    DevelopmentWarningColor = new Color((int)ConsoleColors.Yellow).PlainSequence;
                if (StageTimeColor == "def")
                    StageTimeColor = new Color((int)ConsoleColors.Gray).PlainSequence;
                if (ProgressColor == "def")
                    ProgressColor = new Color((int)ConsoleColors.DarkYellow).PlainSequence;
                if (BackOptionColor == "def")
                    BackOptionColor = new Color((int)ConsoleColors.DarkRed).PlainSequence;
                if (LowPriorityBorderColor == "def")
                    LowPriorityBorderColor = new Color((int)ConsoleColors.White).PlainSequence;
                if (MediumPriorityBorderColor == "def")
                    MediumPriorityBorderColor = new Color((int)ConsoleColors.Yellow).PlainSequence;
                if (HighPriorityBorderColor == "def")
                    HighPriorityBorderColor = new Color((int)ConsoleColors.Red).PlainSequence;
                if (TableSeparatorColor == "def")
                    TableSeparatorColor = new Color((int)ConsoleColors.DarkGray).PlainSequence;
                if (TableHeaderColor == "def")
                    TableHeaderColor = new Color((int)ConsoleColors.White).PlainSequence;
                if (TableValueColor == "def")
                    TableValueColor = new Color((int)ConsoleColors.Gray).PlainSequence;
                if (SelectedOptionColor == "def")
                    SelectedOptionColor = new Color((int)ConsoleColors.Yellow).PlainSequence;
                if (AlternativeOptionColor == "def")
                    AlternativeOptionColor = new Color((int)ConsoleColors.DarkGreen).PlainSequence;
                if (BackgroundColor == "def")
                {
                    BackgroundColor = new Color((int)ConsoleColors.Black).PlainSequence;
                    LoadBack();
                }

                // Set the colors
                try
                {
                    ColorTools.InputColor = new Color(InputColor);
                    ColorTools.LicenseColor = new Color(LicenseColor);
                    ColorTools.ContKernelErrorColor = new Color(ContKernelErrorColor);
                    ColorTools.UncontKernelErrorColor = new Color(UncontKernelErrorColor);
                    ColorTools.HostNameShellColor = new Color(HostNameShellColor);
                    ColorTools.UserNameShellColor = new Color(UserNameShellColor);
                    ColorTools.BackgroundColor = new Color(BackgroundColor);
                    ColorTools.NeutralTextColor = new Color(NeutralTextColor);
                    ColorTools.ListEntryColor = new Color(ListEntryColor);
                    ColorTools.ListValueColor = new Color(ListValueColor);
                    ColorTools.StageColor = new Color(StageColor);
                    ColorTools.ErrorColor = new Color(ErrorColor);
                    ColorTools.WarningColor = new Color(WarningColor);
                    ColorTools.OptionColor = new Color(OptionColor);
                    ColorTools.BannerColor = new Color(BannerColor);
                    ColorTools.NotificationTitleColor = new Color(NotificationTitleColor);
                    ColorTools.NotificationDescriptionColor = new Color(NotificationDescriptionColor);
                    ColorTools.NotificationProgressColor = new Color(NotificationProgressColor);
                    ColorTools.NotificationFailureColor = new Color(NotificationFailureColor);
                    ColorTools.QuestionColor = new Color(QuestionColor);
                    ColorTools.SuccessColor = new Color(SuccessColor);
                    ColorTools.UserDollarColor = new Color(UserDollarColor);
                    ColorTools.TipColor = new Color(TipColor);
                    ColorTools.SeparatorTextColor = new Color(SeparatorTextColor);
                    ColorTools.SeparatorColor = new Color(SeparatorColor);
                    ColorTools.ListTitleColor = new Color(ListTitleColor);
                    ColorTools.DevelopmentWarningColor = new Color(DevelopmentWarningColor);
                    ColorTools.StageTimeColor = new Color(StageTimeColor);
                    ColorTools.ProgressColor = new Color(ProgressColor);
                    ColorTools.BackOptionColor = new Color(BackOptionColor);
                    ColorTools.LowPriorityBorderColor = new Color(LowPriorityBorderColor);
                    ColorTools.MediumPriorityBorderColor = new Color(MediumPriorityBorderColor);
                    ColorTools.HighPriorityBorderColor = new Color(HighPriorityBorderColor);
                    ColorTools.TableSeparatorColor = new Color(TableSeparatorColor);
                    ColorTools.TableHeaderColor = new Color(TableHeaderColor);
                    ColorTools.TableValueColor = new Color(TableValueColor);
                    ColorTools.SelectedOptionColor = new Color(SelectedOptionColor);
                    ColorTools.AlternativeOptionColor = new Color(AlternativeOptionColor);
                    LoadBack();
                    Config.CreateConfig();

                    // Raise event
                    Kernel.Kernel.KernelEventManager.RaiseColorSet();
                }
                catch (Exception ex)
                {
                    DebugWriter.WStkTrc(ex);
                    Kernel.Kernel.KernelEventManager.RaiseColorSetError(ColorSetErrorReasons.InvalidColors);
                    throw new Kernel.Exceptions.ColorException(Translate.DoTranslation("One or more of the colors is invalid.") + " {0}", ex, ex.Message);
                }
            }
            else
            {
                Kernel.Kernel.KernelEventManager.RaiseColorSetError(ColorSetErrorReasons.NoColors);
                throw new InvalidOperationException(Translate.DoTranslation("Colors are not available. Turn on colored shell in the kernel config."));
            }
        }

        /// <summary>
        /// Sets input color
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TrySetInputColor()
        {
            try
            {
                SetInputColor();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Sets input color
        /// </summary>
        public static void SetInputColor()
        {
            DebugWriter.Wdbg(DebugLevel.I, "ColoredShell is {0}", Shell.Shell.ColoredShell);
            if (Shell.Shell.ColoredShell == true)
            {
                SetConsoleColor(InputColor);
                SetConsoleColor(BackgroundColor, true);
            }
        }

        /// <summary>
        /// Gets the gray color according to the brightness of the background color
        /// </summary>
        public static Color GetGray()
        {
            if (BackgroundColor.IsBright)
            {
                return NeutralTextColor;
            }
            else
            {
                return new Color((int)ConsoleColors.Gray);
            }
        }

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        public static void SetConsoleColor(ColTypes colorType)
        {
            SetConsoleColor(colorType, false);
        }

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        public static void SetConsoleColor(ColTypes colorType, bool Background, bool ForceSet = false)
        {
            if (Kernel.Kernel.DefConsoleOut is null | Equals(Kernel.Kernel.DefConsoleOut, Console.Out))
            {
                switch (colorType)
                {
                    case ColTypes.Neutral:
                        {
                            SetConsoleColor(NeutralTextColor, Background, ForceSet);
                            break;
                        }
                    case ColTypes.Continuable:
                        {
                            SetConsoleColor(ContKernelErrorColor, Background, ForceSet);
                            break;
                        }
                    case ColTypes.Uncontinuable:
                        {
                            SetConsoleColor(UncontKernelErrorColor, Background, ForceSet);
                            break;
                        }
                    case ColTypes.HostName:
                        {
                            SetConsoleColor(HostNameShellColor, Background, ForceSet);
                            break;
                        }
                    case ColTypes.UserName:
                        {
                            SetConsoleColor(UserNameShellColor, Background, ForceSet);
                            break;
                        }
                    case ColTypes.License:
                        {
                            SetConsoleColor(LicenseColor, Background, ForceSet);
                            break;
                        }
                    case ColTypes.Gray:
                        {
                            SetConsoleColor(GetGray(), Background, ForceSet);
                            break;
                        }
                    case ColTypes.ListValue:
                        {
                            SetConsoleColor(ListValueColor, Background, ForceSet);
                            break;
                        }
                    case ColTypes.ListEntry:
                        {
                            SetConsoleColor(ListEntryColor, Background, ForceSet);
                            break;
                        }
                    case ColTypes.Stage:
                        {
                            SetConsoleColor(StageColor, Background, ForceSet);
                            break;
                        }
                    case ColTypes.Error:
                        {
                            SetConsoleColor(ErrorColor, Background, ForceSet);
                            break;
                        }
                    case ColTypes.Warning:
                        {
                            SetConsoleColor(WarningColor, Background, ForceSet);
                            break;
                        }
                    case ColTypes.Option:
                        {
                            SetConsoleColor(OptionColor, Background, ForceSet);
                            break;
                        }
                    case ColTypes.Banner:
                        {
                            SetConsoleColor(BannerColor, Background, ForceSet);
                            break;
                        }
                    case ColTypes.NotificationTitle:
                        {
                            SetConsoleColor(NotificationTitleColor, Background, ForceSet);
                            break;
                        }
                    case ColTypes.NotificationDescription:
                        {
                            SetConsoleColor(NotificationDescriptionColor, Background, ForceSet);
                            break;
                        }
                    case ColTypes.NotificationProgress:
                        {
                            SetConsoleColor(NotificationProgressColor, Background, ForceSet);
                            break;
                        }
                    case ColTypes.NotificationFailure:
                        {
                            SetConsoleColor(NotificationFailureColor, Background, ForceSet);
                            break;
                        }
                    case ColTypes.Question:
                    case ColTypes.Input:
                        {
                            SetConsoleColor(QuestionColor, Background, ForceSet);
                            break;
                        }
                    case ColTypes.Success:
                        {
                            SetConsoleColor(SuccessColor, Background, ForceSet);
                            break;
                        }
                    case ColTypes.UserDollarSign:
                        {
                            SetConsoleColor(UserDollarColor, Background, ForceSet);
                            break;
                        }
                    case ColTypes.Tip:
                        {
                            SetConsoleColor(TipColor, Background, ForceSet);
                            break;
                        }
                    case ColTypes.SeparatorText:
                        {
                            SetConsoleColor(SeparatorTextColor, Background, ForceSet);
                            break;
                        }
                    case ColTypes.Separator:
                        {
                            SetConsoleColor(SeparatorColor, Background, ForceSet);
                            break;
                        }
                    case ColTypes.ListTitle:
                        {
                            SetConsoleColor(ListTitleColor, Background, ForceSet);
                            break;
                        }
                    case ColTypes.DevelopmentWarning:
                        {
                            SetConsoleColor(DevelopmentWarningColor, Background, ForceSet);
                            break;
                        }
                    case ColTypes.StageTime:
                        {
                            SetConsoleColor(StageTimeColor, Background, ForceSet);
                            break;
                        }
                    case ColTypes.Progress:
                        {
                            SetConsoleColor(ProgressColor, Background, ForceSet);
                            break;
                        }
                    case ColTypes.BackOption:
                        {
                            SetConsoleColor(BackOptionColor, Background, ForceSet);
                            break;
                        }
                    case ColTypes.LowPriorityBorder:
                        {
                            SetConsoleColor(LowPriorityBorderColor, Background, ForceSet);
                            break;
                        }
                    case ColTypes.MediumPriorityBorder:
                        {
                            SetConsoleColor(MediumPriorityBorderColor, Background, ForceSet);
                            break;
                        }
                    case ColTypes.HighPriorityBorder:
                        {
                            SetConsoleColor(HighPriorityBorderColor, Background, ForceSet);
                            break;
                        }
                    case ColTypes.TableSeparator:
                        {
                            SetConsoleColor(TableSeparatorColor, Background, ForceSet);
                            break;
                        }
                    case ColTypes.TableHeader:
                        {
                            SetConsoleColor(TableHeaderColor, Background, ForceSet);
                            break;
                        }
                    case ColTypes.TableValue:
                        {
                            SetConsoleColor(TableValueColor, Background, ForceSet);
                            break;
                        }
                    case ColTypes.SelectedOption:
                        {
                            SetConsoleColor(SelectedOptionColor, Background, ForceSet);
                            break;
                        }
                    case ColTypes.AlternativeOption:
                        {
                            SetConsoleColor(AlternativeOptionColor, Background, ForceSet);
                            break;
                        }

                    default:
                        {
                            break;
                        }
                }
                if (!Background)
                    SetConsoleColor(BackgroundColor, true);
            }
        }

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="ColorSequence">The color instance</param>
        /// <param name="Background">Whether to set background or not</param>
        /// <param name="ForceSet">Force set background even if background setting is disabled</param>
        public static void SetConsoleColor(Color ColorSequence, bool Background = false, bool ForceSet = false)
        {
            if (Shell.Shell.ColoredShell)
            {
                if (ColorSequence is null)
                    throw new ArgumentNullException(nameof(ColorSequence));

                // Define reset background sequence
                string resetSequence = CharManager.GetEsc() + $"[49m";

                // Set background
                if (Background)
                {
                    if (Flags.SetBackground | ForceSet)
                        TextWriterColor.WritePlain(ColorSequence.VTSequenceBackground, false);
                    else
                        TextWriterColor.WritePlain(resetSequence, false);
                }
                else
                {
                    TextWriterColor.WritePlain(ColorSequence.VTSequenceForeground, false);
                }
            }
        }

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TrySetConsoleColor(ColTypes colorType)
        {
            return TrySetConsoleColor(colorType, false);
        }

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TrySetConsoleColor(ColTypes colorType, bool Background)
        {
            try
            {
                SetConsoleColor(colorType, Background);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="ColorSequence">The color instance</param>
        /// <param name="Background">Whether to set background or not</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TrySetConsoleColor(Color ColorSequence, bool Background)
        {
            try
            {
                SetConsoleColor(ColorSequence, Background);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Tries parsing the color from the specifier string
        /// </summary>
        /// <param name="ColorSpecifier">A color specifier. It must be a valid number from 0-255 if using 255-colors, or a VT sequence if using true color as follows: &lt;R&gt;;&lt;G&gt;;&lt;B&gt;</param>
        /// <returns>True if successful; False if failed</returns>
        public static bool TryParseColor(string ColorSpecifier)
        {
            try
            {
                var ColorInstance = new Color(ColorSpecifier);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Failed trying to parse color: {0}", ex.Message);
                DebugWriter.WStkTrc(ex);
                return false;
            }
        }

        /// <summary>
        /// Tries parsing the color from the specifier string
        /// </summary>
        /// <param name="ColorNum">The color number</param>
        /// <returns>True if successful; False if failed</returns>
        public static bool TryParseColor(int ColorNum)
        {
            try
            {
                var ColorInstance = new Color(ColorNum);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Failed trying to parse color: {0}", ex.Message);
                DebugWriter.WStkTrc(ex);
                return false;
            }
        }

        /// <summary>
        /// Tries parsing the color from the specifier string
        /// </summary>
        /// <param name="R">The red level</param>
        /// <param name="G">The green level</param>
        /// <param name="B">The blue level</param>
        /// <returns>True if successful; False if failed</returns>
        public static bool TryParseColor(int R, int G, int B)
        {
            try
            {
                var ColorInstance = new Color(R, G, B);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Failed trying to parse color: {0}", ex.Message);
                DebugWriter.WStkTrc(ex);
                return false;
            }
        }

        /// <summary>
        /// Converts from the hexadecimal representation of a color to the RGB sequence
        /// </summary>
        /// <param name="Hex">A hexadecimal representation of a color (#AABBCC for example)</param>
        /// <returns>&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</returns>
        public static string ConvertFromHexToRGB(string Hex)
        {
            if (Hex.StartsWith("#"))
            {
                int ColorDecimal = Convert.ToInt32(Hex.RemoveLetter(0), 16);
                int R = (byte)((ColorDecimal & 0xFF0000) >> 0x10);
                int G = (byte)((ColorDecimal & 0xFF00) >> 8);
                int B = (byte)(ColorDecimal & 0xFF);
                return $"{R};{G};{B}";
            }
            else
            {
                throw new Kernel.Exceptions.ColorException(Translate.DoTranslation("Invalid hex color specifier."));
            }
        }

        /// <summary>
        /// Converts from the RGB sequence of a color to the hexadecimal representation
        /// </summary>
        /// <param name="RGBSequence">&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</param>
        /// <returns>A hexadecimal representation of a color (#AABBCC for example)</returns>
        public static string ConvertFromRGBToHex(string RGBSequence)
        {
            if (RGBSequence.Contains(Convert.ToString(';')))
            {
                // Split the VT sequence into three parts
                var ColorSpecifierArray = RGBSequence.Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int R = Convert.ToInt32(ColorSpecifierArray[0]);
                    int G = Convert.ToInt32(ColorSpecifierArray[1]);
                    int B = Convert.ToInt32(ColorSpecifierArray[2]);
                    return $"#{R:X2}{G:X2}{B:X2}";
                }
                else
                {
                    throw new Kernel.Exceptions.ColorException(Translate.DoTranslation("Invalid RGB color specifier."));
                }
            }
            else
            {
                throw new Kernel.Exceptions.ColorException(Translate.DoTranslation("Invalid RGB color specifier."));
            }
        }

        /// <summary>
        /// Converts from the RGB sequence of a color to the hexadecimal representation
        /// </summary>
        /// <param name="R">The red level</param>
        /// <param name="G">The green level</param>
        /// <param name="B">The blue level</param>
        /// <returns>A hexadecimal representation of a color (#AABBCC for example)</returns>
        public static string ConvertFromRGBToHex(int R, int G, int B)
        {
            if (R < 0 | R > 255)
                throw new Kernel.Exceptions.ColorException(Translate.DoTranslation("Invalid red color specifier."));
            if (G < 0 | G > 255)
                throw new Kernel.Exceptions.ColorException(Translate.DoTranslation("Invalid green color specifier."));
            if (B < 0 | B > 255)
                throw new Kernel.Exceptions.ColorException(Translate.DoTranslation("Invalid blue color specifier."));
            return $"#{R:X2}{G:X2}{B:X2}";
        }

    }
}
