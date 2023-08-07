
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.Drivers.RNG;
using System;
using System.Data;
using System.Linq;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Display for RandomSaver
    /// </summary>
    [Obsolete($"Screensaver manager, {nameof(Screensaver)}, and its function, {nameof(Screensaver.ShowSavers)}, already gives \"random\" a special treatment.")]
    public class RandomSaverDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "RandomSaver";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            int ScreensaverIndex = RandomDriver.RandomIdx(Misc.Screensaver.Screensaver.Screensavers.Count);
            string ScreensaverName = Misc.Screensaver.Screensaver.Screensavers.Keys.ElementAtOrDefault(ScreensaverIndex);
            var Screensaver = Misc.Screensaver.Screensaver.Screensavers[ScreensaverName];

            // We don't want another "random" screensaver showing up, so keep selecting until it's no longer "random"
            while (ScreensaverName == "random")
            {
                ScreensaverIndex = RandomDriver.RandomIdx(Misc.Screensaver.Screensaver.Screensavers.Count);
                ScreensaverName = Misc.Screensaver.Screensaver.Screensavers.Keys.ElementAtOrDefault(ScreensaverIndex);
                Screensaver = Misc.Screensaver.Screensaver.Screensavers[ScreensaverName];
            }

            // Run the screensaver
            ScreensaverDisplayer.DisplayScreensaver(Screensaver);
        }

    }
}
