
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using ColorSeq;
using KS.ConsoleBase;
using KS.Kernel.Debugging;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;
using KS.Misc.Writers.FancyWriters.Tools;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Display code for KSX
    /// </summary>
    public class KSXDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "KSX";

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
            int maxSteps = 19;
            Color darkGreen = new(ConsoleColors.DarkGreen);
            Color green = new(ConsoleColors.Green);
            Color black = new(ConsoleColors.Black);

            // Start stepping
            for (step = 1; step <= maxSteps; step++)
            {
                if (ConsoleResizeListener.WasResized(false))
                    break;

                switch (step)
                {
                    // Step 1: fade the X character in, and use figlet
                    case 1:
                        int colorSteps = 30;

                        // Get the color thresholds
                        double thresholdR = darkGreen.R / (double)colorSteps;
                        double thresholdG = darkGreen.G / (double)colorSteps;
                        double thresholdB = darkGreen.B / (double)colorSteps;

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

                            // Now, make a color and write the X character using figlet
                            Color col = new(currentR, currentG, currentB);
                            var figFont = FigletTools.GetFigletFont("Banner");
                            int figWidth = FigletTools.GetFigletWidth("X", figFont) / 2;
                            int figHeight = FigletTools.GetFigletHeight("X", figFont) / 2;
                            int consoleX = (ConsoleWrapper.WindowWidth / 2) - (figWidth / 2);
                            int consoleY = (ConsoleWrapper.WindowHeight / 2) - (figHeight / 2);
                            FigletWhereColor.WriteFigletWhere("X", consoleX, consoleY, true, figFont, col);

                            // Sleep
                            ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        break;
                    case 2:
                        int pulses = 11;
                        for (int currentPulse = 1; currentPulse <= pulses; currentPulse++)
                        {
                            if (ConsoleResizeListener.WasResized(false))
                                break;

                            // Get the final color by using the odd/even comparison
                            var finalCol = currentPulse % 2 == 0 ? green : darkGreen;

                            // Pulse the X character, alternating between darkGreen and Green colors
                            var figFont = FigletTools.GetFigletFont("Banner");
                            int figWidth = FigletTools.GetFigletWidth("X", figFont) / 2;
                            int figHeight = FigletTools.GetFigletHeight("X", figFont) / 2;
                            int consoleX = (ConsoleWrapper.WindowWidth / 2) - (figWidth / 2);
                            int consoleY = (ConsoleWrapper.WindowHeight / 2) - (figHeight / 2);
                            FigletWhereColor.WriteFigletWhere("X", consoleX, consoleY, true, figFont, finalCol);

                            // Sleep
                            ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        break;
                    case 3:
                        colorSteps = 30;

                        // Get the color thresholds
                        thresholdR = darkGreen.R / (double)colorSteps;
                        thresholdG = darkGreen.G / (double)colorSteps;
                        thresholdB = darkGreen.B / (double)colorSteps;

                        // Now, transition from black to the target color
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeListener.WasResized(false))
                                break;

                            // Remove the values according to the threshold
                            currentR = (int)Math.Round(darkGreen.R - thresholdR * currentStep);
                            currentG = (int)Math.Round(darkGreen.G - thresholdG * currentStep);
                            currentB = (int)Math.Round(darkGreen.B - thresholdB * currentStep);

                            // Now, make a color and write the X character using figlet
                            Color col = new(currentR, currentG, currentB);
                            var figFont = FigletTools.GetFigletFont("Banner");
                            int figWidth = FigletTools.GetFigletWidth("X", figFont) / 2;
                            int figHeight = FigletTools.GetFigletHeight("X", figFont) / 2;
                            int consoleX = (ConsoleWrapper.WindowWidth / 2) - (figWidth / 2);
                            int consoleY = (ConsoleWrapper.WindowHeight / 2) - (figHeight / 2);
                            FigletWhereColor.WriteFigletWhere("X", consoleX, consoleY, true, figFont, col);

                            // Sleep
                            ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }

                        // Print the 2018s
                        string sample = "2018|";
                        bool printDone = false;
                        ConsoleWrapper.CursorLeft = 0;
                        ConsoleWrapper.CursorTop = 0;
                        while (!printDone)
                        {
                            // Keep writing 2018 until it reaches the end
                            for (int currentIdx = 0; currentIdx <= sample.Length - 1 && !printDone; currentIdx++)
                            {
                                // Write the current character
                                TextWriterColor.Write(sample[currentIdx].ToString(), false, darkGreen);

                                // Sleep
                                ThreadManager.SleepNoBlock(10, ScreensaverDisplayer.ScreensaverDisplayerThread);

                                // Check to see if we're at the end
                                if (ConsoleWrapper.CursorLeft == ConsoleWrapper.WindowWidth - 1 &&
                                    ConsoleWrapper.CursorTop == ConsoleWrapper.WindowHeight - 1)
                                {
                                    // We're at the end. Increment the index or reset to zero
                                    currentIdx++;
                                    if (currentIdx > sample.Length - 1)
                                        currentIdx = 0;

                                    // Write the current character
                                    TextWriterColor.Write(sample[currentIdx].ToString(), false, darkGreen);

                                    // Reset position
                                    ConsoleWrapper.CursorLeft = 0;
                                    ConsoleWrapper.CursorTop = 0;

                                    // Declare as done
                                    printDone = true;
                                }
                            }
                        }
                        break;
                    case 4:
                        // Print the big 2018
                        var s4figFont = FigletTools.GetFigletFont("Banner");
                        int s4figWidth = FigletTools.GetFigletWidth("2018", s4figFont) / 2;
                        int s4figHeight = FigletTools.GetFigletHeight("2018", s4figFont) / 2;
                        int s4consoleX = (ConsoleWrapper.WindowWidth / 2) - s4figWidth;
                        int s4consoleY = (ConsoleWrapper.WindowHeight / 2) - s4figHeight;
                        FigletWhereColor.WriteFigletWhere("2018", s4consoleX, s4consoleY, true, s4figFont, green);
                        ThreadManager.SleepNoBlock(5000, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        break;
                    case 5:
                        // Fade the console out
                        colorSteps = 30;

                        // Get the color thresholds
                        thresholdR = darkGreen.R / (double)colorSteps;
                        thresholdG = darkGreen.G / (double)colorSteps;
                        thresholdB = darkGreen.B / (double)colorSteps;

                        // Now, transition from target color to black
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeListener.WasResized(false))
                                break;

                            // Remove the values according to the threshold
                            currentR = (int)Math.Round(darkGreen.R - thresholdR * currentStep);
                            currentG = (int)Math.Round(darkGreen.G - thresholdG * currentStep);
                            currentB = (int)Math.Round(darkGreen.B - thresholdB * currentStep);

                            // Now, make a color and fill the console with it
                            Color col = new(currentR, currentG, currentB);
                            ConsoleBase.Colors.ColorTools.LoadBack(col, true);

                            // Sleep
                            ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        break;
                }
            }

            // Reset
            ConsoleResizeListener.WasResized();
            ConsoleBase.Colors.ColorTools.LoadBack(black, true);
        }

    }
}
