﻿//
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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using Terminaux.Colors;
using Terminaux.Colors.Models.Conversion;

namespace Nitrocid.Extras.ColorConvert.Commands
{
    /// <summary>
    /// Converts the color RYB numbers to HSL.
    /// </summary>
    /// <remarks>
    /// If you want to get the HSL representation of the color from the RYB color numbers, you can use this command.
    /// </remarks>
    class ColorRybToHslCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Check to see if we have the numeric arguments
            if (!int.TryParse(parameters.ArgumentsList[0], out int R))
            {
                TextWriters.Write(Translate.DoTranslation("The red color level must be numeric."), true, KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Color);
            }
            if (!int.TryParse(parameters.ArgumentsList[1], out int Y))
            {
                TextWriters.Write(Translate.DoTranslation("The yellow color level must be numeric."), true, KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Color);
            }
            if (!int.TryParse(parameters.ArgumentsList[2], out int B))
            {
                TextWriters.Write(Translate.DoTranslation("The blue color level must be numeric."), true, KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Color);
            }

            // Do the job
            var ryb = new Color($"ryb:{R};{Y};{B}");
            var hsl = HslConversionTools.ConvertFrom(ryb.RGB);
            TextWriters.Write("- " + Translate.DoTranslation("Hue:") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write($"{hsl.HueWhole} [{hsl.Hue:0.00}]", true, KernelColorType.ListValue);
            TextWriters.Write("- " + Translate.DoTranslation("Saturation:") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write($"{hsl.SaturationWhole} [{hsl.Saturation:0.00}]", true, KernelColorType.ListValue);
            TextWriters.Write("- " + Translate.DoTranslation("Lumiance (Lightness):") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write($"{hsl.LightnessWhole} [{hsl.Lightness:0.00}]", true, KernelColorType.ListValue);
            variableValue = $"hsl:{hsl.HueWhole};{hsl.SaturationWhole};{hsl.LightnessWhole}";
            return 0;
        }

    }
}