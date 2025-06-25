//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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
using System.Linq;
using System.Threading;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Base;
using Nitrocid.Base.Kernel.Configuration;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Dissolve
    /// </summary>
    public class DissolveDisplay : BaseScreensaver, IScreensaver
    {

        private bool ColorFilled;
        private readonly List<Tuple<int, int>> CoveredPositions = [];

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Dissolve";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ColorTools.LoadBackDry(new Color(ScreensaverPackInit.SaversConfig.DissolveBackgroundColor));
            ConsoleWrapper.CursorVisible = false;
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", vars: [ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight]);
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
            bool goAhead = true;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Dissolving: {0}", vars: [ColorFilled]);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "End left: {0} | End top: {1}", vars: [EndLeft, EndTop]);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got left: {0} | Got top: {1}", vars: [Left, Top]);

            // Populate color
            Color colorStorage = Color.Empty;
            if (ScreensaverPackInit.SaversConfig.DissolveTrueColor)
            {
                int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.DissolveMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.DissolveMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.DissolveMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.DissolveMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.DissolveMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.DissolveMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                colorStorage = new Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}");
            }
            else
            {
                int ColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.DissolveMinimumColorLevel, ScreensaverPackInit.SaversConfig.DissolveMaximumColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color ({0})", vars: [ColorNum]);
                colorStorage = new Color(ColorNum);
            }

            // Fill the color if not filled
            if (!ColorFilled)
            {
                if (ConsoleResizeHandler.WasResized(false))
                {
                    // Refill, because the console is resized
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "We're refilling...");
                    ColorFilled = false;
                    goAhead = false;
                    ColorTools.LoadBackDry(new Color(ScreensaverPackInit.SaversConfig.DissolveBackgroundColor));
                    CoveredPositions.Clear();
                }

                if (goAhead)
                {
                    if (ConsoleWrapper.CursorLeft >= EndLeft && ConsoleWrapper.CursorTop >= EndTop)
                    {
                        ColorTools.SetConsoleColorDry(Color.Empty);
                        ColorTools.SetConsoleColorDry(colorStorage, true);
                        TextWriterRaw.WritePlain(" ", false);
                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "We're now dissolving... L: {0} = {1} | T: {2} = {3}", vars: [ConsoleWrapper.CursorLeft, EndLeft, ConsoleWrapper.CursorTop, EndTop]);
                        ColorFilled = true;
                    }
                    else
                    {
                        ColorTools.SetConsoleColorDry(Color.Empty);
                        ColorTools.SetConsoleColorDry(colorStorage, true);
                        TextWriterRaw.WritePlain(" ", false);
                    }
                }
            }
            else
            {
                if (!CoveredPositions.Any(t => t.Item1 == Left & t.Item2 == Top))
                {
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Covered position {0}", vars: [Left + " - " + Top]);
                    CoveredPositions.Add(new Tuple<int, int>(Left, Top));
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Covered positions: {0}/{1}", vars: [CoveredPositions.Count, (EndLeft + 1) * (EndTop + 1)]);
                }
                if (!ConsoleResizeHandler.WasResized(false))
                {
                    ConsoleWrapper.SetCursorPosition(Left, Top);
                    ColorTools.SetConsoleColorDry(new Color(ScreensaverPackInit.SaversConfig.DissolveBackgroundColor), true);
                    ConsoleWrapper.Write(" ");
                    if (CoveredPositions.Count == (EndLeft + 1) * (EndTop + 1))
                    {
                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "We're refilling...");
                        ColorFilled = false;
                        ColorTools.LoadBackDry(new Color(ScreensaverPackInit.SaversConfig.DissolveBackgroundColor));
                        CoveredPositions.Clear();
                    }
                }
                else
                {
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "We're refilling...");
                    ColorFilled = false;
                    ColorTools.LoadBackDry(new Color(ScreensaverPackInit.SaversConfig.DissolveBackgroundColor));
                    CoveredPositions.Clear();
                }
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
        }

        /// <inheritdoc/>
        public override void ScreensaverOutro()
        {
            ColorFilled = false;
            CoveredPositions.Clear();
        }

    }
}
