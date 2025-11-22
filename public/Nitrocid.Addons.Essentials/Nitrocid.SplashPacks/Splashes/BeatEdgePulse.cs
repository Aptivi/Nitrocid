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
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Threading;
using Nitrocid.Base.Misc.Splash;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Sequences.Builder.Types;

namespace Nitrocid.SplashPacks.Splashes
{
    class SplashBeatEdgePulse : BaseSplash, ISplash
    {

        private bool _isFadingOut = false;
        private int _currentStep = 0;
        private Color _currentColor = Color.Empty;
        private bool _inited = false;
        private readonly bool _beatedgepulseTrueColor = true;
        private readonly int _beatedgepulseDelay = 120;
        private readonly int _beatedgepulseMaxSteps = 30;
        private readonly bool _beatedgepulseCycleColors = true;
        private readonly string _beatedgepulseBeatColor = "17";
        private readonly int _beatedgepulseMinimumRedColorLevel = 0;
        private readonly int _beatedgepulseMinimumGreenColorLevel = 0;
        private readonly int _beatedgepulseMinimumBlueColorLevel = 0;
        private readonly int _beatedgepulseMinimumColorLevel = 0;
        private readonly int _beatedgepulseMaximumRedColorLevel = 255;
        private readonly int _beatedgepulseMaximumGreenColorLevel = 255;
        private readonly int _beatedgepulseMaximumBlueColorLevel = 255;
        private readonly int _beatedgepulseMaximumColorLevel = 255;

        // Standalone splash information
        public override string SplashName => "BeatEdgePulse";

        public override bool RequiresBackground => true;

        public override string Opening(SplashContext context)
        {
            if (!_inited)
            {
                // If we're cycling colors, set them. Else, use the user-provided color
                int RedColorNum, GreenColorNum, BlueColorNum;
                if (_beatedgepulseCycleColors)
                {
                    // We're cycling. Select the color mode, starting from true color
                    DebugWriter.WriteDebug(DebugLevel.I, "Cycling colors...");
                    if (_beatedgepulseTrueColor)
                    {
                        RedColorNum = RandomDriver.Random(_beatedgepulseMinimumRedColorLevel, _beatedgepulseMaximumRedColorLevel);
                        GreenColorNum = RandomDriver.Random(_beatedgepulseMinimumGreenColorLevel, _beatedgepulseMaximumGreenColorLevel);
                        BlueColorNum = RandomDriver.Random(_beatedgepulseMinimumBlueColorLevel, _beatedgepulseMaximumBlueColorLevel);
                    }
                    else
                    {
                        var ConsoleColor = new Color((ConsoleColors)RandomDriver.Random(_beatedgepulseMinimumColorLevel, _beatedgepulseMaximumColorLevel));
                        RedColorNum = ConsoleColor.RGB.R;
                        GreenColorNum = ConsoleColor.RGB.G;
                        BlueColorNum = ConsoleColor.RGB.B;
                    }
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                }
                else
                {
                    // We're not cycling. Parse the color and then select the color mode, starting from true color
                    DebugWriter.WriteDebug(DebugLevel.I, "Parsing colors... {0}", vars: [_beatedgepulseBeatColor]);
                    var UserColor = new Color(_beatedgepulseBeatColor);
                    if (UserColor.Type == ColorType.TrueColor)
                    {
                        RedColorNum = UserColor.RGB.R;
                        GreenColorNum = UserColor.RGB.G;
                        BlueColorNum = UserColor.RGB.B;
                    }
                    else
                    {
                        var ConsoleColor = new Color((ConsoleColors)Convert.ToInt32(UserColor.PlainSequence));
                        RedColorNum = ConsoleColor.RGB.R;
                        GreenColorNum = ConsoleColor.RGB.G;
                        BlueColorNum = ConsoleColor.RGB.B;
                    }
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                }

                _currentColor = new(RedColorNum, GreenColorNum, BlueColorNum);
                _inited = true;
            }
            return base.Opening(context);
        }

