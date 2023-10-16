
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Text;
using System;
using System.Text;
using Terminaux.Colors;
using Terminaux.Colors.Wheel;
using Terminaux.Sequences.Tools;

namespace KS.ConsoleBase.Colors
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
        private static bool refresh = true;
        private static bool save = true;

        /// <summary>
        /// Opens the color selector
        /// </summary>
        /// <returns>An instance of Color to get the resulting color</returns>
        public static Color OpenColorSelector() =>
            OpenColorSelector(KernelColorTools.CurrentForegroundColor);

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
            if (!KernelFlags.UseNewColorSelector)
                return ColorWheel.InputForColor(initialColor);

            // Initial color is selected
            Color selectedColor = initialColor;
            ColorType type = initialColor.Type;

            // Color selector entry
            try
            {
                // Set initial colors
                switch (type)
                {
                    case ColorType.TrueColor:
                        trueColorHue = selectedColor.HSL.HueWhole;
                        trueColorSaturation = selectedColor.HSL.SaturationWhole;
                        trueColorLightness = selectedColor.HSL.LightnessWhole;
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

                // Now, the selector main loop
                bool bail = false;
                while (!bail)
                {
                    // We need to refresh the screen if it's required
                    if (refresh)
                    {
                        refresh = false;
                        KernelColorTools.LoadBack();
                    }

                    // Now, render the selector and handle input
                    switch (type)
                    {
                        case ColorType.TrueColor:
                            RenderTrueColorSelector(selectedColor);
                            bail = HandleKeypressTrueColor(ref selectedColor, ref type);
                            break;
                        case ColorType._255Color:
                            Render255ColorsSelector(selectedColor);
                            bail = HandleKeypress255Colors(ref selectedColor, ref type);
                            break;
                        case ColorType._16Color:
                            Render16ColorsSelector(selectedColor);
                            bail = HandleKeypress16Colors(ref selectedColor, ref type);
                            break;
                        default:
                            DebugCheck.AssertFail("invalid color type in the color selector");
                            break;
                    }
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
                refresh = true;
                if (!save)
                {
                    save = true;
                    selectedColor = initialColor;
                }
            }
            return selectedColor;
        }

        private static void RenderTrueColorSelector(Color selectedColor)
        {
            // First, render the preview box
            RenderPreviewBox(selectedColor);

            // Then, render the hue, saturation, and lightness bars
            int hueBarX = (ConsoleWrapper.WindowWidth / 2) + 2;
            int hueBarY = 1;
            int saturationBarY = 5;
            int lightnessBarY = 9;
            int rgbRampBarY = 13;
            int boxWidth = (ConsoleWrapper.WindowWidth / 2) - 6;
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

            // Buffer the saturation ramp
            StringBuilder satRamp = new();
            for (int i = 0; i < boxWidth; i++)
            {
                double width = (double)i / boxWidth;
                int sat = (int)(100 * width);
                satRamp.Append($"{new Color($"hsl:{trueColorHue};{sat};50").VTSequenceBackgroundTrueColor} {initialBackground}");
            }

            // Buffer the lightness ramp
            StringBuilder ligRamp = new();
            for (int i = 0; i < boxWidth; i++)
            {
                double width = (double)i / boxWidth;
                int lig = (int)(100 * width);
                ligRamp.Append($"{new Color($"hsl:{trueColorHue};100;{lig}").VTSequenceBackgroundTrueColor} {initialBackground}");
            }

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

            // then, the boxes
            BoxFrameTextColor.WriteBoxFrame(Translate.DoTranslation("Hue") + $": {trueColorHue}/360", hueBarX, hueBarY, boxWidth, boxHeight);
            TextWriterWhereColor.WriteWhere(hueRamp.ToString(), hueBarX + 1, hueBarY + 1);
            BoxFrameTextColor.WriteBoxFrame(Translate.DoTranslation("Saturation") + $": {trueColorSaturation}/100", hueBarX, saturationBarY, boxWidth, boxHeight);
            TextWriterWhereColor.WriteWhere(satRamp.ToString(), hueBarX + 1, saturationBarY + 1);
            BoxFrameTextColor.WriteBoxFrame(Translate.DoTranslation("Lightness") + $": {trueColorLightness}/100", hueBarX, lightnessBarY, boxWidth, boxHeight);
            TextWriterWhereColor.WriteWhere(ligRamp.ToString(), hueBarX + 1, lightnessBarY + 1);
            BoxFrameTextColor.WriteBoxFrame(Translate.DoTranslation("Red, Green, and Blue") + $": {selectedColor.R};{selectedColor.G};{selectedColor.B}", hueBarX, rgbRampBarY, boxWidth, boxHeight + 2);
            TextWriterWhereColor.WriteWhere(redRamp.ToString(), hueBarX + 1, rgbRampBarY + 1);
            TextWriterWhereColor.WriteWhere(greenRamp.ToString(), hueBarX + 1, rgbRampBarY + 2);
            TextWriterWhereColor.WriteWhere(blueRamp.ToString(), hueBarX + 1, rgbRampBarY + 3);

            // Finally, the keybindings
            int bindingsPos = ConsoleWrapper.WindowHeight - 2;
            CenteredTextColor.WriteCentered(bindingsPos, $"[ENTER] {Translate.DoTranslation("Accept color")} - [H] {Translate.DoTranslation("Help")} - [ESC] {Translate.DoTranslation("Exit")}");
        }

        private static void Render255ColorsSelector(Color selectedColor)
        {
            // First, render the preview box
            RenderPreviewBox(selectedColor);

            // Then, render the color info
            int infoBoxX = (ConsoleWrapper.WindowWidth / 2) + 2;
            int infoBoxY = 1;
            int boxWidth = (ConsoleWrapper.WindowWidth / 2) - 6;
            int boxHeight = 6;
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

            // then, the boxes
            BoxFrameTextColor.WriteBoxFrame(Translate.DoTranslation("Info for") + $": {colorValue255}", infoBoxX, infoBoxY, boxWidth, boxHeight);
            BoxColor.WriteBox(infoBoxX + 1, infoBoxY, boxWidth, boxHeight);
            TextWriterWhereColor.WriteWhere(Translate.DoTranslation("Color ID") + $": {(int)colorValue255}", infoBoxX + 1, infoBoxY + 1);
            TextWriterWhereColor.WriteWhere(Translate.DoTranslation("Hex") + $": {selectedColor.Hex}", infoBoxX + 1, infoBoxY + 2);
            TextWriterWhereColor.WriteWhere(Translate.DoTranslation("RGB sequence") + $": {selectedColor.PlainSequence}", infoBoxX + 1, infoBoxY + 3);
            TextWriterWhereColor.WriteWhere(Translate.DoTranslation("RGB sequence (real)") + $": {selectedColor.PlainSequenceTrueColor}", infoBoxX + 1, infoBoxY + 4);
            TextWriterWhereColor.WriteWhere($"CMYK: cmyk:{selectedColor.CMYK.CMY.CWhole};{selectedColor.CMYK.CMY.MWhole};{selectedColor.CMYK.CMY.YWhole};{selectedColor.CMYK.KWhole}", infoBoxX + 1, infoBoxY + 5);
            TextWriterWhereColor.WriteWhere($"HSL: hsl:{selectedColor.HSL.HueWhole};{selectedColor.HSL.SaturationWhole};{selectedColor.HSL.LightnessWhole}", infoBoxX + 1, infoBoxY + 6);
            BoxFrameTextColor.WriteBoxFrame(Translate.DoTranslation("Red, Green, and Blue") + $": {selectedColor.R};{selectedColor.G};{selectedColor.B}", infoBoxX, rgbRampBarY, boxWidth, 3);
            TextWriterWhereColor.WriteWhere(redRamp.ToString(), infoBoxX + 1, rgbRampBarY + 1);
            TextWriterWhereColor.WriteWhere(greenRamp.ToString(), infoBoxX + 1, rgbRampBarY + 2);
            TextWriterWhereColor.WriteWhere(blueRamp.ToString(), infoBoxX + 1, rgbRampBarY + 3);

            // Finally, the keybindings
            int bindingsPos = ConsoleWrapper.WindowHeight - 2;
            CenteredTextColor.WriteCentered(bindingsPos, $"[ENTER] {Translate.DoTranslation("Accept color")} - [H] {Translate.DoTranslation("Help")} - [ESC] {Translate.DoTranslation("Exit")}");
        }

        private static void Render16ColorsSelector(Color selectedColor)
        {
            // First, render the preview box
            RenderPreviewBox(selectedColor);

            // Then, render the color info
            int infoBoxX = (ConsoleWrapper.WindowWidth / 2) + 2;
            int infoBoxY = 1;
            int boxWidth = (ConsoleWrapper.WindowWidth / 2) - 6;
            int boxHeight = 6;
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

            // then, the boxes
            BoxFrameTextColor.WriteBoxFrame(Translate.DoTranslation("Info for") + $": {colorValue16}", infoBoxX, infoBoxY, boxWidth, boxHeight);
            BoxColor.WriteBox(infoBoxX + 1, infoBoxY, boxWidth, boxHeight);
            TextWriterWhereColor.WriteWhere(Translate.DoTranslation("Color ID") + $": {(int)colorValue16}", infoBoxX + 1, infoBoxY + 1);
            TextWriterWhereColor.WriteWhere(Translate.DoTranslation("Hex") + $": {selectedColor.Hex}", infoBoxX + 1, infoBoxY + 2);
            TextWriterWhereColor.WriteWhere(Translate.DoTranslation("RGB sequence") + $": {selectedColor.PlainSequence}", infoBoxX + 1, infoBoxY + 3);
            TextWriterWhereColor.WriteWhere(Translate.DoTranslation("RGB sequence (real)") + $": {selectedColor.PlainSequenceTrueColor}", infoBoxX + 1, infoBoxY + 4);
            TextWriterWhereColor.WriteWhere($"CMYK: cmyk:{selectedColor.CMYK.CMY.CWhole};{selectedColor.CMYK.CMY.MWhole};{selectedColor.CMYK.CMY.YWhole};{selectedColor.CMYK.KWhole}", infoBoxX + 1, infoBoxY + 5);
            TextWriterWhereColor.WriteWhere($"HSL: hsl:{selectedColor.HSL.HueWhole};{selectedColor.HSL.SaturationWhole};{selectedColor.HSL.LightnessWhole}", infoBoxX + 1, infoBoxY + 6);
            BoxFrameTextColor.WriteBoxFrame(Translate.DoTranslation("Red, Green, and Blue") + $": {selectedColor.R};{selectedColor.G};{selectedColor.B}", infoBoxX, rgbRampBarY, boxWidth, 3);
            TextWriterWhereColor.WriteWhere(redRamp.ToString(), infoBoxX + 1, rgbRampBarY + 1);
            TextWriterWhereColor.WriteWhere(greenRamp.ToString(), infoBoxX + 1, rgbRampBarY + 2);
            TextWriterWhereColor.WriteWhere(blueRamp.ToString(), infoBoxX + 1, rgbRampBarY + 3);

            // Finally, the keybindings
            int bindingsPos = ConsoleWrapper.WindowHeight - 2;
            CenteredTextColor.WriteCentered(bindingsPos, $"[ENTER] {Translate.DoTranslation("Accept color")} - [H] {Translate.DoTranslation("Help")} - [ESC] {Translate.DoTranslation("Exit")}");
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
                    refresh = true;
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
                        """
                    );
                    refresh = true;
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
                    refresh = true;
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
                        """
                    );
                    refresh = true;
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
                    refresh = true;
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
                        """
                    );
                    refresh = true;
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

        private static void RenderPreviewBox(Color selectedColor)
        {
            // Draw the box that represents the currently selected color
            int boxX = 2;
            int boxY = 1;
            int boxWidth = (ConsoleWrapper.WindowWidth / 2) - 4;
            int boxHeight = ConsoleWrapper.WindowHeight - 6;

            // First, draw the border
            BoxFrameTextColor.WriteBoxFrame($"{selectedColor.PlainSequence} [{selectedColor.PlainSequenceTrueColor}]", boxX, boxY, boxWidth, boxHeight);

            // then, the box
            BoxColor.WriteBox(boxX + 1, boxY, boxWidth, boxHeight, selectedColor);
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
    }
}
