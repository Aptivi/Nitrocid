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

using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using System.Collections.Generic;
using Textify.General;

namespace Nitrocid.Misc.Progress
{
    /// <summary>
    /// Progress management tools
    /// </summary>
    public static class ProgressManager
    {
        private static readonly List<ProgressHandler> handlers = [];

        /// <summary>
        /// Registers a process handler
        /// </summary>
        /// <param name="handler">Progress handler to register</param>
        public static void RegisterProgressHandler(ProgressHandler handler)
        {
            if (handler is null)
                throw new KernelException(KernelExceptionType.ProgressHandler, Translate.DoTranslation("The progress handler may not be null."));
            handlers.Add(handler);
        }

        /// <summary>
        /// Unregisters a process handler
        /// </summary>
        /// <param name="handler">Progress handler to unregister</param>
        public static void UnregisterProgressHandler(ProgressHandler handler)
        {
            if (handler is null)
                throw new KernelException(KernelExceptionType.ProgressHandler, Translate.DoTranslation("The progress handler may not be null."));
            handlers.Remove(handler);
        }

        /// <summary>
        /// Reports the progress to each progress handler
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="context">Progress context</param>
        public static void ReportProgress(double progress, string context) =>
            ReportProgress(progress, context, Translate.DoTranslation("Processing..."));

        /// <summary>
        /// Reports the progress to each progress handler
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="message">Message to write to the handlers</param>
        /// <param name="context">Progress context</param>
        public static void ReportProgress(double progress, string context, string message) =>
            ReportProgress(progress, context, message, null);

        /// <summary>
        /// Reports the progress to each progress handler
        /// </summary>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="message">Message to write to the handlers</param>
        /// <param name="vars">Variables for message extension</param>
        /// <param name="context">Progress context</param>
        public static void ReportProgress(double progress, string context, string message, params object?[]? vars)
        {
            context = string.IsNullOrEmpty(context) ? "General" : context;
            message = vars is not null ? TextTools.FormatString(message, vars) : message;
            foreach (var handler in handlers)
            {
                if (handler.Context == context)
                    handler.ProgressAction(progress, message);
            }
        }
    }
}
