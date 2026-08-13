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
using Nitrocid.Extras.Animated.Animations.TextReveal;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Writer.ConsoleWriters;
using Threadify.Manager;

namespace Nitrocid.Extras.Animated.Splashes
{
    class SplashTextReveal : BaseSplash, ISplash
    {

        private TextRevealSettings? TextRevealSettingsInstance;

        // Standalone splash information
        public override string SplashName => "TextReveal";

        public override bool RequiresBackground => true;

        // Actual logic
        public override string Opening(SplashContext context)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", vars: [ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight]);
            TextRevealSettingsInstance = new TextRevealSettings()
            {
                TextRevealDelay = AnimatedInit.SplashConfig.TextRevealDelay,
                TextRevealWrite = AnimatedInit.SplashConfig.TextRevealWrite,
                TextRevealFadeOutDelay = AnimatedInit.SplashConfig.TextRevealFadeOutDelay,
                TextRevealNewScreenDelay = AnimatedInit.SplashConfig.TextRevealNewScreenDelay,
                TextRevealMaxSteps = AnimatedInit.SplashConfig.TextRevealMaxSteps,
                TextRevealMinimumRedColorLevel = AnimatedInit.SplashConfig.TextRevealMinimumRedColorLevel,
                TextRevealMinimumGreenColorLevel = AnimatedInit.SplashConfig.TextRevealMinimumGreenColorLevel,
                TextRevealMinimumBlueColorLevel = AnimatedInit.SplashConfig.TextRevealMinimumBlueColorLevel,
                TextRevealMaximumRedColorLevel = AnimatedInit.SplashConfig.TextRevealMaximumRedColorLevel,
                TextRevealMaximumGreenColorLevel = AnimatedInit.SplashConfig.TextRevealMaximumGreenColorLevel,
                TextRevealMaximumBlueColorLevel = AnimatedInit.SplashConfig.TextRevealMaximumBlueColorLevel
            };
            return base.Opening(context);
        }

        public override string Display(SplashContext context)
        {
            TextReveal.Simulate(TextRevealSettingsInstance);
            return base.Display(context);
        }

    }
}
