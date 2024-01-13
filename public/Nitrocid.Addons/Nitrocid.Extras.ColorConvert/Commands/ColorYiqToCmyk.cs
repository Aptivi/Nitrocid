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
    /// Converts the color YIQ numbers to CMYK.
    /// </summary>
    /// <remarks>
    /// If you want to get the CMYK representation of the color from the YIQ color numbers, you can use this command.
    /// </remarks>
    class ColorYiqToCmykCommand : BaseCommand, ICommand
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
            var cmyk = CmykConversionTools.ConvertFrom(yiq.RGB);
            var cmy = cmyk.CMY;
            TextWriters.Write("- " + Translate.DoTranslation("Black key:") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write($"{cmyk.KWhole} [{cmyk.K:0.00}]", true, KernelColorType.ListValue);
            TextWriters.Write("- " + Translate.DoTranslation("Cyan level:") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write($"{cmy.CWhole} [{cmy.C:0.00}]", true, KernelColorType.ListValue);
            TextWriters.Write("- " + Translate.DoTranslation("Magenta level:") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write($"{cmy.MWhole} [{cmy.M:0.00}]", true, KernelColorType.ListValue);
            TextWriters.Write("- " + Translate.DoTranslation("Yellow level:") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write($"{cmy.YWhole} [{cmy.Y:0.00}]", true, KernelColorType.ListValue);
            variableValue = $"cmyk:{cmy.CWhole};{cmy.MWhole};{cmy.YWhole};{cmyk.KWhole}";
            return 0;
        }

    }
}
