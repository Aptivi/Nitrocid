
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

using Extensification.StringExts;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs.Styles;
using KS.ConsoleBase.Themes;
using KS.ConsoleBase.Themes.Studio;
using KS.Files;
using KS.Files.Querying;
using KS.Kernel.Configuration;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;
using System.Linq;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Selects a theme and sets it
    /// </summary>
    /// <remarks>
    /// You can personalize your kernel using themes, which contains the color sets to set colors.
    /// </remarks>
    class ThemeSelCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (Shell.ColoredShell)
            {
                // Selected theme null for now
                string selectedTheme = ListArgsOnly.Length > 0 ? ListArgsOnly[0] : "";
                if (ListArgsOnly.Length == 0)
                {
                    // Let the user select a theme
                    int colorIndex = SelectionStyle.PromptSelection(Translate.DoTranslation("Select a theme"), string.Join("/", ThemeTools.Themes.Keys)) - 1;

                    // Get the theme name from index
                    selectedTheme = ThemeTools.Themes.Keys.ElementAt(colorIndex);
                }

                // Try to load the theme to the theme studio
                string ThemePath = Filesystem.NeutralizePath(selectedTheme);
                if (Checking.FileExists(ThemePath))
                    ThemeStudioTools.LoadThemeFromFile(ThemePath);
                else
                    ThemeStudioTools.LoadThemeFromResource(selectedTheme);

                // Load the preview
                ThemeStudioTools.PreparePreview();

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
            else
                TextWriterColor.Write(Translate.DoTranslation("Colors are not available. Turn on colored shell in the kernel config."), true, ColorTools.ColTypes.NeutralText);
        }

        public override void HelpHelper() => TextWriterColor.Write("<Theme>: ThemeName.json, " + string.Join(", ", ThemeTools.Themes.Keys), true, ColorTools.ColTypes.NeutralText);

    }
}
