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
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs.Styles;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Splash;

namespace Nitrocid.SplashPacks.Splashes
{
    class SplashTextBox : BaseSplash, ISplash
    {

        // Standalone splash information
        public override string SplashName => "TextBox";

        public override void Display(SplashContext context)
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash displaying.");

                // Loop until closing
                while (!SplashClosing)
                    Thread.Sleep(1);
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash done.");
            }
        }

        public override void Report(int Progress, string ProgressReport, params object[] Vars)
        {
            KernelColorTools.LoadBack();
            InfoBoxColor.WriteInfoBox($"*) {ProgressReport}\n\n{Progress}%", false, Vars);
        }

        public override void ReportWarning(int Progress, string WarningReport, Exception ExceptionInfo, params object[] Vars)
        {
            string exceptionMessage = ExceptionInfo is not null ? ExceptionInfo.Message : Translate.DoTranslation("Unknown error!");
            KernelColorTools.LoadBack();
            InfoBoxColor.WriteInfoBox($"!) {WarningReport}\n\n{exceptionMessage}\n\n{Progress}%", false, Vars);
        }

        public override void ReportError(int Progress, string ErrorReport, Exception ExceptionInfo, params object[] Vars)
        {
            string exceptionMessage = ExceptionInfo is not null ? ExceptionInfo.Message : Translate.DoTranslation("Unknown error!");
            KernelColorTools.LoadBack();
            InfoBoxColor.WriteInfoBox($"X) {ErrorReport}\n\n{exceptionMessage}\n\n{Progress}%", false, Vars);
        }
    }
}
