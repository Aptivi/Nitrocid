
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

using KS.Arguments.ArgumentBase;
using KS.Files.Querying;
using KS.Files;
using System.IO;
using System;
using KS.Files.Folders;
using KS.Kernel;

namespace KS.Arguments.CommandLineArguments
{
    class ResetArgument : ArgumentExecutor, IArgument
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            // Delete every single thing found in KernelPaths
            foreach (string PathName in Enum.GetNames(typeof(KernelPathType)))
            {
                var pathType = (KernelPathType)Enum.Parse(typeof(KernelPathType), PathName);
                string TargetPath = Paths.GetKernelPath(pathType);
                switch (pathType)
                {
                    case KernelPathType.Debugging:
                        TargetPath = TargetPath[..TargetPath.LastIndexOf(".log")] + "*.log";
                        string[] debugs = Listing.GetFilesystemEntries(TargetPath);
                        foreach (string debug in debugs)
                            File.Delete(debug);
                        break;
                    case KernelPathType.Journalling:
                        TargetPath = TargetPath[..TargetPath.LastIndexOf(".json")] + "*.json";
                        string[] journals = Listing.GetFilesystemEntries(TargetPath);
                        foreach (string journal in journals)
                            File.Delete(journal);
                        break;
                    default:
                        if (Checking.FileExists(TargetPath))
                            File.Delete(TargetPath);
                        else if (Checking.FolderExists(TargetPath))
                            Directory.Delete(TargetPath, true);
                        break;
                }
            }

            // Delete every dump file
            string dumpPath = $"{Paths.AppDataPath}/dmp_*.txt";
            string[] dumps = Listing.GetFilesystemEntries(dumpPath);
            foreach (string dump in dumps)
                File.Delete(dump);

            // Exit now.
            Flags.KernelShutdown = true;
        }
    }
}
