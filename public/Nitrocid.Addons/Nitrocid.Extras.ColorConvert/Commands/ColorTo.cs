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
    /// Converts the color numbers to the target color model.
    /// </summary>
    /// <remarks>
    /// If you want to get the target color model representation from the source color model numbers, you can use this command.
    /// </remarks>
    class ColorToCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Check to see if we have the numeric arguments
            int fourth = 0;
            if (!int.TryParse(parameters.ArgumentsList[2], out int first))
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_COLORCONVERT_FIRSTLEVELNUMERIC"), true, KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Color);
            }
            if (!int.TryParse(parameters.ArgumentsList[3], out int second))
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_COLORCONVERT_SECONDLEVELNUMERIC"), true, KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Color);
            }
            if (!int.TryParse(parameters.ArgumentsList[4], out int third))
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_COLORCONVERT_THIRDLEVELNUMERIC"), true, KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Color);
            }
            if (parameters.ArgumentsList.Length > 5 && !int.TryParse(parameters.ArgumentsList[3], out fourth))
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_COLORCONVERT_FOURTHLEVELNUMERIC"), true, KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Color);
            }

            // Check the source and the target models
            string source = parameters.ArgumentsList[0];
            string target = parameters.ArgumentsList[1];
            var modelConvert = ColorConvertTools.GetConvertFuncFromModel(source, target);
            if (modelConvert is null)
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_COLORCONVERT_INVALIDMODEL"), true, KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Color);
            }
            var modelConverted = modelConvert.Invoke(first, second, third, fourth);

            // Do the job
            switch (target)
            {
                case "rgb":
                    var rgb = (RedGreenBlue)modelConverted;
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_REDCOLOR") + " ", false, KernelColorType.ListEntry);
                    TextWriters.Write($"{rgb.R}", true, KernelColorType.ListValue);
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_GREENCOLOR") + " ", false, KernelColorType.ListEntry);
                    TextWriters.Write($"{rgb.G}", true, KernelColorType.ListValue);
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_BLUECOLOR") + " ", false, KernelColorType.ListEntry);
                    TextWriters.Write($"{rgb.B}", true, KernelColorType.ListValue);
                    variableValue = rgb.ToString();
                    break;
                case "ryb":
                    var ryb = (RedYellowBlue)modelConverted;
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_REDCOLOR") + " ", false, KernelColorType.ListEntry);
                    TextWriters.Write($"{ryb.R}", true, KernelColorType.ListValue);
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_YELLOWCOLOR") + " ", false, KernelColorType.ListEntry);
                    TextWriters.Write($"{ryb.Y}", true, KernelColorType.ListValue);
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_BLUECOLOR") + " ", false, KernelColorType.ListEntry);
                    TextWriters.Write($"{ryb.B}", true, KernelColorType.ListValue);
                    variableValue = ryb.ToString();
                    break;
                case "cmy":
                    var cmy = (CyanMagentaYellow)modelConverted;
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_CYANCOLOR") + " ", false, KernelColorType.ListEntry);
                    TextWriters.Write($"{cmy.CWhole} [{cmy.C:0.00}]", true, KernelColorType.ListValue);
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_MAGENTACOLOR") + " ", false, KernelColorType.ListEntry);
                    TextWriters.Write($"{cmy.MWhole} [{cmy.M:0.00}]", true, KernelColorType.ListValue);
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_YELLOWCOLOR") + " ", false, KernelColorType.ListEntry);
                    TextWriters.Write($"{cmy.YWhole} [{cmy.Y:0.00}]", true, KernelColorType.ListValue);
                    variableValue = cmy.ToString();
                    break;
                case "cmyk":
                    var cmyk = (CyanMagentaYellowKey)modelConverted;
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_CYANCOLOR") + " ", false, KernelColorType.ListEntry);
                    TextWriters.Write($"{cmyk.CMY.CWhole} [{cmyk.CMY.C:0.00}]", true, KernelColorType.ListValue);
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_MAGENTACOLOR") + " ", false, KernelColorType.ListEntry);
                    TextWriters.Write($"{cmyk.CMY.MWhole} [{cmyk.CMY.M:0.00}]", true, KernelColorType.ListValue);
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_YELLOWCOLOR") + " ", false, KernelColorType.ListEntry);
                    TextWriters.Write($"{cmyk.CMY.YWhole} [{cmyk.CMY.Y:0.00}]", true, KernelColorType.ListValue);
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_BLACKKEY") + " ", false, KernelColorType.ListEntry);
                    TextWriters.Write($"{cmyk.KWhole} [{cmyk.K:0.00}]", true, KernelColorType.ListValue);
                    variableValue = cmyk.ToString();
                    break;
                case "hsv":
                    var hsv = (HueSaturationValue)modelConverted;
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_HUE") + " ", false, KernelColorType.ListEntry);
                    TextWriters.Write($"{hsv.HueWhole} [{hsv.Hue:0.00}]", true, KernelColorType.ListValue);
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_SATURATION") + " ", false, KernelColorType.ListEntry);
                    TextWriters.Write($"{hsv.SaturationWhole} [{hsv.Saturation:0.00}]", true, KernelColorType.ListValue);
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_VALUE") + " ", false, KernelColorType.ListEntry);
                    TextWriters.Write($"{hsv.ValueWhole} [{hsv.Value:0.00}]", true, KernelColorType.ListValue);
                    variableValue = hsv.ToString();
                    break;
                case "hsl":
                    var hsl = (HueSaturationLightness)modelConverted;
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_HUE") + " ", false, KernelColorType.ListEntry);
                    TextWriters.Write($"{hsl.HueWhole} [{hsl.Hue:0.00}]", true, KernelColorType.ListValue);
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_SATURATION") + " ", false, KernelColorType.ListEntry);
                    TextWriters.Write($"{hsl.SaturationWhole} [{hsl.Saturation:0.00}]", true, KernelColorType.ListValue);
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_LUMINANCE") + " ", false, KernelColorType.ListEntry);
                    TextWriters.Write($"{hsl.LightnessWhole} [{hsl.Lightness:0.00}]", true, KernelColorType.ListValue);
                    variableValue = hsl.ToString();
                    break;
                case "yiq":
                    var yiq = (LumaInPhaseQuadrature)modelConverted;
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_LUMA") + " ", false, KernelColorType.ListEntry);
                    TextWriters.Write($"{yiq.Luma}", true, KernelColorType.ListValue);
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_INPHASE") + " ", false, KernelColorType.ListEntry);
                    TextWriters.Write($"{yiq.InPhase}", true, KernelColorType.ListValue);
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_QUADRATURE") + " ", false, KernelColorType.ListEntry);
                    TextWriters.Write($"{yiq.Quadrature}", true, KernelColorType.ListValue);
                    variableValue = yiq.ToString();
                    break;
                case "yuv":
                    var yuv = (LumaChromaUv)modelConverted;
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_LUMA") + " ", false, KernelColorType.ListEntry);
                    TextWriters.Write($"{yuv.Luma}", true, KernelColorType.ListValue);
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_UCHROMA") + " ", false, KernelColorType.ListEntry);
                    TextWriters.Write($"{yuv.ChromaU}", true, KernelColorType.ListValue);
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_VCHROMA") + " ", false, KernelColorType.ListEntry);
                    TextWriters.Write($"{yuv.ChromaV}", true, KernelColorType.ListValue);
                    variableValue = yuv.ToString();
                    break;
                case "xyz":
                    var xyz = (Xyz)modelConverted;
                    TextWriters.Write("- X: ", false, KernelColorType.ListEntry);
                    TextWriters.Write($"{xyz.X:0.##}", true, KernelColorType.ListValue);
                    TextWriters.Write("- Y: ", false, KernelColorType.ListEntry);
                    TextWriters.Write($"{xyz.Y:0.##}", true, KernelColorType.ListValue);
                    TextWriters.Write("- Z: ", false, KernelColorType.ListEntry);
                    TextWriters.Write($"{xyz.Z:0.##}", true, KernelColorType.ListValue);
                    variableValue = xyz.ToString();
                    break;
            }
            return 0;
        }

    }
}
