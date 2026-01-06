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
using System.Text;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Textify.Data.Figlet;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Hacking
    /// </summary>
    public class HackingDisplay : BaseScreensaver, IScreensaver
    {
        private int ColumnLine;
        private readonly List<(int, int, string)> CoveredPositions = [];
        private readonly Color foreground = new("0;255;0");
        private readonly Color background = new("0;0;0");

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Hacking";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            ColorTools.LoadBackDry("0;0;0");
            ConsoleWrapper.Clear();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ColorTools.LoadBackDry("0;0;0");
            ConsoleWrapper.Clear();

            // Use figlet to show the header
            string word = "HACKING";
            string wordDate = "Coming February 12th, 2026";
            var figFont = FigletTools.GetFigletFont("computer");
            int figHeight = FigletTools.GetFigletHeight(word, figFont) / 2;
            int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
            int hashY = ConsoleWrapper.WindowHeight / 2 + figHeight + 2;
            var wordText = new AlignedFigletText(figFont)
            {
                Top = consoleY,
                Text = word,
                ForegroundColor = ConsoleColors.Green,
                Settings = new()
                {
                    Alignment = TextAlignment.Middle,
                }
            };

            // Choose the column for the falling line
            ColumnLine = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);

            // Now, determine the fall start and end position
            int FallStart = 0;
            int FallEnd = ConsoleWrapper.WindowHeight - 1;

            // Make the line fall down
            for (int Fall = FallStart; Fall <= FallEnd; Fall++)
            {
                // Check to see if user decided to resize
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                if (ScreensaverManager.Bailing)
                    return;

                // Print a block and add the covered position to the list so fading down can be done
                string renderedNumber = RandomDriver.Random(1).ToString();
                TextWriterWhereColor.WriteWhereColorBack(renderedNumber, ColumnLine, Fall, false, foreground, background);
                TextWriterRaw.WriteRaw(wordText.Render());
                TextWriterWhereColor.WriteWhereColor(wordDate, (int)Math.Round(ConsoleWrapper.WindowWidth / 2d - wordDate.Length / 2d), hashY, ConsoleColors.Green);
                var PositionTuple = (ColumnLine, Fall, renderedNumber);
                CoveredPositions.Add(PositionTuple);

                // Delay
                ScreensaverManager.Delay(10);
            }

            // Fade the line down. Please note that this requires true-color support in the terminal to work properly.
            for (int StepNum = 0; StepNum <= 25; StepNum++)
            {
                // Check to see if user decided to resize
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                if (ScreensaverManager.Bailing)
                    return;

                // Set thresholds
                double ThresholdRed = foreground.RGB.R / (double)25;
                double ThresholdGreen = foreground.RGB.G / (double)25;
                double ThresholdBlue = foreground.RGB.B / (double)25;
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0})", vars: [ThresholdRed, ThresholdGreen, ThresholdBlue]);

                // Set color fade steps
                int CurrentColorRedOut = (int)Math.Round(foreground.RGB.R - ThresholdRed * StepNum);
                int CurrentColorGreenOut = (int)Math.Round(foreground.RGB.G - ThresholdGreen * StepNum);
                int CurrentColorBlueOut = (int)Math.Round(foreground.RGB.B - ThresholdBlue * StepNum);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", vars: [CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut]);

                // Get the positions and write the block with new color
                var CurrentFadeColor = new Color(CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut);
                var bleedBuilder = new StringBuilder();
                foreach ((int, int, string) PositionTuple in CoveredPositions)
                {
                    // Check to see if user decided to resize
                    if (ConsoleResizeHandler.WasResized(false))
                        break;
                    if (ScreensaverManager.Bailing)
                        return;

                    // Actually fade the line out
                    int PositionLeft = PositionTuple.Item1;
                    int PositionTop = PositionTuple.Item2;
                    string renderedNumber = PositionTuple.Item3;
                    bleedBuilder.Append($"{CsiSequences.GenerateCsiCursorPosition(PositionLeft + 1, PositionTop + 1)}{renderedNumber}");
                }
                TextWriterWhereColor.WriteWhereColorBack(bleedBuilder.ToString(), ColumnLine, 0, false, CurrentFadeColor, background);
                TextWriterRaw.WriteRaw(wordText.Render());
                TextWriterWhereColor.WriteWhereColor(wordDate, (int)Math.Round(ConsoleWrapper.WindowWidth / 2d - wordDate.Length / 2d), hashY, ConsoleColors.Green);

                // Delay
                ScreensaverManager.Delay(10);
            }

            // Reset covered positions
            CoveredPositions.Clear();
            ScreensaverManager.Delay(10);
        }

        /// <inheritdoc/>
        public override void ScreensaverOutro() =>
            CoveredPositions.Clear();

    }
}
