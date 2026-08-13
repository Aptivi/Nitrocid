//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
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
using Colorimetry;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Splash;
using Nitrocid.Extras.Animated.Animations.SquareCorner;
using Terminaux.Base;
using Terminaux.Writer.CyclicWriters.Graphical;
using Threadify.Manager;

namespace Nitrocid.Extras.Animated.Splashes
{
    class SplashSquareCorner : BaseSplash, ISplash
    {

        private SquareCornerSettings? SquareCornerSettingsInstance;

        // Standalone splash information
        public override string SplashName => "SquareCorner";

        public override bool RequiresBackground => true;

        // Actual logic
        public override string Opening(SplashContext context)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", vars: [ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight]);
            SquareCornerSettingsInstance = new SquareCornerSettings()
            {
                SquareCornerDelay = AnimatedInit.SplashConfig.SquareCornerDelay,
                SquareCornerFadeOutDelay = AnimatedInit.SplashConfig.SquareCornerFadeOutDelay,
                SquareCornerMaxSteps = AnimatedInit.SplashConfig.SquareCornerMaxSteps,
                SquareCornerMinimumRedColorLevel = AnimatedInit.SplashConfig.SquareCornerMinimumRedColorLevel,
                SquareCornerMinimumGreenColorLevel = AnimatedInit.SplashConfig.SquareCornerMinimumGreenColorLevel,
                SquareCornerMinimumBlueColorLevel = AnimatedInit.SplashConfig.SquareCornerMinimumBlueColorLevel,
                SquareCornerMaximumRedColorLevel = AnimatedInit.SplashConfig.SquareCornerMaximumRedColorLevel,
                SquareCornerMaximumGreenColorLevel = AnimatedInit.SplashConfig.SquareCornerMaximumGreenColorLevel,
                SquareCornerMaximumBlueColorLevel = AnimatedInit.SplashConfig.SquareCornerMaximumBlueColorLevel,
            };
            return base.Opening(context);
        }

        public override string Display(SplashContext context)
        {
            SquareCorner.Simulate(SquareCornerSettingsInstance);
            return base.Display(context);
        }

    }
}
