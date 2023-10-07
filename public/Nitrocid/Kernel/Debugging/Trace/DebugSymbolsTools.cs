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

#if SPECIFIERREL
using static KS.Misc.Notifications.NotificationManager;
using KS.Files;
using KS.Network.Base;
using KS.Network.Base.Transfer;
using System.Reflection;
using KS.Languages;
using KS.Misc.Notifications;
using KS.Misc.Splash;
using System;
using KS.Files.Operations.Querying;
#endif

namespace KS.Kernel.Debugging.Trace
{
    internal static class DebugSymbolsTools
    {
        /// <summary>
        /// Checks for debug symbols and downloads it if not found. It'll be auto-loaded upon download.
        /// </summary>
        internal static void CheckDebugSymbols()
        {
#if SPECIFIERREL
			if (!NetworkTools.NetworkAvailable)
			{
				NotifySend(new Notification(Translate.DoTranslation("No network while downloading debug data"), Translate.DoTranslation("Check your internet connection and try again."), NotificationPriority.Medium, NotificationType.Normal));
			}
			if (NetworkTools.NetworkAvailable)
			{
				// Check to see if we're running from Ubuntu PPA
				bool PPASpotted = Paths.ExecPath.StartsWith("/usr/lib/ks");
				if (PPASpotted)
					SplashReport.ReportProgressError(Translate.DoTranslation("Use apt to update Nitrocid KS."));

				// Download debug symbols
				if (!Checking.FileExists(Assembly.GetExecutingAssembly().Location.Replace(".exe", ".pdb")) & !PPASpotted)
				{
					try
					{
						NetworkTransfer.DownloadFile($"https://github.com/Aptivi/NitrocidKS/releases/download/v{KernelTools.KernelVersion}-beta/{KernelTools.KernelVersion}.pdb", Assembly.GetExecutingAssembly().Location.Replace(".exe", ".pdb"));
					}
					catch (Exception)
					{
						NotifySend(new Notification(Translate.DoTranslation("Error downloading debug data"), Translate.DoTranslation("There is an error while downloading debug data. Check your internet connection."), NotificationPriority.Medium, NotificationType.Normal));
					}
				}
			}
#endif
        }
    }
}
