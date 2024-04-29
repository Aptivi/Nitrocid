﻿//
// Kernel Simulator  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Kernel Simulator
//
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

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

using System.IO;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Files;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Terminaux.Base;
using Terminaux.Colors;

namespace KS.ConsoleBase.Themes.Studio
{
    static class ThemeStudioTools
    {

        /// <summary>
        /// Selected input color for new theme
        /// </summary>
        internal static Color SelectedInputColor = KernelColorTools.InputColor;
        /// <summary>
        /// Selected license color for new theme
        /// </summary>
        internal static Color SelectedLicenseColor = KernelColorTools.LicenseColor;
        /// <summary>
        /// Selected continuable kernel error color for new theme
        /// </summary>
        internal static Color SelectedContKernelErrorColor = KernelColorTools.ContKernelErrorColor;
        /// <summary>
        /// Selected uncontinuable kernel error color for new theme
        /// </summary>
        internal static Color SelectedUncontKernelErrorColor = KernelColorTools.UncontKernelErrorColor;
        /// <summary>
        /// Selected host name shell color for new theme
        /// </summary>
        internal static Color SelectedHostNameShellColor = KernelColorTools.HostNameShellColor;
        /// <summary>
        /// Selected user name shell color for new theme
        /// </summary>
        internal static Color SelectedUserNameShellColor = KernelColorTools.UserNameShellColor;
        /// <summary>
        /// Selected background color for new theme
        /// </summary>
        internal static Color SelectedBackgroundColor = KernelColorTools.BackgroundColor;
        /// <summary>
        /// Selected neutral text color for new theme
        /// </summary>
        internal static Color SelectedNeutralTextColor = KernelColorTools.NeutralTextColor;
        /// <summary>
        /// Selected list entry color for new theme
        /// </summary>
        internal static Color SelectedListEntryColor = KernelColorTools.ListEntryColor;
        /// <summary>
        /// Selected list value color for new theme
        /// </summary>
        internal static Color SelectedListValueColor = KernelColorTools.ListValueColor;
        /// <summary>
        /// Selected stage color for new theme
        /// </summary>
        internal static Color SelectedStageColor = KernelColorTools.StageColor;
        /// <summary>
        /// Selected error color for new theme
        /// </summary>
        internal static Color SelectedErrorColor = KernelColorTools.ErrorColor;
        /// <summary>
        /// Selected warning color for new theme
        /// </summary>
        internal static Color SelectedWarningColor = KernelColorTools.WarningColor;
        /// <summary>
        /// Selected option color for new theme
        /// </summary>
        internal static Color _SelectedOptionColor = KernelColorTools.OptionColor;
        /// <summary>
        /// Selected banner color for new theme
        /// </summary>
        internal static Color SelectedBannerColor = KernelColorTools.BannerColor;
        /// <summary>
        /// Selected error color for new theme
        /// </summary>
        internal static Color SelectedNotificationTitleColor = KernelColorTools.ErrorColor;
        /// <summary>
        /// Selected warning color for new theme
        /// </summary>
        internal static Color SelectedNotificationDescriptionColor = KernelColorTools.WarningColor;
        /// <summary>
        /// Selected option color for new theme
        /// </summary>
        internal static Color SelectedNotificationProgressColor = KernelColorTools.OptionColor;
        /// <summary>
        /// Selected banner color for new theme
        /// </summary>
        internal static Color SelectedNotificationFailureColor = KernelColorTools.BannerColor;
        /// <summary>
        /// Selected error color for new theme
        /// </summary>
        internal static Color SelectedQuestionColor = KernelColorTools.ErrorColor;
        /// <summary>
        /// Selected warning color for new theme
        /// </summary>
        internal static Color SelectedSuccessColor = KernelColorTools.WarningColor;
        /// <summary>
        /// Selected option color for new theme
        /// </summary>
        internal static Color SelectedUserDollarColor = KernelColorTools.OptionColor;
        /// <summary>
        /// Selected banner color for new theme
        /// </summary>
        internal static Color SelectedTipColor = KernelColorTools.BannerColor;
        /// <summary>
        /// Selected error color for new theme
        /// </summary>
        internal static Color SelectedSeparatorTextColor = KernelColorTools.ErrorColor;
        /// <summary>
        /// Selected warning color for new theme
        /// </summary>
        internal static Color SelectedSeparatorColor = KernelColorTools.WarningColor;
        /// <summary>
        /// Selected option color for new theme
        /// </summary>
        internal static Color SelectedListTitleColor = KernelColorTools.OptionColor;
        /// <summary>
        /// Selected banner color for new theme
        /// </summary>
        internal static Color SelectedDevelopmentWarningColor = KernelColorTools.BannerColor;
        /// <summary>
        /// Selected warning color for new theme
        /// </summary>
        internal static Color SelectedStageTimeColor = KernelColorTools.WarningColor;
        /// <summary>
        /// Selected option color for new theme
        /// </summary>
        internal static Color SelectedProgressColor = KernelColorTools.OptionColor;
        /// <summary>
        /// Selected banner color for new theme
        /// </summary>
        internal static Color SelectedBackOptionColor = KernelColorTools.BannerColor;
        /// <summary>
        /// Selected low priority notification border color for new theme
        /// </summary>
        internal static Color SelectedLowPriorityBorderColor = KernelColorTools.LowPriorityBorderColor;
        /// <summary>
        /// Selected medium priority notification border color for new theme
        /// </summary>
        internal static Color SelectedMediumPriorityBorderColor = KernelColorTools.MediumPriorityBorderColor;
        /// <summary>
        /// Selected high priority notification border color for new theme
        /// </summary>
        internal static Color SelectedHighPriorityBorderColor = KernelColorTools.HighPriorityBorderColor;
        /// <summary>
        /// Selected Table separator color for new theme
        /// </summary>
        internal static Color SelectedTableSeparatorColor = KernelColorTools.TableSeparatorColor;
        /// <summary>
        /// Selected Table header color for new theme
        /// </summary>
        internal static Color SelectedTableHeaderColor = KernelColorTools.TableHeaderColor;
        /// <summary>
        /// Selected Table value color for new theme
        /// </summary>
        internal static Color SelectedTableValueColor = KernelColorTools.TableValueColor;
        /// <summary>
        /// Selected selected option color for new theme
        /// </summary>
        internal static Color SelectedSelectedOptionColor = KernelColorTools.SelectedOptionColor;
        /// <summary>
        /// Selected alternative option color for new theme
        /// </summary>
        internal static Color SelectedAlternativeOptionColor = KernelColorTools.AlternativeOptionColor;

