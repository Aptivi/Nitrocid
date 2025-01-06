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
using System.Threading;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Splash;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Colors.Data;

namespace Nitrocid.SplashPacks.Splashes
{
    class SplashBeatFader : BaseSplash, ISplash
    {

        private int _currentStep = 0;
        private Color _currentColor = Color.Empty;
        private bool _inited = false;
        private readonly bool _beatFaderTrueColor = true;
        private readonly int _beatFaderDelay = 120;
        private readonly int _beatFaderMaxSteps = 30;
        private readonly bool _beatFaderCycleColors = true;
        private readonly string _beatFaderBeatColor = "17";
        private readonly int _beatFaderMinimumRedColorLevel = 0;
        private readonly int _beatFaderMinimumGreenColorLevel = 0;
        private readonly int _beatFaderMinimumBlueColorLevel = 0;
        private readonly int _beatFaderMinimumColorLevel = 0;
        private readonly int _beatFaderMaximumRedColorLevel = 255;
        private readonly int _beatFaderMaximumGreenColorLevel = 255;
        private readonly int _beatFaderMaximumBlueColorLevel = 255;
        private readonly int _beatFaderMaximumColorLevel = 255;

        // Standalone splash information
        public override string SplashName => "BeatFader";

        // Actual logic
        public override string Opening(SplashContext context)
        {
            if (!_inited)
            {
                int RedColorNum;
                int GreenColorNum;
                int BlueColorNum;
                if (_beatFaderCycleColors)
                {
                    // We're cycling. Select the color mode, starting from true color
                    DebugWriter.WriteDebug(DebugLevel.I, "Cycling colors...");
                    if (_beatFaderTrueColor)
                    {
                        RedColorNum = RandomDriver.Random(_beatFaderMinimumRedColorLevel, _beatFaderMaximumRedColorLevel);
                        GreenColorNum = RandomDriver.Random(_beatFaderMinimumGreenColorLevel, _beatFaderMaximumGreenColorLevel);
                        BlueColorNum = RandomDriver.Random(_beatFaderMinimumBlueColorLevel, _beatFaderMaximumBlueColorLevel);
                    }
                    else
                    {
                        var ConsoleColor = new Color((ConsoleColors)RandomDriver.Random(_beatFaderMinimumColorLevel, _beatFaderMaximumColorLevel));
                        RedColorNum = ConsoleColor.RGB.R;
                        GreenColorNum = ConsoleColor.RGB.G;
                        BlueColorNum = ConsoleColor.RGB.B;
                    }
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                }
                else
                {
                    // We're not cycling. Parse the color and then select the color mode, starting from true color
                    DebugWriter.WriteDebug(DebugLevel.I, "Parsing colors... {0}", vars: [_beatFaderBeatColor]);
                    var UserColor = new Color(_beatFaderBeatColor);
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
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash displaying.");
                ConsoleWrapper.CursorVisible = false;
                int BeatInterval = (int)Math.Round(60000d / _beatFaderDelay);
                int BeatIntervalStep = (int)Math.Round(BeatInterval / (double)_beatFaderMaxSteps);
                DebugWriter.WriteDebug(DebugLevel.I, "Beat interval from {0} BPM: {1}", vars: [_beatFaderDelay, BeatInterval]);
                DebugWriter.WriteDebug(DebugLevel.I, "Beat steps: {0} ms", vars: [_beatFaderDelay, BeatIntervalStep]);
                ThreadManager.SleepNoBlock(BeatIntervalStep);

                // If we're cycling colors, set them. Else, use the user-provided color
                int RedColorNum = _currentColor.RGB.R;
                int GreenColorNum = _currentColor.RGB.G;
                int BlueColorNum = _currentColor.RGB.B;

                // Set thresholds
                double ThresholdRed = RedColorNum / (double)_beatFaderMaxSteps;
                double ThresholdGreen = GreenColorNum / (double)_beatFaderMaxSteps;
                double ThresholdBlue = BlueColorNum / (double)_beatFaderMaxSteps;
                DebugWriter.WriteDebug(DebugLevel.I, "Color threshold (R;G;B: {0};{1};{2})", vars: [ThresholdRed, ThresholdGreen, ThresholdBlue]);

                // Fade out
                DebugWriter.WriteDebug(DebugLevel.I, "Step {0}/{1} each {2} ms", vars: [_currentStep, _beatFaderMaxSteps, BeatIntervalStep]);
                ThreadManager.SleepNoBlock(BeatIntervalStep);
                int CurrentColorRedOut = (int)Math.Round(RedColorNum - ThresholdRed * _currentStep);
                int CurrentColorGreenOut = (int)Math.Round(GreenColorNum - ThresholdGreen * _currentStep);
                int CurrentColorBlueOut = (int)Math.Round(BlueColorNum - ThresholdBlue * _currentStep);
                DebugWriter.WriteDebug(DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                ColorTools.LoadBackDry(new Color($"{CurrentColorRedOut};{CurrentColorGreenOut};{CurrentColorBlueOut}"));
                _currentStep++;
                if (_currentStep > _beatFaderMaxSteps)
                    _currentStep = 0;
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash done.");
            }
            return "";
        }

        public override string Closing(SplashContext context, out bool delayRequired)
        {
            _inited = false;
            return base.Closing(context, out delayRequired);
        }

    }
}
