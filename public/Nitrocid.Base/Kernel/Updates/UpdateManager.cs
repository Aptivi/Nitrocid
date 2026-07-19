//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using Newtonsoft.Json.Linq;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Splash;
using Nitrocid.Base.Misc.Reflection.Internal;
using Nitrocid.Base.Network.Transfer;

#if SPECIFIERREL
using Nitrocid.Base.Files.Paths;
using Nitrocid.Base.Kernel.Configuration;
using System.IO;
#endif

namespace Nitrocid.Base.Kernel.Updates
{
    /// <summary>
    /// Update management module
    /// </summary>
    public static class UpdateManager
    {
        /// <summary>
        /// Fetches the GitHub repo to see if there are any updates
        /// </summary>
        /// <param name="kind">The kind of update</param>
        /// <returns>A kernel update instance</returns>
        public static KernelUpdate? FetchKernelUpdates(UpdateKind kind)
        {
            try
            {
                // Because api.github.com requires the UserAgent header to be put, else, 403 error occurs. Fortunately for us, "Aptivi" is enough.
                NetworkTransfer.HttpClient.DefaultRequestHeaders.Add("User-Agent", "Aptivi");

                // Populate the following variables with information
                string UpdateStr = NetworkTransfer.DownloadString("https://api.github.com/repos/Aptivi/Nitrocid/releases", false);
                var UpdateToken = JToken.Parse(UpdateStr);
                var UpdateInstance = new KernelUpdate(UpdateToken, kind);

                // Return the update instance
                return UpdateInstance;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to check for updates: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            finally
            {
                NetworkTransfer.HttpClient.DefaultRequestHeaders.Remove("User-Agent");
            }
            return null;
        }

        /// <summary>
        /// Prompt for checking for kernel updates
        /// </summary>
        public static void CheckKernelUpdates()
        {
#if SPECIFIERREL
            // Check for updates now
            SplashReport.ReportProgress(LanguageTools.GetLocalized("NKS_KERNEL_UPDATES_CHECKING"), 10);
            var AvailableUpdate = FetchKernelUpdates(UpdateKind.Binary);
            if (AvailableUpdate is not null)
            {
                if (!AvailableUpdate.Updated && AvailableUpdate.UpdateVersion is not null)
                {
                    SplashReport.ReportProgress(LanguageTools.GetLocalized("NKS_KERNEL_UPDATES_FOUND"), 10);
                    SplashReport.ReportProgress(AvailableUpdate.UpdateVersion.ToString(), 10);
                    SplashReport.ReportProgress(LanguageTools.GetLocalized("NKS_KERNEL_UPDATES_LINK"), 10);
                    SplashReport.ReportProgress(AvailableUpdate.UpdateURL.ToString(), 10);
                }
                else
                    SplashReport.ReportProgress(LanguageTools.GetLocalized("NKS_KERNEL_UPDATES_UPTODATE"), 10);
            }
            else if (AvailableUpdate is null)
                SplashReport.ReportProgressError(LanguageTools.GetLocalized("NKS_KERNEL_UPDATES_CHECKFAILED"));
#else
            SplashReport.ReportProgressWarning(LanguageTools.GetLocalized("NKS_KERNEL_UPDATES_CHECKDISABLED"));
#endif
        }

        internal static string FetchCurrentChangelogsFromResources()
        {
            // Get the changelogs from resource
            bool exists = ResourcesManager.DataExists("changes.chg", ResourcesType.Misc, out var stream);
            if (!exists)
                return "";

            // Convert the stream to the string and return its contents
            string contents = ResourcesManager.ConvertToString(stream);
            return contents;
        }
    }
}