        /// <summary>
        /// Saves theme to current directory under "<paramref name="Theme"/>.json."
        /// </summary>
        /// <param name="Theme">Theme name</param>
        public static void SaveThemeToCurrentDirectory(string Theme)
        {
            var ThemeJson = GetThemeJson();
            File.WriteAllText(Filesystem.NeutralizePath(Theme + ".json"), JsonConvert.SerializeObject(ThemeJson, Formatting.Indented));
        }

        /// <summary>
        /// Saves theme to another directory under "<paramref name="Theme"/>.json."
        /// </summary>
        /// <param name="Theme">Theme name</param>
        /// <param name="Path">Path name. Neutralized by <see cref="Filesystem.NeutralizePath(string, bool)"/></param>
        public static void SaveThemeToAnotherDirectory(string Theme, string Path)
        {
            var ThemeJson = GetThemeJson();
            File.WriteAllText(Filesystem.NeutralizePath(Path + "/" + Theme + ".json"), JsonConvert.SerializeObject(ThemeJson, Formatting.Indented));
        }

        /// <summary>
        /// Loads theme from resource and places it to the studio
        /// </summary>
        /// <param name="Theme">A theme name</param>
        public static void LoadThemeFromResource(string Theme)
        {
            // Populate theme info
            ThemeInfo ThemeInfo;
            if (Theme == "Default")
            {
                ThemeInfo = new ThemeInfo("_Default");
            }
            else if (Theme == "NFSHP-Cop")
            {
                ThemeInfo = new ThemeInfo("NFSHP_Cop");
            }
            else if (Theme == "NFSHP-Racer")
            {
                ThemeInfo = new ThemeInfo("NFSHP_Racer");
            }
            else if (Theme == "3Y-Diamond")
            {
                ThemeInfo = new ThemeInfo("_3Y_Diamond");
            }
            else
            {
                ThemeInfo = new ThemeInfo(Theme);
            }
            LoadThemeFromThemeInfo(ThemeInfo);
        }

