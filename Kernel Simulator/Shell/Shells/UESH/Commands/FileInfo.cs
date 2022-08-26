
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

using System.IO;
using FluentFTP.Helpers;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Files.LineEndings;
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
    /// Shows file information
    /// </summary>
    /// <remarks>
    /// You can use this command to view file information.
    /// </remarks>
    class FileInfoCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            foreach (string FileName in ListArgsOnly)
            {
                string FilePath = Filesystem.NeutralizePath(FileName);
                DebugWriter.Wdbg(DebugLevel.I, "Neutralized file path: {0} ({1})", FilePath, Checking.FileExists(FilePath));
                SeparatorWriterColor.WriteSeparator(FileName, true);
                if (Checking.FileExists(FilePath))
                {
                    var FileInfo = new FileInfo(FilePath);
                    var Style = LineEndingsTools.GetLineEndingFromFile(FilePath);
                    TextWriterColor.Write(Translate.DoTranslation("Name: {0}"), true, ColorTools.ColTypes.Neutral, FileInfo.Name);
                    TextWriterColor.Write(Translate.DoTranslation("Full name: {0}"), true, ColorTools.ColTypes.Neutral, Filesystem.NeutralizePath(FileInfo.FullName));
                    TextWriterColor.Write(Translate.DoTranslation("File size: {0}"), true, ColorTools.ColTypes.Neutral, FileInfo.Length.FileSizeToString());
                    TextWriterColor.Write(Translate.DoTranslation("Creation time: {0}"), true, ColorTools.ColTypes.Neutral, TimeDateRenderers.Render(FileInfo.CreationTime));
                    TextWriterColor.Write(Translate.DoTranslation("Last access time: {0}"), true, ColorTools.ColTypes.Neutral, TimeDateRenderers.Render(FileInfo.LastAccessTime));
                    TextWriterColor.Write(Translate.DoTranslation("Last write time: {0}"), true, ColorTools.ColTypes.Neutral, TimeDateRenderers.Render(FileInfo.LastWriteTime));
                    TextWriterColor.Write(Translate.DoTranslation("Attributes: {0}"), true, ColorTools.ColTypes.Neutral, FileInfo.Attributes);
                    TextWriterColor.Write(Translate.DoTranslation("Where to find: {0}"), true, ColorTools.ColTypes.Neutral, Filesystem.NeutralizePath(FileInfo.DirectoryName));
                    TextWriterColor.Write(Translate.DoTranslation("Newline style:") + " {0}", true, ColorTools.ColTypes.Neutral, Style.ToString());
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("Can't get information about nonexistent file."), true, ColorTools.ColTypes.Error);
                }
            }
        }

    }
}