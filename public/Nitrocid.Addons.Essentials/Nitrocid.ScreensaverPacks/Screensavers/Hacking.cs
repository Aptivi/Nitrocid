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
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Screensaver;
using Nitrocid.Base.Misc.Screensaver.Displays.Utilities;
using Nitrocid.ScreensaverPacks.Animations.Glitch;
using Nitrocid.ScreensaverPacks.Screensavers.Utilities;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Colors.Transformation;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Simple;
using Textify.Data.Figlet;
using Textify.General;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Hacking
    /// </summary>
    public class HackingDisplay : BaseScreensaver, IScreensaver
    {
        private static int hackingStage = 0;
        private static int hackingProgress = 0;
        private static readonly List<MatrixBleedState> bleedStates = [];
        private static readonly List<(string[] lines, int linePos, int x, int y, int width, int height)> windows = [];

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Hacking";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            ColorTools.LoadBackDry("0;0;0");
            bleedStates.Clear();
            windows.Clear();
            hackingStage = 0;
            hackingProgress = 0;
            ConsoleWrapper.Clear();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Now, determine the fall end position
            int FallEnd = ConsoleWrapper.WindowHeight - 1;

            // Invoke the "chance"-based random number generator to decide whether a line is about to fall.
            bool newFall = RandomDriver.RandomChance(ScreensaverPackInit.SaversConfig.HackingDropChance);
            if (newFall)
                bleedStates.Add(new MatrixBleedState());

            // Now, iterate through the bleed states
            for (int bleedIdx = 0; bleedIdx < bleedStates.Count; bleedIdx++)
            {
                // Choose the column for the falling line
                var bleedState = bleedStates[bleedIdx];

                // Make the line fall down
                switch (bleedState.fallState)
                {
                    case MatrixBleedFallState.Falling:
                        bleedState.Fall();
                        bleedState.fallStep++;
                        if (bleedState.fallStep > FallEnd)
                            bleedState.fallState = MatrixBleedFallState.Fading;
                        break;
                    case MatrixBleedFallState.Fading:
                        bleedState.Fade();
                        bleedState.fadeStep++;
                        if (bleedState.fadeStep > ScreensaverPackInit.SaversConfig.HackingMaxSteps)
                            bleedState.fallState = MatrixBleedFallState.Done;
                        break;
                }
            }

            // Purge the "Done" falls
            for (int bleedIdx = bleedStates.Count - 1; bleedIdx >= 0; bleedIdx--)
            {
                var bleedState = bleedStates[bleedIdx];
                if (bleedState.fallState == MatrixBleedFallState.Done)
                {
                    bleedStates.RemoveAt(bleedIdx);
                    bleedState.Unreserve(bleedState.ColumnLine);
                }
            }

            // Draw and add the buffer
            string buffer = MatrixBleedState.bleedBuffer.ToString();
            var finalBuffer = new StringBuilder();
            finalBuffer.Append(buffer);
            MatrixBleedState.bleedBuffer.Clear();

            // Check to see if we need to draw a new window
            bool needsNewWindow = RandomDriver.RandomChance(5);
            if (needsNewWindow)
            {
                // Position a new window randomly
                int width = RandomDriver.Random(ConsoleWrapper.WindowWidth / 5, ConsoleWrapper.WindowWidth / 2);
                int height = RandomDriver.Random(ConsoleWrapper.WindowHeight / 4, (int)Math.Round(ConsoleWrapper.WindowHeight / 1.5d));
                int posX = RandomDriver.Random(ConsoleWrapper.WindowWidth - width - 2);
                int posY = RandomDriver.Random(ConsoleWrapper.WindowHeight - height - 2);
                windows.Add((GetList(), 0, posX, posY, width, height));
                if (windows.Count > 10)
                {
                    var eraser = new Eraser()
                    {
                        Left = windows[0].x,
                        Top = windows[0].y,
                        Width = windows[0].width + 2,
                        Height = windows[0].height + 2,
                    };
                    finalBuffer.Append(eraser.Render());
                    windows.RemoveAt(0);
                    if (hackingStage == 0)
                        hackingStage++;
                }
            }

            // Draw a window while scrolling down
            for (int i = 0; i < windows.Count; i++)
            {
                var (lines, linePos, x, y, width, height) = windows[i];
                int firstLine = linePos - height;
                int lastLine = linePos;
                int absolutePos = y + 1;

                // Print the window
                var windowFrame = new BoxFrame()
                {
                    Left = x,
                    Top = y,
                    Width = width,
                    Height = height,
                    FrameColor = ConsoleColors.Green
                };
                finalBuffer.Append(windowFrame.Render());

                // Erase the area
                var eraser = new Eraser()
                {
                    Left = x + 1,
                    Top = y + 1,
                    Width = width,
                    Height = height,
                };
                finalBuffer.Append(eraser.Render());

                // Print the lines
                for (int j = firstLine; j <= lastLine - 1; j++, absolutePos++)
                {
                    if (j >= 0 && j < lines.Length)
                    {
                        string line = lines[j].Truncate(width);
                        finalBuffer.Append(TextWriterWhereColor.RenderWhereColor(line, x + 1, absolutePos, ConsoleColors.Green));
                    }
                }

                // Check to see if we've reached the end
                if (linePos > lines.Length + height + 1)
                    windows[i] = (GetList(), 0, x, y, width, height);
                else
                    windows[i] = (lines, linePos + 1, x, y, width, height);
            }

            // If we've reached hacking stage 1, 2, or 3, print a window
            if (hackingStage >= 1 || hackingStage <= 3)
            {
                // Use figlet to show the progress
                if (hackingStage == 3)
                    hackingProgress = 100;
                string process = hackingStage == 3 ? "ACCESS GRANTED" : hackingStage == 2 ? "DOWNLOADING" : "PLEASE WAIT";
                string processInfo = hackingStage == 3 ? "Welcome!" : hackingStage == 2 ? "Copying data..." : "Brute-force in progress...";
                var figFont = FigletTools.GetFigletFont("computer");
                int figFullWidth = FigletTools.GetFigletWidth(process, figFont);
                int figFullHeight = FigletTools.GetFigletHeight(process, figFont);
                int figWidth = figFullWidth / 2;
                int figHeight = figFullHeight / 2;
                int figletX = ConsoleWrapper.WindowWidth / 2 - figWidth;
                int figletY = ConsoleWrapper.WindowHeight / 2 - figHeight - 1;
                int infoY = ConsoleWrapper.WindowHeight / 2 + figHeight + 1;
                int progressY = ConsoleWrapper.WindowHeight / 2 + figHeight + 2;
                var windowFrame = new BoxFrame()
                {
                    Left = figletX - 1,
                    Top = figletY - 1,
                    Width = figFullWidth + 1,
                    Height = figFullHeight + 4,
                    FrameColor = ConsoleColors.Lime
                };
                finalBuffer.Append(windowFrame.Render());

                // Erase the area
                var eraser = new Eraser()
                {
                    Left = figletX,
                    Top = figletY,
                    Width = figFullWidth + 1,
                    Height = figFullHeight + 4,
                };
                finalBuffer.Append(eraser.Render());

                // Add the process text
                var processText = new AlignedFigletText(figFont)
                {
                    Left = figletX + 1,
                    Top = figletY,
                    Text = process,
                    ForegroundColor = ConsoleColors.Lime
                };
                var processProgress = new SimpleProgress(hackingProgress, 100)
                {
                    Width = figFullWidth - 2,
                    ProgressActiveForegroundColor = ConsoleColors.Lime,
                    ProgressForegroundColor = TransformationTools.GetDarkBackground(ConsoleColors.Lime),
                };
                finalBuffer.Append(processText.Render());
                finalBuffer.Append(TextWriterWhereColor.RenderWhereColor(processInfo, figletX + 1, infoY, ConsoleColors.Lime));
                finalBuffer.Append(RendererTools.RenderRenderable(processProgress, new(figletX + 1, progressY)));

                // Increment the progress
                bool increment = hackingStage < 3 && RandomDriver.RandomChance(10);
                increment = hackingStage == 2 ? RandomDriver.RandomChance(5) : increment;
                if (increment)
                {
                    hackingProgress++;
                    if (hackingProgress == 100)
                    {
                        hackingProgress = 0;
                        hackingStage++;
                    }
                }
            }

            // Render the final buffer
            TextWriterRaw.WritePlain(finalBuffer.ToString());

            // Delay
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.HackingDelay);
        }

        private static string[] GetList()
        {
            bool code = RandomDriver.RandomChance(15);
            if (code)
                return HackingCodeList.codeSnippets[RandomDriver.RandomIdx(HackingCodeList.codeSnippets.Length)].SplitNewLines();
            return HackingLogList.logSnippets[RandomDriver.RandomIdx(HackingLogList.logSnippets.Length)].SplitNewLines();
        }
    }
}
