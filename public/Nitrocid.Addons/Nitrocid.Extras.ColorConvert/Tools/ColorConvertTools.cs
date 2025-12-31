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

using System;
using Terminaux.Colors;
using Terminaux.Colors.Models;
using Terminaux.Colors.Models.Conversion;

namespace Nitrocid.Extras.ColorConvert.Tools
{
    /// <summary>
    /// Color conversion tools
    /// </summary>
    public static class ColorConvertTools
    {
        /// <summary>
        /// Gets the color function from the color model
        /// </summary>
        /// <param name="sourceModel"></param>
        /// <returns></returns>
        public static Func<int, int, int, int, Color>? GetColorFuncFromModel(string sourceModel)
        {
            sourceModel = sourceModel.Trim().ToLower();
            if (string.IsNullOrEmpty(sourceModel))
                return null;

            // Now, make a case for source and target models
            return sourceModel switch
            {
                "rgb" => new((int r, int g, int b, int _) => new Color($"{r};{g};{b}")),
                "ryb" => new((int r, int y, int b, int _) => new Color($"{sourceModel}:{r};{y};{b}")),
                "cmy" => new((int c, int m, int y, int _) => new Color($"{sourceModel}:{c};{m};{y}")),
                "cmyk" => new((int c, int m, int y, int k) => new Color($"{sourceModel}:{c};{m};{y};{k}")),
                "hsv" => new((int h, int s, int v, int _) => new Color($"{sourceModel}:{h};{s};{v}")),
                "hsl" => new((int h, int s, int l, int _) => new Color($"{sourceModel}:{h};{s};{l}")),
                "yiq" => new((int y, int i, int q, int _) => new Color($"{sourceModel}:{y};{i};{q}")),
                "yuv" => new((int y, int u, int v, int _) => new Color($"{sourceModel}:{y};{u};{v}")),
                "xyz" => new((int x, int y, int z, int _) => new Color($"{sourceModel}:{x};{y};{z}")),
                _ => null,
            };
        }
        
        /// <summary>
        /// Gets the color model function from the color model
        /// </summary>
        /// <param name="sourceModel"></param>
        /// <returns></returns>
        public static Func<string, BaseColorModel>? GetConvertFuncFromSingleModel(string sourceModel)
        {
            sourceModel = sourceModel.Trim().ToLower();
            if (string.IsNullOrEmpty(sourceModel))
                return null;

            // Now, make a case for source and target models
            return sourceModel switch
            {
                "rgb" => new((string spec) => new Color(spec).RGB),
                "ryb" => new((string spec) => ConversionTools.ConvertFromRgb<RedYellowBlue>(new Color(spec).RGB)),
                "cmy" => new((string spec) => ConversionTools.ConvertFromRgb<CyanMagentaYellow>(new Color(spec).RGB)),
                "cmyk" => new((string spec) => ConversionTools.ConvertFromRgb<CyanMagentaYellowKey>(new Color(spec).RGB)),
                "hsv" => new((string spec) => ConversionTools.ConvertFromRgb<HueSaturationValue>(new Color(spec).RGB)),
                "hsl" => new((string spec) => ConversionTools.ConvertFromRgb<HueSaturationLightness>(new Color(spec).RGB)),
                "yiq" => new((string spec) => ConversionTools.ConvertFromRgb<LumaInPhaseQuadrature>(new Color(spec).RGB)),
                "yuv" => new((string spec) => ConversionTools.ConvertFromRgb<LumaChromaUv>(new Color(spec).RGB)),
                "xyz" => new((string spec) => ConversionTools.ConvertFromRgb<Xyz>(new Color(spec).RGB)),
                _ => null,
            };
        }
        
