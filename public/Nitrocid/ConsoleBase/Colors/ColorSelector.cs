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

using System;
using System.Text;
using Terminaux.Colors;
using Terminaux.Colors.Transformation;
using Textify.Sequences.Builder.Types;
using Terminaux.Colors.Models.Conversion;
using Nitrocid.Kernel.Debugging;
using Nitrocid.ConsoleBase.Inputs;
using Nitrocid.Languages;
using Nitrocid.ConsoleBase.Writers.FancyWriters;
using Nitrocid.ConsoleBase.Inputs.Styles.Infobox;
using Nitrocid.ConsoleBase.Writers.ConsoleWriters;
using Terminaux.Base.Buffered;
using Textify.General;

namespace Nitrocid.ConsoleBase.Colors
{
    /// <summary>
    /// Color selection application
    /// </summary>
    public static class ColorSelector
    {
        private static int trueColorHue = 0;
        private static int trueColorSaturation = 100;
        private static int trueColorLightness = 50;
        private static ConsoleColors colorValue255 = ConsoleColors.Magenta;
        private static ConsoleColor colorValue16 = ConsoleColor.Magenta;
        private static bool save = true;

        /// <summary>
        /// Opens the color selector
        /// </summary>
        /// <returns>An instance of Color to get the resulting color</returns>
        public static Color OpenColorSelector() =>
            OpenColorSelector(KernelColorTools.GetColor(KernelColorType.NeutralText));

        /// <summary>
        /// Opens the color selector
        /// </summary>
        /// <param name="initialColor">Initial color to use</param>
        /// <returns>An instance of Color to get the resulting color</returns>
        public static Color OpenColorSelector(KernelColorType initialColor) =>
            OpenColorSelector(KernelColorTools.GetColor(initialColor));

        /// <summary>
        /// Opens the color selector
        /// </summary>
        /// <param name="initialColor">Initial color to use</param>
        /// <returns>An instance of Color to get the resulting color</returns>
        public static Color OpenColorSelector(ConsoleColors initialColor) =>
            OpenColorSelector(new Color(initialColor));

        /// <summary>
        /// Opens the color selector
        /// </summary>
        /// <param name="initialColor">Initial color to use</param>
        /// <returns>An instance of Color to get the resulting color</returns>
        public static Color OpenColorSelector(Color initialColor)
        {
            // Initial color is selected
            Color selectedColor = initialColor;
            ColorType type = initialColor.Type;

            // Color selector entry
            var screen = new Screen();
            ScreenTools.SetCurrent(screen);
            try
            {
                // Make a screen part
                var screenPart = new ScreenPart();

                // Set initial colors
                var hsl = HslConversionTools.ConvertFrom(selectedColor.RGB);
                switch (type)
                {
                    case ColorType.TrueColor:
                        trueColorHue = hsl.HueWhole;
                        trueColorSaturation = hsl.SaturationWhole;
                        trueColorLightness = hsl.LightnessWhole;
                        break;
                    case ColorType._255Color:
                        colorValue255 = selectedColor.ColorEnum255;
                        break;
                    case ColorType._16Color:
                        colorValue16 = selectedColor.ColorEnum16;
                        break;
                    default:
                        DebugCheck.AssertFail("invalid color type in the color selector");
                        break;
                }
                UpdateColor(ref selectedColor, type);

                // Now, the selector main loop
                bool bail = false;
                while (!bail)
                {
                    // We need to refresh the screen
                    screenPart.AddText(
                        $"{KernelColorTools.GetColor(KernelColorType.Background).VTSequenceBackground}" +
                        $"{CsiSequences.GenerateCsiEraseInDisplay(2)}"
                    );

                    // Now, render the selector and handle input
                    switch (type)
                    {
                        case ColorType.TrueColor:
                            screenPart.AddDynamicText(() =>
                            {
                                ConsoleWrapper.CursorVisible = false;
                                return RenderTrueColorSelector(selectedColor);
                            });
                            screen.AddBufferedPart("Color selector", screenPart);
                            ScreenTools.Render();
                            bail = HandleKeypressTrueColor(ref selectedColor, ref type);
                            break;
                        case ColorType._255Color:
                            screenPart.AddDynamicText(() =>
                            {
                                ConsoleWrapper.CursorVisible = false;
                                return Render255ColorsSelector(selectedColor);
                            });
                            screen.AddBufferedPart("Color selector", screenPart);
                            ScreenTools.Render();
                            bail = HandleKeypress255Colors(ref selectedColor, ref type);
                            break;
                        case ColorType._16Color:
                            screenPart.AddDynamicText(() =>
                            {
                                ConsoleWrapper.CursorVisible = false;
                                return Render16ColorsSelector(selectedColor);
                            });
                            screen.AddBufferedPart("Color selector", screenPart);
                            ScreenTools.Render();
                            bail = HandleKeypress16Colors(ref selectedColor, ref type);
                            break;
                        default:
                            DebugCheck.AssertFail("invalid color type in the color selector");
                            break;
                    }
                    screenPart.Clear();
                    screen.RemoveBufferedParts();
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, $"Color selector failed to do its job: {ex.Message}");
                DebugWriter.WriteDebugStackTrace(ex);
                InfoBoxColor.WriteInfoBox(
                    Translate.DoTranslation("Color selector has failed") + $": {ex.Message}" + CharManager.NewLine + CharManager.NewLine +
                    Translate.DoTranslation("Check your input and try again. If it still didn't work, contact us.")
                );
            }
            finally
            {
                // Return the selected color
                if (!save)
                {
                    save = true;
                    selectedColor = initialColor;
                }
            }
            ScreenTools.UnsetCurrent(screen);
            return selectedColor;
        }

