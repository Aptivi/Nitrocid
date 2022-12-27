
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
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

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

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            string Hex = ListArgsOnly[0];
            int R = default, G = default, B = default;

            // Do the job
            Hex.ConvertFromHexToRgb(ref R, ref G, ref B);
            TextWriterColor.Write("- " + Translate.DoTranslation("Red color level:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.Write($"{R}", true, KernelColorType.ListValue);
            TextWriterColor.Write("- " + Translate.DoTranslation("Green color level:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.Write($"{G}", true, KernelColorType.ListValue);
            TextWriterColor.Write("- " + Translate.DoTranslation("Blue color level:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.Write($"{B}", true, KernelColorType.ListValue);
        }

    }
}