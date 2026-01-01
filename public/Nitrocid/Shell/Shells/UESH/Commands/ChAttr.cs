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

using System;
using System.IO;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Files;
using Nitrocid.Files.Attributes;
using Nitrocid.Files.Operations.Querying;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Security.Permissions;
using Nitrocid.Shell.ShellBase.Commands;

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
            if (Checking.FileExists(NeutralizedFilePath))
            {
                if (parameters.ArgumentsList[2] == "Normal" | parameters.ArgumentsList[2] == "ReadOnly" | parameters.ArgumentsList[2] == "Hidden" | parameters.ArgumentsList[2] == "Archive")
                {
                    if (parameters.ArgumentsList[1] == "add")
                    {
                        FileAttributes Attrib = (FileAttributes)Convert.ToInt32(Enum.Parse(typeof(FileAttributes), parameters.ArgumentsList[2]));
                        if (AttributeManager.TryAddAttributeToFile(NeutralizedFilePath, Attrib))
                        {
                            TextWriterColor.Write(Translate.DoTranslation("Attribute has been added successfully.") + " {0}", parameters.ArgumentsList[2]);
                            return 0;
                        }
                        else
                        {
                            TextWriterColor.Write(Translate.DoTranslation("Failed to add attribute.") + " {0}", parameters.ArgumentsList[2]);
                            return KernelExceptionTools.GetErrorCode(KernelExceptionType.Filesystem);
                        }
                    }
                    else if (parameters.ArgumentsList[1] == "rem")
                    {
                        FileAttributes Attrib = (FileAttributes)Convert.ToInt32(Enum.Parse(typeof(FileAttributes), parameters.ArgumentsList[2]));
                        if (AttributeManager.TryRemoveAttributeFromFile(NeutralizedFilePath, Attrib))
                        {
                            TextWriterColor.Write(Translate.DoTranslation("Attribute has been removed successfully.") + " {0}", parameters.ArgumentsList[2]);
                            return 0;
                        }
                        else
                        {
                            TextWriterColor.Write(Translate.DoTranslation("Failed to remove attribute.") + " {0}", parameters.ArgumentsList[2]);
                            return KernelExceptionTools.GetErrorCode(KernelExceptionType.Filesystem);
                        }
                    }
                }
                else
                {
                    TextWriters.Write(Translate.DoTranslation("Attribute \"{0}\" is invalid."), true, KernelColorType.Error, parameters.ArgumentsList[2]);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.Filesystem);
                }
            }
            else
            {
                TextWriters.Write(Translate.DoTranslation("File not found."), true, KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Filesystem);
            }
            return 0;
        }

        public override void HelpHelper()
        {
            TextWriterColor.Write(Translate.DoTranslation("where <attributes> is one of the following:"));
            TextWriters.Write("- Normal: ", false, KernelColorType.ListEntry);
            TextWriters.Write(Translate.DoTranslation("The file is a normal file"), true, KernelColorType.ListValue);                   // Normal   = 128
            TextWriters.Write("- ReadOnly: ", false, KernelColorType.ListEntry);
            TextWriters.Write(Translate.DoTranslation("The file is a read-only file"), true, KernelColorType.ListValue);              // ReadOnly = 1
            TextWriters.Write("- Hidden: ", false, KernelColorType.ListEntry);
            TextWriters.Write(Translate.DoTranslation("The file is a hidden file"), true, KernelColorType.ListValue);                   // Hidden   = 2
            TextWriters.Write("- Archive: ", false, KernelColorType.ListEntry);
            TextWriters.Write(Translate.DoTranslation("The file is an archive. Used for backups."), true, KernelColorType.ListValue);  // Archive  = 32
        }

    }
}