        private static string RenderTrueColorSelector(Color selectedColor)
        {
            var selector = new StringBuilder();

            // First, render the preview box
            selector.Append(RenderPreviewBox(selectedColor));

            // Then, render the hue, saturation, and lightness bars
            int hueBarX = ConsoleWrapper.WindowWidth / 2 + 3;
            int hueBarY = 1;
            int saturationBarY = 5;
            int lightnessBarY = 9;
            int rgbRampBarY = 13;
            int boxWidth = ConsoleWrapper.WindowWidth / 2 - 6;
            int boxHeight = 1;
            var initialBackground = KernelColorTools.GetColor(KernelColorType.Background).VTSequenceBackground;

            // Buffer the hue ramp
            StringBuilder hueRamp = new();
            for (int i = 0; i < boxWidth; i++)
            {
                double width = (double)i / boxWidth;
                int hue = (int)(360 * width);
                hueRamp.Append($"{new Color($"hsl:{hue};100;50").VTSequenceBackgroundTrueColor} {initialBackground}");
            }
            selector.Append(
                BoxFrameTextColor.RenderBoxFrame(Translate.DoTranslation("Hue") + $": {trueColorHue}/360", hueBarX, hueBarY, boxWidth, boxHeight) +
                CsiSequences.GenerateCsiCursorPosition(hueBarX + 2, hueBarY + 2) +
                hueRamp.ToString()
            );

            // Buffer the saturation ramp
            StringBuilder satRamp = new();
            for (int i = 0; i < boxWidth; i++)
            {
                double width = (double)i / boxWidth;
                int sat = (int)(100 * width);
                satRamp.Append($"{new Color($"hsl:{trueColorHue};{sat};50").VTSequenceBackgroundTrueColor} {initialBackground}");
            }
            selector.Append(
                BoxFrameTextColor.RenderBoxFrame(Translate.DoTranslation("Saturation") + $": {trueColorSaturation}/100", hueBarX, saturationBarY, boxWidth, boxHeight) +
                CsiSequences.GenerateCsiCursorPosition(hueBarX + 2, saturationBarY + 2) +
                satRamp.ToString()
            );

            // Buffer the lightness ramp
            StringBuilder ligRamp = new();
            for (int i = 0; i < boxWidth; i++)
            {
                double width = (double)i / boxWidth;
                int lig = (int)(100 * width);
                ligRamp.Append($"{new Color($"hsl:{trueColorHue};100;{lig}").VTSequenceBackgroundTrueColor} {initialBackground}");
            }
            selector.Append(
                BoxFrameTextColor.RenderBoxFrame(Translate.DoTranslation("Lightness") + $": {trueColorLightness}/100", hueBarX, lightnessBarY, boxWidth, boxHeight) +
                CsiSequences.GenerateCsiCursorPosition(hueBarX + 2, lightnessBarY + 2) +
                ligRamp.ToString()
            );

            // Buffer the RGB ramp
            StringBuilder redRamp = new();
            StringBuilder greenRamp = new();
            StringBuilder blueRamp = new();
            for (int i = 0; i < boxWidth; i++)
            {
                double width = (double)i / boxWidth;
                int red = (int)(selectedColor.R * width);
                int green = (int)(selectedColor.G * width);
                int blue = (int)(selectedColor.B * width);
                redRamp.Append($"{new Color($"{red};0;0").VTSequenceBackgroundTrueColor} {initialBackground}");
                greenRamp.Append($"{new Color($"0;{green};0").VTSequenceBackgroundTrueColor} {initialBackground}");
                blueRamp.Append($"{new Color($"0;0;{blue}").VTSequenceBackgroundTrueColor} {initialBackground}");
            }
            selector.Append(
                BoxFrameTextColor.RenderBoxFrame(Translate.DoTranslation("Red, Green, and Blue") + $": {selectedColor.R};{selectedColor.G};{selectedColor.B}", hueBarX, rgbRampBarY, boxWidth, boxHeight + 2) +
                CsiSequences.GenerateCsiCursorPosition(hueBarX + 2, rgbRampBarY + 2) +
                redRamp.ToString() +
                CsiSequences.GenerateCsiCursorPosition(hueBarX + 2, rgbRampBarY + 3) +
                greenRamp.ToString() +
                CsiSequences.GenerateCsiCursorPosition(hueBarX + 2, rgbRampBarY + 4) +
                blueRamp.ToString()
            );

            // Finally, the keybindings
            int bindingsPos = ConsoleWrapper.WindowHeight - 2;
            selector.Append(CenteredTextColor.RenderCentered(bindingsPos, $"[ENTER] {Translate.DoTranslation("Accept color")} - [H] {Translate.DoTranslation("Help")} - [ESC] {Translate.DoTranslation("Exit")}"));
            return selector.ToString();
        }

