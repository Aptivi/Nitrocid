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

using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Nitrocid.Languages;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Colors.Themes.Colors;
using Nitrocid.ConsoleBase.Writers;

namespace Nitrocid.Shell.Shells.Debug.Commands
{
    /// <summary>
    /// You can list all the available shells
    /// </summary>
    /// <remarks>
    /// This command lets you list all the available shells that either Nitrocid KS registered or your custom mods registered.
    /// </remarks>
    class LsShellsCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_DEBUG_LSSHELLS_TITLE"), ThemeColorsTools.GetColor(ThemeColorType.ListTitle));

            // List all the available shells
            var shellNames = ShellManager.AvailableShells.Keys;
            TextWriters.WriteList(shellNames);
            return 0;
        }

        public override int ExecuteDumb(CommandParameters parameters, ref string variableValue)
        {
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_DEBUG_LSSHELLS_TITLE"));

            // List all the available shells
            var shellNames = ShellManager.AvailableShells.Keys;
            foreach (string shellName in shellNames)
                TextWriterColor.Write($"  - {shellName}");
            return 0;
        }

    }
}
