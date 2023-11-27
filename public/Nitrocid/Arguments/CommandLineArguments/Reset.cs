//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System.IO;
using System;
using KS.Files.Folders;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Languages;
using KS.ConsoleBase.Colors;
using KS.Files.Operations.Querying;
using KS.Kernel.Power;
using KS.Files.Paths;
using KS.ConsoleBase.Inputs.Styles.Choice;

namespace KS.Arguments.CommandLineArguments
{
    class ResetArgument : ArgumentExecutor, IArgument
    {

        public override void Execute(ArgumentParameters parameters)
        {
            // Delete every single thing found in KernelPaths
            foreach (string PathName in Enum.GetNames(typeof(KernelPathType)))
            {
                try
                {
                    var pathType = (KernelPathType)Enum.Parse(typeof(KernelPathType), PathName);
                    string TargetPath = PathsManagement.GetKernelPath(pathType);
                    if (!PathsManagement.IsResettable(pathType))
                        continue;
                    switch (pathType)
                    {
                        case KernelPathType.NotificationRecents:
                            TargetPath = TargetPath[..TargetPath.LastIndexOf(".json")] + "*.json";
                            string[] recents = Listing.GetFilesystemEntries(TargetPath);
                            foreach (string recent in recents)
                                File.Delete(recent);
                            break;
                        case KernelPathType.Debugging:
                            TargetPath = TargetPath[..TargetPath.LastIndexOf(".log")] + "*.log";
                            string[] debugs = Listing.GetFilesystemEntries(TargetPath);
                            foreach (string debug in debugs)
                                File.Delete(debug);
                            break;
                        case KernelPathType.Journaling:
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
                catch (Exception ex)
                {
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("Can't wipe file") + $" {PathName}: {ex.Message}", true, KernelColorType.Error);
                }
            }

            // Delete every dump file
            string dumpPath = $"{PathsManagement.AppDataPath}/dmp_*.txt";
            string[] dumps = Listing.GetFilesystemEntries(dumpPath);
            foreach (string dump in dumps)
            {
                try
                {
                    File.Delete(dump);
                }
                catch (Exception ex)
                {
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("Can't wipe dump file") + $" {dump}: {ex.Message}", true, KernelColorType.Error);
                }
            }

            // Inform user that the wipe was not complete if there are files.
            string[] files = Listing.GetFilesystemEntries(PathsManagement.AppDataPath);
            if (files.Length > 0)
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("The following files are not wiped:"), true, KernelColorType.Warning);
                ListWriterColor.WriteList(files);
                string answer = ChoiceStyle.PromptChoice(Translate.DoTranslation("Are you sure to wipe these files?"), "y/n");
                if (answer == "y")
                {
                    foreach (string file in files)
                    {
                        try
                        {
                            File.Delete(file);
                        }
                        catch (Exception ex)
                        {
                            TextWriterColor.WriteKernelColor(Translate.DoTranslation("Can't wipe miscellaneous file") + $" {file}: {ex.Message}", true, KernelColorType.Error);
                        }
                    }
                }
            }

            // Exit now.
            PowerManager.KernelShutdown = true;
            PowerManager.hardShutdown = true;
        }
    }
}
