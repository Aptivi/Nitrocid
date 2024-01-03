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
    /// Converts the color RGB numbers to RYB.
    /// </summary>
    /// <remarks>
    /// If you want to get the RYB representation of the color from the HSL color numbers, you can use this command.
    /// </remarks>
    class ColorRgbToRybCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (!int.TryParse(parameters.ArgumentsList[0], out int R))
            {
                TextWriters.Write(Translate.DoTranslation("The red color level must be numeric."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Color;
            }
            if (!int.TryParse(parameters.ArgumentsList[1], out int G))
            {
                TextWriters.Write(Translate.DoTranslation("The green color level must be numeric."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Color;
            }
            if (!int.TryParse(parameters.ArgumentsList[2], out int B))
            {
                TextWriters.Write(Translate.DoTranslation("The blue color level must be numeric."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Color;
            }

            // Do the job
            var rgb = new Color(R, G, B);
            var ryb = RybConversionTools.ConvertFrom(rgb.RGB);
            TextWriters.Write("- " + Translate.DoTranslation("Red color level:") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write($"{ryb.R}", true, KernelColorType.ListValue);
            TextWriters.Write("- " + Translate.DoTranslation("Yellow color level:") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write($"{ryb.Y}", true, KernelColorType.ListValue);
            TextWriters.Write("- " + Translate.DoTranslation("Blue color level:") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write($"{ryb.B}", true, KernelColorType.ListValue);
            variableValue = ryb.ToString();
            return 0;
        }

    }
}