        private static string Render255ColorsSelector(Color selectedColor)
        {
            var selector = new StringBuilder();

            // First, render the preview box
            selector.Append(RenderPreviewBox(selectedColor));

            // Then, render the color info
            int infoBoxX = ConsoleWrapper.WindowWidth / 2 + 3;
            int infoBoxY = 1;
            int boxWidth = ConsoleWrapper.WindowWidth / 2 - 6;
            int boxHeight = 9;
            int rgbRampBarY = 13;
            var initialBackground = KernelColorTools.GetColor(KernelColorType.Background).VTSequenceBackground;

            // Buffer the RGB ramp
            StringBuilder redRamp = new();
            StringBuilder greenRamp = new();
            StringBuilder blueRamp = new();
            for (int i = 0; i < boxWidth; i++)
            {
                double width = (double)i / boxWidth;
                int red = (int)(selectedColor.R * width);
                int green = (int)(selectedColor.G * width);
                int blue = (int)(selectedColor.B * width);
                redRamp.Append($"{new Color($"{red};0;0").VTSequenceBackgroundTrueColor} {initialBackground}");
                greenRamp.Append($"{new Color($"0;{green};0").VTSequenceBackgroundTrueColor} {initialBackground}");
                blueRamp.Append($"{new Color($"0;0;{blue}").VTSequenceBackgroundTrueColor} {initialBackground}");
            }
            selector.Append(
                BoxFrameTextColor.RenderBoxFrame(Translate.DoTranslation("Red, Green, and Blue") + $": {selectedColor.R};{selectedColor.G};{selectedColor.B}", infoBoxX, rgbRampBarY, boxWidth, 3) +
                CsiSequences.GenerateCsiCursorPosition(infoBoxX + 2, rgbRampBarY + 2) +
                redRamp.ToString() +
                CsiSequences.GenerateCsiCursorPosition(infoBoxX + 2, rgbRampBarY + 3) +
                greenRamp.ToString() +
                CsiSequences.GenerateCsiCursorPosition(infoBoxX + 2, rgbRampBarY + 4) +
                blueRamp.ToString()
            );

            // then, the boxes
            var mono = ColorTools.RenderColorBlindnessAware(selectedColor, TransformationFormula.Monochromacy, 0.6);
            var ryb = RybConversionTools.ConvertFrom(selectedColor.RGB);
            var cmyk = CmykConversionTools.ConvertFrom(selectedColor.RGB);
            var cmy = CmyConversionTools.ConvertFrom(selectedColor.RGB);
            var hsl = HslConversionTools.ConvertFrom(selectedColor.RGB);
            var hsv = HsvConversionTools.ConvertFrom(selectedColor.RGB);
            selector.Append(
                BoxFrameTextColor.RenderBoxFrame(Translate.DoTranslation("Info for") + $": {colorValue255}", infoBoxX, infoBoxY, boxWidth, boxHeight) +
                BoxColor.RenderBox(infoBoxX + 1, infoBoxY, boxWidth, boxHeight) +
                TextWriterWhereColor.RenderWherePlain(Translate.DoTranslation("Color ID") + $": {(int)colorValue255}", infoBoxX + 1, infoBoxY + 1) +
                TextWriterWhereColor.RenderWherePlain(Translate.DoTranslation("Hex") + $": {selectedColor.Hex}", infoBoxX + 1, infoBoxY + 2) +
                TextWriterWhereColor.RenderWherePlain(Translate.DoTranslation("RGB sequence") + $": {selectedColor.PlainSequence}", infoBoxX + 1, infoBoxY + 3) +
                TextWriterWhereColor.RenderWherePlain(Translate.DoTranslation("RGB sequence (real)") + $": {selectedColor.PlainSequenceTrueColor}", infoBoxX + 1, infoBoxY + 4) +
                TextWriterWhereColor.RenderWherePlain($"CMYK: {cmyk}", infoBoxX + 1, infoBoxY + 5) +
                TextWriterWhereColor.RenderWherePlain($"CMY: {cmy}", infoBoxX + 1, infoBoxY + 6) +
                TextWriterWhereColor.RenderWherePlain($"HSL: {hsl}", infoBoxX + 1, infoBoxY + 7) +
                TextWriterWhereColor.RenderWherePlain($"HSV: {hsv}", infoBoxX + 1, infoBoxY + 8) +
                TextWriterWhereColor.RenderWherePlain($"RYB: {ryb}, " + Translate.DoTranslation("Grayscale") + $": {mono}", infoBoxX + 1, infoBoxY + 9)
            );

            // Finally, the keybindings
            int bindingsPos = ConsoleWrapper.WindowHeight - 2;
            selector.Append(CenteredTextColor.RenderCentered(bindingsPos, $"[ENTER] {Translate.DoTranslation("Accept color")} - [H] {Translate.DoTranslation("Help")} - [ESC] {Translate.DoTranslation("Exit")}"));
            return selector.ToString();
        }

