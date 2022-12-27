
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Threading;
using Extensification.StringExts;
using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Misc.Screensaver;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;

namespace KS.TimeDate
{
    /// <summary>
    /// Top right corner on time and date
    /// </summary>
    public static class TimeDateTopRight
    {

        // Variables
        internal static KernelThread TimeTopRightChange = new("Time/date top right corner updater thread", true, TimeTopRightChange_DoWork);

        /// <summary>
        /// Updates the time and date. Also updates the time and date corner if it was enabled in kernel configuration.
        /// </summary>
        public static void TimeTopRightChange_DoWork()
        {
            try
            {
                int oldWid = default, oldTop = default;
                while (true)
                {
                    string TimeString = $"{TimeDateRenderers.RenderDate()} - {TimeDateRenderers.RenderTime()}";
                    if (Flags.CornerTimeDate == true & !Screensaver.InSaver)
                    {
                        oldWid = ConsoleBase.ConsoleWrapper.WindowWidth - TimeString.Length - 1;
                        oldTop = ConsoleBase.ConsoleWrapper.WindowTop;
                        TextWriterWhereColor.WriteWhere(TimeString, ConsoleBase.ConsoleWrapper.WindowWidth - TimeString.Length - 1, ConsoleBase.ConsoleWrapper.WindowTop, true, KernelColorType.NeutralText);
                    }
                    Thread.Sleep(1000);
                    if (oldWid != 0)
                        TextWriterWhereColor.WriteWhere(" ".Repeat(TimeString.Length), oldWid, oldTop, true, KernelColorType.NeutralText);
                }
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Aborting time/date change thread.");
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Fatal error in time/date changer: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }

        /// <summary>
        /// Starts the time on top right corner
        /// </summary>
        public static void InitTopRightDate()
        {
            if (!TimeTopRightChange.IsAlive && Flags.CornerTimeDate)
                TimeTopRightChange.Start();
        }

    }
}
