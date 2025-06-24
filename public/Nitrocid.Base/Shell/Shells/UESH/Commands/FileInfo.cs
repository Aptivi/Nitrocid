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
using System.Reflection;
using Terminaux.Shell.Commands;
using Nitrocid.Base.Files;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Colors.Themes.Colors;
using Magico.Files;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Reflection;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Time.Renderers;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Files.Extensions;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Shows file information
    /// </summary>
    /// <remarks>
    /// You can use this command to view file information.
    /// </remarks>
    class FileInfoCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            foreach (string FileName in parameters.ArgumentsList)
            {
                string FilePath = FilesystemTools.NeutralizePath(FileName);
                DebugWriter.WriteDebug(DebugLevel.I, "Neutralized file path: {0} ({1})", vars: [FilePath, FilesystemTools.FileExists(FilePath)]);
                SeparatorWriterColor.WriteSeparatorColor(FileName, ThemeColorsTools.GetColor(ThemeColorType.ListTitle));
                if (FilesystemTools.FileExists(FilePath))
                {
                    var FileInfo = new FileInfo(FilePath);

                    // General info
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_ENTRYNAME"), FileInfo.Name);
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_FULLNAME"), FilesystemTools.NeutralizePath(FileInfo.FullName));
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_FILESIZE"), FileInfo.Length.SizeString());
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_CREATIONTIME"), TimeDateRenderers.Render(FileInfo.CreationTime));
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_LASTACCESSTIME"), TimeDateRenderers.Render(FileInfo.LastAccessTime));
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_LASTWRITETIME"), TimeDateRenderers.Render(FileInfo.LastWriteTime));
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_ATTRIBUTES"), FileInfo.Attributes);
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_WHERETOFIND"), FilesystemTools.NeutralizePath(FileInfo.DirectoryName));
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_BINARYFILE") + " {0}", $"{FilesystemTools.IsBinaryFile(FileInfo.FullName)}");
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_MIMEMETADATA") + " {0}", MimeTypes.GetMimeType(FileInfo.Extension));
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_MIMEMETADATAEXT") + ": {0}", MagicHandler.GetMagicMimeInfo(FileInfo.FullName));
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_FILETYPE") + ": {0}\n", MagicHandler.GetMagicInfo(FileInfo.FullName));
                    if (!FilesystemTools.IsBinaryFile(FileInfo.FullName))
                    {
                        var Style = FilesystemTools.GetLineEndingFromFile(FilePath);
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_NEWLINESTYLE") + " {0}", Style.ToString());
                    }
                    TextWriterRaw.Write();

                    // .NET managed info
                    SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_FILEINFO_ASMINFO"), ThemeColorsTools.GetColor(ThemeColorType.ListTitle));
                    if (ReflectionCommon.IsDotnetAssemblyFile(FilePath, out AssemblyName? asmName) && asmName is not null)
                    {
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_ENTRYNAME"), asmName.Name ?? "");
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_FULLNAME") + ": {0}", asmName.FullName);
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_VERSION") + ": {0}", asmName.Version?.ToString() ?? "0.0.0.0");
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_CULTURENAME") + ": {0}", asmName.CultureName ?? "");
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_CONTENTTYPE") + ": {0}", asmName.ContentType.ToString());
                    }
                    else
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_FILEINFO_NOTDOTNETASM"));
                    TextWriterRaw.Write();

                    // Other info handled by the extension handler
                    SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_FILEINFO_EXTRAINFO"), ThemeColorsTools.GetColor(ThemeColorType.ListTitle));
                    if (ExtensionHandlerTools.IsHandlerRegistered(FileInfo.Extension))
                    {
                        var handler = ExtensionHandlerTools.GetExtensionHandler(FileInfo.Extension) ??
                            throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_EXCEPTION_HANDLERFAILED") + $" {FileInfo.Extension}");
                        TextWriterColor.Write(handler.InfoHandler(FilePath));
                    }
                }
                else
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_FILEINFO_FILENOTFOUND"), true, ThemeColorType.Error);
                }
            }
            return 0;
        }

    }
}