        /// <summary>
        /// Loads theme from resource and places it to the studio
        /// </summary>
        /// <param name="Theme">A theme name</param>
        public static void LoadThemeFromFile(string Theme)
        {
            // Populate theme info
            var ThemeStream = new StreamReader(Filesystem.NeutralizePath(Theme));
            var ThemeInfo = new ThemeInfo(ThemeStream);
            ThemeStream.Close();
            LoadThemeFromThemeInfo(ThemeInfo);
        }

        /// <summary>
        /// Loads theme from theme info and places it to the studio
        /// </summary>
        /// <param name="ThemeInfo">A theme info instance</param>
        public static void LoadThemeFromThemeInfo(ThemeInfo ThemeInfo)
        {
            // Place information to the studio
            SelectedInputColor = ThemeInfo.ThemeInputColor;
            SelectedLicenseColor = ThemeInfo.ThemeLicenseColor;
            SelectedContKernelErrorColor = ThemeInfo.ThemeContKernelErrorColor;
            SelectedUncontKernelErrorColor = ThemeInfo.ThemeUncontKernelErrorColor;
            SelectedHostNameShellColor = ThemeInfo.ThemeHostNameShellColor;
            SelectedUserNameShellColor = ThemeInfo.ThemeUserNameShellColor;
            SelectedBackgroundColor = ThemeInfo.ThemeBackgroundColor;
            SelectedNeutralTextColor = ThemeInfo.ThemeNeutralTextColor;
            SelectedListEntryColor = ThemeInfo.ThemeListEntryColor;
            SelectedListValueColor = ThemeInfo.ThemeListValueColor;
            SelectedStageColor = ThemeInfo.ThemeStageColor;
            SelectedErrorColor = ThemeInfo.ThemeErrorColor;
            SelectedWarningColor = ThemeInfo.ThemeWarningColor;
            _SelectedOptionColor = ThemeInfo.ThemeOptionColor;
            SelectedBannerColor = ThemeInfo.ThemeBannerColor;
            SelectedNotificationTitleColor = ThemeInfo.ThemeNotificationTitleColor;
            SelectedNotificationDescriptionColor = ThemeInfo.ThemeNotificationDescriptionColor;
            SelectedNotificationProgressColor = ThemeInfo.ThemeNotificationProgressColor;
            SelectedNotificationFailureColor = ThemeInfo.ThemeNotificationFailureColor;
            SelectedQuestionColor = ThemeInfo.ThemeQuestionColor;
            SelectedSuccessColor = ThemeInfo.ThemeSuccessColor;
            SelectedUserDollarColor = ThemeInfo.ThemeUserDollarColor;
            SelectedTipColor = ThemeInfo.ThemeTipColor;
            SelectedSeparatorTextColor = ThemeInfo.ThemeSeparatorTextColor;
            SelectedSeparatorColor = ThemeInfo.ThemeSeparatorColor;
            SelectedListTitleColor = ThemeInfo.ThemeListTitleColor;
            SelectedDevelopmentWarningColor = ThemeInfo.ThemeDevelopmentWarningColor;
            SelectedStageTimeColor = ThemeInfo.ThemeStageTimeColor;
            SelectedProgressColor = ThemeInfo.ThemeProgressColor;
            SelectedBackOptionColor = ThemeInfo.ThemeBackOptionColor;
            SelectedLowPriorityBorderColor = ThemeInfo.ThemeLowPriorityBorderColor;
            SelectedMediumPriorityBorderColor = ThemeInfo.ThemeMediumPriorityBorderColor;
            SelectedHighPriorityBorderColor = ThemeInfo.ThemeHighPriorityBorderColor;
            SelectedTableSeparatorColor = ThemeInfo.ThemeTableSeparatorColor;
            SelectedTableHeaderColor = ThemeInfo.ThemeTableHeaderColor;
            SelectedTableValueColor = ThemeInfo.ThemeTableValueColor;
            SelectedSelectedOptionColor = ThemeInfo.ThemeSelectedOptionColor;
            SelectedAlternativeOptionColor = ThemeInfo.ThemeAlternativeOptionColor;
        }

