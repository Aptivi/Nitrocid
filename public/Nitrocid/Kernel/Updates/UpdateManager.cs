
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using KS.Kernel.Debugging;
using KS.Network.Base.Transfer;
using Newtonsoft.Json.Linq;

#if SPECIFIERREL
using KS.Files;
using KS.Misc.Splash;
using KS.Languages;
using System.IO;
#endif

#if PACKAGEMANAGERBUILD
using KS.Misc.Splash;
using KS.Languages;
#endif

namespace KS.Kernel.Updates
{
    /// <summary>
    /// Update management module
    /// </summary>
    public static class UpdateManager
    {

        /// <summary>
        /// Fetches the GitHub repo to see if there are any updates
        /// </summary>
        /// <returns>A kernel update instance</returns>
        public static KernelUpdate FetchKernelUpdates()
        {
            try
            {
                // Because api.github.com requires the UserAgent header to be put, else, 403 error occurs. Fortunately for us, "Aptivi" is enough.
                NetworkTransfer.WClient.DefaultRequestHeaders.Add("User-Agent", "Aptivi");

                // Populate the following variables with information
                string UpdateStr = NetworkTransfer.DownloadString("https://api.github.com/repos/Aptivi/NitrocidKS/releases");
                var UpdateToken = JToken.Parse(UpdateStr);
                var UpdateInstance = new KernelUpdate(UpdateToken);

                // Return the update instance
                NetworkTransfer.WClient.DefaultRequestHeaders.Remove("User-Agent");
                return UpdateInstance;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to check for updates: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return null;
        }

        /// <summary>
        /// Prompt for checking for kernel updates
        /// </summary>
        public static void CheckKernelUpdates()
        {
#if SPECIFIERREL && !PACKAGEMANAGERBUILD
            // Check for updates now
            SplashReport.ReportProgress(Translate.DoTranslation("Checking for system updates..."), 10);
            var AvailableUpdate = FetchKernelUpdates();
            if (AvailableUpdate is not null)
            {
                if (!AvailableUpdate.Updated)
                {
                    SplashReport.ReportProgress(Translate.DoTranslation("Found new version: "), 10);
                    SplashReport.ReportProgress(AvailableUpdate.UpdateVersion.ToString(), 10);
                    if (KernelFlags.AutoDownloadUpdate)
                    {
                        NetworkTransfer.DownloadFile(AvailableUpdate.UpdateURL.ToString(), Path.Combine(Paths.ExecPath, "update.rar"));
                        SplashReport.ReportProgress(Translate.DoTranslation("Downloaded the update successfully!"), 10);
                    }
                    else
                    {
                        SplashReport.ReportProgress(Translate.DoTranslation("You can download it at: "), 10);
                        SplashReport.ReportProgress(AvailableUpdate.UpdateURL.ToString(), 10);
                    }
                }
                else
                {
                    SplashReport.ReportProgress(Translate.DoTranslation("You're up to date!"), 10);
                }
            }
            else if (AvailableUpdate is null)
            {
                SplashReport.ReportProgressError(Translate.DoTranslation("Failed to check for updates."));
            }
#elif PACKAGEMANAGERBUILD
            SplashReport.ReportProgressError(Translate.DoTranslation("You've installed Nitrocid KS using your package manager. Please use it to upgrade your kernel instead."));
#endif
        }

    }
}
