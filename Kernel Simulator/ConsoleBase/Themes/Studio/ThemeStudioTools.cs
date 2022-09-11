
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
using System.IO;
using System.Linq;
using ColorSeq;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Renci.SshNet.Security;

namespace KS.ConsoleBase.Themes.Studio
{
    static class ThemeStudioTools
    {

        /// <summary>
        /// Selected theme name
        /// </summary>
        internal static string SelectedThemeName = "";
        internal static readonly Dictionary<string, Color> SelectedColors = new()
        {
            { "Input color", ColorTools.InputColor },
            { "License color", ColorTools.LicenseColor },
            { "Continuable kernel error color", ColorTools.ContKernelErrorColor },
            { "Uncontinuable kernel error color", ColorTools.UncontKernelErrorColor },
            { "Host name color", ColorTools.HostNameShellColor },
            { "User name color", ColorTools.UserNameShellColor },
            { "Background color", ColorTools.BackgroundColor },
            { "Neutral text color", ColorTools.NeutralTextColor },
            { "List entry color", ColorTools.ListEntryColor },
            { "List value color", ColorTools.ListValueColor },
            { "Stage color", ColorTools.StageColor },
            { "Error color", ColorTools.ErrorColor },
            { "Warning color", ColorTools.WarningColor },
            { "Option color", ColorTools.OptionColor },
            { "Banner color", ColorTools.BannerColor },
            { "Notification title color", ColorTools.NotificationTitleColor },
            { "Notification description color", ColorTools.NotificationDescriptionColor },
            { "Notification progress color", ColorTools.NotificationProgressColor },
            { "Notification failure color", ColorTools.NotificationFailureColor },
            { "Question color", ColorTools.QuestionColor },
            { "Success color", ColorTools.SuccessColor },
            { "User dollar color", ColorTools.UserDollarColor },
            { "Tip color", ColorTools.TipColor },
            { "Separator text color", ColorTools.SeparatorTextColor },
            { "Separator color", ColorTools.SeparatorColor },
            { "List title color", ColorTools.ListTitleColor },
            { "Development warning color", ColorTools.DevelopmentWarningColor },
            { "Stage time color", ColorTools.StageTimeColor },
            { "Progress color", ColorTools.ProgressColor },
            { "Back option color", ColorTools.BackOptionColor },
            { "Low priority border color", ColorTools.LowPriorityBorderColor },
            { "Medium priority border color", ColorTools.MediumPriorityBorderColor },
            { "High priority border color", ColorTools.HighPriorityBorderColor },
            { "Table separator color", ColorTools.TableSeparatorColor },
            { "Table header color", ColorTools.TableHeaderColor },
            { "Table value color", ColorTools.TableValueColor },
            { "Selected option color", ColorTools.SelectedOptionColor },
            { "Alternative option color", ColorTools.AlternativeOptionColor },
        };

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
            else if (Theme == "3Y-Diamond")
            {
                ThemeInfo = new ThemeInfo("_3Y_Diamond");
            }
            else
            {
                ThemeInfo = new ThemeInfo(Theme.Replace("-", "_"));
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
            SelectedColors["Input color"] = ThemeInfo.ThemeInputColor;
            SelectedColors["License color"] = ThemeInfo.ThemeLicenseColor;
            SelectedColors["Continuable kernel error color"] = ThemeInfo.ThemeContKernelErrorColor;
            SelectedColors["Uncontinuable kernel error color"] = ThemeInfo.ThemeUncontKernelErrorColor;
            SelectedColors["Host name color"] = ThemeInfo.ThemeHostNameShellColor;
            SelectedColors["User name color"] = ThemeInfo.ThemeUserNameShellColor;
            SelectedColors["Background color"] = ThemeInfo.ThemeBackgroundColor;
            SelectedColors["Neutral text color"] = ThemeInfo.ThemeNeutralTextColor;
            SelectedColors["List entry color"] = ThemeInfo.ThemeListEntryColor;
            SelectedColors["List value color"] = ThemeInfo.ThemeListValueColor;
            SelectedColors["Stage color"] = ThemeInfo.ThemeStageColor;
            SelectedColors["Error color"] = ThemeInfo.ThemeErrorColor;
            SelectedColors["Warning color"] = ThemeInfo.ThemeWarningColor;
            SelectedColors["Option color"] = ThemeInfo.ThemeOptionColor;
            SelectedColors["Banner color"] = ThemeInfo.ThemeBannerColor;
            SelectedColors["Notification title color"] = ThemeInfo.ThemeNotificationTitleColor;
            SelectedColors["Notification description color"] = ThemeInfo.ThemeNotificationDescriptionColor;
            SelectedColors["Notification progress color"] = ThemeInfo.ThemeNotificationProgressColor;
            SelectedColors["Notification failure color"] = ThemeInfo.ThemeNotificationFailureColor;
            SelectedColors["Question color"] = ThemeInfo.ThemeQuestionColor;
            SelectedColors["Success color"] = ThemeInfo.ThemeSuccessColor;
            SelectedColors["User dollar color"] = ThemeInfo.ThemeUserDollarColor;
            SelectedColors["Tip color"] = ThemeInfo.ThemeTipColor;
            SelectedColors["Separator text color"] = ThemeInfo.ThemeSeparatorTextColor;
            SelectedColors["Separator color"] = ThemeInfo.ThemeSeparatorColor;
            SelectedColors["List title color"] = ThemeInfo.ThemeListTitleColor;
            SelectedColors["Development warning color"] = ThemeInfo.ThemeDevelopmentWarningColor;
            SelectedColors["Stage time color"] = ThemeInfo.ThemeStageTimeColor;
            SelectedColors["Progress color"] = ThemeInfo.ThemeProgressColor;
            SelectedColors["Back option color"] = ThemeInfo.ThemeBackOptionColor;
            SelectedColors["Low priority border color"] = ThemeInfo.ThemeLowPriorityBorderColor;
            SelectedColors["Medium priority border color"] = ThemeInfo.ThemeMediumPriorityBorderColor;
            SelectedColors["High priority border color"] = ThemeInfo.ThemeHighPriorityBorderColor;
            SelectedColors["Table separator color"] = ThemeInfo.ThemeTableSeparatorColor;
            SelectedColors["Table header color"] = ThemeInfo.ThemeTableHeaderColor;
            SelectedColors["Table value color"] = ThemeInfo.ThemeTableValueColor;
            SelectedColors["Selected option color"] = ThemeInfo.ThemeSelectedOptionColor;
            SelectedColors["Alternative option color"] = ThemeInfo.ThemeAlternativeOptionColor;
        }

