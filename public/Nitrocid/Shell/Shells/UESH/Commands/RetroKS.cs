
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
using System.Runtime.Loader;
using System.Threading;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Files;
using KS.Files.Operations;
using KS.Files.Querying;
using KS.Kernel;
using KS.Kernel.Power;
using KS.Kernel.Updates;
using KS.Languages;
using KS.Misc.RetroKS;
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

        public override int Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, ref string variableValue)
        {
            string ExecutableName = "RetroKS.dll";
            TextWriterColor.Write(Translate.DoTranslation("Checking for updates..."));

            // Because api.github.com requires the UserAgent header to be put, else, 403 error occurs. Fortunately for us, "Aptivi" is enough.
            NetworkTransfer.WClient.DefaultRequestHeaders.Add("User-Agent", "Aptivi");

            // Populate the following variables with information
            string RetroKSStr = NetworkTransfer.DownloadString("https://api.github.com/repos/Aptivi/RetroKS/releases");
            var RetroKSToken = JToken.Parse(RetroKSStr);
            var update = new KernelUpdate(RetroKSToken);

            // Populate paths
            string RetroKSPath = Filesystem.NeutralizePath("retroks.rar", Paths.RetroKSDownloadPath);
            string RetroExecKSPath = Filesystem.NeutralizePath(ExecutableName, Paths.RetroKSDownloadPath);

            // Make the directory for RetroKS
            Making.MakeDirectory(Paths.RetroKSDownloadPath, false);

            // Check to see if we already have RetroKS installed and up-to-date
            if ((Checking.FileExists(RetroExecKSPath) && Assembly.Load(System.IO.File.ReadAllBytes(RetroExecKSPath)).GetName().Version < update.UpdateVersion) | !Checking.FileExists(RetroExecKSPath))
            {
                TextWriterColor.Write(Translate.DoTranslation("Downloading version") + " {0}...", update.UpdateVersion.ToString());

                // Download RetroKS
                var RetroKSURI = update.UpdateURL;
                NetworkTransfer.DownloadFile(RetroKSURI.ToString(), RetroKSPath);

                // Extract it
                TextWriterColor.Write(Translate.DoTranslation("Installing version") + " {0}...", update.UpdateVersion.ToString());
                using var archive = RarArchive.Open(RetroKSPath);
                foreach (var entry in archive.Entries.Where(e => !e.IsDirectory))
                    entry.WriteToDirectory(Paths.RetroKSDownloadPath, new ExtractionOptions()
                    {
                        ExtractFullPath = true,
                        Overwrite = true
                    });
            }

            // Now, reboot the kernel to Retro mode
            TextWriterColor.Write(Translate.DoTranslation("Going back to 2018..."));
            Flags.IsEnteringRetroMode = true;
            Thread.Sleep(1000);
            PowerManager.PowerManage(PowerMode.Reboot);
            return 0;
        }

    }
}
