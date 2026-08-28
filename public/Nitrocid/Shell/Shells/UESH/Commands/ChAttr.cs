//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.IO;
using Nitrocid.Files;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Security.Permissions;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

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
        public override string Command =>
            "chattr";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHATTR_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "file", new()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHATTR_ARGUMENT_FILE_DESC"
                    }),
                    new CommandArgumentPart(true, "add/rem", new()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHATTR_ARGUMENT_ADDREMOVE_DESC"
                    }),
                    new CommandArgumentPart(true, "Normal/ReadOnly/Hidden/Archive", new()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHATTR_ARGUMENT_NAME_DESC"
                    }),
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string target = FilesystemTools.NeutralizePath(parameters.ArgumentsList[0]);
            string mode = parameters.ArgumentsList[1];
            string attribute = parameters.ArgumentsList[2];
            PermissionsTools.Demand(PermissionTypes.ManageFilesystem);
            if (FilesystemTools.FileExists(target))
            {
                if (attribute == "Normal" || attribute == "ReadOnly" || attribute == "Hidden" || attribute == "Archive")
                {
                    FileAttributes attrib = Enum.Parse<FileAttributes>(attribute);
                    if (mode == "add")
                    {
                        if (FilesystemTools.TryAddAttributeToFile(target, attrib))
                        {
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_CHATTR_ADDSUCCESS") + " {0}", attribute);
                            return 0;
                        }
                        else
                        {
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_CHATTR_ADDFAILED") + " {0}", attribute);
                            return KernelExceptionTools.GetErrorCode(KernelExceptionType.Filesystem);
                        }
                    }
                    else if (mode == "rem")
                    {
                        if (FilesystemTools.TryRemoveAttributeFromFile(target, attrib))
                        {
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_CHATTR_REMOVESUCCESS") + " {0}", attribute);
                            return 0;
                        }
                        else
                        {
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_CHATTR_REMOVEFAILED") + " {0}", attribute);
                            return KernelExceptionTools.GetErrorCode(KernelExceptionType.Filesystem);
                        }
                    }
                }
                else
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_CHATTR_INVALIDATTR"), true, ThemeColorType.Error, attribute);
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

        public override void HelpHelper(IShell? shell)
        {
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_CHATTR_ATTRIBUTES_LIST"));
            ListEntryWriterColor.WriteListEntry(nameof(FileAttributes.Normal), LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_CHATTR_ATTRIBUTES_NORMAL"));
            ListEntryWriterColor.WriteListEntry(nameof(FileAttributes.ReadOnly), LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_CHATTR_ATTRIBUTES_READONLY"));
            ListEntryWriterColor.WriteListEntry(nameof(FileAttributes.Hidden), LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_CHATTR_ATTRIBUTES_HIDDEN"));
            ListEntryWriterColor.WriteListEntry(nameof(FileAttributes.Archive), LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_CHATTR_ATTRIBUTES_ARCHIVE"));
        }

    }
}
