//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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
using Nitrocid.Files;
using Nitrocid.Files.Paths;
using Nitrocid.Misc.Interactives;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Files.Instances;
using Nitrocid.Languages;
using System;
using Nitrocid.ConsoleBase.Colors;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Interactive system host file manager
    /// </summary>
    /// <remarks>
    /// If you are planning to take a look at your filesystem in an interactive manner, use this command.
    /// </remarks>
    class IfmCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var tui = new FileManagerCli
            {
                firstPanePath = parameters.ArgumentsList.Length > 0 ? FilesystemTools.NeutralizePath(parameters.ArgumentsList[0]) : PathsManagement.HomePath,
                secondPanePath = parameters.ArgumentsList.Length > 1 ? FilesystemTools.NeutralizePath(parameters.ArgumentsList[1]) : PathsManagement.HomePath,
            };
            tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(Translate.DoTranslation("Open"), ConsoleKey.Enter, (entry1, _, entry2, _) => tui.Open(entry1, entry2)));
            tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(Translate.DoTranslation("Copy"), ConsoleKey.F1, (entry1, _, entry2, _) => tui.CopyFileOrDir(entry1, entry2)));
            tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(Translate.DoTranslation("Move"), ConsoleKey.F2, (entry1, _, entry2, _) => tui.MoveFileOrDir(entry1, entry2)));
            tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(Translate.DoTranslation("Delete"), ConsoleKey.F3, (entry1, _, entry2, _) => tui.RemoveFileOrDir(entry1, entry2)));
            tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(Translate.DoTranslation("Up"), ConsoleKey.F4, (_, _, _, _) => tui.GoUp()));
            tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(Translate.DoTranslation("Info"), ConsoleKey.F5, (entry1, _, entry2, _) => tui.PrintFileSystemEntry(entry1, entry2)));
            tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(Translate.DoTranslation("Go To"), ConsoleKey.F6, (_, _, _, _) => tui.GoTo()));
            tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(Translate.DoTranslation("Copy To"), ConsoleKey.F1, ConsoleModifiers.Shift, (entry1, _, entry2, _) => tui.CopyTo(entry1, entry2)));
            tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(Translate.DoTranslation("Move to"), ConsoleKey.F2, ConsoleModifiers.Shift, (entry1, _, entry2, _) => tui.MoveTo(entry1, entry2)));
            tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(Translate.DoTranslation("Rename"), ConsoleKey.F9, (entry1, _, entry2, _) => tui.Rename(entry1, entry2)));
            tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(Translate.DoTranslation("New Folder"), ConsoleKey.F10, (_, _, _, _) => tui.MakeDir()));
            tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(Translate.DoTranslation("Hash"), ConsoleKey.F11, (entry1, _, entry2, _) => tui.Hash(entry1, entry2)));
            tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(Translate.DoTranslation("Verify"), ConsoleKey.F12, (entry1, _, entry2, _) => tui.Verify(entry1, entry2)));
            tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(Translate.DoTranslation("Preview"), ConsoleKey.P, (entry1, _, entry2, _) => tui.Preview(entry1, entry2)));
            InteractiveTuiTools.OpenInteractiveTui(tui);
            return 0;
        }
    }
}
