using System;
using System.IO;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Languages;
using KS.Misc.Splash;
using KS.Misc.Writers.DebugWriters;
using KS.Network.Transfer;

// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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

using Newtonsoft.Json.Linq;

namespace KS.Kernel.Updates
{
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
                DebugWriter.Wdbg(DebugLevel.E, "Failed to check for updates: {0}", ex.Message);
                DebugWriter.WStkTrc(ex);
            }
            return null;
        }

        /// <summary>
        /// Prompt for checking for kernel updates
        /// </summary>
        public static void CheckKernelUpdates()
        {
            // Check to see if we're running from Ubuntu PPA
            if (Paths.ExecPath.StartsWith("/usr/lib/ks"))
            {
                SplashReport.ReportProgress(Translate.DoTranslation("Use apt to update Kernel Simulator."), 10, ColorTools.ColTypes.Error);
                return;
            }

            // Check for updates now
            SplashReport.ReportProgress(Translate.DoTranslation("Checking for system updates..."), 10, ColorTools.ColTypes.Neutral);
            var AvailableUpdate = FetchKernelUpdates();
            if (AvailableUpdate is not null)
            {
                if (!AvailableUpdate.Updated)
                {
                    SplashReport.ReportProgress(Translate.DoTranslation("Found new version: "), 10, ColorTools.ColTypes.ListEntry);
                    SplashReport.ReportProgress(AvailableUpdate.UpdateVersion.ToString(), 10, ColorTools.ColTypes.ListValue);
                    if (Flags.AutoDownloadUpdate)
                    {
                        NetworkTransfer.DownloadFile(AvailableUpdate.UpdateURL.ToString(), Path.Combine(Paths.ExecPath, "update.rar"));
                        SplashReport.ReportProgress(Translate.DoTranslation("Downloaded the update successfully!"), 10, ColorTools.ColTypes.Success);
                    }
                    else
                    {
                        SplashReport.ReportProgress(Translate.DoTranslation("You can download it at: "), 10, ColorTools.ColTypes.ListEntry);
                        SplashReport.ReportProgress(AvailableUpdate.UpdateURL.ToString(), 10, ColorTools.ColTypes.ListValue);
                    }
                }
                else
                {
                    SplashReport.ReportProgress(Translate.DoTranslation("You're up to date!"), 10, ColorTools.ColTypes.Neutral);
                }
            }
            else if (AvailableUpdate is null)
            {
                SplashReport.ReportProgress(Translate.DoTranslation("Failed to check for updates."), 10, ColorTools.ColTypes.Error);
            }
        }

    }
}