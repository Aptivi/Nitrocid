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
using Textify.Data.Figlet;
using Nitrocid.ScreensaverPacks.Animations.Glitch;
using Terminaux.Colors;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel.Time.Renderers;
using Nitrocid.Base.Kernel.Time;
using Nitrocid.Base.Languages;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Writer.CyclicWriters.Simple;
using Terminaux.Writer.CyclicWriters.Graphical;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for KSX2
    /// </summary>
    public class KSX2Display : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "KSX2";

        /// <inheritdoc/>
        public override bool ScreensaverContainsFlashingImages =>
            true;

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ColorTools.LoadBackDry(new Color(ConsoleColors.Black));
            ConsoleWrapper.CursorVisible = false;
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", vars: [ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight]);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            int step;
            int maxSteps = 13;
            Color darkGreen = new(0, 128, 0);
            Color darkRed = new(128, 0, 0);
            Color red = new(255, 0, 0);
            Color yellow = new(255, 255, 0);
            Color green = new(0, 255, 0);
            Color black = new(0, 0, 0);
            Color white = new(255, 255, 255);

            // Start stepping
            for (step = 1; step <= maxSteps; step++)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                if (ScreensaverManager.Bailing)
                    return;

                switch (step)
                {
                    // Step 1: Fade out from white to black
                    case 1:
                        // Fade the console out
                        int colorSteps = 30;

                        // Get the color thresholds
                        double thresholdR = white.RGB.R / (double)colorSteps;
                        double thresholdG = white.RGB.G / (double)colorSteps;
                        double thresholdB = white.RGB.B / (double)colorSteps;

                        // Now, transition from target color to black
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;
                            if (ScreensaverManager.Bailing)
                                return;

                            // Remove the values according to the threshold
                            int currentRColor = (int)Math.Round(white.RGB.R - thresholdR * currentStep);
                            int currentGColor = (int)Math.Round(white.RGB.G - thresholdG * currentStep);
                            int currentBColor = (int)Math.Round(white.RGB.B - thresholdB * currentStep);

                            // Now, make a color and fill the console with it
                            Color col = new(currentRColor, currentGColor, currentBColor);
                            ColorTools.LoadBackDry(col);

                            // Sleep
                            ScreensaverManager.Delay(100, true);
                        }
                        break;
                    // Step 2: fade in slowly to dark green, then slowly change to dark red
                    case 2:
                        colorSteps = 30;

                        // Get the color thresholds
                        thresholdR = darkGreen.RGB.R / (double)colorSteps;
                        thresholdG = darkGreen.RGB.G / (double)colorSteps;
                        thresholdB = darkGreen.RGB.B / (double)colorSteps;
                        double transitionThresholdR = (darkRed.RGB.R - darkGreen.RGB.R) / (double)colorSteps;
                        double transitionThresholdG = (darkRed.RGB.G - darkGreen.RGB.G) / (double)colorSteps;
                        double transitionThresholdB = (darkRed.RGB.B - darkGreen.RGB.B) / (double)colorSteps;

                        // Now, transition from black to the target color
                        int currentR = 0;
                        int currentG = 0;
                        int currentB = 0;
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;
                            if (ScreensaverManager.Bailing)
                                return;

                            // Add the values according to the threshold
                            currentR = (int)Math.Round(currentR + thresholdR);
                            currentG = (int)Math.Round(currentG + thresholdG);
                            currentB = (int)Math.Round(currentB + thresholdB);

                            // Now, make a color and fill the console with it
                            Color col = new(currentR, currentG, currentB);
                            ColorTools.LoadBackDry(col);

                            // Sleep
                            ScreensaverManager.Delay(100, true);
                        }
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;
                            if (ScreensaverManager.Bailing)
                                return;

                            // Add the values according to the threshold
                            currentR = (int)Math.Round(currentR + transitionThresholdR);
                            currentG = (int)Math.Round(currentG + transitionThresholdG);
                            currentB = (int)Math.Round(currentB + transitionThresholdB);

                            // Now, make a color and fill the console with it
                            Color col = new(currentR, currentG, currentB);
                            ColorTools.LoadBackDry(col);

                            // Sleep
                            ScreensaverManager.Delay(100, true);
                        }
                        break;
                    // Step 3: Glitch for a bit
                    case 3:
                        for (int delayed = 0; delayed < 5000; delayed += 10)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;
                            if (ScreensaverManager.Bailing)
                                return;

                            ScreensaverManager.Delay(10, true);
                            Glitch.GlitchAt();
                        }
                        break;
                    // Step 4: Show three selectors adjusting to 0.0.16.0
                    case 4:
                        string[] versions =
                        [
                            "0.0.1.0",
                            "0.0.4.0",
                            "0.0.8.0",
                            "0.0.16.0",
                            "0.0.20.0",
                            "0.0.24.0"
                        ];
                        int selector1Selection = 0;
                        int selector2Selection = 0;
                        int selector3Selection = 0;
                        int oneThirdConsoleWidth = ConsoleWrapper.WindowWidth / 3;
                        int selectorsPositionY = ConsoleWrapper.WindowHeight - 2;
                        while (selector1Selection != 3 || selector2Selection != 3 || selector3Selection != 3)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;
                            if (ScreensaverManager.Bailing)
                                return;

                            // Get the random indexes and the random versions
                            selector1Selection = RandomDriver.RandomIdx(versions.Length);
                            selector2Selection = RandomDriver.RandomIdx(versions.Length);
                            selector3Selection = RandomDriver.RandomIdx(versions.Length);
                            string selector1 = $" << {versions[selector1Selection]} >> ";
                            string selector2 = $" << {versions[selector2Selection]} >> ";
                            string selector3 = $" << {versions[selector3Selection]} >> ";

                            // Now, determine the positions
                            int selector1PositionX = oneThirdConsoleWidth / 2 - selector1.Length / 2;
                            int selector2PositionX = 3 * oneThirdConsoleWidth / 2 - selector2.Length / 2;
                            int selector3PositionX = 5 * oneThirdConsoleWidth / 2 - selector3.Length / 2;

                            // Print the selected values to the console
                            TextWriterWhereColor.WriteWhereColorBack(selector1, selector1PositionX, selectorsPositionY, white, darkRed);
                            TextWriterWhereColor.WriteWhereColorBack(selector2, selector2PositionX, selectorsPositionY, white, darkRed);
                            TextWriterWhereColor.WriteWhereColorBack(selector3, selector3PositionX, selectorsPositionY, white, darkRed);

                            // Sleep
                            ScreensaverManager.Delay(100, true);
                        }
                        break;
                    // Step 5: Flash the red screen, alternating between the screen and a figlet text saying "2021"
                    case 5:
                        int maxFlashes = 50;
                        for (int flashes = 0; flashes <= maxFlashes; flashes++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;
                            if (ScreensaverManager.Bailing)
                                return;

                            bool showYear = flashes % 2 == 0;
                            if (showYear)
                            {
                                ColorTools.LoadBackDry(black);
                                var s5figFont = FigletTools.GetFigletFont("banner");
                                int s5figWidth = FigletTools.GetFigletWidth("2021", s5figFont) / 2;
                                int s5figHeight = FigletTools.GetFigletHeight("2021", s5figFont) / 2;
                                int s5consoleX = ConsoleWrapper.WindowWidth / 2 - s5figWidth;
                                int s5consoleY = ConsoleWrapper.WindowHeight / 2 - s5figHeight;
                                var s5Figlet = new FigletText(s5figFont)
                                {
                                    Text = "2021",
                                    ForegroundColor = darkRed,
                                    BackgroundColor = black,
                                };
                                TextWriterRaw.WriteRaw(RendererTools.RenderRenderable(s5Figlet, new(s5consoleX, s5consoleY)));
                            }
                            else
                                ColorTools.LoadBackDry(darkRed);
                            ScreensaverManager.Delay(50, true);
                        }
                        ScreensaverManager.Delay(3000, true);
                        break;
                    // Step 6: Glitch for a bit again
                    case 6:
                        for (int delayed = 0; delayed < 5000; delayed += 10)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;
                            if (ScreensaverManager.Bailing)
                                return;

                            ScreensaverManager.Delay(10, true);
                            Glitch.GlitchAt();
                        }
                        break;
                    // Step 7: Show a 30-second day selection screen trying to adjust itelf to April 30, 2021 while still glitching in the background
                    case 7:
                        // Get things ready
                        var targetDate = new DateTime(2021, 4, 30);
                        string renderedTarget = TimeDateRenderers.RenderDate(targetDate, FormatType.Short);
                        string renderedTargetLong = $"     {TimeDateRenderers.RenderDate(targetDate, FormatType.Long)}     ";
                        var s7figFont = FigletTools.GetFigletFont("small");
                        int s7figWidth = FigletTools.GetFigletWidth(renderedTarget, s7figFont) / 2;
                        int s7figHeight = FigletTools.GetFigletHeight(renderedTarget, s7figFont) / 2;
                        int s7consoleX = ConsoleWrapper.WindowWidth / 2 - s7figWidth;
                        int s7consoleY = ConsoleWrapper.WindowHeight / 2 - s7figHeight;

                        // Select a random day and month of 2021
                        for (int delayed = 0; delayed < 15000; delayed += 10)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;
                            if (ScreensaverManager.Bailing)
                                return;

                            ScreensaverManager.Delay(10, true);
                            int randomMonth = RandomDriver.Random(1, 12);
                            int randomDay = RandomDriver.Random(1, 28);
                            var selectedDate = new DateTime(2021, randomMonth, randomDay);

                            // Show it as a normal font
                            string renderedLong = $"     {TimeDateRenderers.RenderDate(selectedDate, FormatType.Long)}     ";
                            TextWriterWhereColor.WriteWhereColorBack(renderedLong, ConsoleWrapper.WindowWidth / 2 - renderedLong.Length / 2, s7consoleY + 5, darkRed, black);
                            Glitch.GlitchAt();
                        }

                        // Print the target date
                        var s7Figlet = new FigletText(s7figFont)
                        {
                            Text = renderedTarget,
                            ForegroundColor = darkRed,
                            BackgroundColor = black,
                        };
                        TextWriterRaw.WriteRaw(RendererTools.RenderRenderable(s7Figlet, new(s7consoleX, s7consoleY)));
                        TextWriterWhereColor.WriteWhereColorBack(renderedTargetLong, ConsoleWrapper.WindowWidth / 2 - renderedTargetLong.Length / 2, s7consoleY + 5, darkRed, black);
                        for (int delayed = 0; delayed < 5000; delayed += 10)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;
                            if (ScreensaverManager.Bailing)
                                return;

                            ScreensaverManager.Delay(10, true);
                            Glitch.GlitchAt();
                        }
                        break;
                    // Step 8: Cover part of the screen with the "X"
                    case 8:
                        for (int xIter = 0; xIter < 1000; xIter++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;
                            if (ScreensaverManager.Bailing)
                                return;

                            int xwidth = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
                            int xheight = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
                            TextWriterWhereColor.WriteWhereColorBack("X", xwidth, xheight, darkRed, black);
                            ScreensaverManager.Delay(10, true);
                        }
                        break;
                    // Step 9: Show a figlet text "X" while flashing between yellow and red, the yellow color slowly changing to green
                    case 9:
                        maxFlashes = 50;
                        double toGreenThresholdR = (green.RGB.R - yellow.RGB.R) / maxFlashes;
                        double toGreenThresholdG = (green.RGB.G - yellow.RGB.G) / maxFlashes;
                        double toGreenThresholdB = (green.RGB.B - yellow.RGB.B) / maxFlashes;
                        int currentRotR = yellow.RGB.R;
                        int currentRotG = yellow.RGB.G;
                        int currentRotB = yellow.RGB.B;
                        for (int flashes = 0; flashes <= maxFlashes; flashes++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;
                            if (ScreensaverManager.Bailing)
                                return;

                            bool showRot = flashes % 2 != 0;
                            Color color = darkRed;
                            if (showRot)
                            {
                                currentRotR = (int)Math.Round(currentRotR + toGreenThresholdR);
                                currentRotG = (int)Math.Round(currentRotG + toGreenThresholdG);
                                currentRotB = (int)Math.Round(currentRotB + toGreenThresholdB);
                                color = new Color(currentRotR, currentRotG, currentRotB);
                            }
                            ColorTools.LoadBackDry(color);
                            var s9figFont = FigletTools.GetFigletFont("banner");
                            int s9figWidth = FigletTools.GetFigletWidth("X", s9figFont) / 2;
                            int s9figHeight = FigletTools.GetFigletHeight("X", s9figFont) / 2;
                            int s9consoleX = ConsoleWrapper.WindowWidth / 2 - s9figWidth;
                            int s9consoleY = ConsoleWrapper.WindowHeight / 2 - s9figHeight;
                            var s9Figlet = new FigletText(s9figFont)
                            {
                                Text = "X",
                                ForegroundColor = darkRed,
                                BackgroundColor = black,
                            };
                            TextWriterRaw.WriteRaw(RendererTools.RenderRenderable(s9Figlet, new(s9consoleX, s9consoleY)));
                            ScreensaverManager.Delay(50, true);
                        }
                        break;
                    // Step 10: Flash red and green for a few seconds
                    case 10:
                        maxFlashes = 200;
                        for (int flashes = 0; flashes <= maxFlashes; flashes++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;
                            if (ScreensaverManager.Bailing)
                                return;

                            bool showRed = flashes % 2 == 0;
                            if (showRed)
                                ColorTools.LoadBackDry(red);
                            else
                                ColorTools.LoadBackDry(green);
                            ScreensaverManager.Delay(50, true);
                        }
                        break;
                    // Step 11: Show figlet text "0.0.16.0 M5" fading out to black
                    case 11:
                        colorSteps = 30;

                        // Get the color thresholds
                        double toBlackThresholdR = (black.RGB.R - red.RGB.R) / colorSteps;
                        double toBlackThresholdG = (black.RGB.G - red.RGB.G) / colorSteps;
                        double toBlackThresholdB = (black.RGB.B - red.RGB.B) / colorSteps;

                        // Now, transition from red to black
                        int currentFigletR = red.RGB.R;
                        int currentFigletG = red.RGB.G;
                        int currentFigletB = red.RGB.B;
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;
                            if (ScreensaverManager.Bailing)
                                return;

                            // Add the values according to the threshold
                            currentFigletR = (int)Math.Round(currentFigletR + toBlackThresholdR);
                            currentFigletG = (int)Math.Round(currentFigletG + toBlackThresholdG);
                            currentFigletB = (int)Math.Round(currentFigletB + toBlackThresholdB);

                            // Now, make a color and fill the console with it
                            Color col = new(currentFigletR, currentFigletG, currentFigletB);
                            var figFont = FigletTools.GetFigletFont("banner");
                            int figHeight = FigletTools.GetFigletHeight("0.0.16.0 M5", figFont) / 2;
                            int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
                            var xText = new AlignedFigletText(figFont)
                            {
                                Text = "0.0.16.0 M5",
                                Top = consoleY,
                                ForegroundColor = col,
                                BackgroundColor = red,
                                Settings = new()
                                {
                                    Alignment = TextAlignment.Middle,
                                }
                            };
                            TextWriterRaw.WriteRaw(xText.Render());

                            // Sleep
                            ScreensaverManager.Delay(100, true);
                        }
                        break;
                    // Step 12: Fade the console from red to black
                    case 12:
                        colorSteps = 30;

                        // Get the color thresholds
                        thresholdR = red.RGB.R / (double)colorSteps;
                        thresholdG = red.RGB.G / (double)colorSteps;
                        thresholdB = red.RGB.B / (double)colorSteps;

                        // Now, transition from target color to black
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;
                            if (ScreensaverManager.Bailing)
                                return;

                            // Remove the values according to the threshold
                            int currentRColor = (int)Math.Round(red.RGB.R - thresholdR * currentStep);
                            int currentGColor = (int)Math.Round(red.RGB.G - thresholdG * currentStep);
                            int currentBColor = (int)Math.Round(red.RGB.B - thresholdB * currentStep);

                            // Now, make a color and fill the console with it
                            Color col = new(currentRColor, currentGColor, currentBColor);
                            ColorTools.LoadBackDry(col);

                            // Sleep
                            ScreensaverManager.Delay(100, true);
                        }
                        break;
                    // Step 13: Show "to be continued"
                    case 13:
                        string tbc = LanguageTools.GetLocalized("NKS_SCREENSAVERPACKS_KSX_CONTINUED").ToUpper();
                        ScreensaverManager.Delay(100, true);
                        TextWriterWhereColor.WriteWhereColorBack(tbc, ConsoleWrapper.WindowWidth / 2 - tbc.Length / 2, ConsoleWrapper.WindowHeight / 2, green, black);
                        ScreensaverManager.Delay(40, true);
                        TextWriterWhereColor.WriteWhereColorBack(tbc, ConsoleWrapper.WindowWidth / 2 - tbc.Length / 2, ConsoleWrapper.WindowHeight / 2, black, black);
                        ScreensaverManager.Delay(100, true);
                        TextWriterWhereColor.WriteWhereColorBack(tbc, ConsoleWrapper.WindowWidth / 2 - tbc.Length / 2, ConsoleWrapper.WindowHeight / 2, green, black);
                        ScreensaverManager.Delay(50, true);
                        TextWriterWhereColor.WriteWhereColorBack(tbc, ConsoleWrapper.WindowWidth / 2 - tbc.Length / 2, ConsoleWrapper.WindowHeight / 2, black, black);
                        ScreensaverManager.Delay(1000, true);
                        TextWriterWhereColor.WriteWhereColorBack(tbc, ConsoleWrapper.WindowWidth / 2 - tbc.Length / 2, ConsoleWrapper.WindowHeight / 2, green, black);
                        ScreensaverManager.Delay(5000, true);
                        break;
                }
            }

            // Reset
            ConsoleResizeHandler.WasResized();
            ColorTools.LoadBackDry(black);
        }

    }
}
