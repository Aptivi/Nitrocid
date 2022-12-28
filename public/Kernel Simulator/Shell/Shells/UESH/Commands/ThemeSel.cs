
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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

using Extensification.StringExts;
using KS.ConsoleBase.Inputs.Styles;
using KS.ConsoleBase.Themes;
using KS.Files;
using KS.Files.Querying;
using KS.Kernel.Configuration;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Selects a theme and sets it
    /// </summary>
    /// <remarks>
    /// You can personalize your kernel using themes, which contains the color sets to set colors.
    /// </remarks>
    class ThemeSelCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            // Selected theme null for now
            string selectedTheme = ListArgsOnly.Length > 0 ? ListArgsOnly[0] : "";
            if (ListArgsOnly.Length == 0)
            {
                // Let the user select a theme
                List<string> themeAnswers = new();
                List<string> themeWorkingNames = new();
                foreach (string theme in ThemeTools.Themes.Keys)
                {
                    themeAnswers.Add(theme);
                    themeWorkingNames.Add(ThemeTools.Themes[theme].Name);
                }
                int colorIndex = SelectionStyle.PromptSelection(Translate.DoTranslation("Select a theme"), string.Join("/", themeAnswers), themeWorkingNames.ToArray()) - 1;

                // If the color index is -2, exit. PromptSelection returns -1 if ESC is pressed to cancel selecting. However, the index just decreases to -2
                // even if that PromptSelection returned the abovementioned value, so bail if index is -2
                if (colorIndex == -2)
                    return;

                // Get the theme name from index
                selectedTheme = ThemeTools.Themes.Keys.ElementAt(colorIndex);
            }

            // Load the theme to the instance
            string ThemePath = Filesystem.NeutralizePath(selectedTheme);
            ThemeInfo Theme;
            if (Checking.FileExists(ThemePath))
            {
                var ThemeStream = new StreamReader(ThemePath);
                Theme = new ThemeInfo(ThemeStream);
            }
            else
                Theme = ThemeTools.GetThemeInfo(selectedTheme);

            // Now, preview the theme
            ThemeTools.PreviewTheme(Theme);
            TextWriterColor.Write();

            // Pause until a key is pressed
            string answer = ChoiceStyle.PromptChoice(Translate.DoTranslation("Would you like to set this theme to {0}?").FormatString(selectedTheme), "y/n", new[] { Translate.DoTranslation("Yes, set it!"), Translate.DoTranslation("No, don't set it.") }, ChoiceStyle.ChoiceOutputType.Modern);
            if (answer == "y")
            {
                // User answered yes, so set it
                if (Checking.FileExists(ThemePath))
                    ThemeTools.ApplyThemeFromFile(ThemePath);
                else
                    ThemeTools.ApplyThemeFromResources(selectedTheme);

                // Save it to configuration
                Config.CreateConfig();
            }
        }

        public override void HelpHelper() => TextWriterColor.Write("<Theme>: ThemeName.json, " + string.Join(", ", ThemeTools.Themes.Keys));

    }
}
