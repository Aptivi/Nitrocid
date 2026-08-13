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
using Colorimetry.Data;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Splash;
using Nitrocid.Extras.Animated.Animations.BeatPulse;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Threadify.Manager;

namespace Nitrocid.Extras.Animated.Splashes
{
    class SplashBeatPulse : BaseSplash, ISplash
    {

        private BeatPulseSettings? BeatPulseSettingsInstance;

        // Standalone splash information
        public override string SplashName => "BeatPulse";

        public override bool RequiresBackground => true;

        // Actual logic
        public override string Opening(SplashContext context)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", vars: [ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight]);
            BeatPulseSettingsInstance = new BeatPulseSettings()
            {
                BeatPulseTrueColor = AnimatedInit.SplashConfig.BeatPulseTrueColor,
                BeatPulseBeatColor = AnimatedInit.SplashConfig.BeatPulseBeatColor,
                BeatPulseDelay = AnimatedInit.SplashConfig.BeatPulseDelay,
                BeatPulseMaxSteps = AnimatedInit.SplashConfig.BeatPulseMaxSteps,
                BeatPulseCycleColors = AnimatedInit.SplashConfig.BeatPulseCycleColors,
                BeatPulseMinimumRedColorLevel = AnimatedInit.SplashConfig.BeatPulseMinimumRedColorLevel,
                BeatPulseMinimumGreenColorLevel = AnimatedInit.SplashConfig.BeatPulseMinimumGreenColorLevel,
                BeatPulseMinimumBlueColorLevel = AnimatedInit.SplashConfig.BeatPulseMinimumBlueColorLevel,
                BeatPulseMinimumColorLevel = AnimatedInit.SplashConfig.BeatPulseMinimumColorLevel,
                BeatPulseMaximumRedColorLevel = AnimatedInit.SplashConfig.BeatPulseMaximumRedColorLevel,
                BeatPulseMaximumGreenColorLevel = AnimatedInit.SplashConfig.BeatPulseMaximumGreenColorLevel,
                BeatPulseMaximumBlueColorLevel = AnimatedInit.SplashConfig.BeatPulseMaximumBlueColorLevel,
                BeatPulseMaximumColorLevel = AnimatedInit.SplashConfig.BeatPulseMaximumColorLevel
            };
            return base.Opening(context);
        }

        public override string Display(SplashContext context)
        {
            BeatPulse.Simulate(BeatPulseSettingsInstance);
            return base.Display(context);
        }

    }
}
