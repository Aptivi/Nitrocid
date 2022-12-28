
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

using System.IO;
using FluentFTP.Helpers;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Files.Querying;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;
using KS.Shell.ShellBase.Commands;
using KS.TimeDate;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Shows directory information
    /// </summary>
    /// <remarks>
    /// You can use this command to view directory information.
    /// </remarks>
    class DirInfoCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            foreach (string Dir in ListArgsOnly)
            {
                string DirectoryPath = Filesystem.NeutralizePath(Dir);
                DebugWriter.WriteDebug(DebugLevel.I, "Neutralized directory path: {0} ({1})", DirectoryPath, Checking.FolderExists(DirectoryPath));
                SeparatorWriterColor.WriteSeparator(Dir, true);
                if (Checking.FolderExists(DirectoryPath))
                {
                    var DirInfo = new DirectoryInfo(DirectoryPath);
                    TextWriterColor.Write(Translate.DoTranslation("Name: {0}"), DirInfo.Name);
                    TextWriterColor.Write(Translate.DoTranslation("Full name: {0}"), Filesystem.NeutralizePath(DirInfo.FullName));
                    TextWriterColor.Write(Translate.DoTranslation("Size: {0}"), SizeGetter.GetAllSizesInFolder(DirInfo).FileSizeToString());
                    TextWriterColor.Write(Translate.DoTranslation("Creation time: {0}"), TimeDateRenderers.Render(DirInfo.CreationTime));
                    TextWriterColor.Write(Translate.DoTranslation("Last access time: {0}"), TimeDateRenderers.Render(DirInfo.LastAccessTime));
                    TextWriterColor.Write(Translate.DoTranslation("Last write time: {0}"), TimeDateRenderers.Render(DirInfo.LastWriteTime));
                    TextWriterColor.Write(Translate.DoTranslation("Attributes: {0}"), DirInfo.Attributes);
                    TextWriterColor.Write(Translate.DoTranslation("Parent directory: {0}"), Filesystem.NeutralizePath(DirInfo.Parent.FullName));
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("Can't get information about nonexistent directory."), true, KernelColorType.Error);
                }
            }
        }

    }
}