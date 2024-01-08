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

using Nitrocid.ConsoleBase;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Base;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for KSXTheEnd
    /// </summary>
    public class KSXTheEndDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "KSXTheEnd";

        /// <inheritdoc/>
        public override bool ScreensaverContainsFlashingImages { get; set; } = true;

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ColorTools.LoadBack(new Color(0, 0, 0));
            ConsoleWrapper.CursorVisible = false;
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            int step;
            int maxSteps = 1;
            Color green = new(0, 255, 0);
            Color black = new(0, 0, 0);

            // Start stepping
            for (step = 1; step <= maxSteps; step++)
            {
                if (ConsoleResizeListener.WasResized(false))
                    break;

                switch (step)
                {
                    case 1:
                        string tbc = "KS-X: The End - Coming soon - January 31st, 2024".ToUpper();
                        ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        TextWriterWhereColor.WriteWhereColorBack(tbc, ConsoleWrapper.WindowWidth / 2 - tbc.Length / 2, ConsoleWrapper.WindowHeight / 2, green, black);
                        ThreadManager.SleepNoBlock(40, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        TextWriterWhereColor.WriteWhereColorBack(tbc, ConsoleWrapper.WindowWidth / 2 - tbc.Length / 2, ConsoleWrapper.WindowHeight / 2, black, black);
                        ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        TextWriterWhereColor.WriteWhereColorBack(tbc, ConsoleWrapper.WindowWidth / 2 - tbc.Length / 2, ConsoleWrapper.WindowHeight / 2, green, black);
                        ThreadManager.SleepNoBlock(50, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        TextWriterWhereColor.WriteWhereColorBack(tbc, ConsoleWrapper.WindowWidth / 2 - tbc.Length / 2, ConsoleWrapper.WindowHeight / 2, black, black);
                        ThreadManager.SleepNoBlock(1000, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        TextWriterWhereColor.WriteWhereColorBack(tbc, ConsoleWrapper.WindowWidth / 2 - tbc.Length / 2, ConsoleWrapper.WindowHeight / 2, green, black);
                        ThreadManager.SleepNoBlock(5000, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        break;
                }
            }

            // Reset
            ConsoleResizeListener.WasResized();
            ColorTools.LoadBack(black);
        }

    }
}
