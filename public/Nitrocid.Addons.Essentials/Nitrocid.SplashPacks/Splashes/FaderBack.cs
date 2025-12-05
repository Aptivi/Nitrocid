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

namespace Nitrocid.SplashPacks.Splashes
{
    class SplashFaderBack : BaseSplash, ISplash
    {

        private bool _isFadingOut = false;
        private int _currentStep = 0;
        private Color _currentColor = Color.Empty;
        private bool _inited = false;
        private readonly int _faderBackDelay = 50;
        private readonly int _faderBackMaxSteps = 25;
        private readonly int _faderBackMinimumRedColorLevel = 0;
        private readonly int _faderBackMinimumGreenColorLevel = 0;
        private readonly int _faderBackMinimumBlueColorLevel = 0;
        private readonly int _faderBackMaximumRedColorLevel = 255;
        private readonly int _faderBackMaximumGreenColorLevel = 255;
        private readonly int _faderBackMaximumBlueColorLevel = 255;

        // Standalone splash information
        public override string SplashName => "FaderBack";

        public override bool RequiresBackground => true;

        // Actual logic
        public override string Opening(SplashContext context)
        {
            if (!_inited)
            {
                int RedColorNum = RandomDriver.Random(_faderBackMinimumRedColorLevel, _faderBackMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(_faderBackMinimumGreenColorLevel, _faderBackMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(_faderBackMinimumBlueColorLevel, _faderBackMaximumBlueColorLevel);
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
                int RedColorNum = _currentColor.RGB.R;
                int GreenColorNum = _currentColor.RGB.G;
                int BlueColorNum = _currentColor.RGB.B;
                ConsoleWrapper.CursorVisible = false;

                // Set thresholds
                double ThresholdRed = RedColorNum / (double)_faderBackMaxSteps;
                double ThresholdGreen = GreenColorNum / (double)_faderBackMaxSteps;
                double ThresholdBlue = BlueColorNum / (double)_faderBackMaxSteps;
                DebugWriter.WriteDebug(DebugLevel.I, "Color threshold (R;G;B: {0})", ThresholdRed, ThresholdGreen, ThresholdBlue);

                // Fade in or out
                if (_isFadingOut)
                {
                    // Fade out
                    int CurrentColorRedOut = RedColorNum;
                    int CurrentColorGreenOut = GreenColorNum;
                    int CurrentColorBlueOut = BlueColorNum;
                    DebugWriter.WriteDebug(DebugLevel.I, "Step {0}/{1}", _currentStep, _faderBackMaxSteps);
                    ThreadManager.SleepNoBlock(_faderBackDelay);
                    CurrentColorRedOut = (int)Math.Round(CurrentColorRedOut - ThresholdRed * _currentStep);
                    CurrentColorGreenOut = (int)Math.Round(CurrentColorGreenOut - ThresholdGreen * _currentStep);
                    CurrentColorBlueOut = (int)Math.Round(CurrentColorBlueOut - ThresholdBlue * _currentStep);
                    DebugWriter.WriteDebug(DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut);
                    ColorTools.LoadBackDry(new Color($"{CurrentColorRedOut};{CurrentColorGreenOut};{CurrentColorBlueOut}"));
                    _currentStep++;
                    if (_currentStep > _faderBackMaxSteps)
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
                    DebugWriter.WriteDebug(DebugLevel.I, "Step {0}/{1}", _currentStep, _faderBackMaxSteps);
                    ThreadManager.SleepNoBlock(_faderBackDelay);
                    CurrentColorRedIn = (int)Math.Round((CurrentColorRedIn + ThresholdRed) * _currentStep);
                    CurrentColorGreenIn = (int)Math.Round((CurrentColorGreenIn + ThresholdGreen) * _currentStep);
                    CurrentColorBlueIn = (int)Math.Round((CurrentColorBlueIn + ThresholdBlue) * _currentStep);
                    DebugWriter.WriteDebug(DebugLevel.I, "Color in (R;G;B: {0};{1};{2})", CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn);
                    ColorTools.LoadBackDry(new Color($"{CurrentColorRedIn};{CurrentColorGreenIn};{CurrentColorBlueIn}"));
                    _currentStep++;
                    if (_currentStep > _faderBackMaxSteps)
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
            return "";
        }

        public override string Closing(SplashContext context, out bool delayRequired)
        {
            _inited = false;
            return base.Closing(context, out delayRequired);
        }

    }
}
