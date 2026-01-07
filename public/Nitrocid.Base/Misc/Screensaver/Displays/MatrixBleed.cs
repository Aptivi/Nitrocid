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
using Terminaux.Colors;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Base;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Misc.Screensaver.Displays.Utilities;

namespace Nitrocid.Base.Misc.Screensaver.Displays
{
    /// <summary>
    /// Display code for MatrixBleed
    /// </summary>
    public class MatrixBleedDisplay : BaseScreensaver, IScreensaver
    {
        private static readonly List<MatrixBleedState> bleedStates = [];

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "MatrixBleed";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            ColorTools.LoadBackDry("0;0;0");
            ConsoleWrapper.Clear();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Now, determine the fall end position
            int FallEnd = ConsoleWrapper.WindowHeight - 1;

            // Invoke the "chance"-based random number generator to decide whether a line is about to fall.
            bool newFall = RandomDriver.RandomChance(Config.SaverConfig.MatrixBleedDropChance);
            if (newFall)
                bleedStates.Add(new MatrixBleedState());

            // Now, iterate through the bleed states
            for (int bleedIdx = 0; bleedIdx < bleedStates.Count; bleedIdx++)
            {
                // Choose the column for the falling line
                var bleedState = bleedStates[bleedIdx];

                // Make the line fall down
                switch (bleedState.fallState)
                {
                    case MatrixBleedFallState.Falling:
                        bleedState.Fall();
                        bleedState.fallStep++;
                        if (bleedState.fallStep > FallEnd)
                            bleedState.fallState = MatrixBleedFallState.Fading;
                        break;
                    case MatrixBleedFallState.Fading:
                        bleedState.Fade();
                        bleedState.fadeStep++;
                        if (bleedState.fadeStep > Config.SaverConfig.MatrixBleedMaxSteps)
                            bleedState.fallState = MatrixBleedFallState.Done;
                        break;
                }
            }

            // Purge the "Done" falls
            for (int bleedIdx = bleedStates.Count - 1; bleedIdx >= 0; bleedIdx--)
            {
                var bleedState = bleedStates[bleedIdx];
                if (bleedState.fallState == MatrixBleedFallState.Done)
                {
                    bleedStates.RemoveAt(bleedIdx);
                    bleedState.Unreserve(bleedState.ColumnLine);
                }
            }

            // Draw and clean the buffer
            string buffer = MatrixBleedState.bleedBuffer.ToString();
            MatrixBleedState.bleedBuffer.Clear();
            TextWriterRaw.WritePlain(buffer);
            ScreensaverManager.Delay(Config.SaverConfig.MatrixBleedDelay);
        }

        /// <inheritdoc/>
        public override void ScreensaverOutro() =>
            bleedStates.Clear();
    }
}
