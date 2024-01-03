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

namespace Nitrocid.Extras.ColorConvert.Commands
{
    /// <summary>
    /// Converts the color HSV numbers to hex.
    /// </summary>
    /// <remarks>
    /// If you want to get the hexadecimal representation of the color from the HSL color numbers, you can use this command.
    /// </remarks>
    class ColorHsvToHexCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Check to see if we have the numeric arguments
            if (!int.TryParse(parameters.ArgumentsList[0], out int H))
            {
                TextWriters.Write(Translate.DoTranslation("The hue level must be numeric."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Color;
            }
            if (!int.TryParse(parameters.ArgumentsList[1], out int S))
            {
                TextWriters.Write(Translate.DoTranslation("The saturation level must be numeric."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Color;
            }
            if (!int.TryParse(parameters.ArgumentsList[2], out int V))
            {
                TextWriters.Write(Translate.DoTranslation("The luminance or lighting level must be numeric."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Color;
            }

            // Do the job
            string hex = KernelColorConversionTools.ConvertFromHsvToHex(H, S, V);
            TextWriters.Write("- " + Translate.DoTranslation("Color hexadecimal representation:") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write(hex, true, KernelColorType.ListValue);
            variableValue = hex;
            return 0;
        }

    }
}
