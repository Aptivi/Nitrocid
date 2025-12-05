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

using System;
using System.Text;
using System.Threading;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Splash;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Sequences.Builder.Types;

namespace Nitrocid.SplashPacks.Splashes
{
    class SplashEdgePulse : BaseSplash, ISplash
    {

        private bool _isFadingOut = false;
        private int _currentStep = 0;
        private Color _currentColor = Color.Empty;
        private bool _inited = false;
        private readonly int _edgepulseDelay = 50;
        private readonly int _edgepulseMaxSteps = 25;
        private readonly int _edgepulseMinimumRedColorLevel = 0;
        private readonly int _edgepulseMinimumGreenColorLevel = 0;
        private readonly int _edgepulseMinimumBlueColorLevel = 0;
        private readonly int _edgepulseMaximumRedColorLevel = 255;
        private readonly int _edgepulseMaximumGreenColorLevel = 255;
        private readonly int _edgepulseMaximumBlueColorLevel = 255;

        // Standalone splash information
        public override string SplashName => "EdgePulse";

        public override bool RequiresBackground => true;

        // Actual logic
        public override string Opening(SplashContext context)
        {
            if (!_inited)
            {
                int RedColorNum = RandomDriver.Random(_edgepulseMinimumRedColorLevel, _edgepulseMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(_edgepulseMinimumGreenColorLevel, _edgepulseMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(_edgepulseMinimumBlueColorLevel, _edgepulseMaximumBlueColorLevel);
                _currentColor = new(RedColorNum, GreenColorNum, BlueColorNum);
                _inited = true;
            }
            return base.Opening(context);
        }

        public override string Display(SplashContext context)
        {
            var builder = new StringBuilder();
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash displaying.");
                ConsoleWrapper.CursorVisible = false;

                // Get color levels
                int RedColorNum = _currentColor.RGB.R;
                int GreenColorNum = _currentColor.RGB.G;
                int BlueColorNum = _currentColor.RGB.B;

                // Set thresholds
                double ThresholdRed = RedColorNum / (double)_edgepulseMaxSteps;
                double ThresholdGreen = GreenColorNum / (double)_edgepulseMaxSteps;
                double ThresholdBlue = BlueColorNum / (double)_edgepulseMaxSteps;
                DebugWriter.WriteDebug(DebugLevel.I, "Color threshold (R;G;B: {0})", ThresholdRed, ThresholdGreen, ThresholdBlue);

                // Fade in or out
                if (_isFadingOut)
                {
                    // Fade out
                    int CurrentColorRedOut = RedColorNum;
                    int CurrentColorGreenOut = GreenColorNum;
                    int CurrentColorBlueOut = BlueColorNum;
                    DebugWriter.WriteDebug(DebugLevel.I, "Step {0}/{1}", _currentStep, _edgepulseMaxSteps);
                    ThreadManager.SleepNoBlock(_edgepulseDelay);
                    CurrentColorRedOut = (int)Math.Round(CurrentColorRedOut - ThresholdRed * _currentStep);
                    CurrentColorGreenOut = (int)Math.Round(CurrentColorGreenOut - ThresholdGreen * _currentStep);
                    CurrentColorBlueOut = (int)Math.Round(CurrentColorBlueOut - ThresholdBlue * _currentStep);
                    DebugWriter.WriteDebug(DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut);
                    var color = new Color(CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut);
                    builder.Append(
                        color.VTSequenceBackground +
                        FillIn()
                    );
                    _currentStep++;
                    if (_currentStep > _edgepulseMaxSteps)
                    {
                        _currentStep = 0;
                        _isFadingOut = false;
                    }
                }
                else
                {
                    // Fade in
                    int CurrentColorRedIn = 0;
                    int CurrentColorGreenIn = 0;
                    int CurrentColorBlueIn = 0;
                    DebugWriter.WriteDebug(DebugLevel.I, "Step {0}/{1}", _currentStep, _edgepulseMaxSteps);
                    ThreadManager.SleepNoBlock(_edgepulseDelay);
                    CurrentColorRedIn = (int)Math.Round((CurrentColorRedIn + ThresholdRed) * _currentStep);
                    CurrentColorGreenIn = (int)Math.Round((CurrentColorGreenIn + ThresholdGreen) * _currentStep);
                    CurrentColorBlueIn = (int)Math.Round((CurrentColorBlueIn + ThresholdBlue) * _currentStep);
                    DebugWriter.WriteDebug(DebugLevel.I, "Color in (R;G;B: {0};{1};{2})", CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn);
                    var color = new Color(CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn);
                    builder.Append(
                        color.VTSequenceBackground +
                        FillIn()
                    );
                    _currentStep++;
                    if (_currentStep > _edgepulseMaxSteps)
                    {
                        _currentStep = 0;
                        _isFadingOut = true;
                    }
                }
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash done.");
            }
            return builder.ToString();
        }

        private static string FillIn()
        {
            var filled = new StringBuilder();

            int FloorTopLeftEdge = 0;
            int FloorBottomLeftEdge = 0;
            DebugWriter.WriteDebug(DebugLevel.I, "Top left edge: {0}, Bottom left edge: {1}", FloorTopLeftEdge, FloorBottomLeftEdge);

            int FloorTopRightEdge = ConsoleWrapper.WindowWidth - 1;
            int FloorBottomRightEdge = ConsoleWrapper.WindowWidth - 1;
            DebugWriter.WriteDebug(DebugLevel.I, "Top right edge: {0}, Bottom right edge: {1}", FloorTopRightEdge, FloorBottomRightEdge);

            int FloorTopEdge = 0;
            int FloorBottomEdge = ConsoleWrapper.WindowHeight - 1;
            DebugWriter.WriteDebug(DebugLevel.I, "Top edge: {0}, Bottom edge: {1}", FloorTopEdge, FloorBottomEdge);

            int FloorLeftEdge = 0;
            int FloorRightEdge = ConsoleWrapper.WindowWidth - 2;
            DebugWriter.WriteDebug(DebugLevel.I, "Left edge: {0}, Right edge: {1}", FloorLeftEdge, FloorRightEdge);

            // First, draw the floor top edge
            for (int x = FloorTopLeftEdge; x <= FloorTopRightEdge; x++)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Drawing floor top edge ({0}, {1})", x, 0);
                filled.Append(
                    CsiSequences.GenerateCsiCursorPosition(x + 1, 1) +
                    " "
                );
            }

            // Second, draw the floor bottom edge
            for (int x = FloorBottomLeftEdge; x <= FloorBottomRightEdge; x++)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Drawing floor bottom edge ({0}, {1})", x, FloorBottomEdge);
                filled.Append(
                    CsiSequences.GenerateCsiCursorPosition(x + 1, FloorBottomEdge + 1) +
                    " "
                );
            }

            // Third, draw the floor left edge
            for (int y = FloorTopEdge; y <= FloorBottomEdge; y++)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Drawing floor left edge ({0}, {1})", FloorLeftEdge, y);
                filled.Append(
                    CsiSequences.GenerateCsiCursorPosition(FloorLeftEdge + 1, y + 1) +
                    " "
                );
            }

            // Finally, draw the floor right edge
            for (int y = FloorTopEdge; y <= FloorBottomEdge; y++)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Drawing floor right edge ({0}, {1})", FloorRightEdge, y);
                filled.Append(
                    CsiSequences.GenerateCsiCursorPosition(FloorRightEdge + 2, y + 1) +
                    " "
                );
            }
            return filled.ToString();
        }

        public override string Closing(SplashContext context, out bool delayRequired)
        {
            _inited = false;
            return base.Closing(context, out delayRequired);
        }

    }
}
