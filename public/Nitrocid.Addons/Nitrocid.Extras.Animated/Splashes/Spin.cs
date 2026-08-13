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

using System.Text;
using System.Threading;
using Colorimetry;
using Colorimetry.Data;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Splash;
using Nitrocid.Extras.Animated.Animations.Spin;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Threadify.Manager;

namespace Nitrocid.Extras.Animated.Splashes
{
    class SplashSpin : BaseSplash, ISplash
    {

        private SpinSettings? SpinSettingsInstance;

        // Standalone splash information
        public override string SplashName => "Spin";

        public override string Opening(SplashContext context)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", vars: [ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight]);
            SpinSettingsInstance = new SpinSettings()
            {
                SpinDelay = AnimatedInit.SplashConfig.SpinDelay
            };
            return base.Opening(context);
        }

        public override string Display(SplashContext context)
        {
            Spin.Simulate(SpinSettingsInstance);
            return base.Display(context);
        }

    }
}
