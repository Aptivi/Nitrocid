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
using Nitrocid.Shell.ShellBase.Commands;
using Terminaux.Colors.Models;

namespace Nitrocid.Extras.ColorConvert.Commands
{
    /// <summary>
    /// Converts the color specifier to the target color model in KS format.
    /// </summary>
    /// <remarks>
    /// If you want to get the target color model representation in KS format from the source color model specifier, you can use this command.
    /// </remarks>
    class ColorSpecToKSCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Check the source and the target models
            string source = parameters.ArgumentsList[0];
            string specifier = parameters.ArgumentsList[1];
            var modelConvert = ColorConvertTools.GetConvertFuncFromSingleModel(source);
            if (modelConvert is null)
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_COLORCONVERT_INVALIDMODEL", "Nitrocid.Extras.ColorConvert"), true, KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Color);
            }
            var modelConverted = modelConvert.Invoke(specifier);

            // Do the job
            string finalSequence = "";
            switch (source)
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