        /// <summary>
        /// Loads theme from current colors and places it to the studio
        /// </summary>
        public static void LoadThemeFromCurrentColors()
        {
            // Place information to the studio
            SelectedColors["Input color"] = ColorTools.InputColor;
            SelectedColors["License color"] = ColorTools.LicenseColor;
            SelectedColors["Continuable kernel error color"] = ColorTools.ContKernelErrorColor;
            SelectedColors["Uncontinuable kernel error color"] = ColorTools.UncontKernelErrorColor;
            SelectedColors["Host name color"] = ColorTools.HostNameShellColor;
            SelectedColors["User name color"] = ColorTools.UserNameShellColor;
            SelectedColors["Background color"] = ColorTools.BackgroundColor;
            SelectedColors["Neutral text color"] = ColorTools.NeutralTextColor;
            SelectedColors["List entry color"] = ColorTools.ListEntryColor;
            SelectedColors["List value color"] = ColorTools.ListValueColor;
            SelectedColors["Stage color"] = ColorTools.StageColor;
            SelectedColors["Error color"] = ColorTools.ErrorColor;
            SelectedColors["Warning color"] = ColorTools.WarningColor;
            SelectedColors["Option color"] = ColorTools.OptionColor;
            SelectedColors["Banner color"] = ColorTools.BannerColor;
            SelectedColors["Notification title color"] = ColorTools.NotificationTitleColor;
            SelectedColors["Notification description color"] = ColorTools.NotificationDescriptionColor;
            SelectedColors["Notification progress color"] = ColorTools.NotificationProgressColor;
            SelectedColors["Notification failure color"] = ColorTools.NotificationFailureColor;
            SelectedColors["Question color"] = ColorTools.QuestionColor;
            SelectedColors["Success color"] = ColorTools.SuccessColor;
            SelectedColors["User dollar color"] = ColorTools.UserDollarColor;
            SelectedColors["Tip color"] = ColorTools.TipColor;
            SelectedColors["Separator text color"] = ColorTools.SeparatorTextColor;
            SelectedColors["Separator color"] = ColorTools.SeparatorColor;
            SelectedColors["List title color"] = ColorTools.ListTitleColor;
            SelectedColors["Development warning color"] = ColorTools.DevelopmentWarningColor;
            SelectedColors["Stage time color"] = ColorTools.StageTimeColor;
            SelectedColors["Progress color"] = ColorTools.ProgressColor;
            SelectedColors["Back option color"] = ColorTools.BackOptionColor;
            SelectedColors["Low priority border color"] = ColorTools.LowPriorityBorderColor;
            SelectedColors["Medium priority border color"] = ColorTools.MediumPriorityBorderColor;
            SelectedColors["High priority border color"] = ColorTools.HighPriorityBorderColor;
            SelectedColors["Table separator color"] = ColorTools.TableSeparatorColor;
            SelectedColors["Table header color"] = ColorTools.TableHeaderColor;
            SelectedColors["Table value color"] = ColorTools.TableValueColor;
            SelectedColors["Selected option color"] = ColorTools.SelectedOptionColor;
            SelectedColors["Alternative option color"] = ColorTools.AlternativeOptionColor;
        }

