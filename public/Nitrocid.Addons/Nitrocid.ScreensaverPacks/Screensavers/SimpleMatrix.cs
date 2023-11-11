//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.Drivers.RNG;
using KS.Kernel.Debugging;
using KS.Kernel.Threading;
using KS.Misc.Screensaver;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for SimpleMatrix
    /// </summary>
    public static class SimpleMatrixSettings
    {

        /// <summary>
        /// [SimpleMatrix] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int SimpleMatrixDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.SimpleMatrixDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1;
                ScreensaverPackInit.SaversConfig.SimpleMatrixDelay = value;
            }
        }

    }

    /// <summary>
    /// Display code for SimpleMatrix
    /// </summary>
    public class SimpleMatrixDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "SimpleMatrix";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            KernelColorTools.LoadBack(new Color(ConsoleColors.Black));
            KernelColorTools.SetConsoleColor(ConsoleColors.Green);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;
            if (!ConsoleResizeListener.WasResized(false))
            {
                ConsoleWrapper.Write(RandomDriver.Random(1).ToString());
            }
            else
            {
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing...");
                ConsoleWrapper.Clear();
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(SimpleMatrixSettings.SimpleMatrixDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
