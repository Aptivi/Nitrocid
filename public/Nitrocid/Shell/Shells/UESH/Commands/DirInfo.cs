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

using System.IO;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Files;
using Nitrocid.Misc.Reflection;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Kernel.Time.Renderers;
using Nitrocid.Languages;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Shows directory information
    /// </summary>
    /// <remarks>
    /// You can use this command to view directory information.
    /// </remarks>
    class DirInfoCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            foreach (string Dir in parameters.ArgumentsList)
            {
                string DirectoryPath = FilesystemTools.NeutralizePath(Dir);
                DebugWriter.WriteDebug(DebugLevel.I, "Neutralized directory path: {0} ({1})", vars: [DirectoryPath, FilesystemTools.FolderExists(DirectoryPath)]);
                SeparatorWriterColor.WriteSeparatorColor(Dir, KernelColorTools.GetColor(KernelColorType.ListTitle));
                if (FilesystemTools.FolderExists(DirectoryPath))
                {
                    var DirInfo = new DirectoryInfo(DirectoryPath);
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_ENTRYNAME"), DirInfo.Name);
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_FULLNAME"), FilesystemTools.NeutralizePath(DirInfo.FullName));
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_ENTRYSIZE"), FilesystemTools.GetAllSizesInFolder(DirInfo).SizeString());
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_CREATIONTIME"), TimeDateRenderers.Render(DirInfo.CreationTime));
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_LASTACCESSTIME"), TimeDateRenderers.Render(DirInfo.LastAccessTime));
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_LASTWRITETIME"), TimeDateRenderers.Render(DirInfo.LastWriteTime));
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_ATTRIBUTES"), DirInfo.Attributes);
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_PARENTDIRECTORY"), FilesystemTools.NeutralizePath(DirInfo.Parent?.FullName));
                }
                else
                {
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_DIRINFO_DIRNOTFOUND"), true, KernelColorType.Error);
                }
            }
            return 0;
        }

    }
}
