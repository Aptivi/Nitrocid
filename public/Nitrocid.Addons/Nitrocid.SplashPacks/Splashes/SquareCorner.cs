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
using Terminaux.Colors;
using Terminaux.Base;
using Terminaux.Writer.CyclicWriters.Graphical;

namespace Nitrocid.SplashPacks.Splashes
{
    class SplashSquareCorner : BaseSplash, ISplash
    {

        private static SquareCornerDirection cornerDirection;
        private bool _isFadingOut = false;
        private int _currentStep = 0;
        private Color _currentColor = Color.Empty;
        private bool _inited = false;
        private readonly int _squareCornerDelay = 10;
        private readonly int _squareCornerMaxSteps = 25;
        private readonly int _squareCornerMinimumRedColorLevel = 0;
        private readonly int _squareCornerMinimumGreenColorLevel = 0;
        private readonly int _squareCornerMinimumBlueColorLevel = 0;
        private readonly int _squareCornerMaximumRedColorLevel = 255;
        private readonly int _squareCornerMaximumGreenColorLevel = 255;
        private readonly int _squareCornerMaximumBlueColorLevel = 255;

        // Standalone splash information
        public override string SplashName => "SquareCorner";

        // Actual logic
        public override string Opening(SplashContext context)
        {
            if (!_inited)
            {
                int RedColorNum = RandomDriver.Random(_squareCornerMinimumRedColorLevel, _squareCornerMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(_squareCornerMinimumGreenColorLevel, _squareCornerMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(_squareCornerMinimumBlueColorLevel, _squareCornerMaximumBlueColorLevel);
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
                int RedColorNum = _currentColor.RGB.R;
                int GreenColorNum = _currentColor.RGB.G;
                int BlueColorNum = _currentColor.RGB.B;
                ConsoleWrapper.CursorVisible = false;

                // Set thresholds
                double ThresholdRed = RedColorNum / (double)_squareCornerMaxSteps;
                double ThresholdGreen = GreenColorNum / (double)_squareCornerMaxSteps;
                double ThresholdBlue = BlueColorNum / (double)_squareCornerMaxSteps;
                DebugWriter.WriteDebug(DebugLevel.I, "Color threshold (R;G;B: {0})", vars: [ThresholdRed, ThresholdGreen, ThresholdBlue]);

                // Determine direction based on value
                cornerDirection = (SquareCornerDirection)RandomDriver.Random(3);
                int left = 2;
                int top = 0;
                int width = 6;
                int height = 3;
                switch (cornerDirection)
                {
                    case SquareCornerDirection.UpperLeft:
                        left = 2;
                        top = 0;
                        break;
                    case SquareCornerDirection.UpperRight:
                        left = ConsoleWrapper.WindowWidth - width - 2;
                        top = 0;
                        break;
                    case SquareCornerDirection.LowerLeft:
                        left = 2;
                        top = ConsoleWrapper.WindowHeight - height - 2;
                        break;
                    case SquareCornerDirection.LowerRight:
                        left = ConsoleWrapper.WindowWidth - width - 2;
                        top = ConsoleWrapper.WindowHeight - height - 2;
                        break;
                }

                // Fade in or out
                if (_isFadingOut)
                {
                    // Fade out
                    int CurrentColorRedOut = RedColorNum;
                    int CurrentColorGreenOut = GreenColorNum;
                    int CurrentColorBlueOut = BlueColorNum;
                    DebugWriter.WriteDebug(DebugLevel.I, "Step {0}/{1}", vars: [_currentStep, _squareCornerMaxSteps]);
                    ThreadManager.SleepNoBlock(_squareCornerDelay);
                    CurrentColorRedOut = (int)Math.Round(CurrentColorRedOut - ThresholdRed * _currentStep);
                    CurrentColorGreenOut = (int)Math.Round(CurrentColorGreenOut - ThresholdGreen * _currentStep);
                    CurrentColorBlueOut = (int)Math.Round(CurrentColorBlueOut - ThresholdBlue * _currentStep);
                    DebugWriter.WriteDebug(DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", vars: [CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut]);
                    
                    var box = new Box()
                    {
                        Left = left,
                        Top = top,
                        Width = width,
                        Height = height,
                        Color = new Color(CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut),
                    };
                    builder.Append(box.Render());

                    _currentStep++;
                    if (_currentStep > _squareCornerMaxSteps)
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
                    DebugWriter.WriteDebug(DebugLevel.I, "Step {0}/{1}", vars: [_currentStep, _squareCornerMaxSteps]);
                    ThreadManager.SleepNoBlock(_squareCornerDelay);
                    CurrentColorRedIn = (int)Math.Round((CurrentColorRedIn + ThresholdRed) * _currentStep);
                    CurrentColorGreenIn = (int)Math.Round((CurrentColorGreenIn + ThresholdGreen) * _currentStep);
                    CurrentColorBlueIn = (int)Math.Round((CurrentColorBlueIn + ThresholdBlue) * _currentStep);
                    DebugWriter.WriteDebug(DebugLevel.I, "Color in (R;G;B: {0};{1};{2})", vars: [CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn]);

                    var box = new Box()
                    {
                        Left = left,
                        Top = top,
                        Width = width,
                        Height = height,
                        Color = new Color(CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn),
                    };
                    builder.Append(box.Render());

                    _currentStep++;
                    if (_currentStep > _squareCornerMaxSteps)
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

    /// <summary>
    /// Square corner direction
    /// </summary>
    public enum SquareCornerDirection
    {
        /// <summary>
        /// Upper left corner
        /// </summary>
        UpperLeft,
        /// <summary>
        /// Upper right corner
        /// </summary>
        UpperRight,
        /// <summary>
        /// Lower left corner
        /// </summary>
        LowerRight,
        /// <summary>
        /// Lower right corner
        /// </summary>
        LowerLeft
    }
}
