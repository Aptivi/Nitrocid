
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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Themes;
using KS.Files;
using KS.Files.Querying;
using KS.Kernel.Configuration;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Sets your theme
    /// </summary>
    /// <remarks>
    /// You can personalize your kernel using themes, which contains the color sets to set colors.
    /// </remarks>
    class SetThemesCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (Shell.ColoredShell)
            {
                // Try to apply the theme
                string ThemePath = Filesystem.NeutralizePath(ListArgsOnly[0]);
                if (Checking.FileExists(ThemePath))
                {
                    ThemeTools.ApplyThemeFromFile(ThemePath);
                }
                else
                {
                    ThemeTools.ApplyThemeFromResources(ListArgsOnly[0]);
                }

                // Save it to configuration
                Config.CreateConfig();
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Colors are not available. Turn on colored shell in the kernel config."), true, ColorTools.ColTypes.NeutralText);
            }
        }

        public override void HelpHelper() => TextWriterColor.Write("<Theme>: ThemeName.json, " + string.Join(", ", ThemeTools.Themes.Keys), true, ColorTools.ColTypes.NeutralText);

    }
}