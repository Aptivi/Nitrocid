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

using System.IO;
using System.Reflection;
using Terminaux.Shell.Commands;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Files;
using Nitrocid.Misc.Reflection;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Kernel.Time.Renderers;
using Nitrocid.Languages;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Files.Extensions;
using Terminaux.Writer.ConsoleWriters;
using Magico.Files;
using Nitrocid.Kernel.Exceptions;

namespace Nitrocid.Shell.Shells.UESH.Commands
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
                SeparatorWriterColor.WriteSeparatorColor(FileName, KernelColorTools.GetColor(KernelColorType.ListTitle));
                if (FilesystemTools.FileExists(FilePath))
                {
                    var FileInfo = new FileInfo(FilePath);

                    // General info
                    TextWriterColor.Write(Translate.DoTranslation("Name: {0}"), FileInfo.Name);
                    TextWriterColor.Write(Translate.DoTranslation("Full name: {0}"), FilesystemTools.NeutralizePath(FileInfo.FullName));
                    TextWriterColor.Write(Translate.DoTranslation("File size: {0}"), FileInfo.Length.SizeString());
                    TextWriterColor.Write(Translate.DoTranslation("Creation time: {0}"), TimeDateRenderers.Render(FileInfo.CreationTime));
                    TextWriterColor.Write(Translate.DoTranslation("Last access time: {0}"), TimeDateRenderers.Render(FileInfo.LastAccessTime));
                    TextWriterColor.Write(Translate.DoTranslation("Last write time: {0}"), TimeDateRenderers.Render(FileInfo.LastWriteTime));
                    TextWriterColor.Write(Translate.DoTranslation("Attributes: {0}"), FileInfo.Attributes);
                    TextWriterColor.Write(Translate.DoTranslation("Where to find: {0}"), FilesystemTools.NeutralizePath(FileInfo.DirectoryName));
                    TextWriterColor.Write(Translate.DoTranslation("Binary file:") + " {0}", $"{FilesystemTools.IsBinaryFile(FileInfo.FullName)}");
                    TextWriterColor.Write(Translate.DoTranslation("MIME metadata:") + " {0}", MimeTypes.GetMimeType(FileInfo.Extension));
                    TextWriterColor.Write(Translate.DoTranslation("MIME metadata (extended)") + ": {0}", MagicHandler.GetMagicMimeInfo(FileInfo.FullName));
                    TextWriterColor.Write(Translate.DoTranslation("File type") + ": {0}\n", MagicHandler.GetMagicInfo(FileInfo.FullName));
                    if (!FilesystemTools.IsBinaryFile(FileInfo.FullName))
                    {
                        var Style = FilesystemTools.GetLineEndingFromFile(FilePath);
                        TextWriterColor.Write(Translate.DoTranslation("Newline style:") + " {0}", Style.ToString());
                    }
                    TextWriterRaw.Write();

                    // .NET managed info
                    SeparatorWriterColor.WriteSeparatorColor(Translate.DoTranslation(".NET assembly info"), KernelColorTools.GetColor(KernelColorType.ListTitle));
                    if (ReflectionCommon.IsDotnetAssemblyFile(FilePath, out AssemblyName? asmName) && asmName is not null)
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Name: {0}"), asmName.Name ?? "");
                        TextWriterColor.Write(Translate.DoTranslation("Full name") + ": {0}", asmName.FullName);
                        TextWriterColor.Write(Translate.DoTranslation("Version") + ": {0}", asmName.Version?.ToString() ?? "0.0.0.0");
                        TextWriterColor.Write(Translate.DoTranslation("Culture name") + ": {0}", asmName.CultureName ?? "");
                        TextWriterColor.Write(Translate.DoTranslation("Content type") + ": {0}", asmName.ContentType.ToString());
                    }
                    else
                        TextWriterColor.Write(Translate.DoTranslation("File is not a valid .NET assembly."));
                    TextWriterRaw.Write();

                    // Other info handled by the extension handler
                    SeparatorWriterColor.WriteSeparatorColor(Translate.DoTranslation("Extra info"), KernelColorTools.GetColor(KernelColorType.ListTitle));
                    if (ExtensionHandlerTools.IsHandlerRegistered(FileInfo.Extension))
                    {
                        var handler = ExtensionHandlerTools.GetExtensionHandler(FileInfo.Extension) ??
                            throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Handler is registered but somehow failed to get an extension handler for") + $" {FileInfo.Extension}");
                        TextWriterColor.Write(handler.InfoHandler(FilePath));
                    }
                }
                else
                {
                    TextWriters.Write(Translate.DoTranslation("Can't get information about nonexistent file."), true, KernelColorType.Error);
                }
            }
            return 0;
        }

    }
}
