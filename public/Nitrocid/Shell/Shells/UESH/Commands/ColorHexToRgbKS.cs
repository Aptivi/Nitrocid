
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
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Converts the hexadecimal representation of the color to RGB numbers in KS format.
    /// </summary>
    /// <remarks>
    /// If you want to get the semicolon-delimited sequence of the RGB color numbers from the hexadecimal representation of the color, you can use this command. You can use this to form a complete VT sequence of changing color.
    /// </remarks>
    class ColorHexToRgbKSCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            string Hex = ListArgsOnly[0];
            string RGB;

            // Do the job
            RGB = KernelColorTools.ConvertFromHexToRGB(Hex);
            TextWriterColor.Write("- " + Translate.DoTranslation("RGB color sequence:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(RGB, true, KernelColorType.ListValue);
        }

    }
}