        private static string Render16ColorsSelector(Color selectedColor)
        {
            var selector = new StringBuilder();

            // First, render the preview box
            selector.Append(RenderPreviewBox(selectedColor));

            // Then, render the color info
            int infoBoxX = ConsoleWrapper.WindowWidth / 2 + 3;
            int infoBoxY = 1;
            int boxWidth = ConsoleWrapper.WindowWidth / 2 - 6;
            int boxHeight = 9;
            int rgbRampBarY = 13;
            var initialBackground = KernelColorTools.GetColor(KernelColorType.Background).VTSequenceBackground;

            // Buffer the RGB ramp
            StringBuilder redRamp = new();
            StringBuilder greenRamp = new();
            StringBuilder blueRamp = new();
            for (int i = 0; i < boxWidth; i++)
            {
                double width = (double)i / boxWidth;
                int red = (int)(selectedColor.R * width);
                int green = (int)(selectedColor.G * width);
                int blue = (int)(selectedColor.B * width);
                redRamp.Append($"{new Color($"{red};0;0").VTSequenceBackgroundTrueColor} {initialBackground}");
                greenRamp.Append($"{new Color($"0;{green};0").VTSequenceBackgroundTrueColor} {initialBackground}");
                blueRamp.Append($"{new Color($"0;0;{blue}").VTSequenceBackgroundTrueColor} {initialBackground}");
            }
            selector.Append(
                BoxFrameTextColor.RenderBoxFrame(Translate.DoTranslation("Red, Green, and Blue") + $": {selectedColor.R};{selectedColor.G};{selectedColor.B}", infoBoxX, rgbRampBarY, boxWidth, 3) +
                CsiSequences.GenerateCsiCursorPosition(infoBoxX + 2, rgbRampBarY + 2) +
                redRamp.ToString() +
                CsiSequences.GenerateCsiCursorPosition(infoBoxX + 2, rgbRampBarY + 3) +
                greenRamp.ToString() +
                CsiSequences.GenerateCsiCursorPosition(infoBoxX + 2, rgbRampBarY + 4) +
                blueRamp.ToString()
            );

            // then, the boxes
            var mono = ColorTools.RenderColorBlindnessAware(selectedColor, TransformationFormula.Monochromacy, 0.6);
            var ryb = RybConversionTools.ConvertFrom(selectedColor.RGB);
            var cmyk = CmykConversionTools.ConvertFrom(selectedColor.RGB);
            var cmy = CmyConversionTools.ConvertFrom(selectedColor.RGB);
            var hsl = HslConversionTools.ConvertFrom(selectedColor.RGB);
            var hsv = HsvConversionTools.ConvertFrom(selectedColor.RGB);
            selector.Append(
                BoxFrameTextColor.RenderBoxFrame(Translate.DoTranslation("Info for") + $": {colorValue16}", infoBoxX, infoBoxY, boxWidth, boxHeight) +
                BoxColor.RenderBox(infoBoxX + 1, infoBoxY, boxWidth, boxHeight) +
                TextWriterWhereColor.RenderWherePlain(Translate.DoTranslation("Color ID") + $": {(int)colorValue16}", infoBoxX + 1, infoBoxY + 1) +
                TextWriterWhereColor.RenderWherePlain(Translate.DoTranslation("Hex") + $": {selectedColor.Hex}", infoBoxX + 1, infoBoxY + 2) +
                TextWriterWhereColor.RenderWherePlain(Translate.DoTranslation("RGB sequence") + $": {selectedColor.PlainSequence}", infoBoxX + 1, infoBoxY + 3) +
                TextWriterWhereColor.RenderWherePlain(Translate.DoTranslation("RGB sequence (real)") + $": {selectedColor.PlainSequenceTrueColor}", infoBoxX + 1, infoBoxY + 4) +
                TextWriterWhereColor.RenderWherePlain($"CMYK: {cmyk}", infoBoxX + 1, infoBoxY + 5) +
                TextWriterWhereColor.RenderWherePlain($"CMY: {cmy}", infoBoxX + 1, infoBoxY + 6) +
                TextWriterWhereColor.RenderWherePlain($"HSL: {hsl}", infoBoxX + 1, infoBoxY + 7) +
                TextWriterWhereColor.RenderWherePlain($"HSV: {hsv}", infoBoxX + 1, infoBoxY + 8) +
                TextWriterWhereColor.RenderWherePlain($"RYB: {ryb}, " + Translate.DoTranslation("Grayscale") + $": {mono}", infoBoxX + 1, infoBoxY + 9)
            );

            // Finally, the keybindings
            int bindingsPos = ConsoleWrapper.WindowHeight - 2;
            selector.Append(CenteredTextColor.RenderCentered(bindingsPos, $"[ENTER] {Translate.DoTranslation("Accept color")} - [H] {Translate.DoTranslation("Help")} - [ESC] {Translate.DoTranslation("Exit")}"));
            return selector.ToString();
        }

