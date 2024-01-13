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
    /// Converts the color YIQ numbers to HSV.
    /// </summary>
    /// <remarks>
    /// If you want to get the HSV representation of the color from the YIQ color numbers, you can use this command.
    /// </remarks>
    class ColorYiqToHsvCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Check to see if we have the numeric arguments
            if (!double.TryParse(parameters.ArgumentsList[0], out double Y))
            {
                TextWriters.Write(Translate.DoTranslation("The Y component level must be numeric."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Color;
            }
            if (!double.TryParse(parameters.ArgumentsList[1], out double I))
            {
                TextWriters.Write(Translate.DoTranslation("The I component level must be numeric."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Color;
            }
            if (!double.TryParse(parameters.ArgumentsList[2], out double Q))
            {
                TextWriters.Write(Translate.DoTranslation("The Q component level must be numeric."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Color;
            }

            // Do the job
            var yiq = new Color($"yiq:{Y};{I};{Q}");
            var hsv = HsvConversionTools.ConvertFrom(yiq.RGB);
            TextWriters.Write("- " + Translate.DoTranslation("Hue:") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write($"{hsv.HueWhole} [{hsv.Hue:0.00}]", true, KernelColorType.ListValue);
            TextWriters.Write("- " + Translate.DoTranslation("Saturation:") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write($"{hsv.SaturationWhole} [{hsv.Saturation:0.00}]", true, KernelColorType.ListValue);
            TextWriters.Write("- " + Translate.DoTranslation("Value:") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write($"{hsv.ValueWhole} [{hsv.Value:0.00}]", true, KernelColorType.ListValue);
            variableValue = $"hsv:{hsv.HueWhole};{hsv.SaturationWhole};{hsv.ValueWhole}";
            return 0;
        }

    }
}
