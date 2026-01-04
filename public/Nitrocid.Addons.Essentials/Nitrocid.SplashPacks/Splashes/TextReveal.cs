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
using System.Text;
using System.Threading;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Threading;
using Nitrocid.Base.Misc.Splash;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.SplashPacks.Splashes
{
    class SplashTextReveal : BaseSplash, ISplash
    {

        private int _currentStep = 0;
        private int _top = 0;
        private int _left = 0;
        private Color _currentColor = Color.Empty;
        private bool _inited = false;
        private readonly int _textRevealDelay = 50;
        private readonly string _textRevealWrite = "Nitrocid KS";
        private readonly int _textRevealMaxSteps = 25;
        private readonly int _textRevealMinimumRedColorLevel = 0;
        private readonly int _textRevealMinimumGreenColorLevel = 0;
        private readonly int _textRevealMinimumBlueColorLevel = 0;
        private readonly int _textRevealMaximumRedColorLevel = 255;
        private readonly int _textRevealMaximumGreenColorLevel = 255;
        private readonly int _textRevealMaximumBlueColorLevel = 255;

        // Standalone splash information
        public override string SplashName => "TextReveal";

        public override bool RequiresBackground => true;

        // Actual logic
        public override string Opening(SplashContext context)
        {
            if (!_inited)
            {
                int RedColorNum = RandomDriver.Random(_textRevealMinimumRedColorLevel, _textRevealMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(_textRevealMinimumGreenColorLevel, _textRevealMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(_textRevealMinimumBlueColorLevel, _textRevealMaximumBlueColorLevel);
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
                builder.Append(
                    ColorTools.RenderSetConsoleColor(new Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}"), true) +
                    ConsoleClearing.GetClearWholeScreenSequence()
                );

                // Check the text
                DebugWriter.WriteDebug(DebugLevel.I, "Selected left and top: {0}, {1}", vars: [_left, _top]);
                int textWidth = ConsoleChar.EstimateCellWidth(_textRevealWrite);
                if (textWidth + _left >= ConsoleWrapper.WindowWidth)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Text length of {0} exceeded window width of {1}.", vars: [textWidth + _left, ConsoleWrapper.WindowWidth]);
                    _left -= textWidth + 1;
                }

                // Set thresholds
                double ThresholdRed = RedColorNum / (double)_textRevealMaxSteps;
                double ThresholdGreen = GreenColorNum / (double)_textRevealMaxSteps;
                double ThresholdBlue = BlueColorNum / (double)_textRevealMaxSteps;
                DebugWriter.WriteDebug(DebugLevel.I, "Color threshold (R;G;B: {0})", vars: [ThresholdRed, ThresholdGreen, ThresholdBlue]);

                // Fade out
                int CurrentColorRedOut = RedColorNum;
                int CurrentColorGreenOut = GreenColorNum;
                int CurrentColorBlueOut = BlueColorNum;
                DebugWriter.WriteDebug(DebugLevel.I, "Step {0}/{1}", vars: [_currentStep, _textRevealMaxSteps]);
                ThreadManager.SleepNoBlock(_textRevealDelay);
                CurrentColorRedOut = (int)Math.Round(CurrentColorRedOut - ThresholdRed * _currentStep);
                CurrentColorGreenOut = (int)Math.Round(CurrentColorGreenOut - ThresholdGreen * _currentStep);
                CurrentColorBlueOut = (int)Math.Round(CurrentColorBlueOut - ThresholdBlue * _currentStep);
                DebugWriter.WriteDebug(DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", vars: [CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut]);
                var color = new Color($"{CurrentColorRedOut};{CurrentColorGreenOut};{CurrentColorBlueOut}");
                builder.Append(
                    color.VTSequenceForeground +
                    TextWriterWhereColor.RenderWhere(_textRevealWrite, _left, _top, true)
                );
                _currentStep++;
                if (_currentStep > _textRevealMaxSteps)
                    _currentStep = 0;
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
