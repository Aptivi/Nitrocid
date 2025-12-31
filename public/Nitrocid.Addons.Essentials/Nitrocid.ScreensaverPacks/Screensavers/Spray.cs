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
    /// Display code for Spray
    /// </summary>
    public class SprayDisplay : BaseScreensaver, IScreensaver
    {

        private bool moveUp = false;
        private int offsetY = 0;
        private readonly List<Tuple<int, int>> Stars = [];

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Spray";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            Stars.Clear();
            offsetY = 0;
            ColorTools.LoadBackDry(0);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            moveUp = RandomDriver.RandomBoolean();
            ConsoleWrapper.CursorVisible = false;

            // Move the stars left
            for (int Star = 0; Star <= Stars.Count - 1; Star++)
            {
                int newStarX = Stars[Star].Item1 - 1;
                int newStarY = Stars[Star].Item2;
                Stars[Star] = new Tuple<int, int>(newStarX, newStarY);
            }

            // If any star is out of X range, delete it
            for (int StarIndex = Stars.Count - 1; StarIndex >= 0; StarIndex -= 1)
            {
                var Star = Stars[StarIndex];
                if (Star.Item1 < 0)
                {
                    // The star went beyond. Remove it.
                    Stars.RemoveAt(StarIndex);
                }
            }

            // Add new star
            int StarX = ConsoleWrapper.WindowWidth - 1;
            int StarY = (ConsoleWrapper.WindowHeight / 2) - offsetY;
            if (moveUp)
            {
                offsetY++;
                if ((ConsoleWrapper.WindowHeight / 2) - offsetY < 0)
                    offsetY--;
            }
            else
            {
                offsetY--;
                if ((ConsoleWrapper.WindowHeight / 2) - offsetY >= ConsoleWrapper.WindowHeight)
                    offsetY++;
            }
            Stars.Add(new Tuple<int, int>(StarX, StarY));

            // Draw stars
            var starsBuffer = new StringBuilder();
            for (int StarIndex = Stars.Count - 1; StarIndex >= 0; StarIndex -= 1)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                var Star = Stars[StarIndex];
                char StarSymbol = '*';
                starsBuffer.Append(TextWriterWhereColor.RenderWhere(Convert.ToString(StarSymbol), Star.Item1, Star.Item2, false, ConsoleColors.White, ConsoleColors.Black));
            }
            if (ConsoleResizeHandler.WasResized(false))
            {
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing...");
                Stars.Clear();
            }
            else
                TextWriterRaw.WritePlain(starsBuffer.ToString(), false);

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.SprayDelay);
            ColorTools.LoadBackDry(0);
        }

        /// <inheritdoc/>
        public override void ScreensaverResizeSync()
        {
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing...");
            Stars.Clear();
            offsetY = 0;
            base.ScreensaverResizeSync();
        }

    }
}
