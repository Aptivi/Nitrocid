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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Extras.ColorConvert.Tools;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
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
                TextWriters.Write(Translate.DoTranslation("The first color level must be numeric."), true, KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Color);
            }
            if (!int.TryParse(parameters.ArgumentsList[3], out int second))
            {
                TextWriters.Write(Translate.DoTranslation("The second color level must be numeric."), true, KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Color);
            }
            if (!int.TryParse(parameters.ArgumentsList[4], out int third))
            {
                TextWriters.Write(Translate.DoTranslation("The third color level must be numeric."), true, KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Color);
            }
            if (parameters.ArgumentsList.Length > 5 && !int.TryParse(parameters.ArgumentsList[3], out fourth))
            {
                TextWriters.Write(Translate.DoTranslation("The fourth key level must be numeric."), true, KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Color);
            }

            // Check the source and the target models
            string source = parameters.ArgumentsList[0];
            string target = parameters.ArgumentsList[1];
            var modelConvert = ColorConvertTools.GetConvertFuncFromModel(source, target);
            if (modelConvert is null)
            {
                TextWriters.Write(Translate.DoTranslation("Model specification is invalid."), true, KernelColorType.Error);
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
            TextWriters.Write(finalSequence, KernelColorType.NeutralText);
            variableValue = finalSequence;
            return 0;
        }

    }
}
