//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using KS.Languages;
using KS.Shell.ShellBase.Commands;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Shells;
using KS.ConsoleBase.Writers.FancyWriters;

namespace KS.Shell.Shells.Debug.Commands
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
            SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("List of shells"), true);

            // List all the available shells
            var shellNames = ShellManager.AvailableShells.Keys;
            ListWriterColor.WriteList(shellNames);
            return 0;
        }

        public override int ExecuteDumb(CommandParameters parameters, ref string variableValue)
        {
            TextWriterColor.Write(Translate.DoTranslation("List of shells"));

            // List all the available shells
            var shellNames = ShellManager.AvailableShells.Keys;
            foreach (string shellName in shellNames)
                TextWriterColor.Write($"  - {shellName}");
            return 0;
        }

    }
}
