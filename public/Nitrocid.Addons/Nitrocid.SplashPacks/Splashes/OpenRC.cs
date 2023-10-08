
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Threading;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Misc.Text;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using Terminaux.Colors;
using KS.Misc.Splash;
using KS.ConsoleBase;

namespace Nitrocid.SplashPacks.Splashes
{
    class SplashOpenRC : BaseSplash, ISplash
    {

        // Standalone splash information
        public override string SplashName => "openrc";

        // Private variables
        private int IndicatorLeft;
        private int IndicatorTop;
        private bool Beginning = true;
        private readonly Color OpenRCVersionColor = new(85, 255, 255);
        private readonly Color OpenRCIndicatorColor = new((int)ConsoleColor.Green);
        private readonly Color OpenRCPlaceholderColor = new(85, 85, 255);

        // Actual logic
        public override void Opening()
        {
            Beginning = true;
            DebugWriter.WriteDebug(DebugLevel.I, "Splash opening. Clearing console...");
            ConsoleWrapper.Clear();
            TextWriterColor.Write(CharManager.NewLine + $"   {OpenRCIndicatorColor.VTSequenceForeground}OpenRC {OpenRCVersionColor.VTSequenceForeground}0.13.11 {KernelColorTools.GetColor(KernelColorType.NeutralText).VTSequenceForeground}is starting up {OpenRCPlaceholderColor.VTSequenceForeground}Nitrocid KS {KernelTools.KernelVersion}" + CharManager.NewLine);
        }

        public override void Display()
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash displaying.");
                IndicatorLeft = ConsoleWrapper.WindowWidth - 8;
                IndicatorTop = ConsoleWrapper.CursorTop;
                while (!SplashClosing)
                    Thread.Sleep(1);
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash done.");
            }
        }

        public override void Closing()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Splash closing. Clearing console...");
            ConsoleWrapper.Clear();
        }

        public override void Report(int Progress, string ProgressReport, params object[] Vars)
        {
            if (!Beginning)
            {
                TextWriterWhereColor.WriteWhereColor("[    ]", IndicatorLeft, IndicatorTop, true, OpenRCPlaceholderColor);
                TextWriterWhereColor.WriteWhereColor(" ok ", IndicatorLeft + 1, IndicatorTop, true, OpenRCIndicatorColor);
            }
            TextWriterColor.WriteColor($" * ", false, OpenRCIndicatorColor);
            TextWriterColor.Write(ProgressReport, Vars);
            if (!Beginning)
            {
                IndicatorLeft = ConsoleWrapper.WindowWidth - 8;
                IndicatorTop = ConsoleWrapper.CursorTop - 1;
            }
            Beginning = false;
        }

        public override void ReportError(int Progress, string ErrorReport, Exception ExceptionInfo, params object[] Vars)
        {
            if (!Beginning)
            {
                TextWriterWhereColor.WriteWhereColor("[    ]", IndicatorLeft, IndicatorTop, true, OpenRCPlaceholderColor);
                TextWriterWhereColor.WriteWhereColor("fail", IndicatorLeft + 1, IndicatorTop, true, OpenRCIndicatorColor);
            }
            TextWriterColor.WriteColor($" * ", false, OpenRCIndicatorColor);
            TextWriterColor.Write(ErrorReport, Vars);
            if (!Beginning)
            {
                IndicatorLeft = ConsoleWrapper.WindowWidth - 8;
                IndicatorTop = ConsoleWrapper.CursorTop - 1;
            }
            Beginning = false;
        }

        public override void ReportWarning(int Progress, string WarningReport, Exception ExceptionInfo, params object[] Vars)
        {
            if (!Beginning)
            {
                TextWriterWhereColor.WriteWhereColor("[    ]", IndicatorLeft, IndicatorTop, true, OpenRCPlaceholderColor);
                TextWriterWhereColor.WriteWhereColor("warn", IndicatorLeft + 1, IndicatorTop, true, OpenRCIndicatorColor);
            }
            TextWriterColor.WriteColor($" * ", false, OpenRCIndicatorColor);
            TextWriterColor.Write(WarningReport, Vars);
            if (!Beginning)
            {
                IndicatorLeft = ConsoleWrapper.WindowWidth - 8;
                IndicatorTop = ConsoleWrapper.CursorTop - 1;
            }
            Beginning = false;
        }

    }
}
