//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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

using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Extras.ColorConvert.Tools;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Commands;
using Terminaux.Colors.Models;

namespace Nitrocid.Extras.ColorConvert.Commands
{
    /// <summary>
    /// Converts the color numbers to a specified color model in KS format.
    /// </summary>
    /// <remarks>
    /// If you want to get the semicolon-delimited sequence of the target model color numbers from the source model color numbers, you can use this command. You can use this to form a valid color sequence to generate new color instances for your mods.
    /// </remarks>
    class ColorToKSCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Check to see if we have the numeric arguments
            int fourth = 0;
            if (!int.TryParse(parameters.ArgumentsList[2], out int first))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_COLORCONVERT_FIRSTLEVELNUMERIC"), true, ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Color);
            }
            if (!int.TryParse(parameters.ArgumentsList[3], out int second))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_COLORCONVERT_SECONDLEVELNUMERIC"), true, ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Color);
            }
            if (!int.TryParse(parameters.ArgumentsList[4], out int third))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_COLORCONVERT_THIRDLEVELNUMERIC"), true, ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Color);
            }
            if (parameters.ArgumentsList.Length > 5 && !int.TryParse(parameters.ArgumentsList[3], out fourth))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_COLORCONVERT_FOURTHLEVELNUMERIC"), true, ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Color);
            }

            // Check the source and the target models
            string source = parameters.ArgumentsList[0];
            string target = parameters.ArgumentsList[1];
            var modelConvert = ColorConvertTools.GetConvertFuncFromModel(source, target);
            if (modelConvert is null)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_COLORCONVERT_INVALIDMODEL"), true, ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Color);
            }
            var modelConverted = modelConvert.Invoke(first, second, third, fourth);

            // Do the job
            string finalSequence = "";
            switch (target)
            {
                case "rgb":
                    finalSequence = ((RedGreenBlue)modelConverted).ToString();
                    break;
                case "ryb":
                    finalSequence = ((RedYellowBlue)modelConverted).ToString();
                    break;
                case "cmy":
                    finalSequence = ((CyanMagentaYellow)modelConverted).ToString();
                    break;
                case "cmyk":
                    finalSequence = ((CyanMagentaYellowKey)modelConverted).ToString();
                    break;
                case "hsv":
                    finalSequence = ((HueSaturationValue)modelConverted).ToString();
                    break;
                case "hsl":
                    finalSequence = ((HueSaturationLightness)modelConverted).ToString();
                    break;
                case "yiq":
                    finalSequence = ((LumaInPhaseQuadrature)modelConverted).ToString();
                    break;
                case "yuv":
                    finalSequence = ((LumaChromaUv)modelConverted).ToString();
                    break;
                case "xyz":
                    finalSequence = ((Xyz)modelConverted).ToString();
                    break;
            }
            TextWriterColor.Write(finalSequence, ThemeColorType.NeutralText);
            variableValue = finalSequence;
            return 0;
        }

    }
}
