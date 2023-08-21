
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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using Terminaux.Colors;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Converts the hexadecimal representation of the color to RGB numbers.
    /// </summary>
    /// <remarks>
    /// If you want to get the RGB color numbers from the hexadecimal representation of the color, you can use this command.
    /// </remarks>
    class ColorHexToRgbCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, ref string variableValue)
        {
            string Hex = ListArgsOnly[0];

            // Do the job
            Color color = new(Hex);
            TextWriterColor.Write("- " + Translate.DoTranslation("Red color level:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.Write($"{color.R}", true, KernelColorType.ListValue);
            TextWriterColor.Write("- " + Translate.DoTranslation("Green color level:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.Write($"{color.G}", true, KernelColorType.ListValue);
            TextWriterColor.Write("- " + Translate.DoTranslation("Blue color level:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.Write($"{color.B}", true, KernelColorType.ListValue);
            variableValue = color.PlainSequence;
            return 0;
        }

    }
}
