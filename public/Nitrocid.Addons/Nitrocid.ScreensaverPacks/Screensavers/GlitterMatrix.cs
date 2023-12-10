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
    /// Settings for GlitterMatrix
    /// </summary>
    public static class GlitterMatrixSettings
    {

        /// <summary>
        /// [GlitterMatrix] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int GlitterMatrixDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GlitterMatrixDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1;
                ScreensaverPackInit.SaversConfig.GlitterMatrixDelay = value;
            }
        }
        /// <summary>
        /// [GlitterMatrix] Screensaver background color
        /// </summary>
        public static string GlitterMatrixBackgroundColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GlitterMatrixBackgroundColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.GlitterMatrixBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [GlitterMatrix] Screensaver foreground color
        /// </summary>
        public static string GlitterMatrixForegroundColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GlitterMatrixForegroundColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.GlitterMatrixForegroundColor = new Color(value).PlainSequence;
            }
        }

    }

    /// <summary>
    /// Display code for GlitterMatrix
    /// </summary>
    public class GlitterMatrixDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "GlitterMatrix";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            KernelColorTools.SetConsoleColor(new Color(GlitterMatrixSettings.GlitterMatrixForegroundColor));
            KernelColorTools.LoadBack(new Color(GlitterMatrixSettings.GlitterMatrixBackgroundColor));
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;
            int Left = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
            int Top = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", Left, Top);
            ConsoleWrapper.SetCursorPosition(Left, Top);
            if (!ConsoleResizeListener.WasResized(false))
            {
                ConsoleWrapper.Write(RandomDriver.Random(1).ToString());
            }
            else
            {
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.W, "Color-syncing. Clearing...");
                ConsoleWrapper.Clear();
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(GlitterMatrixSettings.GlitterMatrixDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
