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
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using Terminaux.Colors;

namespace Nitrocid.Extras.ColorConvert.Commands
{
    /// <summary>
    /// Converts the hexadecimal representation of the color to HSV numbers.
    /// </summary>
    /// <remarks>
    /// If you want to get the HSV color numbers from the hexadecimal representation of the color, you can use this command.
    /// </remarks>
    class ColorHexToHsvCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string Hex = parameters.ArgumentsList[0];

            // Do the job
            Color color = new(Hex);
            var hsv = color.HSV;
            TextWriterColor.WriteKernelColor("- " + Translate.DoTranslation("Hue:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.WriteKernelColor($"{hsv.HueWhole} [{hsv.Hue:0.00}]", true, KernelColorType.ListValue);
            TextWriterColor.WriteKernelColor("- " + Translate.DoTranslation("Saturation:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.WriteKernelColor($"{hsv.SaturationWhole} [{hsv.Saturation:0.00}]", true, KernelColorType.ListValue);
            TextWriterColor.WriteKernelColor("- " + Translate.DoTranslation("Value:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.WriteKernelColor($"{hsv.ValueWhole} [{hsv.Value:0.00}]", true, KernelColorType.ListValue);
            variableValue = $"hsv:{hsv.HueWhole};{hsv.SaturationWhole};{hsv.ValueWhole}";
            return 0;
        }

    }
}
