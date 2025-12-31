//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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

using Nitrocid.Base.Languages;
using System;

namespace Nitrocid.Base.Misc.Splash
{
    /// <summary>
    /// Splash report info
    /// </summary>
    public class SplashReportInfo
    {
        private readonly DateTime time;
        private readonly int progress;
        private readonly SplashReportSeverity severity;
        private readonly string renderedMessage;

        /// <summary>
        /// Time of the incident in a string
        /// </summary>
        public DateTime Time =>
            time;

        /// <summary>
        /// Progress from 0% to 100%
        /// </summary>
        public int Progress =>
            progress;

        /// <summary>
        /// Incident severity
        /// </summary>
        public SplashReportSeverity Severity =>
            severity;

        /// <summary>
        /// Rendered messages with their variables formatted
        /// </summary>
        public string RenderedMessage =>
            renderedMessage;

        internal SplashReportInfo(DateTime time, int progress, SplashReportSeverity severity, string renderedMessage)
        {
            this.time = time;
            this.progress = progress;
            this.severity = severity;
            this.renderedMessage = renderedMessage ??
                LanguageTools.GetLocalized("NKS_MISC_SPLASH_NOINFO");
        }
    }
}
