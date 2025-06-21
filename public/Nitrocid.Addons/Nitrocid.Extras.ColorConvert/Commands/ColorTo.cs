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
                TextWriters.Write(LanguageTools.GetLocalized("NKS_COLORCONVERT_FIRSTLEVELNUMERIC"), true, ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Color);
            }
            if (!int.TryParse(parameters.ArgumentsList[3], out int second))
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_COLORCONVERT_SECONDLEVELNUMERIC"), true, ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Color);
            }
            if (!int.TryParse(parameters.ArgumentsList[4], out int third))
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_COLORCONVERT_THIRDLEVELNUMERIC"), true, ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Color);
            }
            if (parameters.ArgumentsList.Length > 5 && !int.TryParse(parameters.ArgumentsList[3], out fourth))
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_COLORCONVERT_FOURTHLEVELNUMERIC"), true, ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Color);
            }

            // Check the source and the target models
            string source = parameters.ArgumentsList[0];
            string target = parameters.ArgumentsList[1];
            var modelConvert = ColorConvertTools.GetConvertFuncFromModel(source, target);
            if (modelConvert is null)
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_COLORCONVERT_INVALIDMODEL"), true, ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Color);
            }
            var modelConverted = modelConvert.Invoke(first, second, third, fourth);

            // Do the job
            switch (target)
            {
                case "rgb":
                    var rgb = (RedGreenBlue)modelConverted;
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_REDCOLOR") + " ", false, ThemeColorType.ListEntry);
                    TextWriters.Write($"{rgb.R}", true, ThemeColorType.ListValue);
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_GREENCOLOR") + " ", false, ThemeColorType.ListEntry);
                    TextWriters.Write($"{rgb.G}", true, ThemeColorType.ListValue);
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_BLUECOLOR") + " ", false, ThemeColorType.ListEntry);
                    TextWriters.Write($"{rgb.B}", true, ThemeColorType.ListValue);
                    variableValue = rgb.ToString();
                    break;
                case "ryb":
                    var ryb = (RedYellowBlue)modelConverted;
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_REDCOLOR") + " ", false, ThemeColorType.ListEntry);
                    TextWriters.Write($"{ryb.R}", true, ThemeColorType.ListValue);
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_YELLOWCOLOR") + " ", false, ThemeColorType.ListEntry);
                    TextWriters.Write($"{ryb.Y}", true, ThemeColorType.ListValue);
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_BLUECOLOR") + " ", false, ThemeColorType.ListEntry);
                    TextWriters.Write($"{ryb.B}", true, ThemeColorType.ListValue);
                    variableValue = ryb.ToString();
                    break;
                case "cmy":
                    var cmy = (CyanMagentaYellow)modelConverted;
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_CYANCOLOR") + " ", false, ThemeColorType.ListEntry);
                    TextWriters.Write($"{cmy.CWhole} [{cmy.C:0.00}]", true, ThemeColorType.ListValue);
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_MAGENTACOLOR") + " ", false, ThemeColorType.ListEntry);
                    TextWriters.Write($"{cmy.MWhole} [{cmy.M:0.00}]", true, ThemeColorType.ListValue);
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_YELLOWCOLOR") + " ", false, ThemeColorType.ListEntry);
                    TextWriters.Write($"{cmy.YWhole} [{cmy.Y:0.00}]", true, ThemeColorType.ListValue);
                    variableValue = cmy.ToString();
                    break;
                case "cmyk":
                    var cmyk = (CyanMagentaYellowKey)modelConverted;
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_CYANCOLOR") + " ", false, ThemeColorType.ListEntry);
                    TextWriters.Write($"{cmyk.CMY.CWhole} [{cmyk.CMY.C:0.00}]", true, ThemeColorType.ListValue);
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_MAGENTACOLOR") + " ", false, ThemeColorType.ListEntry);
                    TextWriters.Write($"{cmyk.CMY.MWhole} [{cmyk.CMY.M:0.00}]", true, ThemeColorType.ListValue);
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_YELLOWCOLOR") + " ", false, ThemeColorType.ListEntry);
                    TextWriters.Write($"{cmyk.CMY.YWhole} [{cmyk.CMY.Y:0.00}]", true, ThemeColorType.ListValue);
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_BLACKKEY") + " ", false, ThemeColorType.ListEntry);
                    TextWriters.Write($"{cmyk.KWhole} [{cmyk.K:0.00}]", true, ThemeColorType.ListValue);
                    variableValue = cmyk.ToString();
                    break;
                case "hsv":
                    var hsv = (HueSaturationValue)modelConverted;
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_HUE") + " ", false, ThemeColorType.ListEntry);
                    TextWriters.Write($"{hsv.HueWhole} [{hsv.Hue:0.00}]", true, ThemeColorType.ListValue);
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_SATURATION") + " ", false, ThemeColorType.ListEntry);
                    TextWriters.Write($"{hsv.SaturationWhole} [{hsv.Saturation:0.00}]", true, ThemeColorType.ListValue);
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_VALUE") + " ", false, ThemeColorType.ListEntry);
                    TextWriters.Write($"{hsv.ValueWhole} [{hsv.Value:0.00}]", true, ThemeColorType.ListValue);
                    variableValue = hsv.ToString();
                    break;
                case "hsl":
                    var hsl = (HueSaturationLightness)modelConverted;
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_HUE") + " ", false, ThemeColorType.ListEntry);
                    TextWriters.Write($"{hsl.HueWhole} [{hsl.Hue:0.00}]", true, ThemeColorType.ListValue);
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_SATURATION") + " ", false, ThemeColorType.ListEntry);
                    TextWriters.Write($"{hsl.SaturationWhole} [{hsl.Saturation:0.00}]", true, ThemeColorType.ListValue);
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_LUMINANCE") + " ", false, ThemeColorType.ListEntry);
                    TextWriters.Write($"{hsl.LightnessWhole} [{hsl.Lightness:0.00}]", true, ThemeColorType.ListValue);
                    variableValue = hsl.ToString();
                    break;
                case "yiq":
                    var yiq = (LumaInPhaseQuadrature)modelConverted;
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_LUMA") + " ", false, ThemeColorType.ListEntry);
                    TextWriters.Write($"{yiq.Luma}", true, ThemeColorType.ListValue);
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_INPHASE") + " ", false, ThemeColorType.ListEntry);
                    TextWriters.Write($"{yiq.InPhase}", true, ThemeColorType.ListValue);
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_QUADRATURE") + " ", false, ThemeColorType.ListEntry);
                    TextWriters.Write($"{yiq.Quadrature}", true, ThemeColorType.ListValue);
                    variableValue = yiq.ToString();
                    break;
                case "yuv":
                    var yuv = (LumaChromaUv)modelConverted;
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_LUMA") + " ", false, ThemeColorType.ListEntry);
                    TextWriters.Write($"{yuv.Luma}", true, ThemeColorType.ListValue);
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_UCHROMA") + " ", false, ThemeColorType.ListEntry);
                    TextWriters.Write($"{yuv.ChromaU}", true, ThemeColorType.ListValue);
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_VCHROMA") + " ", false, ThemeColorType.ListEntry);
                    TextWriters.Write($"{yuv.ChromaV}", true, ThemeColorType.ListValue);
                    variableValue = yuv.ToString();
                    break;
                case "xyz":
                    var xyz = (Xyz)modelConverted;
                    TextWriters.Write("- X: ", false, ThemeColorType.ListEntry);
                    TextWriters.Write($"{xyz.X:0.##}", true, ThemeColorType.ListValue);
                    TextWriters.Write("- Y: ", false, ThemeColorType.ListEntry);
                    TextWriters.Write($"{xyz.Y:0.##}", true, ThemeColorType.ListValue);
                    TextWriters.Write("- Z: ", false, ThemeColorType.ListEntry);
                    TextWriters.Write($"{xyz.Z:0.##}", true, ThemeColorType.ListValue);
                    variableValue = xyz.ToString();
                    break;
            }
            return 0;
        }

    }
}
