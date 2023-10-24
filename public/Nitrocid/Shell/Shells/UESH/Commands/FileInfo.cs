
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

using System.IO;
using System.Reflection;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Files;
using KS.Files.Extensions;
using KS.Files.LineEndings;
using KS.Files.Operations.Querying;
using KS.Kernel.Debugging;
using KS.Kernel.Time.Renderers;
using KS.Languages;
using KS.Misc.Reflection;
using KS.Shell.ShellBase.Commands;
using MimeKit;

namespace KS.Shell.Shells.UESH.Commands
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
                DebugWriter.WriteDebug(DebugLevel.I, "Neutralized file path: {0} ({1})", FilePath, Checking.FileExists(FilePath));
                SeparatorWriterColor.WriteSeparator(FileName, true);
                if (Checking.FileExists(FilePath))
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
                    TextWriterColor.Write(Translate.DoTranslation("Binary file:") + " {0}", $"{Parsing.IsBinaryFile(FileInfo.FullName)}");
                    TextWriterColor.Write(Translate.DoTranslation("MIME metadata:") + " {0}", MimeTypes.GetMimeType(FilesystemTools.NeutralizePath(FileInfo.FullName)));
                    if (!Parsing.IsBinaryFile(FileInfo.FullName))
                    {
                        var Style = LineEndingsTools.GetLineEndingFromFile(FilePath);
                        TextWriterColor.Write(Translate.DoTranslation("Newline style:") + " {0}", Style.ToString());
                    }
                    TextWriterColor.Write();

                    // .NET managed info
                    SeparatorWriterColor.WriteSeparator(Translate.DoTranslation(".NET assembly info"), true);
                    if (ReflectionCommon.IsDotnetAssemblyFile(FilePath, out AssemblyName asmName))
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Name: {0}"), asmName.Name);
                        TextWriterColor.Write(Translate.DoTranslation("Full name") + ": {0}", asmName.FullName);
                        TextWriterColor.Write(Translate.DoTranslation("Version") + ": {0}", asmName.Version.ToString());
                        TextWriterColor.Write(Translate.DoTranslation("Culture name") + ": {0}", asmName.CultureName);
                        TextWriterColor.Write(Translate.DoTranslation("Content type") + ": {0}", asmName.ContentType.ToString());
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("File is not a valid .NET assembly."));
                    }
                    TextWriterColor.Write();

                    // Other info handled by the extension handler
                    SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Extra info"), true);
                    if (ExtensionHandlerTools.IsHandlerRegistered(FileInfo.Extension))
                    {
                        var handler = ExtensionHandlerTools.GetFirstExtensionHandler(FileInfo.Extension);
                        TextWriterColor.Write(handler.InfoHandler(FilePath));
                    }
                }
                else
                {
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("Can't get information about nonexistent file."), true, KernelColorType.Error);
                }
            }
            return 0;
        }

    }
}
