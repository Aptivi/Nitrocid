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
using System.Threading;
using KS.Kernel.Debugging;
using KS.Misc.Text;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using Terminaux.Colors;
using KS.Misc.Splash;
using KS.ConsoleBase;
using KS.Kernel;
using System.Text;

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
        public override string Opening(SplashContext context)
        {
            var builder = new StringBuilder();
            Beginning = true;
            DebugWriter.WriteDebug(DebugLevel.I, "Splash opening. Clearing console...");
            ConsoleWrapper.Clear();
            builder.Append(
                CharManager.NewLine +
                $"   {OpenRCIndicatorColor.VTSequenceForeground}OpenRC " +
                $"{OpenRCVersionColor.VTSequenceForeground}0.13.11 " +
                $"{KernelColorTools.GetColor(KernelColorType.NeutralText).VTSequenceForeground}is starting up " +
                $"{OpenRCPlaceholderColor.VTSequenceForeground}Nitrocid KS {KernelMain.VersionFullStr}" + CharManager.NewLine
            );
            return builder.ToString();
        }

        public override string Display(SplashContext context)
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash displaying.");
                IndicatorLeft = ConsoleWrapper.WindowWidth - 8;
                IndicatorTop = ConsoleWrapper.CursorTop;
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash done.");
            }
            return "";
        }

        public override string Closing(SplashContext context, out bool delayRequired) =>
            base.Closing(context, out delayRequired);

        public override string Report(int Progress, string ProgressReport, params object[] Vars)
        {
            var builder = new StringBuilder();
            if (!Beginning)
            {
                builder.Append(
                    OpenRCPlaceholderColor.VTSequenceForeground +
                    TextWriterWhereColor.RenderWherePlain("[    ]", IndicatorLeft, IndicatorTop, true) +
                    OpenRCIndicatorColor.VTSequenceForeground +
                    TextWriterWhereColor.RenderWherePlain(" ok ", IndicatorLeft + 1, IndicatorTop, true)
                );
            }
            builder.Append(
                OpenRCIndicatorColor.VTSequenceForeground +
                $" * {TextTools.FormatString(ProgressReport, Vars)}"
            );
            if (!Beginning)
            {
                IndicatorLeft = ConsoleWrapper.WindowWidth - 8;
                IndicatorTop = ConsoleWrapper.CursorTop - 1;
            }
            Beginning = false;
            return builder.ToString();
        }

        public override string ReportError(int Progress, string ErrorReport, Exception ExceptionInfo, params object[] Vars)
        {
            var builder = new StringBuilder();
            if (!Beginning)
            {
                builder.Append(
                    OpenRCPlaceholderColor.VTSequenceForeground +
                    TextWriterWhereColor.RenderWherePlain("[    ]", IndicatorLeft, IndicatorTop, true) +
                    OpenRCIndicatorColor.VTSequenceForeground +
                    TextWriterWhereColor.RenderWherePlain("fail", IndicatorLeft + 1, IndicatorTop, true)
                );
            }
            builder.Append(
                OpenRCIndicatorColor.VTSequenceForeground +
                $" * {TextTools.FormatString(ErrorReport, Vars)}"
            );
            if (!Beginning)
            {
                IndicatorLeft = ConsoleWrapper.WindowWidth - 8;
                IndicatorTop = ConsoleWrapper.CursorTop - 1;
            }
            Beginning = false;
            return builder.ToString();
        }

        public override string ReportWarning(int Progress, string WarningReport, Exception ExceptionInfo, params object[] Vars)
        {
            var builder = new StringBuilder();
            if (!Beginning)
            {
                builder.Append(
                    OpenRCPlaceholderColor.VTSequenceForeground +
                    TextWriterWhereColor.RenderWherePlain("[    ]", IndicatorLeft, IndicatorTop, true) +
                    OpenRCIndicatorColor.VTSequenceForeground +
                    TextWriterWhereColor.RenderWherePlain("warn", IndicatorLeft + 1, IndicatorTop, true)
                );
            }
            builder.Append(
                OpenRCIndicatorColor.VTSequenceForeground +
                $" * {TextTools.FormatString(WarningReport, Vars)}"
            );
            if (!Beginning)
            {
                IndicatorLeft = ConsoleWrapper.WindowWidth - 8;
                IndicatorTop = ConsoleWrapper.CursorTop - 1;
            }
            Beginning = false;
            return builder.ToString();
        }

    }
}
