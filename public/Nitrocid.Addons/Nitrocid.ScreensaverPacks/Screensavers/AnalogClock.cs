//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Threading;
using Nitrocid.Kernel.Time.Renderers;
using Nitrocid.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Nitrocid.Kernel.Time;
using System;
using Terminaux.Graphics;
using Terminaux.Writer.CyclicWriters.Shapes;
using Nitrocid.Users.Login.Widgets;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for AnalogClock
    /// </summary>
    public class AnalogClockDisplay : BaseScreensaver, IScreensaver
    {
        /// <inheritdoc/>
        public override string ScreensaverName =>
            "AnalogClock";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            WidgetTools.InitializeWidget("AnalogClock");
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // TODO: Add back configuration
            WidgetTools.RenderWidget("AnalogClock");
            ThreadManager.SleepNoBlock(ScreensaverPackInit.SaversConfig.AnalogClockDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

        /// <inheritdoc/>
        public override void ScreensaverOutro()
        {
            WidgetTools.CleanupWidget("AnalogClock");
            base.ScreensaverOutro();
        }
    }
}