        /// <summary>
        /// Gets the full theme JSON object
        /// </summary>
        /// <returns>A JSON object</returns>
        public static JObject GetThemeJson() => 
            new (
                /*
                 * Metadata instance with the format of:
                 * 
                 *     "Metadata": {
                 *         "Name": "ThemeName",
                 *         "255ColorsRequired": true,
                 *         "TrueColorRequired": true
                 *     },
                 */
                new JProperty("Metadata",
                    new JObject(
                        new JProperty("Name", SelectedThemeName),
                        new JProperty("255ColorsRequired", Is255ColorsRequired()),
                        new JProperty("TrueColorRequired", IsTrueColorRequired())
                    )
                ),

                // Color instances
                new JProperty("InputColor", SelectedColors["Input color"].PlainSequence),
                new JProperty("LicenseColor", SelectedColors["License color"].PlainSequence),
                new JProperty("ContKernelErrorColor", SelectedColors["Continuable kernel error color"].PlainSequence),
                new JProperty("UncontKernelErrorColor", SelectedColors["Uncontinuable kernel error color"].PlainSequence),
                new JProperty("HostNameShellColor", SelectedColors["Host name color"].PlainSequence),
                new JProperty("UserNameShellColor", SelectedColors["User name color"].PlainSequence),
                new JProperty("BackgroundColor", SelectedColors["Background color"].PlainSequence),
                new JProperty("NeutralTextColor", SelectedColors["Neutral text color"].PlainSequence),
                new JProperty("ListEntryColor", SelectedColors["List entry color"].PlainSequence),
                new JProperty("ListValueColor", SelectedColors["List value color"].PlainSequence),
                new JProperty("StageColor", SelectedColors["Stage color"].PlainSequence),
                new JProperty("ErrorColor", SelectedColors["Error color"].PlainSequence),
                new JProperty("WarningColor", SelectedColors["Warning color"].PlainSequence),
                new JProperty("OptionColor", SelectedColors["Option color"].PlainSequence),
                new JProperty("BannerColor", SelectedColors["Banner color"].PlainSequence),
                new JProperty("NotificationTitleColor", SelectedColors["Notification title color"].PlainSequence),
                new JProperty("NotificationDescriptionColor", SelectedColors["Notification description color"].PlainSequence),
                new JProperty("NotificationProgressColor", SelectedColors["Notification progress color"].PlainSequence),
                new JProperty("NotificationFailureColor", SelectedColors["Notification failure color"].PlainSequence),
                new JProperty("QuestionColor", SelectedColors["Question color"].PlainSequence),
                new JProperty("SuccessColor", SelectedColors["Success color"].PlainSequence),
                new JProperty("UserDollarColor", SelectedColors["User dollar color"].PlainSequence),
                new JProperty("TipColor", SelectedColors["Tip color"].PlainSequence),
                new JProperty("SeparatorTextColor", SelectedColors["Separator text color"].PlainSequence),
                new JProperty("SeparatorColor", SelectedColors["Separator color"].PlainSequence),
                new JProperty("ListTitleColor", SelectedColors["List title color"].PlainSequence),
                new JProperty("DevelopmentWarningColor", SelectedColors["Development warning color"].PlainSequence),
                new JProperty("StageTimeColor", SelectedColors["Stage time color"].PlainSequence),
                new JProperty("ProgressColor", SelectedColors["Progress color"].PlainSequence),
                new JProperty("BackOptionColor", SelectedColors["Back option color"].PlainSequence),
                new JProperty("LowPriorityBorderColor", SelectedColors["Low priority border color"].PlainSequence),
                new JProperty("MediumPriorityBorderColor", SelectedColors["Medium priority border color"].PlainSequence),
                new JProperty("HighPriorityBorderColor", SelectedColors["High priority border color"].PlainSequence),
                new JProperty("TableSeparatorColor", SelectedColors["Table separator color"].PlainSequence),
                new JProperty("TableHeaderColor", SelectedColors["Table header color"].PlainSequence),
                new JProperty("TableValueColor", SelectedColors["Table value color"].PlainSequence),
                new JProperty("SelectedOptionColor", SelectedColors["Selected option color"].PlainSequence),
                new JProperty("AlternativeOptionColor", SelectedColors["Alternative option color"].PlainSequence)
            );

