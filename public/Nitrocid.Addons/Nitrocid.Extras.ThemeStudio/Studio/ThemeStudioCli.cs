//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using System.Collections.Generic;
using System.Linq;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Files;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using Terminaux.Colors;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Infobox;
using Textify.General;

namespace Nitrocid.Extras.ThemeStudio.Studio
{
    internal class ThemeStudioCli : BaseInteractiveTui<KernelColorType>, IInteractiveTui<KernelColorType>
    {
        internal Dictionary<KernelColorType, Color> originalColors = [];
        internal string themeName = "";

        /// <inheritdoc/>
        public override IEnumerable<KernelColorType> PrimaryDataSource =>
            originalColors.Keys;

        /// <inheritdoc/>
        public override string GetStatusFromItem(KernelColorType item) =>
            $"{item} [{originalColors[item]}]";

        /// <inheritdoc/>
        public override string GetEntryFromItem(KernelColorType item) =>
            $"{item} [{originalColors[item]}]";

        public override string GetInfoFromItem(KernelColorType item)
        {
            var color = originalColors[item];
            return
                $"{LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_TUI_COLORTYPE")}: {item}\n" +
                $"{LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_TUI_COLOR")}: {color}\n" +
                $"{LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_TUI_COLORNAME")}: {color.Name}\n" +
                $"{LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_TUI_COLORHEX")}: {color.Hex}\n" +
                $"{LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_TUI_COLORBRIGHT")}: {color.Brightness}\n\n" +
                $"{ColorTools.RenderSetConsoleColor(color)}- Lorem ipsum dolor sit amet, consectetur adipiscing elit.{ColorTools.RenderRevertForeground()}";
        }

        internal void Change(object type)
        {
            // Requested to remove language
            var colorType = (KernelColorType)type;
            var color = ColorSelector.OpenColorSelector(originalColors[colorType]);
            originalColors[colorType] = color;
        }

        internal void Save()
        {
            foreach (var type in ThemeStudioTools.SelectedColors.Keys)
                ThemeStudioTools.SelectedColors[type] = originalColors[type];
            var choices = new InputChoiceInfo[]
            {
                new("1", LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_SAVETOCURRENT")),
                new("2", LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_SAVETOOTHER")),
                new("3", LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_SAVETOCURRENTAS")),
                new("4", LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_SAVETOOTHERAS")),
            };
            int choice = InfoBoxSelectionColor.WriteInfoBoxSelection(choices, LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_TUI_HOWSAVE"));
            if (choice < 0)
                return;
            switch (choice)
            {
                case 0:
                    {
                        // Save theme to current directory
                        ThemeStudioTools.SaveThemeToCurrentDirectory(themeName);
                        break;
                    }
                case 1:
                    {
                        // Save theme to another directory...
                        DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for directory name...");
                        string DirectoryName = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_SAVETODIRPROMPT") + " [{0}] ", vars: [FilesystemTools.CurrentDir]);
                        DirectoryName = string.IsNullOrWhiteSpace(DirectoryName) ? FilesystemTools.CurrentDir : DirectoryName;
                        DebugWriter.WriteDebug(DebugLevel.I, "Got directory name {0}.", vars: [DirectoryName]);
                        ThemeStudioTools.SaveThemeToAnotherDirectory(themeName, DirectoryName);
                        break;
                    }
                case 2:
                    {
                        // Save theme to current directory as...
                        DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for theme name...");
                        string AltThemeName = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_THEMENAMEPROMPT") + " [{0}] ", vars: [themeName]);
                        AltThemeName = string.IsNullOrWhiteSpace(AltThemeName) ? themeName : AltThemeName;
                        DebugWriter.WriteDebug(DebugLevel.I, "Got theme name {0}.", vars: [AltThemeName]);
                        ThemeStudioTools.SaveThemeToCurrentDirectory(AltThemeName);
                        break;
                    }
                case 3:
                    {
                        // Save theme to another directory as...
                        DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for theme and directory name...");
                        string DirectoryName = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_SAVETODIRPROMPT") + " [{0}] ", vars: [FilesystemTools.CurrentDir]);
                        DirectoryName = string.IsNullOrWhiteSpace(DirectoryName) ? FilesystemTools.CurrentDir : DirectoryName;
                        DebugWriter.WriteDebug(DebugLevel.I, "Got directory name {0}.", vars: [DirectoryName]);
                        DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for theme name...");
                        string AltThemeName = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_THEMENAMEPROMPT") + " [{0}] ", vars: [themeName]);
                        AltThemeName = string.IsNullOrWhiteSpace(AltThemeName) ? themeName : AltThemeName;
                        DebugWriter.WriteDebug(DebugLevel.I, "Got theme name {0}.", vars: [AltThemeName]);
                        ThemeStudioTools.SaveThemeToAnotherDirectory(AltThemeName, DirectoryName);
                        break;
                    }
            }
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
                        ThemeStudioTools.LoadThemeFromFile(AltThemeName);
                        break;
                    }
                case 1:
                    {
                        // Load Theme From Prebuilt Themes...
                        DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for theme name...");
                        string AltThemeName = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_THEMENAMEPROMPT"));
                        DebugWriter.WriteDebug(DebugLevel.I, "Got theme name {0}.", vars: [AltThemeName]);
                        ThemeStudioTools.LoadThemeFromResource(AltThemeName);
                        break;
                    }
                case 2:
                    {
                        // Load Current Colors
                        DebugWriter.WriteDebug(DebugLevel.I, "Loading current colors...");
                        ThemeStudioTools.LoadThemeFromCurrentColors();
                        break;
                    }
            }
        }

        internal void Copy(object type)
        {
            var colorType = (KernelColorType)type;
            var sourceColor = originalColors[colorType];

            // Specify the target...
            var sources = originalColors.Select((kvp, idx) => new InputChoiceInfo($"{idx + 1}", $"{kvp.Key}")).ToArray();
            int[] targetColors = InfoBoxSelectionMultipleColor.WriteInfoBoxSelectionMultiple([.. sources], LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_TARGETTYPEPROMPT").FormatString(colorType));
            if (targetColors.Length == 0)
                return;

            // FilesystemTools...
            foreach (int idx in targetColors)
            {
                var targetType = (KernelColorType)idx;
                originalColors[targetType] = sourceColor;
            }
        }
    }
}
