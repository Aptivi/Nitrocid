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

using Terminaux.Inputs.Interactive;
using Terminaux.Shell.Commands;
using Nitrocid.Files.Instances;
using Nitrocid.Languages;
using System;
using Renci.SshNet.Sftp;
using Nitrocid.Extras.SftpShell.SFTP.Interactive;

namespace Nitrocid.Extras.SftpShell.SFTP.Commands
{
    class IfmCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var tui = new SFTPFileManagerCli();
            tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry, ISftpFile>(Translate.DoTranslation("Open"), ConsoleKey.Enter, (entry1, _, entry2, _) => tui.Open(entry1, entry2)));
            tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry, ISftpFile>(Translate.DoTranslation("Copy"), ConsoleKey.F1, (entry1, _, entry2, _) => tui.CopyFile(entry1, entry2)));
            tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry, ISftpFile>(Translate.DoTranslation("Move"), ConsoleKey.F2, (entry1, _, entry2, _) => tui.MoveFile(entry1, entry2)));
            tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry, ISftpFile>(Translate.DoTranslation("Delete"), ConsoleKey.F3, (entry1, _, entry2, _) => tui.RemoveFileOrDir(entry1, entry2)));
            tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry, ISftpFile>(Translate.DoTranslation("Up"), ConsoleKey.F4, (_, _, _, _) => tui.GoUp()));
            tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry, ISftpFile>(Translate.DoTranslation("Info"), ConsoleKey.F5, (entry1, _, entry2, _) => tui.PrintFileSystemEntry(entry1, entry2)));
            tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry, ISftpFile>(Translate.DoTranslation("Copy To"), ConsoleKey.F1, ConsoleModifiers.Shift, (entry1, _, entry2, _) => tui.CopyTo(entry1, entry2)));
            tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry, ISftpFile>(Translate.DoTranslation("Move to"), ConsoleKey.F2, ConsoleModifiers.Shift, (entry1, _, entry2, _) => tui.MoveTo(entry1, entry2)));
            tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry, ISftpFile>(Translate.DoTranslation("New Folder"), ConsoleKey.F7, (_, _, _, _) => tui.MakeDir()));
            InteractiveTuiTools.OpenInteractiveTui(tui);
            return 0;
        }
    }
}