        /// <summary>
        /// Loads theme from current colors and places it to the studio
        /// </summary>
        public static void LoadThemeFromCurrentColors()
        {
            // Place information to the studio
            SelectedInputColor = KernelColorTools.InputColor;
            SelectedLicenseColor = KernelColorTools.LicenseColor;
            SelectedContKernelErrorColor = KernelColorTools.ContKernelErrorColor;
            SelectedUncontKernelErrorColor = KernelColorTools.UncontKernelErrorColor;
            SelectedHostNameShellColor = KernelColorTools.HostNameShellColor;
            SelectedUserNameShellColor = KernelColorTools.UserNameShellColor;
            SelectedBackgroundColor = KernelColorTools.BackgroundColor;
            SelectedNeutralTextColor = KernelColorTools.NeutralTextColor;
            SelectedListEntryColor = KernelColorTools.ListEntryColor;
            SelectedListValueColor = KernelColorTools.ListValueColor;
            SelectedStageColor = KernelColorTools.StageColor;
            SelectedErrorColor = KernelColorTools.ErrorColor;
            SelectedWarningColor = KernelColorTools.WarningColor;
            _SelectedOptionColor = KernelColorTools.OptionColor;
            SelectedBannerColor = KernelColorTools.BannerColor;
            SelectedNotificationTitleColor = KernelColorTools.NotificationTitleColor;
            SelectedNotificationDescriptionColor = KernelColorTools.NotificationDescriptionColor;
            SelectedNotificationProgressColor = KernelColorTools.NotificationProgressColor;
            SelectedNotificationFailureColor = KernelColorTools.NotificationFailureColor;
            SelectedQuestionColor = KernelColorTools.QuestionColor;
            SelectedSuccessColor = KernelColorTools.SuccessColor;
            SelectedUserDollarColor = KernelColorTools.UserDollarColor;
            SelectedTipColor = KernelColorTools.TipColor;
            SelectedSeparatorTextColor = KernelColorTools.SeparatorTextColor;
            SelectedSeparatorColor = KernelColorTools.SeparatorColor;
            SelectedListTitleColor = KernelColorTools.ListTitleColor;
            SelectedDevelopmentWarningColor = KernelColorTools.DevelopmentWarningColor;
            SelectedStageTimeColor = KernelColorTools.StageTimeColor;
            SelectedProgressColor = KernelColorTools.ProgressColor;
            SelectedBackOptionColor = KernelColorTools.BackOptionColor;
            SelectedLowPriorityBorderColor = KernelColorTools.LowPriorityBorderColor;
            SelectedMediumPriorityBorderColor = KernelColorTools.MediumPriorityBorderColor;
            SelectedHighPriorityBorderColor = KernelColorTools.HighPriorityBorderColor;
            SelectedTableSeparatorColor = KernelColorTools.TableSeparatorColor;
            SelectedTableHeaderColor = KernelColorTools.TableHeaderColor;
            SelectedTableValueColor = KernelColorTools.TableValueColor;
            SelectedSelectedOptionColor = KernelColorTools.SelectedOptionColor;
            SelectedAlternativeOptionColor = KernelColorTools.AlternativeOptionColor;
        }

