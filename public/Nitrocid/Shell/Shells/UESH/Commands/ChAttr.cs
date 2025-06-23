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

using System;
using System.IO;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Files;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Security.Permissions;
using Terminaux.Shell.Commands;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Changes attributes of file
    /// </summary>
    /// <remarks>
    /// You can use this command to change attributes of a file.
    /// <br></br>
    /// <list type="table">
    /// <listheader>
    /// <term>Attribute</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>Normal</term>
    /// <description>The file is a normal file</description>
    /// </item>
    /// <item>
    /// <term>ReadOnly</term>
    /// <description>The file is a read-only file</description>
    /// </item>
    /// <item>
    /// <term>Hidden</term>
    /// <description>The file is a hidden file</description>
    /// </item>
    /// <item>
    /// <term>Archive</term>
    /// <description>The file is an archive. Used for backups.</description>
    /// </item>
    /// </list>
    /// <br></br>
    /// </remarks>
    class ChAttrCommand : BaseCommand, ICommand
    {

        // Warning: Don't use parameters.SwitchesList to replace parameters.ArgumentsList(1); the removal signs of ChAttr are treated as switches and will cause unexpected behavior if changed.
        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string NeutralizedFilePath = FilesystemTools.NeutralizePath(parameters.ArgumentsList[0]);
            PermissionsTools.Demand(PermissionTypes.ManageFilesystem);
            if (FilesystemTools.FileExists(NeutralizedFilePath))
            {
                if (parameters.ArgumentsList[2] == "Normal" | parameters.ArgumentsList[2] == "ReadOnly" | parameters.ArgumentsList[2] == "Hidden" | parameters.ArgumentsList[2] == "Archive")
                {
                    if (parameters.ArgumentsList[1] == "add")
                    {
                        FileAttributes Attrib = (FileAttributes)Convert.ToInt32(Enum.Parse(typeof(FileAttributes), parameters.ArgumentsList[2]));
                        if (FilesystemTools.TryAddAttributeToFile(NeutralizedFilePath, Attrib))
                        {
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_CHATTR_ADDSUCCESS") + " {0}", parameters.ArgumentsList[2]);
                            return 0;
                        }
                        else
                        {
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_CHATTR_ADDFAILED") + " {0}", parameters.ArgumentsList[2]);
                            return KernelExceptionTools.GetErrorCode(KernelExceptionType.Filesystem);
                        }
                    }
                    else if (parameters.ArgumentsList[1] == "rem")
                    {
                        FileAttributes Attrib = (FileAttributes)Convert.ToInt32(Enum.Parse(typeof(FileAttributes), parameters.ArgumentsList[2]));
                        if (FilesystemTools.TryRemoveAttributeFromFile(NeutralizedFilePath, Attrib))
                        {
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_CHATTR_REMOVESUCCESS") + " {0}", parameters.ArgumentsList[2]);
                            return 0;
                        }
                        else
                        {
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_CHATTR_REMOVEFAILED") + " {0}", parameters.ArgumentsList[2]);
                            return KernelExceptionTools.GetErrorCode(KernelExceptionType.Filesystem);
                        }
                    }
                }
                else
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_CHATTR_INVALIDATTR"), true, ThemeColorType.Error, parameters.ArgumentsList[2]);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.Filesystem);
                }
            }
            else
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_CHATTR_FILENOTFOUND"), true, ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Filesystem);
            }
            return 0;
        }

        public override void HelpHelper()
        {
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_CHATTR_ATTRIBUTES_LIST"));
            TextWriterColor.Write("- Normal: ", false, ThemeColorType.ListEntry);
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_CHATTR_ATTRIBUTES_NORMAL"), true, ThemeColorType.ListValue);                   // Normal   = 128
            TextWriterColor.Write("- ReadOnly: ", false, ThemeColorType.ListEntry);
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_CHATTR_ATTRIBUTES_READONLY"), true, ThemeColorType.ListValue);              // ReadOnly = 1
            TextWriterColor.Write("- Hidden: ", false, ThemeColorType.ListEntry);
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_CHATTR_ATTRIBUTES_HIDDEN"), true, ThemeColorType.ListValue);                   // Hidden   = 2
            TextWriterColor.Write("- Archive: ", false, ThemeColorType.ListEntry);
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_CHATTR_ATTRIBUTES_ARCHIVE"), true, ThemeColorType.ListValue);  // Archive  = 32
        }

    }
}
