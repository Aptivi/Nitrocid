
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Misc.Probers.Placeholder;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Echoes the text
    /// </summary>
    /// <remarks>
    /// This command will repeat back the string that you have entered. It is used in scripting to print text. It supports $variable parsing.
    /// </remarks>
    class EchoCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, ref string variableValue)
        {
            bool parsePlaces = !SwitchManager.ContainsSwitch(ListSwitchesOnly, "-noparse");
            if (ListSwitchesOnly.Length == 0)
                parsePlaces = true;
            string result = parsePlaces ? PlaceParse.ProbePlaces(StringArgs) : StringArgs;
            TextWriterColor.Write(result);
            variableValue = result;
            return 0;
        }
    }
}
