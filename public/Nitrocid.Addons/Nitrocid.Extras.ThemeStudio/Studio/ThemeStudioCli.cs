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
using Nitrocid.Files.Folders;
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
                $"{Translate.DoTranslation("Color type")}: {item}\n" +
                $"{Translate.DoTranslation("Color")}: {color}\n" +
                $"{Translate.DoTranslation("Color name")}: {color.Name}\n" +
                $"{Translate.DoTranslation("Color hex")}: {color.Hex}\n" +
                $"{Translate.DoTranslation("Color brightness")}: {color.Brightness}\n\n" +
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
                new("1", Translate.DoTranslation("Save Theme to Current Directory")),
                new("2", Translate.DoTranslation("Save Theme to Another Directory...")),
                new("3", Translate.DoTranslation("Save Theme to Current Directory as...")),
                new("4", Translate.DoTranslation("Save Theme to Another Directory as...")),
            };
            int choice = InfoBoxSelectionColor.WriteInfoBoxSelection(choices, Translate.DoTranslation("Choose how do you want to save this theme"));
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
                        string DirectoryName = InfoBoxInputColor.WriteInfoBoxInput(Translate.DoTranslation("Specify directory to save theme to:") + " [{0}] ", vars: [CurrentDirectory.CurrentDir]);
                        DirectoryName = string.IsNullOrWhiteSpace(DirectoryName) ? CurrentDirectory.CurrentDir : DirectoryName;
                        DebugWriter.WriteDebug(DebugLevel.I, "Got directory name {0}.", DirectoryName);
                        ThemeStudioTools.SaveThemeToAnotherDirectory(themeName, DirectoryName);
                        break;
                    }
                case 2:
                    {
                        // Save theme to current directory as...
                        DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for theme name...");
                        string AltThemeName = InfoBoxInputColor.WriteInfoBoxInput(Translate.DoTranslation("Specify theme name:") + " [{0}] ", vars: [themeName]);
                        AltThemeName = string.IsNullOrWhiteSpace(AltThemeName) ? themeName : AltThemeName;
                        DebugWriter.WriteDebug(DebugLevel.I, "Got theme name {0}.", AltThemeName);
                        ThemeStudioTools.SaveThemeToCurrentDirectory(AltThemeName);
                        break;
                    }
                case 3:
                    {
                        // Save theme to another directory as...
                        DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for theme and directory name...");
                        string DirectoryName = InfoBoxInputColor.WriteInfoBoxInput(Translate.DoTranslation("Specify directory to save theme to:") + " [{0}] ", vars: [CurrentDirectory.CurrentDir]);
                        DirectoryName = string.IsNullOrWhiteSpace(DirectoryName) ? CurrentDirectory.CurrentDir : DirectoryName;
                        DebugWriter.WriteDebug(DebugLevel.I, "Got directory name {0}.", DirectoryName);
                        DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for theme name...");
                        string AltThemeName = InfoBoxInputColor.WriteInfoBoxInput(Translate.DoTranslation("Specify theme name:") + " [{0}] ", vars: [themeName]);
                        AltThemeName = string.IsNullOrWhiteSpace(AltThemeName) ? themeName : AltThemeName;
                        DebugWriter.WriteDebug(DebugLevel.I, "Got theme name {0}.", AltThemeName);
                        ThemeStudioTools.SaveThemeToAnotherDirectory(AltThemeName, DirectoryName);
                        break;
                    }
            }
        }

        internal void Load()
        {
            var choices = new InputChoiceInfo[]
            {
                new("1", Translate.DoTranslation("Load Theme From File...")),
                new("2", Translate.DoTranslation("Load Theme From Prebuilt Themes...")),
                new("3", Translate.DoTranslation("Load Current Colors")),
            };
            int choice = InfoBoxSelectionColor.WriteInfoBoxSelection(choices, Translate.DoTranslation("Choose how do you want to load a theme"));
            if (choice < 0)
                return;
            switch (choice)
            {
                case 0:
                    {
                        // Load Theme From File...
                        DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for theme name...");
                        string AltThemeName = InfoBoxInputColor.WriteInfoBoxInput(Translate.DoTranslation("Specify theme file name wihout the .json extension:")) + ".json";
                        DebugWriter.WriteDebug(DebugLevel.I, "Got theme name {0}.", AltThemeName);
                        ThemeStudioTools.LoadThemeFromFile(AltThemeName);
                        break;
                    }
                case 1:
                    {
                        // Load Theme From Prebuilt Themes...
                        DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for theme name...");
                        string AltThemeName = InfoBoxInputColor.WriteInfoBoxInput(Translate.DoTranslation("Specify theme name:"));
                        DebugWriter.WriteDebug(DebugLevel.I, "Got theme name {0}.", AltThemeName);
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
            int[] targetColors = InfoBoxSelectionMultipleColor.WriteInfoBoxSelectionMultiple([.. sources], Translate.DoTranslation("Select the target color types to copy the source color type, {0}, to.").FormatString(colorType));
            if (targetColors.Length == 0)
                return;

            // Copying...
            foreach (int idx in targetColors)
            {
                var targetType = (KernelColorType)idx;
                originalColors[targetType] = sourceColor;
            }
        }
    }
}
