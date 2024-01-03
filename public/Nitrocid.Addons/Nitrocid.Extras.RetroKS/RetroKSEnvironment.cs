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

using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Newtonsoft.Json.Linq;
using Nitrocid.ConsoleBase.Writers.ConsoleWriters;
using Nitrocid.Files;
using Nitrocid.Files.Operations;
using Nitrocid.Files.Operations.Querying;
using Nitrocid.Files.Paths;
using Nitrocid.Kernel.Updates;
using Nitrocid.Languages;
using Nitrocid.Network.Base.Transfer;
using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;
using Textify.Versioning;

namespace Nitrocid.Extras.RetroKS
{
    internal static class RetroKSEnvironment
    {
        internal static void EntryPoint(string[] args)
        {
            string ExecutableName = "RetroKS.dll";
            TextWriterColor.Write(Translate.DoTranslation("Checking for updates..."));

            // Because api.github.com requires the UserAgent header to be put, else, 403 error occurs. Fortunately for us, "Aptivi" is enough.
            NetworkTransfer.WClient.DefaultRequestHeaders.Add("User-Agent", "Aptivi");

            // Populate the following variables with information
            string RetroKSStr = NetworkTransfer.DownloadString("https://api.github.com/repos/Aptivi/RetroKS/releases");
            var RetroKSToken = JToken.Parse(RetroKSStr);
            var update = new KernelUpdate(RetroKSToken, UpdateKind.Binary);

            // Populate paths
            string RetroKSPath = FilesystemTools.NeutralizePath("retroks.rar", PathsManagement.RetroKSDownloadPath);
            string RetroExecKSPath = FilesystemTools.NeutralizePath(ExecutableName, PathsManagement.RetroKSDownloadPath);

            // Make the directory for RetroKS
            Making.MakeDirectory(PathsManagement.RetroKSDownloadPath, false);

            // Check to see if we already have RetroKS installed and up-to-date
            var currentVersion = Checking.FileExists(RetroExecKSPath) ? AssemblyName.GetAssemblyName(RetroExecKSPath).Version.ToString() : "0.0.0.0";
            if (Checking.FileExists(RetroExecKSPath) && SemVer.ParseWithRev(currentVersion) < update.UpdateVersion || !Checking.FileExists(RetroExecKSPath))
            {
                TextWriterColor.Write(Translate.DoTranslation("Downloading version") + " {0}...", update.UpdateVersion.ToString());

                // Download RetroKS
                var RetroKSURI = update.UpdateURL;
                NetworkTransfer.DownloadFile(RetroKSURI.ToString(), RetroKSPath);

                // Extract it
                TextWriterColor.Write(Translate.DoTranslation("Installing version") + " {0}...", update.UpdateVersion.ToString());
                using var archive = ZipArchive.Open(RetroKSPath);
                foreach (var entry in archive.Entries.Where(e => !e.IsDirectory))
                    entry.WriteToDirectory(PathsManagement.RetroKSDownloadPath, new ExtractionOptions()
                    {
                        ExtractFullPath = true,
                        Overwrite = true
                    });
            }

            // Now, go to Retro mode
            TextWriterColor.Write(Translate.DoTranslation("Going back to 2018..."));
            var retroContext = new RetroKSContext
            {
                resolver = new AssemblyDependencyResolver(RetroExecKSPath)
            };
            var asm = retroContext.LoadFromAssemblyPath(RetroExecKSPath);
            asm.EntryPoint.Invoke("", args);
        }
    }
}
