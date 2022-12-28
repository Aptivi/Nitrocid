
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
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
                string UpdateStr = NetworkTransfer.DownloadString("https://api.github.com/repos/Aptivi/Kernel-Simulator/releases");
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
#if SPECIFIERREL
            // Check to see if we're running from Ubuntu PPA
            if (Paths.ExecPath.StartsWith("/usr/lib/ks"))
            {
                SplashReport.ReportProgressError(Translate.DoTranslation("Use apt to update Kernel Simulator."));
                return;
            }

            // Check for updates now
            SplashReport.ReportProgress(Translate.DoTranslation("Checking for system updates..."), 10, KernelColorType.NeutralText);
            var AvailableUpdate = FetchKernelUpdates();
            if (AvailableUpdate is not null)
            {
                if (!AvailableUpdate.Updated)
                {
                    SplashReport.ReportProgress(Translate.DoTranslation("Found new version: "), 10, KernelColorType.ListEntry);
                    SplashReport.ReportProgress(AvailableUpdate.UpdateVersion.ToString(), 10, KernelColorType.ListValue);
                    if (Flags.AutoDownloadUpdate)
                    {
                        NetworkTransfer.DownloadFile(AvailableUpdate.UpdateURL.ToString(), Path.Combine(Paths.ExecPath, "update.rar"));
                        SplashReport.ReportProgress(Translate.DoTranslation("Downloaded the update successfully!"), 10, KernelColorType.Success);
                    }
                    else
                    {
                        SplashReport.ReportProgress(Translate.DoTranslation("You can download it at: "), 10, KernelColorType.ListEntry);
                        SplashReport.ReportProgress(AvailableUpdate.UpdateURL.ToString(), 10, KernelColorType.ListValue);
                    }
                }
                else
                {
                    SplashReport.ReportProgress(Translate.DoTranslation("You're up to date!"), 10, KernelColorType.NeutralText);
                }
            }
            else if (AvailableUpdate is null)
            {
                SplashReport.ReportProgressError(Translate.DoTranslation("Failed to check for updates."));
            }
#endif
        }

    }
}
