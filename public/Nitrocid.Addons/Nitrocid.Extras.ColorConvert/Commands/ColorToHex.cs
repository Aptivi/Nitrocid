//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Extras.ColorConvert.Tools;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;

namespace Nitrocid.Extras.ColorConvert.Commands
{
    /// <summary>
    /// Converts the color numbers to hex.
    /// </summary>
    /// <remarks>
    /// If you want to get the hexadecimal representation of the color from the color numbers, you can use this command.
    /// </remarks>
    class ColorToHexCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Check to see if we have the numeric arguments
            int fourth = 0;
            if (!int.TryParse(parameters.ArgumentsList[1], out int first))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_COLORCONVERT_FIRSTLEVELNUMERIC"), true, ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Color);
            }
            if (!int.TryParse(parameters.ArgumentsList[2], out int second))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_COLORCONVERT_SECONDLEVELNUMERIC"), true, ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Color);
            }
            if (!int.TryParse(parameters.ArgumentsList[3], out int third))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_COLORCONVERT_THIRDLEVELNUMERIC"), true, ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Color);
            }
            if (parameters.ArgumentsList.Length > 4 && !int.TryParse(parameters.ArgumentsList[4], out fourth))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_COLORCONVERT_FOURTHLEVELNUMERIC"), true, ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Color);
            }

            // Check the source and the target models
            string source = parameters.ArgumentsList[0];
            var colorFunc = ColorConvertTools.GetColorFuncFromModel(source);
            if (colorFunc is null)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_COLORCONVERT_INVALIDMODEL"), true, ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Color);
            }
            var color = colorFunc.Invoke(first, second, third, fourth);
            TextWriterColor.Write(color.Hex, ThemeColorType.NeutralText);
            variableValue = color.Hex;
            return 0;
        }

    }
}
