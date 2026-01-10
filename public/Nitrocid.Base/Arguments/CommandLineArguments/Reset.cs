//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Inputs.Styles.Choice;
using Terminaux.Shell.Arguments.Base;
using Terminaux.Themes.Colors;
using Nitrocid.Base.Files;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Files.Paths;
using Nitrocid.Base.Kernel.Power;

namespace Nitrocid.Base.Arguments.CommandLineArguments
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
                            string[] recents = FilesystemTools.GetFilesystemEntries(TargetPath);
                            foreach (string recent in recents)
                                File.Delete(recent);
                            break;
                        case KernelPathType.Journaling:
                            TargetPath = TargetPath[..TargetPath.LastIndexOf(".json")] + "*.json";
                            string[] journals = FilesystemTools.GetFilesystemEntries(TargetPath);
                            foreach (string journal in journals)
                                File.Delete(journal);
                            break;
                        default:
                            if (FilesystemTools.FileExists(TargetPath))
                                File.Delete(TargetPath);
                            else if (FilesystemTools.FolderExists(TargetPath))
                                Directory.Delete(TargetPath, true);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_ARGUMENTS_RESET_WIPEFAILED") + $" {PathName}: {ex.Message}", true, ThemeColorType.Error);
                }
            }

            // Delete every dump file
            string dumpPath = $"{PathsManagement.AppDataPath}/dmp_*.txt";
            string[] dumps = FilesystemTools.GetFilesystemEntries(dumpPath);
            foreach (string dump in dumps)
            {
                try
                {
                    File.Delete(dump);
                }
                catch (Exception ex)
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_ARGUMENTS_RESET_WIPEDUMPFAILED") + $" {dump}: {ex.Message}", true, ThemeColorType.Error);
                }
            }

            // Wipe debug logs
            try
            {
                DebugWriter.RemoveDebugLogs();
            }
            catch (Exception ex)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_ARGUMENTS_RESET_WIPEDEBUGFAILED") + $": {ex.Message}", true, ThemeColorType.Error);
            }

            // Inform user that the wipe was not complete if there are files.
            string[] files = FilesystemTools.GetFilesystemEntries(PathsManagement.AppDataPath);
            if (files.Length > 0)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_ARGUMENTS_RESET_LEFTOVERSLISTING"), true, ThemeColorType.Warning);
                ListWriterColor.WriteList(files);
                string answer = ChoiceStyle.PromptChoice(LanguageTools.GetLocalized("NKS_ARGUMENTS_RESET_LEFTOVERSPROMPT"), [("y", "Yes"), ("n", "No")]);
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
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_ARGUMENTS_RESET_WIPEMISCFAILED") + $" {file}: {ex.Message}", true, ThemeColorType.Error);
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
