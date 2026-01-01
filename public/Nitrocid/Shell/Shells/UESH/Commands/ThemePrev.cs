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

using Terminaux.Inputs.Styles.Selection;
using Nitrocid.ConsoleBase.Themes;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Files;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using Terminaux.Inputs.Styles;
using Nitrocid.ConsoleBase.Colors;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Previews a theme
    /// </summary>
    /// <remarks>
    /// This comand lets you preview a theme in an interactive way.
    /// </remarks>
    class ThemePrevCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string selectedTheme;
            string ThemePath;

            bool bail = false;
            int step = 1;
            string[] categoryNames = Enum.GetNames(typeof(ThemeCategory));
            int categoryIndex = 0;
            while (!bail)
            {
                // Selected theme null for now
                bool proceed = false;
                selectedTheme = parameters.ArgumentsList.Length > 0 ? parameters.ArgumentsList[0] : "";
                if (parameters.ArgumentsList.Length == 0)
                {
                    if (step == 1)
                    {
                        while (true)
                        {
                            // Let the user select a theme category
                            List<InputChoiceInfo> themeCategoryChoices = [];
                            List<InputChoiceInfo> themeCategoryAltChoices =
                            [
                                new($"{categoryNames.Length + 1}", Translate.DoTranslation("Exit"))
                            ];
                            for (int i = 0; i < categoryNames.Length; i++)
                            {
                                string category = categoryNames[i];
                                var ici = new InputChoiceInfo(
                                    $"{i + 1}",
                                    $"{category}"
                                );
                                themeCategoryChoices.Add(ici);
                            }
                            categoryIndex = SelectionStyle.PromptSelection(Translate.DoTranslation("Select a category"), [.. themeCategoryChoices], [.. themeCategoryAltChoices]) - 1;

                            // If the color index is -2, exit. PromptSelection returns -1 if ESC is pressed to cancel selecting. However, the index just decreases to -2
                            // even if that PromptSelection returned the abovementioned value, so bail if index is -2
                            if (categoryIndex == -2 || categoryIndex >= categoryNames.Length)
                            {
                                KernelColorTools.LoadBackground();
                                return 3;
                            }
                            else
                                break;
                        }
                        step = 2;
                    }
                    else
                    {
                        while (true)
                        {
                            // Let the user select a theme
                            var finalCategory = Enum.Parse<ThemeCategory>(categoryNames[categoryIndex]);
                            List<InputChoiceInfo> themeChoices = [];
                            List<InputChoiceInfo> themeAltChoices =
                            [
                                new("<--", Translate.DoTranslation("Back"))
                            ];
                            foreach (string theme in ThemeTools.GetInstalledThemesByCategory(finalCategory).Keys)
                            {
                                var themeInstance = ThemeTools.GetThemeInfo(theme);
                                string name = themeInstance.Name;
                                string desc = themeInstance.Localizable ? Translate.DoTranslation(themeInstance.Description) : themeInstance.Description;
                                var ici = new InputChoiceInfo(
                                    theme,
                                    $"{name}{(themeInstance.IsEvent ? $" - [{themeInstance.StartMonth}/{themeInstance.StartDay} -> {themeInstance.EndMonth}/{themeInstance.EndDay} / {(themeInstance.IsExpired ? Translate.DoTranslation("Expired") : Translate.DoTranslation("Available"))}]" : "")}",
                                    desc
                                );
                                themeChoices.Add(ici);
                            }
                            int colorIndex = SelectionStyle.PromptSelection(Translate.DoTranslation("Select a theme"), [.. themeChoices], [.. themeAltChoices]) - 1;

                            // If the color index is -2, exit. PromptSelection returns -1 if ESC is pressed to cancel selecting. However, the index just decreases to -2
                            // even if that PromptSelection returned the abovementioned value, so bail if index is -2
                            if (colorIndex == -2)
                            {
                                KernelColorTools.LoadBackground();
                                return 3;
                            }
                            else if (colorIndex < themeChoices.Count)
                            {
                                // Get the theme name from index
                                proceed = true;
                                selectedTheme = ThemeTools.GetInstalledThemesByCategory(finalCategory).Keys.ElementAt(colorIndex);
                                break;
                            }
                            else
                                break;
                        }
                        step = 1;
                    }
                }
                else
                {
                    proceed = true;
                    bail = true;
                }

                if (!proceed)
                    continue;
                step = 2;

                // Load the theme to the instance
                ThemePath = FilesystemTools.NeutralizePath(selectedTheme);
                ThemeInfo Theme;
                if (FilesystemTools.FileExists(ThemePath))
                    Theme = new ThemeInfo(ThemePath);
                else
                    Theme = ThemeTools.GetThemeInfo(selectedTheme, true);

                // Now, preview the theme
                ThemePreviewTools.PreviewTheme(Theme);
            }
            return 0;
        }

        public override void HelpHelper() =>
            TextWriterColor.Write("<Theme>: ThemeName.json, " + string.Join(", ", ThemeTools.GetInstalledThemes().Keys));

    }
}
