//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
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
using Colorimetry;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nitrocid.Base.Files;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Languages;
using Terminaux.Base.Extensions;
using Terminaux.Inputs;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.Modules;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Themes;
using Terminaux.Themes.Colors;
using Textify.General;

namespace Nitrocid.Extras.ThemeStudio.Studio
{
    internal class ThemeStudioCli : BaseInteractiveTui<string>, IInteractiveTui<string>
    {
        internal Dictionary<string, Color> originalColors = ThemeColorsTools.PopulateColorsCurrent();
        internal string themeName = "";

        /// <inheritdoc/>
        public override InteractiveTuiHelpPage[] HelpPages =>
        [
            new()
            {
                HelpTitle = /* Localizable */ "NKS_THEMESTUDIO_APP_TUI_HELP01_TITLE",
                HelpDescription = /* Localizable */ "NKS_THEMESTUDIO_APP_TUI_HELP01_DESC",
                HelpBody =
                    LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_TUI_HELP01_BODY") + "\n\n" +
#pragma warning disable NLOC0001
                    LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_COMMON_HELP_MOREINFO") + ": https://aptivi.gitbook.io/aptivi/nitrocid-ks-manual/fundamentals/simulated-kernel-features/extra-features/theme-studio",
#pragma warning restore NLOC0001
            }
        ];

        /// <inheritdoc/>
        public override IEnumerable<string> PrimaryDataSource =>
            originalColors.Keys;

        /// <inheritdoc/>
        public override string GetStatusFromItem(string item) =>
            $"{item} [{originalColors[item]}]";

        /// <inheritdoc/>
        public override string GetEntryFromItem(string item) =>
            $"{item} [{originalColors[item]}]";

