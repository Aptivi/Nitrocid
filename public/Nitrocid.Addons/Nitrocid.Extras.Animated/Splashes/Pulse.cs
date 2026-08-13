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
using Nitrocid.Extras.Animated.Animations.Pulse;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Threadify.Manager;

namespace Nitrocid.Extras.Animated.Splashes
{
    class SplashPulse : BaseSplash, ISplash
    {

        private PulseSettings? PulseSettingsInstance;

        // Standalone splash information
        public override string SplashName => "Pulse";

        public override bool RequiresBackground => true;

        // Actual logic
        public override string Opening(SplashContext context)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", vars: [ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight]);
            PulseSettingsInstance = new PulseSettings()
            {
                PulseDelay = AnimatedInit.SplashConfig.PulseDelay,
                PulseMaxSteps = AnimatedInit.SplashConfig.PulseMaxSteps,
                PulseMinimumRedColorLevel = AnimatedInit.SplashConfig.PulseMinimumRedColorLevel,
                PulseMinimumGreenColorLevel = AnimatedInit.SplashConfig.PulseMinimumGreenColorLevel,
                PulseMinimumBlueColorLevel = AnimatedInit.SplashConfig.PulseMinimumBlueColorLevel,
                PulseMaximumRedColorLevel = AnimatedInit.SplashConfig.PulseMaximumRedColorLevel,
                PulseMaximumGreenColorLevel = AnimatedInit.SplashConfig.PulseMaximumGreenColorLevel,
                PulseMaximumBlueColorLevel = AnimatedInit.SplashConfig.PulseMaximumBlueColorLevel
            };
            return base.Opening(context);
        }

        public override string Display(SplashContext context)
        {
            Pulse.Simulate(PulseSettingsInstance);
            return base.Display(context);
        }

    }
}
