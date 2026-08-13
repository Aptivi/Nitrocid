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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Colorimetry;
using Colorimetry.Data;
using Colorimetry.Transformation;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for BlazeFury
    /// </summary>
    public class BlazeFuryDisplay : BaseScreensaver, IScreensaver
    {

        private bool ColorFilled;
        private readonly List<Tuple<int, int, int>> CoveredPositions = [];

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            base.ScreensaverPreparation();
            ConsoleColoring.LoadBackDry(ConsoleColors.Black);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            if (ColorFilled)
                Thread.Sleep(1);
            int EndLeft = ConsoleWrapper.WindowWidth - 1;
            int EndTop = ConsoleWrapper.WindowHeight - 1;
            int Left = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
            int Top = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Dissolving: {0}", vars: [ColorFilled]);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "End left: {0} | End top: {1}", vars: [EndLeft, EndTop]);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got left: {0} | Got top: {1}", vars: [Left, Top]);

            // Fill the color if not filled
            if (!CoveredPositions.Any(t => t.Item1 == Left & t.Item2 == Top))
            {
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Covered position {0}", vars: [Left + " - " + Top]);
                CoveredPositions.Add(new Tuple<int, int, int>(Left, Top, 0));
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Covered positions: {0}/{1}", vars: [CoveredPositions.Count, (EndLeft + 1) * (EndTop + 1)]);
            }
            if (!ConsoleResizeHandler.WasResized(false))
            {
                for (int i = 0; i < CoveredPositions.Count; i++)
                {
                    Tuple<int, int, int> coveredPosition = CoveredPositions[i];
                    int level = coveredPosition.Item3;
                    if (level > 100)
                        continue;
                    Color color = TransformationTools.BlendColor(ConsoleColors.Black, ConsoleColors.Orange1, level / 100d);
                    ConsoleWrapper.SetCursorPosition(coveredPosition.Item1, coveredPosition.Item2);
                    ConsoleColoring.SetConsoleColorDry(color, true);
                    ConsoleWrapper.Write(" ");
                    level++;
                    CoveredPositions[i] = new(coveredPosition.Item1, coveredPosition.Item2, level);
                }

                // Check to see if we're full
                if (CoveredPositions.Count == (EndLeft + 1) * (EndTop + 1))
                {
                    if (CoveredPositions.All((element) => element.Item3 > 100))
                    {
                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "We're refilling...");
                        ColorFilled = false;
                        ConsoleColoring.LoadBackDry(ConsoleColors.Black);
                        CoveredPositions.Clear();
                    }
                }
            }
            else
            {
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "We're refilling...");
                ColorFilled = false;
                ConsoleColoring.LoadBackDry(ConsoleColors.Black);
                CoveredPositions.Clear();
            }
        }

        /// <inheritdoc/>
        public override void ScreensaverOutro()
        {
            ColorFilled = false;
            CoveredPositions.Clear();
        }

    }
}
