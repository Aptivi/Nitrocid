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

using Nitrocid.Base.Kernel.Debugging;
using System;

namespace Nitrocid.Base.Misc.Progress
{
    /// <summary>
    /// Progress handler class instance
    /// </summary>
    public class ProgressHandler
    {
        private readonly Action<double, string> progressActionOrig =
            (num, text) => DebugWriter.WriteDebug(DebugLevel.I, $"{num}% {text}");
        private readonly Action<double, string> progressAction;
        private readonly string context = "General";

        /// <summary>
        /// The progress action delegate to use
        /// </summary>
        public Action<double, string> ProgressAction =>
            progressAction;

        /// <summary>
        /// The progress context
        /// </summary>
        public string Context =>
            context;

        /// <summary>
        /// Makes a new instance of the progress handler
        /// </summary>
        /// <param name="progressAction">The progress action delegate to use</param>
        /// <param name="context">Progress context</param>
        public ProgressHandler(Action<double, string> progressAction, string context)
        {
            this.progressAction = progressAction ?? progressActionOrig;
            this.context = string.IsNullOrEmpty(context) ? "General" : context;
        }
    }
}