        /// <summary>
        /// Gets the full theme JSON object
        /// </summary>
        /// <returns>A JSON object</returns>
        public static JObject GetThemeJson()
        {
            return new JObject(new JProperty("InputColor", SelectedInputColor.PlainSequence), new JProperty("LicenseColor", SelectedLicenseColor.PlainSequence), new JProperty("ContKernelErrorColor", SelectedContKernelErrorColor.PlainSequence), new JProperty("UncontKernelErrorColor", SelectedUncontKernelErrorColor.PlainSequence), new JProperty("HostNameShellColor", SelectedHostNameShellColor.PlainSequence), new JProperty("UserNameShellColor", SelectedUserNameShellColor.PlainSequence), new JProperty("BackgroundColor", SelectedBackgroundColor.PlainSequence), new JProperty("NeutralTextColor", SelectedNeutralTextColor.PlainSequence), new JProperty("ListEntryColor", SelectedListEntryColor.PlainSequence), new JProperty("ListValueColor", SelectedListValueColor.PlainSequence), new JProperty("StageColor", SelectedStageColor.PlainSequence), new JProperty("ErrorColor", SelectedErrorColor.PlainSequence), new JProperty("WarningColor", SelectedWarningColor.PlainSequence), new JProperty("OptionColor", _SelectedOptionColor.PlainSequence), new JProperty("BannerColor", SelectedBannerColor.PlainSequence), new JProperty("NotificationTitleColor", SelectedNotificationTitleColor.PlainSequence), new JProperty("NotificationDescriptionColor", SelectedNotificationDescriptionColor.PlainSequence), new JProperty("NotificationProgressColor", SelectedNotificationProgressColor.PlainSequence), new JProperty("NotificationFailureColor", SelectedNotificationFailureColor.PlainSequence), new JProperty("QuestionColor", SelectedQuestionColor.PlainSequence), new JProperty("SuccessColor", SelectedSuccessColor.PlainSequence), new JProperty("UserDollarColor", SelectedUserDollarColor.PlainSequence), new JProperty("TipColor", SelectedTipColor.PlainSequence), new JProperty("SeparatorTextColor", SelectedSeparatorTextColor.PlainSequence), new JProperty("SeparatorColor", SelectedSeparatorColor.PlainSequence), new JProperty("ListTitleColor", SelectedListTitleColor.PlainSequence), new JProperty("DevelopmentWarningColor", SelectedDevelopmentWarningColor.PlainSequence), new JProperty("StageTimeColor", SelectedStageTimeColor.PlainSequence), new JProperty("ProgressColor", SelectedProgressColor.PlainSequence), new JProperty("BackOptionColor", SelectedBackOptionColor.PlainSequence), new JProperty("LowPriorityBorderColor", SelectedLowPriorityBorderColor.PlainSequence), new JProperty("MediumPriorityBorderColor", SelectedMediumPriorityBorderColor.PlainSequence), new JProperty("HighPriorityBorderColor", SelectedHighPriorityBorderColor.PlainSequence), new JProperty("TableSeparatorColor", SelectedTableSeparatorColor.PlainSequence), new JProperty("TableHeaderColor", SelectedTableHeaderColor.PlainSequence), new JProperty("TableValueColor", SelectedTableValueColor.PlainSequence), new JProperty("SelectedOptionColor", SelectedSelectedOptionColor.PlainSequence), new JProperty("AlternativeOptionColor", SelectedAlternativeOptionColor.PlainSequence));
        }

        /// <summary>
        /// Prepares the preview
        /// </summary>
        public static void PreparePreview()
        {
            ConsoleWrapper.Clear();
            TextWriterColor.Write(Translate.DoTranslation("Here's how your theme will look like:") + Kernel.Kernel.NewLine, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));

