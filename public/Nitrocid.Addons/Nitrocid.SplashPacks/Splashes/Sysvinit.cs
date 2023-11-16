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
using KS.ConsoleBase.Colors;
using KS.Kernel.Debugging;
using KS.Misc.Splash;
using KS.Misc.Text;

namespace Nitrocid.SplashPacks.Splashes
{
    class SplashSysvinit : BaseSplash, ISplash
    {

        // Standalone splash information
        public override string SplashName => "sysvinit";

        // Private variables
        private bool Beginning = true;

        // Actual logic
        public override string Display(SplashContext context)
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash displaying.");
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash done.");
            }
            return "";
        }

        public override string Report(int Progress, string ProgressReport, params object[] Vars)
        {
            var builder = new StringBuilder(KernelColorTools.GetColor(KernelColorType.NeutralText).VTSequenceForeground);
            if (!Beginning)
                builder.AppendLine(".");
            builder.Append($"{TextTools.FormatString(ProgressReport, Vars)}:");
            Beginning = false;
            return builder.ToString();
        }

        public override string ReportWarning(int Progress, string WarningReport, Exception ExceptionInfo, params object[] Vars) =>
            Report(Progress, WarningReport, Vars);

        public override string ReportError(int Progress, string ErrorReport, Exception ExceptionInfo, params object[] Vars) =>
            Report(Progress, ErrorReport, Vars);

    }
}
