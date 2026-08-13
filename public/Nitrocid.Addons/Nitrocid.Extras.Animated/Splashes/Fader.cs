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
using Nitrocid.Extras.Animated.Animations.Fader;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Writer.ConsoleWriters;
using Threadify.Manager;

namespace Nitrocid.Extras.Animated.Splashes
{
    class SplashFader : BaseSplash, ISplash
    {

        private FaderSettings? FaderSettingsInstance;

        // Standalone splash information
        public override string SplashName => "Fader";

        // Actual logic
        public override string Opening(SplashContext context)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", vars: [ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight]);
            FaderSettingsInstance = new FaderSettings()
            {
                FaderDelay = AnimatedInit.SplashConfig.FaderDelay,
                FaderWrite = AnimatedInit.SplashConfig.FaderWrite,
                FaderBackgroundColor = AnimatedInit.SplashConfig.FaderBackgroundColor,
                FaderFadeOutDelay = AnimatedInit.SplashConfig.FaderFadeOutDelay,
                FaderMaxSteps = AnimatedInit.SplashConfig.FaderMaxSteps,
                FaderMinimumRedColorLevel = AnimatedInit.SplashConfig.FaderMinimumRedColorLevel,
                FaderMinimumGreenColorLevel = AnimatedInit.SplashConfig.FaderMinimumGreenColorLevel,
                FaderMinimumBlueColorLevel = AnimatedInit.SplashConfig.FaderMinimumBlueColorLevel,
                FaderMaximumRedColorLevel = AnimatedInit.SplashConfig.FaderMaximumRedColorLevel,
                FaderMaximumGreenColorLevel = AnimatedInit.SplashConfig.FaderMaximumGreenColorLevel,
                FaderMaximumBlueColorLevel = AnimatedInit.SplashConfig.FaderMaximumBlueColorLevel
            };
            return base.Opening(context);
        }

        public override string Display(SplashContext context)
        {
            Fader.Simulate(FaderSettingsInstance);
            return base.Display(context);
        }

    }
}
