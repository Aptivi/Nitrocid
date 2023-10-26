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
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using Terminaux.Colors;

namespace Nitrocid.Extras.ColorConvert.Commands
{
    /// <summary>
    /// Converts the color CMYK numbers to HSL.
    /// </summary>
    /// <remarks>
    /// If you want to get the HSL representation of the color from the CMYK color numbers, you can use this command.
    /// </remarks>
    class ColorCmykToHslCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Check to see if we have the numeric arguments
            if (!int.TryParse(parameters.ArgumentsList[0], out int C))
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("The cyan color level must be numeric."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Color;
            }
            if (!int.TryParse(parameters.ArgumentsList[1], out int M))
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("The magenta color level must be numeric."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Color;
            }
            if (!int.TryParse(parameters.ArgumentsList[2], out int Y))
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("The yellow color level must be numeric."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Color;
            }
            if (!int.TryParse(parameters.ArgumentsList[3], out int K))
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("The black key level must be numeric."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Color;
            }

            // Do the job
            var rgb = new Color($"cmyk:{C};{M};{Y};{K}");
            var hsl = rgb.HSL;
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
