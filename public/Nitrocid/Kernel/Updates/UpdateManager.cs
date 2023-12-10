//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Extensions;
using KS.Network.Base.Transfer;
using Newtonsoft.Json.Linq;

#if SPECIFIERREL
using KS.Files.Paths;
using KS.Misc.Splash;
using KS.Languages;
using System.IO;
#endif

#if PACKAGEMANAGERBUILD
#if !SPECIFIERREL
using KS.Misc.Splash;
using KS.Languages;
#endif
#endif

namespace KS.Kernel.Updates
{
    /// <summary>
    /// Update management module
    /// </summary>
    public static class UpdateManager
    {

        /// <summary>
        /// Whether or not to check for updates on startup
        /// </summary>
        public static bool CheckUpdateStart =>
            Config.MainConfig.CheckUpdateStart;

        /// <summary>
        /// Automatically downloads the kernel updates and notifies the user
        /// </summary>
        public static bool AutoDownloadUpdate =>
            Config.MainConfig.AutoDownloadUpdate;

        /// <summary>
        /// Fetches the GitHub repo to see if there are any updates
        /// </summary>
        /// <param name="kind">The kind of update</param>
        /// <returns>A kernel update instance</returns>
        public static KernelUpdate FetchKernelUpdates(UpdateKind kind)
        {
            try
            {
                // Because api.github.com requires the UserAgent header to be put, else, 403 error occurs. Fortunately for us, "Aptivi" is enough.
                NetworkTransfer.WClient.DefaultRequestHeaders.Add("User-Agent", "Aptivi");

                // Populate the following variables with information
                string UpdateStr = NetworkTransfer.DownloadString("https://api.github.com/repos/Aptivi/NitrocidKS/releases", false);
                var UpdateToken = JToken.Parse(UpdateStr);
                var UpdateInstance = new KernelUpdate(UpdateToken, kind);

                // Return the update instance
                return UpdateInstance;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to check for updates: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            finally
            {
                NetworkTransfer.WClient.DefaultRequestHeaders.Remove("User-Agent");
            }
            return null;
        }

        /// <summary>
        /// Fetches the GitHub repo to see if there are any updates
        /// </summary>
        /// <returns>A kernel update instance</returns>
        public static KernelUpdate FetchBinaryArchive()
        {
            try
            {
                // Determine the update kind by fetching the addons
                var kind = UpdateKind.Binary;
                if (AddonTools.ListAddons().Count == 0)
                    kind = UpdateKind.BinaryLite;
                return FetchKernelUpdates(kind);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to check for updates: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return null;
        }

        /// <summary>
        /// Fetches the GitHub repo for addon pack
        /// </summary>
        /// <returns>A kernel update instance</returns>
        public static KernelUpdate FetchAddonPack()
        {
            try
            {
                return FetchKernelUpdates(UpdateKind.Addons);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to check for updates: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return null;
        }

        /// <summary>
        /// Fetches the GitHub repo for changelogs
        /// </summary>
        /// <returns>A kernel update instance</returns>
        public static KernelUpdate FetchChangelogs()
        {
            try
            {
                return FetchKernelUpdates(UpdateKind.Changelogs);
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
            var AvailableUpdate = FetchBinaryArchive();
            if (AvailableUpdate is not null)
            {
                if (!AvailableUpdate.Updated)
                {
                    SplashReport.ReportProgress(Translate.DoTranslation("Found new version: "), 10);
                    SplashReport.ReportProgress(AvailableUpdate.UpdateVersion.ToString(), 10);
                    if (AutoDownloadUpdate)
                    {
                        NetworkTransfer.DownloadFile(AvailableUpdate.UpdateURL.ToString(), Path.Combine(PathsManagement.ExecPath, "update.zip"));
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

        /// <summary>
        /// Gets the changelogs for the current kernel version
        /// </summary>
        /// <returns>A string representing the changelogs for this kernel version</returns>
        public static string GetVersionChangelogs()
        {
            var changelogsUpdate = FetchChangelogs();
            string changes = NetworkTransfer.DownloadString(changelogsUpdate.UpdateURL.ToString());
            return changes;
        }

    }
}
