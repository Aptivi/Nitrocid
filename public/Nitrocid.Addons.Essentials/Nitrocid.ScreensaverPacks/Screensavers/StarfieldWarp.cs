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
using Terminaux.Base;
using Terminaux.Colors.Data;
using Terminaux.Base.Extensions;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for StarfieldWarp
    /// </summary>
    public class StarfieldWarpDisplay : BaseScreensaver, IScreensaver
    {

        private readonly List<(double, double, int, int, double, double)> Stars = [];

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "StarfieldWarp";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            Stars.Clear();
            ConsoleColoring.LoadBackDry(0);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Move the stars to the direction, depending on the move direction
            for (int Star = 0; Star <= Stars.Count - 1; Star++)
            {
                double StarX = Stars[Star].Item1 - Stars[Star].Item5;
                double StarY = Stars[Star].Item2 - Stars[Star].Item6;
                Stars[Star] = (StarX, StarY, Stars[Star].Item3, Stars[Star].Item4, Stars[Star].Item5, Stars[Star].Item6);
            }

            // If any star is out of any range, delete it
            for (int StarIndex = Stars.Count - 1; StarIndex >= 0; StarIndex -= 1)
            {
                var Star = Stars[StarIndex];
                if (Star.Item1 < 0 || Star.Item1 >= ConsoleWrapper.WindowWidth ||
                    Star.Item2 < 0 || Star.Item2 >= ConsoleWrapper.WindowHeight)
                {
                    // The star went beyond. Remove it.
                    Stars.RemoveAt(StarIndex);
                }
            }

            // Add new star if guaranteed
            bool StarShowGuaranteed = RandomDriver.RandomChance(25);
            int widthHalf = ConsoleWrapper.WindowWidth / 2;
            int heightHalf = ConsoleWrapper.WindowHeight / 2;
            if (StarShowGuaranteed)
            {
                int StarX = widthHalf;
                int StarY = heightHalf;
                int stepsX = StarX;
                int stepsY = StarY;

                // Determine the end position based on the edge mode
                int starEndX = 0;
                int starEndY = 0;
                var edgeMode = (EdgeDirection)RandomDriver.Random(3);
                switch (edgeMode)
                {
                    case EdgeDirection.Left:
                        // (X = 0, Y = random)
                        starEndY = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);

                        // (StarX = widthHalf - 5 .. widthHalf)
                        StarX = RandomDriver.Random(widthHalf - 5, widthHalf);
                        break;
                    case EdgeDirection.Right:
                        // (X = max, Y = random)
                        starEndX = ConsoleWrapper.WindowWidth - 1;
                        starEndY = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);

                        // (StarX = widthHalf .. widthHalf + 5)
                        StarX = RandomDriver.Random(widthHalf, widthHalf + 5);
                        break;
                    case EdgeDirection.Top:
                        // (X = random, Y = 0)
                        starEndX = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);

                        // (StarY = heightHalf - 5 .. heightHalf)
                        StarY = RandomDriver.Random(heightHalf - 5, heightHalf);
                        break;
                    case EdgeDirection.Bottom:
                        // (X = random, Y = max)
                        starEndX = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
                        starEndY = ConsoleWrapper.WindowHeight - 1;

                        // (StarY = heightHalf .. heightHalf + 5)
                        StarY = RandomDriver.Random(heightHalf, heightHalf + 5);
                        break;
                }

                // Determine the thresholds
                int starDiffX = -(starEndX - StarX);
                int starDiffY = -(starEndY - StarY);
                double starThresholdX = (double)starDiffX / stepsX;
                double starThresholdY = (double)starDiffY / stepsY;
                Stars.Add((StarX, StarY, starEndX, starEndY, starThresholdX, starThresholdY));
            }

            // Draw stars
            var starsBuffer = new StringBuilder();
            for (int StarIndex = Stars.Count - 1; StarIndex >= 0; StarIndex -= 1)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                if (ScreensaverManager.Bailing)
                    return;

                var Star = Stars[StarIndex];
                char StarSymbol = '*';
                int StarX = (int)Star.Item1;
                int StarY = (int)Star.Item2;
                starsBuffer.Append(TextWriterWhereColor.RenderWhereColor(Convert.ToString(StarSymbol), StarX, StarY, false, ConsoleColors.White));
            }
            if (ConsoleResizeHandler.WasResized(false))
            {
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing...");
                Stars.Clear();
            }
            else
                TextWriterRaw.WritePlain(starsBuffer.ToString(), false);
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.StarfieldWarpDelay);
            ConsoleColoring.LoadBackDry(0);
        }

        /// <inheritdoc/>
        public override void ScreensaverOutro()
        {
            Stars.Clear();
        }

    }

    /// <summary>
    /// Edge direction to move the star
    /// </summary>
    enum EdgeDirection
    {
        /// <summary>
        /// The star moves to the left.  (X = 0, Y = random)
        /// </summary>
        Left,
        /// <summary>
        /// The star moves to the right. (X = max, Y = random)
        /// </summary>
        Right,
        /// <summary>
        /// The star moves to the left.  (X = random, Y = 0)
        /// </summary>
        Top,
        /// <summary>
        /// The star moves to the left.  (X = random, Y = max)
        /// </summary>
        Bottom
    }
}
