//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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
using System.Collections.Generic;
using System.Text;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Screensaver;
using Nitrocid.Base.Kernel.Configuration;
using Terminaux.Colors;
using Terminaux.Base;
using Terminaux.Colors.Data;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Evaporate
    /// </summary>
    public class EvaporateDisplay : BaseScreensaver, IScreensaver
    {
        // TODO: NKS_SCREENSAVERPACKS_EVAPORATE_SETTINGS_DESC -> Simulates droplets evaporating
        private readonly List<Tuple<int, int>> droplets = [];
        private readonly List<Tuple<int, int>> evaporations = [];

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Evaporate";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            droplets.Clear();
            evaporations.Clear();
            ColorTools.LoadBackDry(0);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Move the droplets down
            for (int droplet = 0; droplet <= droplets.Count - 1; droplet++)
            {
                int dropletX = droplets[droplet].Item1;
                int dropletY = droplets[droplet].Item2 + 1;
                droplets[droplet] = new Tuple<int, int>(dropletX, dropletY);
            }

            // Move the evaporations up
            for (int evaporation = 0; evaporation <= evaporations.Count - 1; evaporation++)
            {
                int evaporationX = evaporations[evaporation].Item1;
                int evaporationY = evaporations[evaporation].Item2 - 1;
                evaporations[evaporation] = new Tuple<int, int>(evaporationX, evaporationY);
            }

            // If any droplet is out of Y range, delete it
            for (int dropletIndex = droplets.Count - 1; dropletIndex >= 0; dropletIndex -= 1)
            {
                var droplet = droplets[dropletIndex];
                if (droplet.Item2 >= ConsoleWrapper.WindowHeight)
                {
                    // The droplet went beyond. Remove it and evaporate it.
                    droplets.RemoveAt(dropletIndex);
                    evaporations.Add(new(droplet.Item1, droplet.Item2 - 1));
                }
            }

            // If any evaporation is out of Y range, delete it
            for (int evaporationIndex = evaporations.Count - 1; evaporationIndex >= 0; evaporationIndex -= 1)
            {
                var evaporation = evaporations[evaporationIndex];
                if (evaporation.Item2 < 0)
                {
                    // The evaporation went beyond. Remove it.
                    evaporations.RemoveAt(evaporationIndex);
                }
            }

            // Add new droplet if guaranteed
            bool dropletShowGuaranteed = RandomDriver.RandomChance(10);
            if (dropletShowGuaranteed)
            {
                int dropletX = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
                int dropletY = 0;
                droplets.Add(new Tuple<int, int>(dropletX, dropletY));
            }

            // Draw droplets
            var dropletsBuffer = new StringBuilder();
            for (int dropletIndex = droplets.Count - 1; dropletIndex >= 0; dropletIndex -= 1)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                if (ScreensaverManager.Bailing)
                    return;
                var droplet = droplets[dropletIndex];
                char dropletSymbol = '|';
                int dropletX = droplet.Item1;
                int dropletY = droplet.Item2;
                dropletsBuffer.Append(TextWriterWhereColor.RenderWhereColor(Convert.ToString(dropletSymbol), dropletX, dropletY, false, ConsoleColors.Aqua));
            }

            // Draw evaporations
            for (int evaporationIndex = evaporations.Count - 1; evaporationIndex >= 0; evaporationIndex -= 1)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                if (ScreensaverManager.Bailing)
                    return;
                var evaporation = evaporations[evaporationIndex];
                char evaporationSymbol = '*';
                int evaporationX = evaporation.Item1;
                int evaporationY = evaporation.Item2;
                dropletsBuffer.Append(TextWriterWhereColor.RenderWhereColor(Convert.ToString(evaporationSymbol), evaporationX, evaporationY, false, ConsoleColors.Grey78));
            }

            // Handle resize
            if (ConsoleResizeHandler.WasResized(false))
            {
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing...");
                droplets.Clear();
                evaporations.Clear();
            }
            else
                TextWriterRaw.WriteRaw(dropletsBuffer.ToString());

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.EvaporateDelay);
            ColorTools.LoadBackDry(0);
        }

        /// <inheritdoc/>
        public override void ScreensaverOutro()
        {
            droplets.Clear();
        }

    }
}
