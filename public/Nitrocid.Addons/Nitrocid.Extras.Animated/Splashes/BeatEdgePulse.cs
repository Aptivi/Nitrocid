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
using Nitrocid.Base.Misc.Screensaver;
using Nitrocid.Base.Misc.Splash;
using Nitrocid.Extras.Animated.Animations.BeatEdgePulse;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Sequences.Builder.Types;
using Threadify.Manager;

namespace Nitrocid.Extras.Animated.Splashes
{
    class SplashBeatEdgePulse : BaseSplash, ISplash
    {

        private BeatEdgePulseSettings? BeatEdgePulseSettingsInstance;

        // Standalone splash information
        public override string SplashName => "BeatEdgePulse";

        public override bool RequiresBackground => true;

        public override string Opening(SplashContext context)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", vars: [ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight]);
            BeatEdgePulseSettingsInstance = new BeatEdgePulseSettings()
            {
                BeatEdgePulseTrueColor = AnimatedInit.SplashConfig.BeatEdgePulseTrueColor,
                BeatEdgePulseBeatColor = AnimatedInit.SplashConfig.BeatEdgePulseBeatColor,
                BeatEdgePulseDelay = AnimatedInit.SplashConfig.BeatEdgePulseDelay,
                BeatEdgePulseMaxSteps = AnimatedInit.SplashConfig.BeatEdgePulseMaxSteps,
                BeatEdgePulseCycleColors = AnimatedInit.SplashConfig.BeatEdgePulseCycleColors,
                BeatEdgePulseMinimumRedColorLevel = AnimatedInit.SplashConfig.BeatEdgePulseMinimumRedColorLevel,
                BeatEdgePulseMinimumGreenColorLevel = AnimatedInit.SplashConfig.BeatEdgePulseMinimumGreenColorLevel,
                BeatEdgePulseMinimumBlueColorLevel = AnimatedInit.SplashConfig.BeatEdgePulseMinimumBlueColorLevel,
                BeatEdgePulseMinimumColorLevel = AnimatedInit.SplashConfig.BeatEdgePulseMinimumColorLevel,
                BeatEdgePulseMaximumRedColorLevel = AnimatedInit.SplashConfig.BeatEdgePulseMaximumRedColorLevel,
                BeatEdgePulseMaximumGreenColorLevel = AnimatedInit.SplashConfig.BeatEdgePulseMaximumGreenColorLevel,
                BeatEdgePulseMaximumBlueColorLevel = AnimatedInit.SplashConfig.BeatEdgePulseMaximumBlueColorLevel,
                BeatEdgePulseMaximumColorLevel = AnimatedInit.SplashConfig.BeatEdgePulseMaximumColorLevel
            };
            return base.Opening(context);
        }

        // Actual logic
        public override string Display(SplashContext context)
        {
            BeatEdgePulse.Simulate(BeatEdgePulseSettingsInstance);
            return base.Display(context);
        }

    }
}