        private static bool HandleKeypressTrueColor(ref Color selectedColor, ref ColorType type)
        {
            bool bail = false;
            var keypress = Input.DetectKeypress();
            switch (keypress.Key)
            {
                case ConsoleKey.Tab:
                    if (keypress.Modifiers.HasFlag(ConsoleModifiers.Shift))
                    {
                        type--;
                        if (type < ColorType.TrueColor)
                            type = ColorType._16Color;
                    }
                    else
                    {
                        type++;
                        if (type > ColorType._16Color)
                            type = ColorType.TrueColor;
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    if (keypress.Modifiers.HasFlag(ConsoleModifiers.Control))
                    {
                        trueColorLightness--;
                        if (trueColorLightness < 0)
                            trueColorLightness = 100;
                    }
                    else
                    {
                        trueColorHue--;
                        if (trueColorHue < 0)
                            trueColorHue = 360;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (keypress.Modifiers.HasFlag(ConsoleModifiers.Control))
                    {
                        trueColorLightness++;
                        if (trueColorLightness > 100)
                            trueColorLightness = 0;
                    }
                    else
                    {
                        trueColorHue++;
                        if (trueColorHue > 360)
                            trueColorHue = 0;
                    }
                    break;
                case ConsoleKey.UpArrow:
                    trueColorSaturation++;
                    if (trueColorSaturation > 100)
                        trueColorSaturation = 0;
                    break;
                case ConsoleKey.DownArrow:
                    trueColorSaturation--;
                    if (trueColorSaturation < 0)
                        trueColorSaturation = 100;
                    break;
                case ConsoleKey.H:
                    InfoBoxColor.WriteInfoBox(
                        $$"""
                        {{Translate.DoTranslation("Available keybindings")}}

                        [ENTER]              | {{Translate.DoTranslation("Accept color")}}
                        [ESC]                | {{Translate.DoTranslation("Exit")}}
                        [H]                  | {{Translate.DoTranslation("Help page")}}
                        [LEFT]               | {{Translate.DoTranslation("Reduce hue")}}
                        [CTRL] + [LEFT]      | {{Translate.DoTranslation("Reduce lightness")}}
                        [RIGHT]              | {{Translate.DoTranslation("Increase hue")}}
                        [CTRL] + [RIGHT]     | {{Translate.DoTranslation("Increase lightness")}}
                        [DOWN]               | {{Translate.DoTranslation("Reduce saturation")}}
                        [UP]                 | {{Translate.DoTranslation("Increase saturation")}}
                        [TAB]                | {{Translate.DoTranslation("Change color mode")}}
                        [I]                  | {{Translate.DoTranslation("Color information")}}
                        """
                    );
                    break;
                case ConsoleKey.I:
                    ShowColorInfo(selectedColor);
                    break;
                case ConsoleKey.Enter:
                    bail = true;
                    break;
                case ConsoleKey.Escape:
                    bail = true;
                    save = false;
                    break;
            }
            UpdateColor(ref selectedColor, type);
            return bail;
        }

        private static bool HandleKeypress255Colors(ref Color selectedColor, ref ColorType type)
        {
            bool bail = false;
            var keypress = Input.DetectKeypress();
            switch (keypress.Key)
            {
                case ConsoleKey.Tab:
                    if (keypress.Modifiers.HasFlag(ConsoleModifiers.Shift))
                    {
                        type--;
                        if (type < ColorType.TrueColor)
                            type = ColorType._16Color;
                    }
                    else
                    {
                        type++;
                        if (type > ColorType._16Color)
                            type = ColorType.TrueColor;
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    colorValue255--;
                    if (colorValue255 < ConsoleColors.Black)
                        colorValue255 = ConsoleColors.Grey93;
                    break;
                case ConsoleKey.RightArrow:
                    colorValue255++;
                    if (colorValue255 > ConsoleColors.Grey93)
                        colorValue255 = ConsoleColors.Black;
                    break;
                case ConsoleKey.H:
                    InfoBoxColor.WriteInfoBox(
                        $$"""
                        {{Translate.DoTranslation("Available keybindings")}}

                        [ENTER]              | {{Translate.DoTranslation("Accept color")}}
                        [ESC]                | {{Translate.DoTranslation("Exit")}}
                        [H]                  | {{Translate.DoTranslation("Help page")}}
                        [LEFT]               | {{Translate.DoTranslation("Previous color")}}
                        [RIGHT]              | {{Translate.DoTranslation("Next color")}}
                        [TAB]                | {{Translate.DoTranslation("Change color mode")}}
                        [I]                  | {{Translate.DoTranslation("Color information")}}
                        """
                    );
                    break;
                case ConsoleKey.I:
                    ShowColorInfo(selectedColor);
                    break;
                case ConsoleKey.Enter:
                    bail = true;
                    break;
                case ConsoleKey.Escape:
                    bail = true;
                    save = false;
                    break;
            }
            UpdateColor(ref selectedColor, type);
            return bail;
        }

        private static bool HandleKeypress16Colors(ref Color selectedColor, ref ColorType type)
        {
            bool bail = false;
            var keypress = Input.DetectKeypress();
            switch (keypress.Key)
            {
                case ConsoleKey.Tab:
                    if (keypress.Modifiers.HasFlag(ConsoleModifiers.Shift))
                    {
                        type--;
                        if (type < ColorType.TrueColor)
                            type = ColorType._16Color;
                    }
                    else
                    {
                        type++;
                        if (type > ColorType._16Color)
                            type = ColorType.TrueColor;
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    colorValue16--;
                    if (colorValue16 < ConsoleColor.Black)
                        colorValue16 = ConsoleColor.White;
                    break;
                case ConsoleKey.RightArrow:
                    colorValue16++;
                    if (colorValue16 > ConsoleColor.White)
                        colorValue16 = ConsoleColor.Black;
                    break;
                case ConsoleKey.H:
                    InfoBoxColor.WriteInfoBox(
                        $$"""
                        {{Translate.DoTranslation("Available keybindings")}}

                        [ENTER]              | {{Translate.DoTranslation("Accept color")}}
                        [ESC]                | {{Translate.DoTranslation("Exit")}}
                        [H]                  | {{Translate.DoTranslation("Help page")}}
                        [LEFT]               | {{Translate.DoTranslation("Previous color")}}
                        [RIGHT]              | {{Translate.DoTranslation("Next color")}}
                        [TAB]                | {{Translate.DoTranslation("Change color mode")}}
                        [I]                  | {{Translate.DoTranslation("Color information")}}
                        """
                    );
                    break;
                case ConsoleKey.I:
                    ShowColorInfo(selectedColor);
                    break;
                case ConsoleKey.Enter:
                    bail = true;
                    break;
                case ConsoleKey.Escape:
                    bail = true;
                    save = false;
                    break;
            }
            UpdateColor(ref selectedColor, type);
            return bail;
        }

        private static string RenderPreviewBox(Color selectedColor)
        {
            var builder = new StringBuilder();

            // Draw the box that represents the currently selected color
            int boxX = 2;
            int boxY = 1;
            int boxWidth = ConsoleWrapper.WindowWidth / 2 - 4;
            int boxHeight = ConsoleWrapper.WindowHeight - 6;

            // First, draw the border
            builder.Append(BoxFrameTextColor.RenderBoxFrame($"{selectedColor.PlainSequence} [{selectedColor.PlainSequenceTrueColor}]", boxX, boxY, boxWidth, boxHeight));

            // then, the box
            builder.Append(
                selectedColor.VTSequenceBackground +
                BoxColor.RenderBox(boxX + 1, boxY, boxWidth, boxHeight) +
                KernelColorTools.GetColor(KernelColorType.Background).VTSequenceBackground
            );
            return builder.ToString();
        }

        private static void UpdateColor(ref Color selectedColor, ColorType newType)
        {
            switch (newType)
            {
                case ColorType.TrueColor:
                    selectedColor = new($"hsl:{trueColorHue};{trueColorSaturation};{trueColorLightness}");
                    break;
                case ColorType._255Color:
                    selectedColor = colorValue255;
                    break;
                case ColorType._16Color:
                    selectedColor = colorValue16;
                    break;
            }
        }

        private static void ShowColorInfo(Color selectedColor)
        {
            ShowColorInfoBox(
                Translate.DoTranslation("Color info"),
                selectedColor
            );
            ShowColorInfoBox(
                Translate.DoTranslation("Color info (Protanomaly)"),
                selectedColor,
                true
            );
            ShowColorInfoBox(
                Translate.DoTranslation("Color info (Protanopia)"),
                selectedColor,
                true,
                TransformationFormula.Protan, 1.0
            );
            ShowColorInfoBox(
                Translate.DoTranslation("Color info (Deuteranomaly)"),
                selectedColor,
                true,
                TransformationFormula.Deutan
            );
            ShowColorInfoBox(
                Translate.DoTranslation("Color info (Deuteranopia)"),
                selectedColor,
                true,
                TransformationFormula.Deutan, 1.0
            );
            ShowColorInfoBox(
                Translate.DoTranslation("Color info (Tritanomaly)"),
                selectedColor,
                true,
                TransformationFormula.Tritan
            );
            ShowColorInfoBox(
                Translate.DoTranslation("Color info (Tritanopia)"),
                selectedColor,
                true,
                TransformationFormula.Tritan, 1.0
            );
            ShowColorInfoBox(
                Translate.DoTranslation("Color info (Monochromacy)"),
                selectedColor,
                true,
                TransformationFormula.Monochromacy
            );
        }

        private static void ShowColorInfoBox(string localizedTextTitle, Color selectedColor, bool colorBlind = false, TransformationFormula TransformationFormula = TransformationFormula.Protan, double severity = 0.6)
        {
            selectedColor =
                colorBlind ?
                ColorTools.RenderColorBlindnessAware(selectedColor, TransformationFormula, severity) :
                selectedColor;
            string separator = new('-', localizedTextTitle.Length);
            var ryb = RybConversionTools.ConvertFrom(selectedColor.RGB);
            var cmyk = CmykConversionTools.ConvertFrom(selectedColor.RGB);
            var cmy = CmyConversionTools.ConvertFrom(selectedColor.RGB);
            var hsl = HslConversionTools.ConvertFrom(selectedColor.RGB);
            var hsv = HsvConversionTools.ConvertFrom(selectedColor.RGB);
            InfoBoxColor.WriteInfoBox(
                $$"""
                {{localizedTextTitle}}
                {{separator}}

                {{Translate.DoTranslation("RGB level:")}}          {{selectedColor.PlainSequence}}
                {{Translate.DoTranslation("RGB level (true):")}}   {{selectedColor.PlainSequenceTrueColor}}
                {{Translate.DoTranslation("RGB hex code:")}}       {{selectedColor.Hex}}
                {{Translate.DoTranslation("Color type:")}}         {{selectedColor.Type}}
                    
                {{Translate.DoTranslation("RYB information:")}}
                    - {{Translate.DoTranslation("Red:")}}            {{ryb.R,3}}
                    - {{Translate.DoTranslation("Yellow:")}}         {{ryb.Y,3}}
                    - {{Translate.DoTranslation("Blue:")}}           {{ryb.B,3}}

                {{Translate.DoTranslation("CMYK information:")}}
                    - {{Translate.DoTranslation("Black key:")}}      {{cmyk.KWhole,3}}
                    - {{Translate.DoTranslation("Cyan:")}}           {{cmyk.CMY.CWhole,3}}
                    - {{Translate.DoTranslation("Magenta:")}}        {{cmyk.CMY.MWhole,3}}
                    - {{Translate.DoTranslation("Yellow:")}}         {{cmyk.CMY.YWhole,3}}
                    
                {{Translate.DoTranslation("CMY information:")}}
                    - {{Translate.DoTranslation("Cyan:")}}           {{cmy.CWhole,3}}
                    - {{Translate.DoTranslation("Magenta:")}}        {{cmy.CWhole,3}}
                    - {{Translate.DoTranslation("Yellow:")}}         {{cmy.CWhole,3}}
                    
                {{Translate.DoTranslation("HSL information:")}}
                    - {{Translate.DoTranslation("Hue (degs):")}}     {{hsl.HueWhole,3}}'
                    - {{Translate.DoTranslation("Reverse Hue:")}}    {{hsl.ReverseHueWhole,3}}'
                    - {{Translate.DoTranslation("Saturation:")}}     {{hsl.SaturationWhole,3}}
                    - {{Translate.DoTranslation("Lightness:")}}      {{hsl.LightnessWhole,3}}
                    
                {{Translate.DoTranslation("HSV information:")}}
                    - {{Translate.DoTranslation("Hue (degs):")}}     {{hsv.HueWhole,3}}'
                    - {{Translate.DoTranslation("Reverse Hue:")}}    {{hsv.ReverseHueWhole,3}}'
                    - {{Translate.DoTranslation("Saturation:")}}     {{hsv.SaturationWhole,3}}
                    - {{Translate.DoTranslation("Value:")}}          {{hsv.ValueWhole,3}}
                """
            );
        }
    }
}
