//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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
using Terminaux.Inputs.Styles.Infobox;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using Nitrocid.Misc.Splash;
using Terminaux.Colors.Themes.Colors;

namespace Nitrocid.SplashPacks.Splashes
{
    class SplashTextBox : BaseSplash, ISplash
    {

        // Standalone splash information
        public override string SplashName => "TextBox";

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
            ThemeColorsTools.LoadBackground();
            InfoBoxNonModalColor.WriteInfoBox($"*) {ProgressReport}\n\n{Progress}%", Vars);
            return "";
        }

        public override string ReportWarning(int Progress, string WarningReport, Exception? ExceptionInfo, params object[] Vars)
        {
            string exceptionMessage =
                ExceptionInfo is not null ?
                ExceptionInfo.Message :
                LanguageTools.GetLocalized("NKS_SPLASHPACKS_TEXTBOX_UNKNOWNERROR");
            ThemeColorsTools.LoadBackground();
            InfoBoxNonModalColor.WriteInfoBox($"!) {WarningReport}\n\n{exceptionMessage}\n\n{Progress}%", Vars);
            return "";
        }

        public override string ReportError(int Progress, string ErrorReport, Exception? ExceptionInfo, params object[] Vars)
        {
            string exceptionMessage =
                ExceptionInfo is not null ?
                ExceptionInfo.Message :
                LanguageTools.GetLocalized("NKS_SPLASHPACKS_TEXTBOX_UNKNOWNERROR");
            ThemeColorsTools.LoadBackground();
            InfoBoxNonModalColor.WriteInfoBox($"X) {ErrorReport}\n\n{exceptionMessage}\n\n{Progress}%", Vars);
            return "";
        }
    }
}
