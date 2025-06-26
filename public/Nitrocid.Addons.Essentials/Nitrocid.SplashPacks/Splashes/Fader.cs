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
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Splash;
using Terminaux.Colors;
using Terminaux.Base;

namespace Nitrocid.SplashPacks.Splashes
{
    class SplashFader : BaseSplash, ISplash
    {

        private bool _isFadingOut = false;
        private int _currentStep = 0;
        private int _top = 0;
        private int _left = 0;
        private Color _currentColor = Color.Empty;
        private bool _inited = false;
        private readonly int _faderDelay = 50;
        private readonly string _faderWrite = "Nitrocid KS";
        private readonly int _faderMaxSteps = 25;
        private readonly int _faderMinimumRedColorLevel = 0;
        private readonly int _faderMinimumGreenColorLevel = 0;
        private readonly int _faderMinimumBlueColorLevel = 0;
        private readonly int _faderMaximumRedColorLevel = 255;
        private readonly int _faderMaximumGreenColorLevel = 255;
        private readonly int _faderMaximumBlueColorLevel = 255;

        // Standalone splash information
        public override string SplashName => "Fader";

        // Actual logic
        public override string Opening(SplashContext context)
        {
            if (!_inited)
            {
                int RedColorNum = RandomDriver.Random(_faderMinimumRedColorLevel, _faderMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(_faderMinimumGreenColorLevel, _faderMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(_faderMinimumBlueColorLevel, _faderMaximumBlueColorLevel);
                _left = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
                _top = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
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
                int RedColorNum = _currentColor.RGB.R;
                int GreenColorNum = _currentColor.RGB.G;
                int BlueColorNum = _currentColor.RGB.B;

                // Check the text
                DebugWriter.WriteDebug(DebugLevel.I, "Selected left and top: {0}, {1}", _left, _top);
                if (_faderWrite.Length + _left >= ConsoleWrapper.WindowWidth)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Text length of {0} exceeded window width of {1}.", _faderWrite.Length + _left, ConsoleWrapper.WindowWidth);
                    _left -= _faderWrite.Length + 1;
                }

                // Set thresholds
                double ThresholdRed = RedColorNum / (double)_faderMaxSteps;
                double ThresholdGreen = GreenColorNum / (double)_faderMaxSteps;
                double ThresholdBlue = BlueColorNum / (double)_faderMaxSteps;
                DebugWriter.WriteDebug(DebugLevel.I, "Color threshold (R;G;B: {0})", ThresholdRed, ThresholdGreen, ThresholdBlue);

                // Fade in or out
                if (_isFadingOut)
                {
                    // Fade out
                    int CurrentColorRedOut = RedColorNum;
                    int CurrentColorGreenOut = GreenColorNum;
                    int CurrentColorBlueOut = BlueColorNum;
                    DebugWriter.WriteDebug(DebugLevel.I, "Step {0}/{1}", _currentStep, _faderMaxSteps);
                    ThreadManager.SleepNoBlock(_faderDelay);
                    CurrentColorRedOut = (int)Math.Round(CurrentColorRedOut - ThresholdRed * _currentStep);
                    CurrentColorGreenOut = (int)Math.Round(CurrentColorGreenOut - ThresholdGreen * _currentStep);
                    CurrentColorBlueOut = (int)Math.Round(CurrentColorBlueOut - ThresholdBlue * _currentStep);
                    DebugWriter.WriteDebug(DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut);
                    var color = new Color($"{CurrentColorRedOut};{CurrentColorGreenOut};{CurrentColorBlueOut}");
                    builder.Append(
                        color.VTSequenceForeground +
                        TextWriterWhereColor.RenderWhere(_faderWrite, _left, _top, true)
                    );
                    _currentStep++;
                    if (_currentStep > _faderMaxSteps)
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
                    DebugWriter.WriteDebug(DebugLevel.I, "Step {0}/{1}", _currentStep, _faderMaxSteps);
                    ThreadManager.SleepNoBlock(_faderDelay);
                    CurrentColorRedIn = (int)Math.Round((CurrentColorRedIn + ThresholdRed) * _currentStep);
                    CurrentColorGreenIn = (int)Math.Round((CurrentColorGreenIn + ThresholdGreen) * _currentStep);
                    CurrentColorBlueIn = (int)Math.Round((CurrentColorBlueIn + ThresholdBlue) * _currentStep);
                    DebugWriter.WriteDebug(DebugLevel.I, "Color in (R;G;B: {0};{1};{2})", CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn);
                    var color = new Color($"{CurrentColorRedIn};{CurrentColorGreenIn};{CurrentColorBlueIn}");
                    builder.Append(
                        color.VTSequenceForeground +
                        TextWriterWhereColor.RenderWhere(_faderWrite, _left, _top, true)
                    );
                    _currentStep++;
                    if (_currentStep > _faderMaxSteps)
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
