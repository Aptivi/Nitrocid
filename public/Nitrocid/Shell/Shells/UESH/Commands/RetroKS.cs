
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
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Files.Operations;
using KS.Files.Querying;
using KS.Kernel.Updates;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Network.Base.Transfer;
using KS.Shell.ShellBase.Commands;
using Newtonsoft.Json.Linq;
using SharpCompress.Archives;
using SharpCompress.Archives.Rar;
using SharpCompress.Common;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Retro Nitrocid KS based on 0.0.4.1
    /// </summary>
    /// <remarks>
    /// This command runs a legacy version of Nitrocid KS based on 0.0.4.1 with added optimizations for both Linux and Windows operating systems.
    /// </remarks>
    class RetroKSCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            string ExecutableName = "RetroKS.dll";
            TextWriterColor.Write(Translate.DoTranslation("Checking for updates..."));

            // Because api.github.com requires the UserAgent header to be put, else, 403 error occurs. Fortunately for us, "Aptivi" is enough.
            NetworkTransfer.WClient.DefaultRequestHeaders.Add("User-Agent", "Aptivi");

            // Populate the following variables with information
            string RetroKSStr = NetworkTransfer.DownloadString("https://api.github.com/repos/Aptivi/RetroKS/releases");
            var RetroKSToken = JToken.Parse(RetroKSStr);
            var SortedVersions = new List<KernelUpdateInfo>();
            foreach (JToken RetroKS in RetroKSToken)
            {
                var RetroKSVer = new Version(RetroKS.SelectToken("tag_name").ToString());
                string RetroKSURL;
                var RetroKSAssets = RetroKS.SelectToken("assets");
                RetroKSURL = (string)RetroKSAssets[0]["browser_download_url"];
                var RetroKSInfo = new KernelUpdateInfo(RetroKSVer, RetroKSURL);
                SortedVersions.Add(RetroKSInfo);
            }
            SortedVersions = SortedVersions.OrderByDescending(x => x.UpdateVersion).ToList();
            NetworkTransfer.WClient.DefaultRequestHeaders.Remove("User-Agent");

            // Populate paths
            string RetroKSPath = Filesystem.NeutralizePath("retroks.rar", Paths.RetroKSDownloadPath);
            string RetroExecKSPath = Filesystem.NeutralizePath(ExecutableName, Paths.RetroKSDownloadPath);

            // Make the directory for RetroKS
            Making.MakeDirectory(Paths.RetroKSDownloadPath, false);

            // Check to see if we already have RetroKS installed and up-to-date
            if ((Checking.FileExists(RetroExecKSPath) && Assembly.Load(System.IO.File.ReadAllBytes(RetroExecKSPath)).GetName().Version < SortedVersions[0].UpdateVersion) | !Checking.FileExists(RetroExecKSPath))
            {
                TextWriterColor.Write(Translate.DoTranslation("Downloading version") + " {0}...", SortedVersions[0].UpdateVersion.ToString());

                // Download RetroKS
                var RetroKSURI = SortedVersions[0].UpdateURL;
                NetworkTransfer.DownloadFile(RetroKSURI.ToString(), RetroKSPath);

                // Extract it
                TextWriterColor.Write(Translate.DoTranslation("Installing version") + " {0}...", SortedVersions[0].UpdateVersion.ToString());
                using (var archive = RarArchive.Open(RetroKSPath))
                {
                    foreach (var entry in archive.Entries.Where(e => !e.IsDirectory))
                        entry.WriteToDirectory(Paths.RetroKSDownloadPath, new ExtractionOptions()
                        {
                            ExtractFullPath = true,
                            Overwrite = true
                        });
                }
            }

            // Now, run the assembly
            TextWriterColor.Write(Translate.DoTranslation("Going back to 2018..."));
            Assembly.LoadFrom(RetroExecKSPath).EntryPoint.Invoke("", Array.Empty<object>());

            // Clear the console
            ColorTools.SetConsoleColor(KernelColorType.Background, true);
            ConsoleBase.ConsoleWrapper.Clear();
        }

    }
}
