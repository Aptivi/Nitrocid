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
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Splash;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Textify.General;
using static System.Net.Mime.MediaTypeNames;

namespace Nitrocid.SplashPacks.Splashes
{
    class SplashTextBox : BaseSplash, ISplash
    {
        private InfoBox infoBox = new();

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
            string text = $"*) {ProgressReport}\n\n{Progress}%";
            infoBox.Text = text.FormatString(Vars);
            int increment = 0;
            return infoBox.Render(ref increment, 0, true, false);
        }

        public override string ReportWarning(int Progress, string WarningReport, Exception? ExceptionInfo, params object[] Vars)
        {
            string exceptionMessage =
                ExceptionInfo is not null ?
                ExceptionInfo.Message :
                LanguageTools.GetLocalized("NKS_SPLASHPACKS_TEXTBOX_UNKNOWNERROR");
            string text = $"!) {WarningReport}\n\n{exceptionMessage}\n\n{Progress}%";
            infoBox.Text = text.FormatString(Vars);
            int increment = 0;
            return infoBox.Render(ref increment, 0, true, false);
        }

        public override string ReportError(int Progress, string ErrorReport, Exception? ExceptionInfo, params object[] Vars)
        {
            string exceptionMessage =
                ExceptionInfo is not null ?
                ExceptionInfo.Message :
                LanguageTools.GetLocalized("NKS_SPLASHPACKS_TEXTBOX_UNKNOWNERROR");
            string text = $"X) {ErrorReport}\n\n{exceptionMessage}\n\n{Progress}%";
            infoBox.Text = text.FormatString(Vars);
            int increment = 0;
            return infoBox.Render(ref increment, 0, true, false);
        }
    }
}
