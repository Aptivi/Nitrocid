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
    /// Converts the hexadecimal representation of the color to CMYK numbers.
    /// </summary>
    /// <remarks>
    /// If you want to get the CMYK color numbers from the hexadecimal representation of the color, you can use this command.
    /// </remarks>
    class ColorHexToCmykCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string Hex = parameters.ArgumentsList[0];

            // Do the job
            Color color = new(Hex);
            var cmyk = color.CMYK;
            var cmy = color.CMYK.CMY;
            TextWriterColor.WriteKernelColor("- " + Translate.DoTranslation("Black key:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.WriteKernelColor($"{cmyk.KWhole} [{cmyk.K:0.00}]", true, KernelColorType.ListValue);
            TextWriterColor.WriteKernelColor("- " + Translate.DoTranslation("Cyan level:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.WriteKernelColor($"{cmy.CWhole} [{cmy.C:0.00}]", true, KernelColorType.ListValue);
            TextWriterColor.WriteKernelColor("- " + Translate.DoTranslation("Magenta level:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.WriteKernelColor($"{cmy.MWhole} [{cmy.M:0.00}]", true, KernelColorType.ListValue);
            TextWriterColor.WriteKernelColor("- " + Translate.DoTranslation("Yellow level:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.WriteKernelColor($"{cmy.YWhole} [{cmy.Y:0.00}]", true, KernelColorType.ListValue);
            variableValue = $"cmyk:{cmy.CWhole};{cmy.MWhole};{cmy.YWhole};{cmyk.KWhole}";
            return 0;
        }

    }
}