        public override string GetInfoFromItem(string item)
        {
            var color = originalColors[item];
            return
                $"{LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_TUI_COLORTYPE")}: {item}\n" +
                $"{LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_TUI_COLOR")}: {color}\n" +
                $"{LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_TUI_COLORNAME")}: {color.Name}\n" +
                $"{LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_TUI_COLORHEX")}: {color.Hex}\n" +
                $"{LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_TUI_COLORBRIGHT")}: {color.Brightness}\n\n" +
                $"{ConsoleColoring.RenderSetConsoleColor(color)}- Lorem ipsum dolor sit amet, consectetur adipiscing elit.{ConsoleColoring.RenderRevertForeground()}";
        }

        internal void Change(string colorType)
        {
            // Requested to change the color type
            var color = ColorSelector.OpenColorSelector(originalColors[colorType]);
            originalColors[colorType] = color;
        }

        internal void Load()
        {
            var choices = new InputChoiceInfo[]
            {
                new("1", LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_LOADFROM")),
                new("2", LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_LOADFROMBUILTIN")),
                new("3", LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_LOADCURRENT")),
            };
            int choice = InfoBoxSelectionColor.WriteInfoBoxSelection(choices, LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_TUI_HOWLOAD"));
            if (choice < 0)
                return;
            switch (choice)
            {
                case 0:
                    {
                        // Load Theme From File...
                        DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for theme name...");
                        string AltThemeName = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_THEMEFILEPROMPT")) + ".json";
                        DebugWriter.WriteDebug(DebugLevel.I, "Got theme name {0}.", vars: [AltThemeName]);
                        LoadThemeFromFile(AltThemeName);
                        break;
                    }
                case 1:
                    {
                        // Load Theme From Prebuilt Themes...
                        DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for theme name...");
                        string AltThemeName = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_THEMENAMEPROMPT"));
                        DebugWriter.WriteDebug(DebugLevel.I, "Got theme name {0}.", vars: [AltThemeName]);
                        LoadThemeFromResource(AltThemeName);
                        break;
                    }
                case 2:
                    {
                        // Load Current Colors
                        DebugWriter.WriteDebug(DebugLevel.I, "Loading current colors...");
                        LoadThemeFromCurrentColors();
                        break;
                    }
            }
        }

        internal void Copy(string colorType)
        {
            var sourceColor = originalColors[colorType];

            // Specify the target...
            var sources = originalColors.Select((kvp, idx) => new InputChoiceInfo($"{idx + 1}", $"{kvp.Key}")).ToArray();
            int[] targetColors = InfoBoxSelectionMultipleColor.WriteInfoBoxSelectionMultiple([.. sources], LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_TARGETTYPEPROMPT").FormatString(colorType));
            if (targetColors.Length == 0)
                return;

            // Copy the color
            foreach (int idx in targetColors)
            {
                var targetType = originalColors.ElementAt(idx).Key;
                originalColors[targetType] = sourceColor;
            }
        }

        internal void SaveThemeToCurrentDirectory() =>
            SaveThemeToCurrentDirectory(themeName);

        internal void SaveThemeToAnotherDirectoryPrompt()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for directory name...");
            string DirectoryName = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_SAVETODIRPROMPT") + " [{0}] ", vars: [FilesystemTools.CurrentDir]);
            DirectoryName = string.IsNullOrWhiteSpace(DirectoryName) ? FilesystemTools.CurrentDir : DirectoryName;
            DebugWriter.WriteDebug(DebugLevel.I, "Got directory name {0}.", vars: [DirectoryName]);
            SaveThemeToAnotherDirectory(themeName, DirectoryName);
        }

        internal void SaveThemeToCurrentDirectoryAltPrompt()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for theme name...");
            string AltThemeName = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_THEMENAMEPROMPT") + " [{0}] ", vars: [themeName]);
            AltThemeName = string.IsNullOrWhiteSpace(AltThemeName) ? themeName : AltThemeName;
            DebugWriter.WriteDebug(DebugLevel.I, "Got theme name {0}.", vars: [AltThemeName]);
            SaveThemeToCurrentDirectory(AltThemeName);
        }

        internal void SaveThemeToAnotherDirectoryAltPrompt()
        {
            // TODO: NKS_THEMESTUDIO_APP_THEMEDIR -> Enter target directory and theme name below.
            DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for theme and directory name...");
            InputModule[] dirTheme =
            [
                new TextBoxModule()
                {
                    // TODO: NKS_THEMESTUDIO_APP_SAVETODIR -> Target directory
                    Name = LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_SAVETODIR"),
                    Description = LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_SAVETODIRPROMPT"),
                    Value = FilesystemTools.CurrentDir,
                },
                new TextBoxModule()
                {
                    // TODO: NKS_THEMESTUDIO_APP_THEMENAME -> Theme name
                    Name = LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_THEMENAME"),
                    Description = LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_THEMENAMEPROMPT"),
                    Value = themeName,
                },
            ];
            bool done = InfoBoxMultiInputColor.WriteInfoBoxMultiInput(dirTheme, LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_THEMEDIR"), Settings.InfoBoxSettings);
            if (!done)
                return;

            // Parse the directory name and alt theme name
            string DirectoryName = dirTheme[0].GetValue<string>() ?? "";
            string AltThemeName = dirTheme[1].GetValue<string>() ?? "";
            DirectoryName = string.IsNullOrWhiteSpace(DirectoryName) ? FilesystemTools.CurrentDir : DirectoryName;
            AltThemeName = string.IsNullOrWhiteSpace(AltThemeName) ? themeName : AltThemeName;
            DebugWriter.WriteDebug(DebugLevel.I, "Got directory name {0}.", vars: [DirectoryName]);
            DebugWriter.WriteDebug(DebugLevel.I, "Got theme name {0}.", vars: [AltThemeName]);
            SaveThemeToAnotherDirectory(AltThemeName, DirectoryName);
        }

        internal void LoadThemeFromFilePrompt()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for theme name...");
            string AltThemeName = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_THEMEFILEPROMPT")) + ".json";
            DebugWriter.WriteDebug(DebugLevel.I, "Got theme name {0}.", vars: [AltThemeName]);
            LoadThemeFromFile(AltThemeName);
        }

        internal void LoadThemeFromResourcePrompt()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for theme name...");
            string AltThemeName = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_THEMENAMEPROMPT"));
            DebugWriter.WriteDebug(DebugLevel.I, "Got theme name {0}.", vars: [AltThemeName]);
            LoadThemeFromResource(AltThemeName);
        }

        internal void LoadThemeFromCurrentColors()
        {
            // Place information to the studio
            for (int typeIndex = 0; typeIndex < Enum.GetValues<ThemeColorType>().Length; typeIndex++)
            {
                string type = originalColors.Keys.ElementAt(typeIndex);
                originalColors[type] = ThemeColorsTools.GetColor(type);
            }
        }

        private void SaveThemeToCurrentDirectory(string Theme)
        {
            var ThemeJson = GetThemeJson(Theme);
            FilesystemTools.WriteContentsText(FilesystemTools.NeutralizePath(Theme + ".json"), ThemeJson);
        }

        private void SaveThemeToAnotherDirectory(string Theme, string Path)
        {
            var ThemeJson = GetThemeJson(Theme);
            FilesystemTools.WriteContentsText(FilesystemTools.NeutralizePath(Path + "/" + Theme + ".json"), ThemeJson);
        }

        private string GetThemeJson(string theme)
        {
            var ThemeInfo = new ThemeInfo(
                JToken.Parse($$"""
                {
                    "Metadata": {
                        "Name": "{{theme}}"
                    }
                }
                """));
            foreach (var originalColor in originalColors.Keys)
                ThemeInfo.SetColor(originalColor, originalColors[originalColor]);
            var ThemeJson = ThemeInfo.ExportToJson();
            return JsonConvert.SerializeObject(ThemeJson, Formatting.Indented);
        }

        private void LoadThemeFromResource(string Theme) =>
            LoadThemeFromThemeInfo(ThemeTools.GetThemeInfo(Theme));

        private void LoadThemeFromFile(string Theme)
        {
            // Populate theme info
            var ThemeInfo = new ThemeInfo(FilesystemTools.NeutralizePath(Theme));
            LoadThemeFromThemeInfo(ThemeInfo);
        }

        private void LoadThemeFromThemeInfo(ThemeInfo themeInfo)
        {
            // Place information to the studio
            for (int typeIndex = 0; typeIndex < Enum.GetValues<ThemeColorType>().Length; typeIndex++)
            {
                string type = originalColors.Keys.ElementAt(typeIndex);
                originalColors[type] = ThemeTools.GetColorsFromTheme(themeInfo)[type];
            }
        }
    }
}
