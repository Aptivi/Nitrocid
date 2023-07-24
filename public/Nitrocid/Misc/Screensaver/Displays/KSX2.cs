
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

using System;
using System.Collections.Generic;
using System.Linq;
using ColorSeq;
using KS.ConsoleBase;
using KS.Drivers;
using KS.Drivers.RNG;
using KS.Kernel.Debugging;
using KS.Kernel.Time;
using KS.Kernel.Time.Renderers;
using KS.Languages;
using KS.Misc.Animations.BSOD.Simulations;
using KS.Misc.Animations.Glitch;
using KS.Misc.Text;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;
using KS.Misc.Writers.FancyWriters.Tools;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Display code for KSX2
    /// </summary>
    public class KSX2Display : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "KSX2";

        /// <inheritdoc/>
        public override bool ScreensaverContainsFlashingImages { get; set; } = true;

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ConsoleBase.Colors.ColorTools.LoadBack(new Color(ConsoleColors.Black), true);
            ConsoleWrapper.CursorVisible = false;
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            int step;
            int maxSteps = 13;
            Color darkGreen = new(ConsoleColors.DarkGreen);
            Color darkRed = new(ConsoleColors.DarkRed);
            Color red = new(ConsoleColors.Red);
            Color yellow = new(ConsoleColors.Yellow);
            Color green = new(ConsoleColors.Green);
            Color black = new(ConsoleColors.Black);
            Color white = new(ConsoleColors.White);

            // Start stepping
            for (step = 1; step <= maxSteps; step++)
            {
                if (ConsoleResizeListener.WasResized(false))
                    break;

                switch (step)
                {
                    // Step 1: Fade out from white to black
                    case 1:
                        // Fade the console out
                        int colorSteps = 30;

                        // Get the color thresholds
                        double thresholdR = white.R / (double)colorSteps;
                        double thresholdG = white.G / (double)colorSteps;
                        double thresholdB = white.B / (double)colorSteps;

                        // Now, transition from target color to black
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeListener.WasResized(false))
                                break;

                            // Remove the values according to the threshold
                            int currentRColor = (int)Math.Round(white.R - thresholdR * currentStep);
                            int currentGColor = (int)Math.Round(white.G - thresholdG * currentStep);
                            int currentBColor = (int)Math.Round(white.B - thresholdB * currentStep);

                            // Now, make a color and fill the console with it
                            Color col = new(currentRColor, currentGColor, currentBColor);
                            ConsoleBase.Colors.ColorTools.LoadBack(col, true);

                            // Sleep
                            ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        break;
                    // Step 2: fade in slowly to dark green, then slowly change to dark red
                    case 2:
                        colorSteps = 30;

                        // Get the color thresholds
                        thresholdR = darkGreen.R / (double)colorSteps;
                        thresholdG = darkGreen.G / (double)colorSteps;
                        thresholdB = darkGreen.B / (double)colorSteps;
                        double transitionThresholdR = (darkRed.R - darkGreen.R) / (double)colorSteps;
                        double transitionThresholdG = (darkRed.G - darkGreen.G) / (double)colorSteps;
                        double transitionThresholdB = (darkRed.B - darkGreen.B) / (double)colorSteps;

                        // Now, transition from black to the target color
                        int currentR = 0;
                        int currentG = 0;
                        int currentB = 0;
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeListener.WasResized(false))
                                break;

                            // Add the values according to the threshold
                            currentR = (int)Math.Round(currentR + thresholdR);
                            currentG = (int)Math.Round(currentG + thresholdG);
                            currentB = (int)Math.Round(currentB + thresholdB);

                            // Now, make a color and fill the console with it
                            Color col = new(currentR, currentG, currentB);
                            ConsoleBase.Colors.ColorTools.LoadBack(col, true);

                            // Sleep
                            ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeListener.WasResized(false))
                                break;

                            // Add the values according to the threshold
                            currentR = (int)Math.Round(currentR + transitionThresholdR);
                            currentG = (int)Math.Round(currentG + transitionThresholdG);
                            currentB = (int)Math.Round(currentB + transitionThresholdB);

                            // Now, make a color and fill the console with it
                            Color col = new(currentR, currentG, currentB);
                            ConsoleBase.Colors.ColorTools.LoadBack(col, true);

                            // Sleep
                            ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        break;
                    // Step 3: Glitch for a bit
                    case 3:
                        for (int delayed = 0; delayed < 5000; delayed += 10)
                        {
                            ThreadManager.SleepNoBlock(10, ScreensaverDisplayer.ScreensaverDisplayerThread);
                            Glitch.GlitchAt();
                        }
                        break;
                    // Step 4: Show three selectors adjusting to 0.0.16.0
                    case 4:
                        string[] versions = new[]
                        {
                            "0.0.1.0", "0.0.4.0", "0.0.8.0", "0.0.16.0", "0.0.20.0", "0.0.24.0"
                        };
                        int selector1Selection = 0;
                        int selector2Selection = 0;
                        int selector3Selection = 0;
                        int oneThirdConsoleWidth = ConsoleWrapper.WindowWidth / 3;
                        int selectorsPositionY = ConsoleWrapper.WindowHeight - 2;
                        while (selector1Selection != 3 || selector2Selection != 3 || selector3Selection != 3)
                        {
                            // Get the random indexes and the random versions
                            selector1Selection = RandomDriver.RandomIdx(versions.Length);
                            selector2Selection = RandomDriver.RandomIdx(versions.Length);
                            selector3Selection = RandomDriver.RandomIdx(versions.Length);
                            string selector1 = $"< {versions[selector1Selection]} >";
                            string selector2 = $"< {versions[selector2Selection]} >";
                            string selector3 = $"< {versions[selector3Selection]} >";

                            // Now, determine the positions
                            int selector1PositionX =     (oneThirdConsoleWidth / 2) - (selector1.Length / 2);
                            int selector2PositionX = (3 * oneThirdConsoleWidth / 2) - (selector2.Length / 2);
                            int selector3PositionX = (5 * oneThirdConsoleWidth / 2) - (selector3.Length / 2);

                            // Print the selected values to the console
                            TextWriterWhereColor.WriteWhere(selector1, selector1PositionX, selectorsPositionY, white, darkRed);
                            TextWriterWhereColor.WriteWhere(selector2, selector2PositionX, selectorsPositionY, white, darkRed);
                            TextWriterWhereColor.WriteWhere(selector3, selector3PositionX, selectorsPositionY, white, darkRed);

                            // Sleep
                            ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        break;
                    // Step 5: Flash the red screen, alternating between the screen and a figlet text saying "2021"
                    case 5:
                        int maxFlashes = 50;
                        for (int flashes = 0; flashes <= maxFlashes; flashes++)
                        {
                            bool showYear = flashes % 2 == 0;
                            if (showYear)
                            {
                                ConsoleBase.Colors.ColorTools.LoadBack(black, true);
                                var s5figFont = FigletTools.GetFigletFont("Banner");
                                int s5figWidth = FigletTools.GetFigletWidth("2021", s5figFont) / 2;
                                int s5figHeight = FigletTools.GetFigletHeight("2021", s5figFont) / 2;
                                int s5consoleX = (ConsoleWrapper.WindowWidth / 2) - s5figWidth;
                                int s5consoleY = (ConsoleWrapper.WindowHeight / 2) - s5figHeight;
                                FigletWhereColor.WriteFigletWhere("2021", s5consoleX, s5consoleY, true, s5figFont, darkRed, black);
                            }
                            else
                                ConsoleBase.Colors.ColorTools.LoadBack(darkRed, true);
                            ThreadManager.SleepNoBlock(50, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        ThreadManager.SleepNoBlock(3000, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        break;
                    // Step 6: Glitch for a bit again
                    case 6:
                        for (int delayed = 0; delayed < 5000; delayed += 10)
                        {
                            ThreadManager.SleepNoBlock(10, ScreensaverDisplayer.ScreensaverDisplayerThread);
                            Glitch.GlitchAt();
                        }
                        break;
                    // Step 7: Show a 30-second day selection screen trying to adjust itelf to April 30, 2021 while still glitching in the background
                    case 7:
                        // Get things ready
                        var targetDate = new DateTime(2021, 4, 30);
                        string renderedTarget = TimeDateRenderers.RenderDate(targetDate, FormatType.Short);
                        string renderedTargetLong = TimeDateRenderers.RenderDate(targetDate, FormatType.Long);
                        var s7figFont = FigletTools.GetFigletFont("Small");
                        int s7figWidth = FigletTools.GetFigletWidth(renderedTarget, s7figFont) / 2;
                        int s7figHeight = FigletTools.GetFigletHeight(renderedTarget, s7figFont) / 2;
                        int s7consoleX = (ConsoleWrapper.WindowWidth / 2) - s7figWidth;
                        int s7consoleY = (ConsoleWrapper.WindowHeight / 2) - s7figHeight;

                        // Select a random day and month of 2021
                        for (int delayed = 0; delayed < 15000; delayed += 10)
                        {
                            ThreadManager.SleepNoBlock(10, ScreensaverDisplayer.ScreensaverDisplayerThread);
                            int randomMonth = RandomDriver.Random(1, 12);
                            int randomDay = RandomDriver.Random(1, 28);
                            var selectedDate = new DateTime(2021, randomMonth, randomDay);

                            // Show it as a normal font
                            string renderedLong = $"     {TimeDateRenderers.RenderDate(selectedDate, FormatType.Long)}     ";
                            TextWriterWhereColor.WriteWhere(renderedLong, (ConsoleWrapper.WindowWidth / 2) - (renderedLong.Length / 2), s7consoleY + 5, darkRed, black);
                            Glitch.GlitchAt();
                        }

                        // Print the target date
                        FigletWhereColor.WriteFigletWhere(renderedTarget, s7consoleX, s7consoleY, true, s7figFont, darkRed, black);
                        TextWriterWhereColor.WriteWhere(renderedTargetLong, (ConsoleWrapper.WindowWidth / 2) - (renderedTargetLong.Length / 2), s7consoleY + 5, darkRed, black);
                        for (int delayed = 0; delayed < 5000; delayed += 10)
                        {
                            ThreadManager.SleepNoBlock(10, ScreensaverDisplayer.ScreensaverDisplayerThread);
                            Glitch.GlitchAt();
                        }
                        break;
                    // Step 8: Cover part of the screen with the "X"
                    case 8:
                        for (int xIter = 0; xIter < 1000; xIter++)
                        {
                            int xwidth = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
                            int xheight = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
                            TextWriterWhereColor.WriteWhere("X", xwidth, xheight, darkRed, black);
                            ThreadManager.SleepNoBlock(10, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        break;
                    // Step 9: Show a figlet text "X" while flashing between yellow and red, the yellow color slowly changing to green
                    case 9:
                        maxFlashes = 50;
                        double toGreenThresholdR = (green.R - yellow.R) / maxFlashes;
                        double toGreenThresholdG = (green.G - yellow.G) / maxFlashes;
                        double toGreenThresholdB = (green.B - yellow.B) / maxFlashes;
                        int currentRotR = yellow.R;
                        int currentRotG = yellow.G;
                        int currentRotB = yellow.B;
                        for (int flashes = 0; flashes <= maxFlashes; flashes++)
                        {
                            bool showRot = flashes % 2 != 0;
                            Color color = darkRed;
                            if (showRot)
                            {
                                currentRotR = (int)Math.Round(currentRotR + toGreenThresholdR);
                                currentRotG = (int)Math.Round(currentRotG + toGreenThresholdG);
                                currentRotB = (int)Math.Round(currentRotB + toGreenThresholdB);
                                color = new Color(currentRotR, currentRotG, currentRotB);
                            }
                            ConsoleBase.Colors.ColorTools.LoadBack(color, true);
                            var s5figFont = FigletTools.GetFigletFont("Banner");
                            int s5figWidth = FigletTools.GetFigletWidth("X", s5figFont) / 2;
                            int s5figHeight = FigletTools.GetFigletHeight("X", s5figFont) / 2;
                            int s5consoleX = (ConsoleWrapper.WindowWidth / 2) - s5figWidth;
                            int s5consoleY = (ConsoleWrapper.WindowHeight / 2) - s5figHeight;
                            FigletWhereColor.WriteFigletWhere("X", s5consoleX, s5consoleY, true, s5figFont, darkRed, black);
                            ThreadManager.SleepNoBlock(50, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        break;
                    // Step 10: Flash red and green for a few seconds
                    case 10:
                        maxFlashes = 200;
                        for (int flashes = 0; flashes <= maxFlashes; flashes++)
                        {
                            bool showRed = flashes % 2 == 0;
                            if (showRed)
                                ConsoleBase.Colors.ColorTools.LoadBack(red, true);
                            else
                                ConsoleBase.Colors.ColorTools.LoadBack(green, true);
                            ThreadManager.SleepNoBlock(50, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        break;
                    // Step 11: Show figlet text "0.0.16.0 M5" fading out to black
                    case 11:
                        colorSteps = 30;

                        // Get the color thresholds
                        double toBlackThresholdR = (black.R - red.R) / colorSteps;
                        double toBlackThresholdG = (black.G - red.G) / colorSteps;
                        double toBlackThresholdB = (black.B - red.B) / colorSteps;

                        // Now, transition from red to black
                        int currentFigletR = red.R;
                        int currentFigletG = red.G;
                        int currentFigletB = red.B;
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeListener.WasResized(false))
                                break;

                            // Add the values according to the threshold
                            currentFigletR = (int)Math.Round(currentFigletR + toBlackThresholdR);
                            currentFigletG = (int)Math.Round(currentFigletG + toBlackThresholdG);
                            currentFigletB = (int)Math.Round(currentFigletB + toBlackThresholdB);

                            // Now, make a color and fill the console with it
                            Color col = new(currentFigletR, currentFigletG, currentFigletB);
                            var figFont = FigletTools.GetFigletFont("Banner");
                            int figWidth = FigletTools.GetFigletWidth("0.0.16.0 M5", figFont) / 2;
                            int figHeight = FigletTools.GetFigletHeight("0.0.16.0 M5", figFont) / 2;
                            int consoleX = (ConsoleWrapper.WindowWidth / 2) - figWidth;
                            int consoleY = (ConsoleWrapper.WindowHeight / 2) - figHeight;
                            FigletWhereColor.WriteFigletWhere("0.0.16.0 M5", consoleX, consoleY, true, figFont, col, red);

                            // Sleep
                            ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        break;
                    // Step 12: Fade the console from red to black
                    case 12:
                        colorSteps = 30;

                        // Get the color thresholds
                        thresholdR = red.R / (double)colorSteps;
                        thresholdG = red.G / (double)colorSteps;
                        thresholdB = red.B / (double)colorSteps;

                        // Now, transition from target color to black
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeListener.WasResized(false))
                                break;

                            // Remove the values according to the threshold
                            int currentRColor = (int)Math.Round(red.R - thresholdR * currentStep);
                            int currentGColor = (int)Math.Round(red.G - thresholdG * currentStep);
                            int currentBColor = (int)Math.Round(red.B - thresholdB * currentStep);

                            // Now, make a color and fill the console with it
                            Color col = new(currentRColor, currentGColor, currentBColor);
                            ConsoleBase.Colors.ColorTools.LoadBack(col, true);

                            // Sleep
                            ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        break;
                    // Step 13: Show "to be continued"
                    case 13:
                        string tbc = Translate.DoTranslation("To be continued...").ToUpper();
                        ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        TextWriterWhereColor.WriteWhere(tbc, (ConsoleWrapper.WindowWidth / 2) - (tbc.Length / 2), ConsoleWrapper.WindowHeight / 2, green);
                        ThreadManager.SleepNoBlock(40, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        TextWriterWhereColor.WriteWhere(tbc, (ConsoleWrapper.WindowWidth / 2) - (tbc.Length / 2), ConsoleWrapper.WindowHeight / 2, black);
                        ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        TextWriterWhereColor.WriteWhere(tbc, (ConsoleWrapper.WindowWidth / 2) - (tbc.Length / 2), ConsoleWrapper.WindowHeight / 2, green);
                        ThreadManager.SleepNoBlock(50, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        TextWriterWhereColor.WriteWhere(tbc, (ConsoleWrapper.WindowWidth / 2) - (tbc.Length / 2), ConsoleWrapper.WindowHeight / 2, black);
                        ThreadManager.SleepNoBlock(1000, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        TextWriterWhereColor.WriteWhere(tbc, (ConsoleWrapper.WindowWidth / 2) - (tbc.Length / 2), ConsoleWrapper.WindowHeight / 2, green);
                        ThreadManager.SleepNoBlock(5000, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        break;
                }
            }

            // Reset
            ConsoleResizeListener.WasResized();
            ConsoleBase.Colors.ColorTools.LoadBack(black, true);
        }

    }
}
