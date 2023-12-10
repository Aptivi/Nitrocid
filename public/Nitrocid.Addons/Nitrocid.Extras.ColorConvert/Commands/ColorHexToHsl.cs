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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using Terminaux.Colors;

namespace Nitrocid.Extras.ColorConvert.Commands
{
    /// <summary>
    /// Converts the hexadecimal representation of the color to HSL numbers.
    /// </summary>
    /// <remarks>
    /// If you want to get the HSL color numbers from the hexadecimal representation of the color, you can use this command.
    /// </remarks>
    class ColorHexToHslCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string Hex = parameters.ArgumentsList[0];

            // Do the job
            Color color = new(Hex);
            var hsl = color.HSL;
            TextWriterColor.WriteKernelColor("- " + Translate.DoTranslation("Hue:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.WriteKernelColor($"{hsl.HueWhole} [{hsl.Hue:0.00}]", true, KernelColorType.ListValue);
            TextWriterColor.WriteKernelColor("- " + Translate.DoTranslation("Saturation:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.WriteKernelColor($"{hsl.SaturationWhole} [{hsl.Saturation:0.00}]", true, KernelColorType.ListValue);
            TextWriterColor.WriteKernelColor("- " + Translate.DoTranslation("Lumiance (Lightness):") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.WriteKernelColor($"{hsl.LightnessWhole} [{hsl.Lightness:0.00}]", true, KernelColorType.ListValue);
            variableValue = $"hsl:{hsl.HueWhole};{hsl.SaturationWhole};{hsl.LightnessWhole}";
            return 0;
        }

    }
}