        /// <summary>
        /// Is the 255 color support required?
        /// </summary>
        /// <returns>If required, then true.</returns>
        public static bool Is255ColorsRequired()
        {
            // Check for true color requirement
            if (IsTrueColorRequired())
                return true;

            // Now, check for 255-color requirement
            for (int key = 0; key < SelectedColors.Count; key++)
                if (SelectedColors.Values.ElementAt(key).Type == ColorType._255Color)
                    return true;

            // Else, 255 color support is not required
            return false;
        }

        /// <summary>
        /// Is the true color support required?
        /// </summary>
        /// <returns>If required, then true.</returns>
        public static bool IsTrueColorRequired()
        {
            // Check for true color requirement according to color type
            for (int key = 0; key < SelectedColors.Count; key++)
                if (SelectedColors.Values.ElementAt(key).Type == ColorType.TrueColor)
                    return true;

            // Else, no requirement
            return false;
        }

        /// <summary>
        /// Prepares the preview
        /// </summary>
        public static void PreparePreview()
        {
            ConsoleWrapper.Clear();
            TextWriterColor.Write(Translate.DoTranslation("Here's how your theme will look like:") + Kernel.Kernel.NewLine, true, ColorTools.ColTypes.Neutral);

            // Print every possibility of color types
            for (int key = 0; key < SelectedColors.Count; key++)
            {
                TextWriterColor.Write("*) " + Translate.DoTranslation(SelectedColors.Keys.ElementAt(key)) + ": ", false, ColorTools.ColTypes.Option);
                TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedColors.Values.ElementAt(key));
            }
        }

    }
}