        /// <summary>
        /// Gets the conversion function from the color model
        /// </summary>
        /// <param name="sourceModel"></param>
        /// <param name="targetModel"></param>
        /// <returns></returns>
        public static Func<int, int, int, int, BaseColorModel>? GetConvertFuncFromModel(string sourceModel, string targetModel)
        {
            sourceModel = sourceModel.Trim().ToLower();
            targetModel = targetModel.Trim().ToLower();
            if (string.IsNullOrEmpty(sourceModel))
                return null;
            if (string.IsNullOrEmpty(targetModel))
                return null;
            if (sourceModel == targetModel)
                return null;

            // Now, make a case for source and target models
            return sourceModel switch
            {
                "rgb" => targetModel switch
                {
                    "ryb" => new((int r, int g, int b, int _) => ConversionTools.ConvertFromRgb<RedYellowBlue>(new Color($"{r};{g};{b}").RGB)),
                    "cmy" => new((int r, int g, int b, int _) => ConversionTools.ConvertFromRgb<CyanMagentaYellow>(new Color($"{r};{g};{b}").RGB)),
                    "cmyk" => new((int r, int g, int b, int _) => ConversionTools.ConvertFromRgb<CyanMagentaYellowKey>(new Color($"{r};{g};{b}").RGB)),
                    "hsv" => new((int r, int g, int b, int _) => ConversionTools.ConvertFromRgb<HueSaturationValue>(new Color($"{r};{g};{b}").RGB)),
                    "hsl" => new((int r, int g, int b, int _) => ConversionTools.ConvertFromRgb<HueSaturationLightness>(new Color($"{r};{g};{b}").RGB)),
                    "yiq" => new((int r, int g, int b, int _) => ConversionTools.ConvertFromRgb<LumaInPhaseQuadrature>(new Color($"{r};{g};{b}").RGB)),
                    "yuv" => new((int r, int g, int b, int _) => ConversionTools.ConvertFromRgb<LumaChromaUv>(new Color($"{r};{g};{b}").RGB)),
                    "xyz" => new((int r, int g, int b, int _) => ConversionTools.ConvertFromRgb<Xyz>(new Color($"{r};{g};{b}").RGB)),
                    _ => null,
                },
                "ryb" => targetModel switch
                {
                    "rgb" => new((int r, int y, int b, int _) => new Color($"ryb:{r};{y};{b}").RGB),
                    "cmy" => new((int r, int y, int b, int _) => ConversionTools.ConvertFromRgb<CyanMagentaYellow>(new Color($"ryb:{r};{y};{b}").RGB)),
                    "cmyk" => new((int r, int y, int b, int _) => ConversionTools.ConvertFromRgb<CyanMagentaYellowKey>(new Color($"ryb:{r};{y};{b}").RGB)),
                    "hsv" => new((int r, int y, int b, int _) => ConversionTools.ConvertFromRgb<HueSaturationValue>(new Color($"ryb:{r};{y};{b}").RGB)),
                    "hsl" => new((int r, int y, int b, int _) => ConversionTools.ConvertFromRgb<HueSaturationLightness>(new Color($"ryb:{r};{y};{b}").RGB)),
                    "yiq" => new((int r, int y, int b, int _) => ConversionTools.ConvertFromRgb<LumaInPhaseQuadrature>(new Color($"ryb:{r};{y};{b}").RGB)),
                    "yuv" => new((int r, int y, int b, int _) => ConversionTools.ConvertFromRgb<LumaChromaUv>(new Color($"ryb:{r};{y};{b}").RGB)),
                    "xyz" => new((int r, int y, int b, int _) => ConversionTools.ConvertFromRgb<Xyz>(new Color($"ryb:{r};{y};{b}").RGB)),
                    _ => null,
                },
                "cmy" => targetModel switch
                {
                    "rgb" => new((int c, int m, int y, int _) => new Color($"cmy:{c};{m};{y}").RGB),
                    "ryb" => new((int c, int m, int y, int _) => ConversionTools.ConvertFromRgb<RedYellowBlue>(new Color($"ryb:{c};{m};{y}").RGB)),
                    "cmyk" => new((int c, int m, int y, int _) => ConversionTools.ConvertFromRgb<CyanMagentaYellowKey>(new Color($"ryb:{c};{m};{y}").RGB)),
                    "hsv" => new((int c, int m, int y, int _) => ConversionTools.ConvertFromRgb<HueSaturationValue>(new Color($"ryb:{c};{m};{y}").RGB)),
                    "hsl" => new((int c, int m, int y, int _) => ConversionTools.ConvertFromRgb<HueSaturationLightness>(new Color($"ryb:{c};{m};{y}").RGB)),
                    "yiq" => new((int c, int m, int y, int _) => ConversionTools.ConvertFromRgb<LumaInPhaseQuadrature>(new Color($"ryb:{c};{m};{y}").RGB)),
                    "yuv" => new((int c, int m, int y, int _) => ConversionTools.ConvertFromRgb<LumaChromaUv>(new Color($"ryb:{c};{m};{y}").RGB)),
                    "xyz" => new((int c, int m, int y, int _) => ConversionTools.ConvertFromRgb<Xyz>(new Color($"ryb:{c};{m};{y}").RGB)),
                    _ => null,
                },
                "cmyk" => targetModel switch
                {
                    "rgb" => new((int c, int m, int y, int k) => new Color($"cmyk:{c};{m};{y};{k}").RGB),
                    "ryb" => new((int c, int m, int y, int k) => ConversionTools.ConvertFromRgb<RedYellowBlue>(new Color($"cmyk:{c};{m};{y};{k}").RGB)),
                    "cmy" => new((int c, int m, int y, int k) => ConversionTools.ConvertFromRgb<CyanMagentaYellow>(new Color($"cmyk:{c};{m};{y};{k}").RGB)),
                    "hsv" => new((int c, int m, int y, int k) => ConversionTools.ConvertFromRgb<HueSaturationValue>(new Color($"cmyk:{c};{m};{y};{k}").RGB)),
                    "hsl" => new((int c, int m, int y, int k) => ConversionTools.ConvertFromRgb<HueSaturationLightness>(new Color($"cmyk:{c};{m};{y};{k}").RGB)),
                    "yiq" => new((int c, int m, int y, int k) => ConversionTools.ConvertFromRgb<LumaInPhaseQuadrature>(new Color($"cmyk:{c};{m};{y};{k}").RGB)),
                    "yuv" => new((int c, int m, int y, int k) => ConversionTools.ConvertFromRgb<LumaChromaUv>(new Color($"cmyk:{c};{m};{y};{k}").RGB)),
                    "xyz" => new((int c, int m, int y, int k) => ConversionTools.ConvertFromRgb<Xyz>(new Color($"cmyk:{c};{m};{y};{k}").RGB)),
                    _ => null,
                },
                "hsv" => targetModel switch
                {
                    "rgb" => new((int h, int s, int v, int _) => new Color($"hsv:{h};{s};{v}").RGB),
                    "ryb" => new((int h, int s, int v, int _) => ConversionTools.ConvertFromRgb<RedYellowBlue>(new Color($"hsv:{h};{s};{v}").RGB)),
                    "cmy" => new((int h, int s, int v, int _) => ConversionTools.ConvertFromRgb<CyanMagentaYellow>(new Color($"hsv:{h};{s};{v}").RGB)),
                    "cmyk" => new((int h, int s, int v, int _) => ConversionTools.ConvertFromRgb<CyanMagentaYellowKey>(new Color($"hsv:{h};{s};{v}").RGB)),
                    "hsl" => new((int h, int s, int v, int _) => ConversionTools.ConvertFromRgb<HueSaturationLightness>(new Color($"hsv:{h};{s};{v}").RGB)),
                    "yiq" => new((int h, int s, int v, int _) => ConversionTools.ConvertFromRgb<LumaInPhaseQuadrature>(new Color($"hsv:{h};{s};{v}").RGB)),
                    "yuv" => new((int h, int s, int v, int _) => ConversionTools.ConvertFromRgb<LumaChromaUv>(new Color($"hsv:{h};{s};{v}").RGB)),
                    "xyz" => new((int h, int s, int v, int _) => ConversionTools.ConvertFromRgb<Xyz>(new Color($"hsv:{h};{s};{v}").RGB)),
                    _ => null,
                },
                "hsl" => targetModel switch
                {
                    "rgb" => new((int h, int s, int l, int _) => new Color($"hsl:{h};{s};{l}").RGB),
                    "ryb" => new((int h, int s, int l, int _) => ConversionTools.ConvertFromRgb<RedYellowBlue>(new Color($"hsl:{h};{s};{l}").RGB)),
                    "cmy" => new((int h, int s, int l, int _) => ConversionTools.ConvertFromRgb<CyanMagentaYellow>(new Color($"hsl:{h};{s};{l}").RGB)),
                    "cmyk" => new((int h, int s, int l, int _) => ConversionTools.ConvertFromRgb<CyanMagentaYellowKey>(new Color($"hsl:{h};{s};{l}").RGB)),
                    "hsv" => new((int h, int s, int l, int _) => ConversionTools.ConvertFromRgb<HueSaturationValue>(new Color($"hsl:{h};{s};{l}").RGB)),
                    "yiq" => new((int h, int s, int l, int _) => ConversionTools.ConvertFromRgb<LumaInPhaseQuadrature>(new Color($"hsl:{h};{s};{l}").RGB)),
                    "yuv" => new((int h, int s, int l, int _) => ConversionTools.ConvertFromRgb<LumaChromaUv>(new Color($"hsl:{h};{s};{l}").RGB)),
                    "xyz" => new((int h, int s, int l, int _) => ConversionTools.ConvertFromRgb<Xyz>(new Color($"hsl:{h};{s};{l}").RGB)),
                    _ => null,
                },
                "yiq" => targetModel switch
                {
                    "rgb" => new((int y, int i, int q, int _) => new Color($"yiq:{y};{i};{q}").RGB),
                    "ryb" => new((int y, int i, int q, int _) => ConversionTools.ConvertFromRgb<RedYellowBlue>(new Color($"yiq:{y};{i};{q}").RGB)),
                    "cmy" => new((int y, int i, int q, int _) => ConversionTools.ConvertFromRgb<CyanMagentaYellow>(new Color($"yiq:{y};{i};{q}").RGB)),
                    "cmyk" => new((int y, int i, int q, int _) => ConversionTools.ConvertFromRgb<CyanMagentaYellowKey>(new Color($"yiq:{y};{i};{q}").RGB)),
                    "hsv" => new((int y, int i, int q, int _) => ConversionTools.ConvertFromRgb<HueSaturationValue>(new Color($"yiq:{y};{i};{q}").RGB)),
                    "hsl" => new((int y, int i, int q, int _) => ConversionTools.ConvertFromRgb<HueSaturationLightness>(new Color($"yiq:{y};{i};{q}").RGB)),
                    "yuv" => new((int y, int i, int q, int _) => ConversionTools.ConvertFromRgb<LumaChromaUv>(new Color($"yiq:{y};{i};{q}").RGB)),
                    "xyz" => new((int y, int i, int q, int _) => ConversionTools.ConvertFromRgb<Xyz>(new Color($"yiq:{y};{i};{q}").RGB)),
                    _ => null,
                },
                "yuv" => targetModel switch
                {
                    "rgb" => new((int y, int u, int v, int _) => new Color($"yuv:{y};{u};{v}").RGB),
                    "ryb" => new((int y, int u, int v, int _) => ConversionTools.ConvertFromRgb<RedYellowBlue>(new Color($"yuv:{y};{u};{v}").RGB)),
                    "cmy" => new((int y, int u, int v, int _) => ConversionTools.ConvertFromRgb<CyanMagentaYellow>(new Color($"yuv:{y};{u};{v}").RGB)),
                    "cmyk" => new((int y, int u, int v, int _) => ConversionTools.ConvertFromRgb<CyanMagentaYellowKey>(new Color($"yuv:{y};{u};{v}").RGB)),
                    "hsv" => new((int y, int u, int v, int _) => ConversionTools.ConvertFromRgb<HueSaturationValue>(new Color($"yuv:{y};{u};{v}").RGB)),
                    "hsl" => new((int y, int u, int v, int _) => ConversionTools.ConvertFromRgb<HueSaturationLightness>(new Color($"yuv:{y};{u};{v}").RGB)),
                    "yiq" => new((int y, int u, int v, int _) => ConversionTools.ConvertFromRgb<LumaInPhaseQuadrature>(new Color($"yuv:{y};{u};{v}").RGB)),
                    "xyz" => new((int y, int u, int v, int _) => ConversionTools.ConvertFromRgb<Xyz>(new Color($"yuv:{y};{u};{v}").RGB)),
                    _ => null,
                },
                "xyz" => targetModel switch
                {
                    "rgb" => new((int x, int y, int z, int _) => new Color($"xyz:{x};{y};{z}").RGB),
                    "ryb" => new((int x, int y, int z, int _) => ConversionTools.ConvertFromRgb<RedYellowBlue>(new Color($"xyz:{x};{y};{z}").RGB)),
                    "cmy" => new((int x, int y, int z, int _) => ConversionTools.ConvertFromRgb<CyanMagentaYellow>(new Color($"xyz:{x};{y};{z}").RGB)),
                    "cmyk" => new((int x, int y, int z, int _) => ConversionTools.ConvertFromRgb<CyanMagentaYellowKey>(new Color($"xyz:{x};{y};{z}").RGB)),
                    "hsv" => new((int x, int y, int z, int _) => ConversionTools.ConvertFromRgb<HueSaturationValue>(new Color($"xyz:{x};{y};{z}").RGB)),
                    "hsl" => new((int x, int y, int z, int _) => ConversionTools.ConvertFromRgb<HueSaturationLightness>(new Color($"xyz:{x};{y};{z}").RGB)),
                    "yiq" => new((int x, int y, int z, int _) => ConversionTools.ConvertFromRgb<LumaInPhaseQuadrature>(new Color($"xyz:{x};{y};{z}").RGB)),
                    "yuv" => new((int x, int y, int z, int _) => ConversionTools.ConvertFromRgb<LumaChromaUv>(new Color($"xyz:{x};{y};{z}").RGB)),
                    _ => null,
                },
                _ => null,
            };
        }
    }
}
