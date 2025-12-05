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
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Colors.Data;

namespace Nitrocid.SplashPacks.Splashes
{
    class SplashBeatPulse : BaseSplash, ISplash
    {

        private bool _isFadingOut = false;
        private int _currentStep = 0;
        private Color _currentColor = Color.Empty;
        private bool _inited = false;
        private readonly bool _beatpulseTrueColor = true;
        private readonly int _beatpulseDelay = 120;
        private readonly int _beatpulseMaxSteps = 30;
        private readonly bool _beatpulseCycleColors = true;
        private readonly string _beatpulseBeatColor = "17";
        private readonly int _beatpulseMinimumRedColorLevel = 0;
        private readonly int _beatpulseMinimumGreenColorLevel = 0;
        private readonly int _beatpulseMinimumBlueColorLevel = 0;
        private readonly int _beatpulseMinimumColorLevel = 0;
        private readonly int _beatpulseMaximumRedColorLevel = 255;
        private readonly int _beatpulseMaximumGreenColorLevel = 255;
        private readonly int _beatpulseMaximumBlueColorLevel = 255;
        private readonly int _beatpulseMaximumColorLevel = 255;

        // Standalone splash information
        public override string SplashName => "BeatPulse";

        public override bool RequiresBackground => true;

        // Actual logic
        public override string Opening(SplashContext context)
        {
            if (!_inited)
            {
                int RedColorNum, GreenColorNum, BlueColorNum;
                if (_beatpulseCycleColors)
                {
                    // We're cycling. Select the color mode, starting from true color
                    DebugWriter.WriteDebug(DebugLevel.I, "Cycling colors...");
                    if (_beatpulseTrueColor)
                    {
                        RedColorNum = RandomDriver.Random(_beatpulseMinimumRedColorLevel, _beatpulseMaximumRedColorLevel);
                        GreenColorNum = RandomDriver.Random(_beatpulseMinimumGreenColorLevel, _beatpulseMaximumGreenColorLevel);
                        BlueColorNum = RandomDriver.Random(_beatpulseMinimumBlueColorLevel, _beatpulseMaximumBlueColorLevel);
                    }
                    else
                    {
                        var ConsoleColor = new Color((ConsoleColors)RandomDriver.Random(_beatpulseMinimumColorLevel, _beatpulseMaximumColorLevel));
                        RedColorNum = ConsoleColor.RGB.R;
                        GreenColorNum = ConsoleColor.RGB.G;
                        BlueColorNum = ConsoleColor.RGB.B;
                    }
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                }
                else
                {
                    // We're not cycling. Parse the color and then select the color mode, starting from true color
                    DebugWriter.WriteDebug(DebugLevel.I, "Parsing colors... {0}", vars: [_beatpulseBeatColor]);
                    var UserColor = new Color(_beatpulseBeatColor);
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

                _currentColor = new Color(RedColorNum, GreenColorNum, BlueColorNum);
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
                int BeatInterval = (int)Math.Round(60000d / _beatpulseDelay);
                int BeatIntervalStep = (int)Math.Round(BeatInterval / (double)_beatpulseMaxSteps);
                DebugWriter.WriteDebug(DebugLevel.I, "Beat interval from {0} BPM: {1}", vars: [_beatpulseDelay, BeatInterval]);
                DebugWriter.WriteDebug(DebugLevel.I, "Beat steps: {0} ms", vars: [_beatpulseDelay, BeatIntervalStep]);
                ThreadManager.SleepNoBlock(BeatIntervalStep);

                // If we're cycling colors, set them. Else, use the user-provided color
                int RedColorNum = _currentColor.RGB.R;
                int GreenColorNum = _currentColor.RGB.G;
                int BlueColorNum = _currentColor.RGB.B;

                // Set thresholds
                double ThresholdRed = RedColorNum / (double)_beatpulseMaxSteps;
                double ThresholdGreen = GreenColorNum / (double)_beatpulseMaxSteps;
                double ThresholdBlue = BlueColorNum / (double)_beatpulseMaxSteps;
                DebugWriter.WriteDebug(DebugLevel.I, "Color threshold (R;G;B: {0};{1};{2})", vars: [ThresholdRed, ThresholdGreen, ThresholdBlue]);

                // Fade in or out
                if (_isFadingOut)
                {
                    // Fade out
                    int CurrentColorRedOut = RedColorNum;
                    int CurrentColorGreenOut = GreenColorNum;
                    int CurrentColorBlueOut = BlueColorNum;
                    DebugWriter.WriteDebug(DebugLevel.I, "Step {0}/{1} each {2} ms", vars: [_currentStep, _beatpulseMaxSteps, BeatIntervalStep]);
                    ThreadManager.SleepNoBlock(BeatIntervalStep);
                    CurrentColorRedOut = (int)Math.Round(CurrentColorRedOut - ThresholdRed * _currentStep);
                    CurrentColorGreenOut = (int)Math.Round(CurrentColorGreenOut - ThresholdGreen * _currentStep);
                    CurrentColorBlueOut = (int)Math.Round(CurrentColorBlueOut - ThresholdBlue * _currentStep);
                    DebugWriter.WriteDebug(DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                    builder.Append(
                        ColorTools.RenderSetConsoleColor(new Color(CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut), true) +
                        ConsoleClearing.GetClearWholeScreenSequence()
                    );
                    _currentStep++;
                    if (_currentStep > _beatpulseMaxSteps)
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
                    builder.Append(
                        ColorTools.RenderSetConsoleColor(new Color(CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn), true) +
                        ConsoleClearing.GetClearWholeScreenSequence()
                    );
                    _currentStep++;
                    if (_currentStep > _beatpulseMaxSteps)
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

    }
}
