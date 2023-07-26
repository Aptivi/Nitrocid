
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
using System.IO;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Files;
using KS.Files.Folders;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// You can change your current working directory to another directory
    /// </summary>
    /// <remarks>
    /// You can change your working directory to another directory to execute listing commands, removing files, or creating directories on another directory.
    /// </remarks>
    class ChDirCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            try
            {
                CurrentDirectory.SetCurrDir(ListArgsOnly[0]);
            }
            catch (System.Security.SecurityException sex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Security error: {0} ({1})", sex.Message, sex.PermissionType);
                TextWriterColor.Write(Translate.DoTranslation("You are unauthorized to set current directory to {0}: {1}"), true, KernelColorType.Error, ListArgsOnly[0], sex.Message);
                DebugWriter.WriteDebugStackTrace(sex);
            }
            catch (PathTooLongException ptlex)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Directory length: {0}", Filesystem.NeutralizePath(ListArgsOnly[0]).Length);
                TextWriterColor.Write(Translate.DoTranslation("The path you've specified is too long."), true, KernelColorType.Error);
                DebugWriter.WriteDebugStackTrace(ptlex);
            }
            catch (Exception ex)
            {
                TextWriterColor.Write(Translate.DoTranslation("Changing directory has failed: {0}"), true, KernelColorType.Error, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }

    }
}
