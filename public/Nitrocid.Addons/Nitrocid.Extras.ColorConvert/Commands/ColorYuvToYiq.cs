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
    /// Converts the color YUV numbers to YIQ.
    /// </summary>
    /// <remarks>
    /// If you want to get the YIQ representation of the color from the YUV color numbers, you can use this command.
    /// </remarks>
    class ColorYuvToYiqCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Check to see if we have the numeric arguments
            if (!double.TryParse(parameters.ArgumentsList[0], out double Y))
            {
                TextWriters.Write(Translate.DoTranslation("The Y component level must be numeric."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Color;
            }
            if (!double.TryParse(parameters.ArgumentsList[1], out double U))
            {
                TextWriters.Write(Translate.DoTranslation("The U component level must be numeric."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Color;
            }
            if (!double.TryParse(parameters.ArgumentsList[2], out double V))
            {
                TextWriters.Write(Translate.DoTranslation("The V component level must be numeric."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Color;
            }

            // Do the job
            var color = new Color($"yuv:{Y};{U};{V}");
            var yiq = YiqConversionTools.ConvertFrom(color.RGB);
            TextWriters.Write("- " + Translate.DoTranslation("Luma:") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write($"{yiq.Luma}", true, KernelColorType.ListValue);
            TextWriters.Write("- " + Translate.DoTranslation("In-phase:") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write($"{yiq.InPhase}", true, KernelColorType.ListValue);
            TextWriters.Write("- " + Translate.DoTranslation("Quadrature:") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write($"{yiq.Quadrature}", true, KernelColorType.ListValue);
            variableValue = yiq.ToString();
            return 0;
        }

    }
}