        // Actual logic
        public override string Display(SplashContext context)
        {
            var builder = new StringBuilder();
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash displaying.");
                ConsoleWrapper.CursorVisible = false;
                int BeatInterval = (int)Math.Round(60000d / _beatedgepulseDelay);
                int BeatIntervalStep = (int)Math.Round(BeatInterval / (double)_beatedgepulseMaxSteps);
                DebugWriter.WriteDebug(DebugLevel.I, "Beat interval from {0} BPM: {1}", vars: [_beatedgepulseDelay, BeatInterval]);
                DebugWriter.WriteDebug(DebugLevel.I, "Beat steps: {0} ms", vars: [_beatedgepulseDelay, BeatIntervalStep]);
                ThreadManager.SleepNoBlock(BeatIntervalStep);

                // If we're cycling colors, set them. Else, use the user-provided color
                int RedColorNum = _currentColor.RGB.R;
                int GreenColorNum = _currentColor.RGB.G;
                int BlueColorNum = _currentColor.RGB.B;
                builder.Append(_currentColor.VTSequenceBackground);

                // Set thresholds
                double ThresholdRed = RedColorNum / (double)_beatedgepulseMaxSteps;
                double ThresholdGreen = GreenColorNum / (double)_beatedgepulseMaxSteps;
                double ThresholdBlue = BlueColorNum / (double)_beatedgepulseMaxSteps;
                DebugWriter.WriteDebug(DebugLevel.I, "Color threshold (R;G;B: {0};{1};{2})", vars: [ThresholdRed, ThresholdGreen, ThresholdBlue]);

                // Fade in or out
                if (_isFadingOut)
                {
                    // Fade out
                    int CurrentColorRedOut = RedColorNum;
                    int CurrentColorGreenOut = GreenColorNum;
                    int CurrentColorBlueOut = BlueColorNum;
                    DebugWriter.WriteDebug(DebugLevel.I, "Step {0}/{1} each {2} ms", vars: [_currentStep, _beatedgepulseMaxSteps, BeatIntervalStep]);
                    ThreadManager.SleepNoBlock(BeatIntervalStep);
                    CurrentColorRedOut = (int)Math.Round(CurrentColorRedOut - ThresholdRed * _currentStep);
                    CurrentColorGreenOut = (int)Math.Round(CurrentColorGreenOut - ThresholdGreen * _currentStep);
                    CurrentColorBlueOut = (int)Math.Round(CurrentColorBlueOut - ThresholdBlue * _currentStep);
                    DebugWriter.WriteDebug(DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                    var color = new Color($"{CurrentColorRedOut};{CurrentColorGreenOut};{CurrentColorBlueOut}");
                    builder.Append(
                        color.VTSequenceBackground +
                        FillIn()
                    );
                    _currentStep++;
                    if (_currentStep > _beatedgepulseMaxSteps)
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
                    DebugWriter.WriteDebug(DebugLevel.I, "Step {0}/{1}", vars: [_currentStep, BeatIntervalStep]);
                    ThreadManager.SleepNoBlock(BeatIntervalStep);
                    CurrentColorRedIn = (int)Math.Round((CurrentColorRedIn + ThresholdRed) * _currentStep);
                    CurrentColorGreenIn = (int)Math.Round((CurrentColorGreenIn + ThresholdGreen) * _currentStep);
                    CurrentColorBlueIn = (int)Math.Round((CurrentColorBlueIn + ThresholdBlue) * _currentStep);
                    DebugWriter.WriteDebug(DebugLevel.I, "Color in (R;G;B: {0};{1};{2})", vars: [CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn]);
                    var color = new Color(CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn);
                    builder.Append(
                        color.VTSequenceBackground +
                        FillIn()
                    );
                    _currentStep++;
                    if (_currentStep > _beatedgepulseMaxSteps)
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

        public override string Closing(SplashContext context, out bool delayRequired)
        {
            _inited = false;
            return base.Closing(context, out delayRequired);
        }

        private static string FillIn()
        {
            var filled = new StringBuilder();

            int FloorTopLeftEdge = 0;
            int FloorBottomLeftEdge = 0;
            DebugWriter.WriteDebug(DebugLevel.I, "Top left edge: {0}, Bottom left edge: {1}", vars: [FloorTopLeftEdge, FloorBottomLeftEdge]);

            int FloorTopRightEdge = ConsoleWrapper.WindowWidth - 1;
            int FloorBottomRightEdge = ConsoleWrapper.WindowWidth - 1;
            DebugWriter.WriteDebug(DebugLevel.I, "Top right edge: {0}, Bottom right edge: {1}", vars: [FloorTopRightEdge, FloorBottomRightEdge]);

            int FloorTopEdge = 0;
            int FloorBottomEdge = ConsoleWrapper.WindowHeight - 1;
            DebugWriter.WriteDebug(DebugLevel.I, "Top edge: {0}, Bottom edge: {1}", vars: [FloorTopEdge, FloorBottomEdge]);

            int FloorLeftEdge = 0;
            int FloorRightEdge = ConsoleWrapper.WindowWidth - 2;
            DebugWriter.WriteDebug(DebugLevel.I, "Left edge: {0}, Right edge: {1}", vars: [FloorLeftEdge, FloorRightEdge]);

            // First, draw the floor top edge
            for (int x = FloorTopLeftEdge; x <= FloorTopRightEdge; x++)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Drawing floor top edge ({0}, {1})", vars: [x, 0]);
                filled.Append(
                    CsiSequences.GenerateCsiCursorPosition(x + 1, 1) +
                    " "
                );
            }

            // Second, draw the floor bottom edge
            for (int x = FloorBottomLeftEdge; x <= FloorBottomRightEdge; x++)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Drawing floor bottom edge ({0}, {1})", vars: [x, FloorBottomEdge]);
                filled.Append(
                    CsiSequences.GenerateCsiCursorPosition(x + 1, FloorBottomEdge + 1) +
                    " "
                );
            }

            // Third, draw the floor left edge
            for (int y = FloorTopEdge; y <= FloorBottomEdge; y++)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Drawing floor left edge ({0}, {1})", vars: [FloorLeftEdge, y]);
                filled.Append(
                    CsiSequences.GenerateCsiCursorPosition(FloorLeftEdge + 1, y + 1) +
                    " "
                );
            }

            // Finally, draw the floor right edge
            for (int y = FloorTopEdge; y <= FloorBottomEdge; y++)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Drawing floor right edge ({0}, {1})", vars: [FloorRightEdge, y]);
                filled.Append(
                    CsiSequences.GenerateCsiCursorPosition(FloorRightEdge + 2, y + 1) +
                    " "
                );
            }
            return filled.ToString();
        }

    }
}
