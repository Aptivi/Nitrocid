//
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Inputs.Styles;
using KS.ConsoleBase.Themes;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Files;
using KS.Files.Operations.Querying;
using KS.Kernel.Configuration;
using KS.Languages;
using KS.Misc.Text;
using KS.Shell.ShellBase.Commands;
using System.Collections.Generic;
using System.Linq;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Selects a theme and sets it
    /// </summary>
    /// <remarks>
    /// You can personalize your kernel using themes, which contains the color sets to set colors.
    /// </remarks>
    class ThemeSetCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string answer = "";
            string selectedTheme = "";
            string ThemePath = "";

            bool bail = false;
            while (answer != "y" && !bail)
            {
                // Selected theme null for now
                selectedTheme = parameters.ArgumentsList.Length > 0 ? parameters.ArgumentsList[0] : "";
                if (parameters.ArgumentsList.Length == 0)
                {
                    // Let the user select a theme
                    List<InputChoiceInfo> themeChoices = new();
                    foreach (string theme in ThemeTools.GetInstalledThemes().Keys)
                    {
                        var themeInstance = ThemeTools.GetThemeInfo(theme);
                        string name = themeInstance.Name;
                        string desc = themeInstance.Localizable ? Translate.DoTranslation(themeInstance.Description) : themeInstance.Description;
                        var ici = new InputChoiceInfo(theme,
                            $"{name}{(themeInstance.IsEvent ? $" - [{themeInstance.StartMonth}/{themeInstance.StartDay} -> {themeInstance.EndMonth}/{themeInstance.EndDay} / {(themeInstance.IsExpired ? Translate.DoTranslation("Expired") : Translate.DoTranslation("Available"))}]" : "")}", desc);
                        themeChoices.Add(ici);
                    }
                    int colorIndex = SelectionStyle.PromptSelection(Translate.DoTranslation("Select a theme"), themeChoices) - 1;

                    // If the color index is -2, exit. PromptSelection returns -1 if ESC is pressed to cancel selecting. However, the index just decreases to -2
                    // even if that PromptSelection returned the abovementioned value, so bail if index is -2
                    if (colorIndex == -2)
                    {
                        KernelColorTools.LoadBack();
                        return 3;
                    }

                    // Get the theme name from index
                    selectedTheme = ThemeTools.GetInstalledThemes().Keys.ElementAt(colorIndex);
                }

                // Load the theme to the instance
                ThemePath = FilesystemTools.NeutralizePath(selectedTheme);
                ThemeInfo Theme;
                if (Checking.FileExists(ThemePath))
                    Theme = new ThemeInfo(ThemePath);
                else
                    Theme = ThemeTools.GetThemeInfo(selectedTheme);

                // Now, preview the theme
                ThemePreviewTools.PreviewThemeSimple(Theme);
                TextWriterColor.Write();

                // Pause until a key is pressed
                answer = ChoiceStyle.PromptChoice(
                    TextTools.FormatString(Translate.DoTranslation("Would you like to set this theme?") + "\n{0}: {1}", selectedTheme, Theme.Localizable ? Translate.DoTranslation(Theme.Description) : Theme.Description), "y/n",
                    new[] {
                        Translate.DoTranslation("Yes, set it!"),
                        Translate.DoTranslation("No, don't set it.")
                    },
                    ChoiceOutputType.Modern
                );
                if (answer == "n" && parameters.ArgumentsList.Length > 0)
                    bail = true;
            }

            if (bail)
                return 0;

            // User answered yes, so set it
            if (Checking.FileExists(ThemePath))
                ThemeTools.ApplyThemeFromFile(ThemePath);
            else
                ThemeTools.ApplyThemeFromResources(selectedTheme);

            // Save it to configuration
            Config.CreateConfig();
            return 0;
        }

        public override void HelpHelper() =>
            TextWriterColor.Write("<Theme>: ThemeName.json, " + string.Join(", ", ThemeTools.GetInstalledThemes().Keys));

    }
}
