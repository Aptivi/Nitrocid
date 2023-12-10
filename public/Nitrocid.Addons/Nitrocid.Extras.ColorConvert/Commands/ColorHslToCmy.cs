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
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using Terminaux.Colors;

namespace Nitrocid.Extras.ColorConvert.Commands
{
    /// <summary>
    /// Converts the color HSL numbers to CMY.
    /// </summary>
    /// <remarks>
    /// If you want to get the CMY representation of the color from the HSL color numbers, you can use this command.
    /// </remarks>
    class ColorHslToCmyCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Check to see if we have the numeric arguments
            if (!int.TryParse(parameters.ArgumentsList[0], out int H))
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("The hue level must be numeric."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Color;
            }
            if (!int.TryParse(parameters.ArgumentsList[1], out int S))
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("The saturation level must be numeric."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Color;
            }
            if (!int.TryParse(parameters.ArgumentsList[2], out int L))
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("The luminance or lighting level must be numeric."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Color;
            }

            // Do the job
            var rgb = new Color($"hsl:{H};{S};{L}");
            var cmy = rgb.CMY;
            TextWriterColor.WriteKernelColor("- " + Translate.DoTranslation("Cyan level:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.WriteKernelColor($"{cmy.CWhole} [{cmy.C:0.00}]", true, KernelColorType.ListValue);
            TextWriterColor.WriteKernelColor("- " + Translate.DoTranslation("Magenta level:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.WriteKernelColor($"{cmy.MWhole} [{cmy.M:0.00}]", true, KernelColorType.ListValue);
            TextWriterColor.WriteKernelColor("- " + Translate.DoTranslation("Yellow level:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.WriteKernelColor($"{cmy.YWhole} [{cmy.Y:0.00}]", true, KernelColorType.ListValue);
            variableValue = $"cmyk:{cmy.CWhole};{cmy.MWhole};{cmy.YWhole}";
            return 0;
        }

    }
}