            // Print every possibility of color types
            // Input color
            TextWriterColor.Write("*) " + Translate.DoTranslation("Input color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedInputColor);

            // License color
            TextWriterColor.Write("*) " + Translate.DoTranslation("License color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedLicenseColor);

            // Continuable kernel error color
            TextWriterColor.Write("*) " + Translate.DoTranslation("Continuable kernel error color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedContKernelErrorColor);

            // Uncontinuable kernel error color
            TextWriterColor.Write("*) " + Translate.DoTranslation("Uncontinuable kernel error color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedUncontKernelErrorColor);

            // Host name color
            TextWriterColor.Write("*) " + Translate.DoTranslation("Host name color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedHostNameShellColor);

            // User name color
            TextWriterColor.Write("*) " + Translate.DoTranslation("User name color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedUserNameShellColor);

            // Background color
            TextWriterColor.Write("*) " + Translate.DoTranslation("Background color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedBackgroundColor);

            // Neutral text color
            TextWriterColor.Write("*) " + Translate.DoTranslation("Neutral text color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedNeutralTextColor);

            // List entry color
            TextWriterColor.Write("*) " + Translate.DoTranslation("List entry color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedListEntryColor);

            // List value color
            TextWriterColor.Write("*) " + Translate.DoTranslation("List value color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedListValueColor);

            // Stage color
            TextWriterColor.Write("*) " + Translate.DoTranslation("Stage color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedStageColor);

            // Error color
            TextWriterColor.Write("*) " + Translate.DoTranslation("Error color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedErrorColor);

            // Warning color
            TextWriterColor.Write("*) " + Translate.DoTranslation("Warning color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedWarningColor);

            // Option color
            TextWriterColor.Write("*) " + Translate.DoTranslation("Option color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, _SelectedOptionColor);

            // Banner color
            TextWriterColor.Write("*) " + Translate.DoTranslation("Banner color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedBannerColor);

            // Notification title color
            TextWriterColor.Write("*) " + Translate.DoTranslation("Notification title color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedNotificationTitleColor);

            // Notification description color
            TextWriterColor.Write("*) " + Translate.DoTranslation("Notification description color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedNotificationDescriptionColor);

            // Notification progress color
            TextWriterColor.Write("*) " + Translate.DoTranslation("Notification progress color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedNotificationProgressColor);

            // Notification failure color
            TextWriterColor.Write("*) " + Translate.DoTranslation("Notification failure color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedNotificationFailureColor);

            // Question color
            TextWriterColor.Write("*) " + Translate.DoTranslation("Question color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedQuestionColor);

            // Success color
            TextWriterColor.Write("*) " + Translate.DoTranslation("Success color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedSuccessColor);

            // User dollar color
            TextWriterColor.Write("*) " + Translate.DoTranslation("User dollar color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedUserDollarColor);

            // Tip color
            TextWriterColor.Write("*) " + Translate.DoTranslation("Tip color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedTipColor);

            // Separator text color
            TextWriterColor.Write("*) " + Translate.DoTranslation("Separator text color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedSeparatorTextColor);

            // Separator color
            TextWriterColor.Write("*) " + Translate.DoTranslation("Separator color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedSeparatorColor);

            // List title color
            TextWriterColor.Write("*) " + Translate.DoTranslation("List title color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedListTitleColor);

            // Development warning color
            TextWriterColor.Write("*) " + Translate.DoTranslation("Development warning color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedDevelopmentWarningColor);

            // Stage time color
            TextWriterColor.Write("*) " + Translate.DoTranslation("Stage time color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedStageTimeColor);

            // Progress color
            TextWriterColor.Write("*) " + Translate.DoTranslation("Progress color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedProgressColor);

            // Back option color
            TextWriterColor.Write("*) " + Translate.DoTranslation("Back option color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedBackOptionColor);

            // Low priority border color
            TextWriterColor.Write("*) " + Translate.DoTranslation("Low priority border color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedLowPriorityBorderColor);

            // Medium priority border color
            TextWriterColor.Write("*) " + Translate.DoTranslation("Medium priority border color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedMediumPriorityBorderColor);

            // High priority border color
            TextWriterColor.Write("*) " + Translate.DoTranslation("High priority border color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedHighPriorityBorderColor);

            // Table separator color
            TextWriterColor.Write("*) " + Translate.DoTranslation("Table separator color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedTableSeparatorColor);

            // Table header color
            TextWriterColor.Write("*) " + Translate.DoTranslation("Table header color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedTableHeaderColor);

            // Table value color
            TextWriterColor.Write("*) " + Translate.DoTranslation("Table value color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedTableValueColor);

            // Selected option color
            TextWriterColor.Write("*) " + Translate.DoTranslation("Selected option color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedSelectedOptionColor);

            // Selected option color
            TextWriterColor.Write("*) " + Translate.DoTranslation("Alternative option color") + ": ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
            TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedAlternativeOptionColor);

            // Pause until a key is pressed
            TextWriterColor.Write(Kernel.Kernel.NewLine + Translate.DoTranslation("Press any key to go back."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
            Input.DetectKeypress();
        }

    }
}