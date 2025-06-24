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
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Commands;
using Terminaux.Colors.Models;

namespace Nitrocid.Extras.ColorConvert.Commands
{
    /// <summary>
    /// Converts the color specifier to the target color model.
    /// </summary>
    /// <remarks>
    /// If you want to get the target color model representation from the source color model specifier, you can use this command.
    /// </remarks>
    class ColorSpecToCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Check the source and the target models
            string source = parameters.ArgumentsList[0];
            string specifier = parameters.ArgumentsList[1];
            var modelConvert = ColorConvertTools.GetConvertFuncFromSingleModel(source);
            if (modelConvert is null)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_COLORCONVERT_INVALIDMODEL"), true, ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Color);
            }
            var modelConverted = modelConvert.Invoke(specifier);

            // Do the job
            switch (source)
            {
                case "rgb":
                    var rgb = (RedGreenBlue)modelConverted;
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_REDCOLOR") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{rgb.R}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_GREENCOLOR") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{rgb.G}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_BLUECOLOR") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{rgb.B}", true, ThemeColorType.ListValue);
                    variableValue = rgb.ToString();
                    break;
                case "ryb":
                    var ryb = (RedYellowBlue)modelConverted;
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_REDCOLOR") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{ryb.R}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_YELLOWCOLOR") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{ryb.Y}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_BLUECOLOR") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{ryb.B}", true, ThemeColorType.ListValue);
                    variableValue = ryb.ToString();
                    break;
                case "cmy":
                    var cmy = (CyanMagentaYellow)modelConverted;
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_CYANCOLOR") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{cmy.CWhole} [{cmy.C:0.00}]", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_MAGENTACOLOR") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{cmy.MWhole} [{cmy.M:0.00}]", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_YELLOWCOLOR") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{cmy.YWhole} [{cmy.Y:0.00}]", true, ThemeColorType.ListValue);
                    variableValue = cmy.ToString();
                    break;
                case "cmyk":
                    var cmyk = (CyanMagentaYellowKey)modelConverted;
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_CYANCOLOR") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{cmyk.CMY.CWhole} [{cmyk.CMY.C:0.00}]", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_MAGENTACOLOR") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{cmyk.CMY.MWhole} [{cmyk.CMY.M:0.00}]", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_YELLOWCOLOR") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{cmyk.CMY.YWhole} [{cmyk.CMY.Y:0.00}]", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_BLACKKEY") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{cmyk.KWhole} [{cmyk.K:0.00}]", true, ThemeColorType.ListValue);
                    variableValue = cmyk.ToString();
                    break;
                case "hsv":
                    var hsv = (HueSaturationValue)modelConverted;
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_HUE") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{hsv.HueWhole} [{hsv.Hue:0.00}]", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_SATURATION") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{hsv.SaturationWhole} [{hsv.Saturation:0.00}]", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_VALUE") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{hsv.ValueWhole} [{hsv.Value:0.00}]", true, ThemeColorType.ListValue);
                    variableValue = hsv.ToString();
                    break;
                case "hsl":
                    var hsl = (HueSaturationLightness)modelConverted;
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_HUE") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{hsl.HueWhole} [{hsl.Hue:0.00}]", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_SATURATION") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{hsl.SaturationWhole} [{hsl.Saturation:0.00}]", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_LUMINANCE") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{hsl.LightnessWhole} [{hsl.Lightness:0.00}]", true, ThemeColorType.ListValue);
                    variableValue = hsl.ToString();
                    break;
                case "yiq":
                    var yiq = (LumaInPhaseQuadrature)modelConverted;
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_LUMA") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{yiq.Luma}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_INPHASE") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{yiq.InPhase}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_QUADRATURE") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{yiq.Quadrature}", true, ThemeColorType.ListValue);
                    variableValue = yiq.ToString();
                    break;
                case "yuv":
                    var yuv = (LumaChromaUv)modelConverted;
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_LUMA") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{yuv.Luma}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_UCHROMA") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{yuv.ChromaU}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_COLORCONVERT_VCHROMA") + " ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{yuv.ChromaV}", true, ThemeColorType.ListValue);
                    variableValue = yuv.ToString();
                    break;
                case "xyz":
                    var xyz = (Xyz)modelConverted;
                    TextWriterColor.Write("- X: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{xyz.X:0.##}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- Y: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{xyz.Y:0.##}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write("- Z: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{xyz.Z:0.##}", true, ThemeColorType.ListValue);
                    variableValue = xyz.ToString();
                    break;
            }
            return 0;
        }

    }
}
