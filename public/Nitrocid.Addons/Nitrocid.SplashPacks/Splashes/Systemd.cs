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

using System;
using System.Text;
using System.Threading;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Debugging;
using KS.Misc.Splash;
using KS.Misc.Text;

namespace Nitrocid.SplashPacks.Splashes
{
    class SplashSystemd : BaseSplash, ISplash
    {

        // Standalone splash information
        public override string SplashName => "systemd";

        // Private variables
        private int IndicatorLeft;
        private int IndicatorTop;
        private bool Beginning = true;

        // Actual logic
        public override string Display(SplashContext context)
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash displaying.");
                IndicatorLeft = ConsoleWrapper.CursorLeft + 2;
                IndicatorTop = ConsoleWrapper.CursorTop;
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash done.");
            }
            return base.Display(context);
        }

        public override string Report(int Progress, string ProgressReport, params object[] Vars)
        {
            var builder = new StringBuilder();
            if (!Beginning)
                builder.Append(
                    KernelColorTools.GetColor(KernelColorType.Success).VTSequenceForeground +
                    TextWriterWhereColor.RenderWherePlain("  OK  ", IndicatorLeft, IndicatorTop, true)
                );
            builder.Append(
                KernelColorTools.GetColor(KernelColorType.NeutralText).VTSequenceForeground +
                $" [      ] {TextTools.FormatString(ProgressReport, Vars)}"
            );
            if (!Beginning)
            {
                IndicatorLeft = 2;
                IndicatorTop = ConsoleWrapper.CursorTop - 1;
            }
            Beginning = false;
            return builder.ToString();
        }

        public override string ReportWarning(int Progress, string WarningReport, Exception ExceptionInfo, params object[] Vars)
        {
            var builder = new StringBuilder();
            if (!Beginning)
                builder.Append(
                    KernelColorTools.GetColor(KernelColorType.Warning).VTSequenceForeground +
                    TextWriterWhereColor.RenderWherePlain(" WARN ", IndicatorLeft, IndicatorTop, true)
                );
            builder.Append(
                KernelColorTools.GetColor(KernelColorType.NeutralText).VTSequenceForeground +
                $" [      ] {TextTools.FormatString(WarningReport, Vars)}"
            );
            if (!Beginning)
            {
                IndicatorLeft = 2;
                IndicatorTop = ConsoleWrapper.CursorTop - 1;
            }
            Beginning = false;
            return builder.ToString();
        }

        public override string ReportError(int Progress, string ErrorReport, Exception ExceptionInfo, params object[] Vars)
        {
            var builder = new StringBuilder();
            if (!Beginning)
                builder.Append(
                    KernelColorTools.GetColor(KernelColorType.Error).VTSequenceForeground +
                    TextWriterWhereColor.RenderWherePlain("FAILED", IndicatorLeft, IndicatorTop, true)
                );
            builder.Append(
                KernelColorTools.GetColor(KernelColorType.NeutralText).VTSequenceForeground +
                $" [      ] {TextTools.FormatString(ErrorReport, Vars)}"
            );
            if (!Beginning)
            {
                IndicatorLeft = 2;
                IndicatorTop = ConsoleWrapper.CursorTop - 1;
            }
            Beginning = false;
            return builder.ToString();
        }

    }
}
