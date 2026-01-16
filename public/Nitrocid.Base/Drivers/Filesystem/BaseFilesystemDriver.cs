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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nitrocid.Base.Files;
using Nitrocid.Base.Files.Folders;
using Nitrocid.Base.Files.Instances;
using Nitrocid.Base.Files.LineEndings;
using Nitrocid.Base.Files.Paths;
using Nitrocid.Base.Kernel;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Events;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Progress;
using Nitrocid.Base.Misc.Reflection;
using Nitrocid.Base.Misc.Text.Probers.Regexp;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;
using Textify.General.Comparers;
using FS = Nitrocid.Base.Files.FilesystemTools;
using IOPath = System.IO.Path;

namespace Nitrocid.Base.Drivers.Filesystem
{
    /// <summary>
    /// Base Filesystem driver
    /// </summary>
    public abstract class BaseFilesystemDriver : IFilesystemDriver
    {
        /// <inheritdoc/>
        public virtual string DriverName => "Default";

        /// <inheritdoc/>
        public virtual DriverTypes DriverType => DriverTypes.Filesystem;

        /// <inheritdoc/>
        public virtual bool DriverInternal => false;

        /// <inheritdoc/>
        public virtual void AddAttributeToFile(string FilePath, FileAttributes Attributes)
        {
            FilePath = FS.NeutralizePath(FilePath);
            DebugWriter.WriteDebug(DebugLevel.I, "Setting file attribute to {0}...", vars: [Attributes]);
            File.SetAttributes(FilePath, Attributes);

            // Raise event
            EventsManager.FireEvent(EventType.FileAttributeAdded, FilePath, Attributes);
        }

        /// <inheritdoc/>
        public virtual void AddToPathLookup(string Path)
        {
            var LookupPaths = GetPathList();
            Path = FS.NeutralizePath(Path);
            LookupPaths.Add(Path);
            Config.MainConfig.PathsToLookup = string.Join(PathLookupTools.PathLookupDelimiter, LookupPaths);
        }

        /// <inheritdoc/>
        public virtual void AddToPathLookup(string Path, string RootPath)
        {
            var LookupPaths = GetPathList();
            Path = FS.NeutralizePath(Path, RootPath);
            LookupPaths.Add(Path);
            Config.MainConfig.PathsToLookup = string.Join(PathLookupTools.PathLookupDelimiter, LookupPaths);
        }

        /// <inheritdoc/>
        public virtual void ClearFile(string Path)
        {
            FileStream clearer = new(Path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            clearer.SetLength(0);
            clearer.Close();
        }

        /// <inheritdoc/>
        public virtual byte[] CombineBinaryFiles(string Input, string[] TargetInputs)
        {
            try
            {
                var CombinedContents = new List<byte>();

                // Add the input contents
                    if (!FS.IsBinaryFile(Input))
                    throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_COMBINETEXT") + " " + nameof(CombineTextFiles) + "(" + Input + ")");
                CombinedContents.AddRange(FS.ReadAllBytes(Input));

                // Enumerate the target inputs
                for (int i = 0; i < TargetInputs.Length; i++)
                {
                    string TargetInput = TargetInputs[i];
                            if (!FS.IsBinaryFile(TargetInput))
                        throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_COMBINETEXT") + " " + nameof(CombineTextFiles) + "(" + TargetInput + ")");
                    ProgressManager.ReportProgress((i + 1) / TargetInputs.Length, nameof(CombineBinaryFiles), $"{Input} + {TargetInput}");
                    CombinedContents.AddRange(FS.ReadAllBytes(TargetInput));
                }

                // Return the combined contents
                return [.. CombinedContents];
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to combine files: {0}", vars: [ex.Message]);
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_COMBINEFAILED"), ex);
            }
        }

        /// <inheritdoc/>
        public virtual string[] CombineTextFiles(string Input, string[] TargetInputs)
        {
            try
            {
                var CombinedContents = new List<string>();

                // Add the input contents
                    if (FS.IsBinaryFile(Input))
                    throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_COMBINEBINARY") + " " + nameof(CombineBinaryFiles) + "(" + Input + ")");
                CombinedContents.AddRange(FS.ReadContents(Input));

                // Enumerate the target inputs
                for (int i = 0; i < TargetInputs.Length; i++)
                {
                    string TargetInput = TargetInputs[i];
                            if (FS.IsBinaryFile(TargetInput))
                        throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_COMBINEBINARY") + " " + nameof(CombineBinaryFiles) + "(" + TargetInput + ")");
                    ProgressManager.ReportProgress((i + 1) / TargetInputs.Length, nameof(CombineTextFiles), $"{Input} + {TargetInput}");
                    CombinedContents.AddRange(FS.ReadContents(TargetInput));
                }

                // Return the combined contents
                return [.. CombinedContents];
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to combine files: {0}", vars: [ex.Message]);
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_COMBINEFAILED"), ex);
            }
        }

        /// <inheritdoc/>
        public virtual void ConvertLineEndings(string TextFile) =>
            ConvertLineEndings(TextFile, FS.NewlineStyle);

        /// <inheritdoc/>
        public virtual void ConvertLineEndings(string TextFile, FilesystemNewlineStyle LineEndingStyle)
        {
            TextFile = FS.NeutralizePath(TextFile);
            if (!FS.FileExists(TextFile))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_FILENOTFOUND2"), TextFile);

            // Get all the file lines, regardless of the new line style on the target file
            var FileContents = FS.ReadAllLinesNoBlock(TextFile);
            DebugWriter.WriteDebug(DebugLevel.I, "Got {0} lines. Converting newlines in {1} to {2}...", vars: [FileContents.Length, TextFile, LineEndingStyle.ToString()]);

            // Get the newline string according to the current style
            string NewLineString = FS.GetLineEndingString(LineEndingStyle);

            // Convert the newlines now
            var Result = new StringBuilder();
            for (int i = 0; i < FileContents.Length; i++)
            {
                string FileContent = FileContents[i];
                ProgressManager.ReportProgress((i + 1) / FileContents.Length, nameof(ConvertLineEndings), FileContent);
                Result.Append(FileContent + NewLineString);
            }

            // Save the changes
            FS.WriteContentsText(TextFile, Result.ToString());
        }

        /// <inheritdoc/>
        public virtual void CopyDirectory(string Source, string Destination) =>
            CopyDirectory(Source, Destination, Config.MainConfig.ShowFilesystemProgress);

        /// <inheritdoc/>
        public virtual void CopyDirectory(string Source, string Destination, bool ShowProgress)
        {
            if (!FS.FolderExists(Source))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_DIRECTORYNOTFOUND1"), Source);

            // Get all source directories and files
            var SourceDirInfo = new DirectoryInfo(Source);
            var SourceDirectories = SourceDirInfo.GetDirectories();
            DebugWriter.WriteDebug(DebugLevel.I, "Source directories: {0}", vars: [SourceDirectories.Length]);
            var SourceFiles = SourceDirInfo.GetFiles();
            DebugWriter.WriteDebug(DebugLevel.I, "Source files: {0}", vars: [SourceFiles.Length]);

            // Make a destination directory if it doesn't exist
            if (!FS.FolderExists(Destination))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Destination directory {0} doesn't exist. Creating...", vars: [Destination]);
                Directory.CreateDirectory(Destination);
            }

            // Iterate through every file and copy them to destination
            for (int i = 0; i < SourceFiles.Length; i++)
            {
                FileInfo SourceFile = SourceFiles[i];
                string DestinationFilePath = IOPath.Combine(Destination, SourceFile.Name);
                DebugWriter.WriteDebug(DebugLevel.I, "Copying file {0} to destination...", vars: [DestinationFilePath]);
                if (ShowProgress)
                    ProgressManager.RegisterProgressHandler(new(new((perc, message) => TextWriterColor.Write($"{perc}% - {message}")), SourceFile.FullName));
                ProgressManager.ReportProgress((i + 1) / SourceFiles.Length, SourceFile.FullName, $"-> {DestinationFilePath}");
                SourceFile.CopyTo(DestinationFilePath, true);
            }

            // Iterate through every subdirectory and copy them to destination
            for (int i = 0; i < SourceDirectories.Length; i++)
            {
                DirectoryInfo SourceDirectory = SourceDirectories[i];
                string DestinationDirectoryPath = IOPath.Combine(Destination, SourceDirectory.Name);
                DebugWriter.WriteDebug(DebugLevel.I, "Calling CopyDirectory() with destination {0}...", vars: [DestinationDirectoryPath]);
                if (ShowProgress)
                    ProgressManager.RegisterProgressHandler(new(new((perc, message) => TextWriterColor.Write($"{perc}% - {message}")), SourceDirectory.FullName));
                ProgressManager.ReportProgress((i + 1) / SourceDirectories.Length, SourceDirectory.FullName, $"*  {DestinationDirectoryPath}");
                CopyDirectory(SourceDirectory.FullName, DestinationDirectoryPath);
            }
        }

        /// <inheritdoc/>
        public virtual void CopyFile(string Source, string Destination)
        {
            Source = FS.NeutralizePath(Source);
            DebugWriter.WriteDebug(DebugLevel.I, "Source directory: {0}", vars: [Source]);
            Destination = FS.NeutralizePath(Destination);
            DebugWriter.WriteDebug(DebugLevel.I, "Target directory: {0}", vars: [Destination]);
            string FileName = IOPath.GetFileName(Source);
            DebugWriter.WriteDebug(DebugLevel.I, "Source file name: {0}", vars: [FileName]);
            if (FS.FileExists(Source) & FS.FolderExists(Destination))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Source is a file and destination is a directory");
                File.Copy(Source, Destination + "/" + FileName, true);

                // Raise event
                EventsManager.FireEvent(EventType.FileCopied, Source, Destination + "/" + FileName);
            }
            else if (FS.FileExists(Source))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Source is a file and destination is a file");
                File.Copy(Source, Destination, true);

                // Raise event
                EventsManager.FireEvent(EventType.FileCopied, Source, Destination);
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Source or destination are invalid.");
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_SOURCENOTEXISTS"));
            }
        }

        /// <inheritdoc/>
        public virtual void CopyFileOrDir(string Source, string Destination)
        {
            Source = FS.NeutralizePath(Source);
            DebugWriter.WriteDebug(DebugLevel.I, "Source directory: {0}", vars: [Source]);
            Destination = FS.NeutralizePath(Destination);
            DebugWriter.WriteDebug(DebugLevel.I, "Target directory: {0}", vars: [Destination]);
            string FileName = IOPath.GetFileName(Source);
            DebugWriter.WriteDebug(DebugLevel.I, "Source file name: {0}", vars: [FileName]);
            if (FS.FolderExists(Source))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Source and destination are directories");
                CopyDirectory(Source, Destination);

                // Raise event
                EventsManager.FireEvent(EventType.DirectoryCopied, Source, Destination);
            }
            else if (FS.FileExists(Source) & FS.FolderExists(Destination))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Source is a file and destination is a directory");
                File.Copy(Source, Destination + "/" + FileName, true);

                // Raise event
                EventsManager.FireEvent(EventType.FileCopied, Source, Destination + "/" + FileName);
            }
            else if (FS.FileExists(Source))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Source is a file and destination is a file");
                File.Copy(Source, Destination, true);

                // Raise event
                EventsManager.FireEvent(EventType.FileCopied, Source, Destination);
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Source or destination are invalid.");
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_NOTFILEORDIR"));
            }
        }

        /// <inheritdoc/>
        public virtual List<FileSystemEntry> CreateList(string folder, bool Sorted = false, bool Recursive = false)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Folder {0} will be listed...", vars: [folder]);
            var FilesystemEntries = new List<FileSystemEntry>();

            // List files and folders
            folder = FS.NeutralizePath(folder);
            if (FS.FolderExists(folder) | folder.ContainsAnyOf(["?", "*"]))
            {
                IEnumerable<string> enumeration;
                try
                {
                    enumeration = GetFilesystemEntries(folder, false, Recursive);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to make a list of filesystem entries for directory {0}: {1}", vars: [folder, ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_LISTCREATEFAILED") + " {0}", ex, folder);
                }
                foreach (string Entry in enumeration)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Enumerating {0}...", vars: [Entry]);
                    try
                    {
                        var createdEntry = new FileSystemEntry(Entry);
                        DebugWriter.WriteDebug(DebugLevel.I, "Entry is a {0}. Adding {1} to list...", vars: [createdEntry.Type.ToString(), Entry]);
                        FilesystemEntries.Add(createdEntry);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, "Failed to enumerate {0} for directory {1}: {2}", vars: [Entry, folder, ex.Message]);
                        DebugWriter.WriteDebugStackTrace(ex);
                    }
                }
            }

            // Return the resulting list immediately if not sorted. Otherwise, sort it.
            if (Sorted & !(FilesystemEntries.Count == 0))
            {
                // We define the max string length for the largest size. This is to overcome the limitation of sorting when it comes to numbers.
                int MaxLength = FilesystemEntries
                    .Max((fse) => fse.FileSize.GetDigits());

                // Select whether or not to sort descending.
                switch (Config.MainConfig.SortDirection)
                {
                    case (int)FilesystemSortDirection.Ascending:
                        {
                            FilesystemEntries = [.. FilesystemEntries.OrderBy(x => SortSelector(x, MaxLength), Config.MainConfig.SortLogically ? new LogicalComparer() : StringComparer.OrdinalIgnoreCase)];
                            break;
                        }
                    case (int)FilesystemSortDirection.Descending:
                        {
                            FilesystemEntries = [.. FilesystemEntries.OrderByDescending(x => SortSelector(x, MaxLength), Config.MainConfig.SortLogically ? new LogicalComparer() : StringComparer.OrdinalIgnoreCase)];
                            break;
                        }
                }
            }

            // We would most likely need to put the folders first, then the files.
            var listFolders = new List<FileSystemEntry>();
            var listFiles = new List<FileSystemEntry>();
            foreach (var entry in FilesystemEntries)
            {
                switch (entry.Type)
                {
                    case FileSystemEntryType.Directory:
                        listFolders.Add(entry);
                        break;
                    case FileSystemEntryType.File:
                        listFiles.Add(entry);
                        break;
                }
            }
            return listFolders.Union(listFiles).ToList();
        }

        /// <inheritdoc/>
        public virtual void DisplayInHex(long StartByte, long EndByte, byte[] FileByte, bool colors = true) =>
            DisplayInHex(0, false, StartByte, EndByte, FileByte, colors);

        /// <inheritdoc/>
        public virtual void DisplayInHex(byte ByteContent, bool HighlightResults, long StartByte, long EndByte, byte[] FileByte, bool colors = true)
        {
            // Now, do the job!
            DebugWriter.WriteDebug(DebugLevel.I, "File Bytes: {0}", vars: [FileByte.LongLength]);
            StartByte.SwapIfSourceLarger(ref EndByte);
            if (StartByte < 1)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_BYTENUMISZERO"));
                return;
            }
            if (StartByte <= FileByte.LongLength && EndByte <= FileByte.LongLength)
            {
                string rendered = RenderContentsInHex(ByteContent, HighlightResults, StartByte, EndByte, FileByte, colors);
                TextWriterRaw.WriteRaw(rendered);
            }
            else if (StartByte > FileByte.LongLength)
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_STARTBYTENUMTOOLARGE"), true, ThemeColorType.Error);
            else if (EndByte > FileByte.LongLength)
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_ENDBYTENUMTOOLARGE"), true, ThemeColorType.Error);
        }

        /// <inheritdoc/>
        public virtual string RenderContentsInHex(long StartByte, long EndByte, byte[] FileByte, bool colors = true) =>
            RenderContentsInHex(0, false, StartByte, EndByte, FileByte, colors);

        /// <inheritdoc/>
        public virtual string RenderContentsInHex(byte ByteContent, bool HighlightResults, long StartByte, long EndByte, byte[] FileByte, bool colors = true)
        {
            // Get the un-highlighted and highlighted colors
            var entryColor = ThemeColorsTools.GetColor(ThemeColorType.ListEntry);
            var unhighlightedColor = ThemeColorsTools.GetColor(ThemeColorType.ListValue);
            var highlightedColor = HighlightResults ? ThemeColorsTools.GetColor(ThemeColorType.Success) : unhighlightedColor;

            // Now, do the job!
            DebugWriter.WriteDebug(DebugLevel.I, "File Bytes: {0}", vars: [FileByte.LongLength]);
            StartByte.SwapIfSourceLarger(ref EndByte);
            if (StartByte < 1)
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_BYTENUMISZERO"));

            if (StartByte <= FileByte.LongLength && EndByte <= FileByte.LongLength)
            {
                // We need to know how to write the bytes and their contents in this shape:
                // -> 0x00000010  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
                //    0x00000020  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
                //    0x00000030  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
                // ... and so on.
                var builder = new StringBuilder();
                for (long CurrentByteNumber = StartByte; CurrentByteNumber <= EndByte; CurrentByteNumber += 16)
                {
                    if (colors)
                        builder.Append(entryColor.VTSequenceForeground());
                    builder.Append($"0x{CurrentByteNumber - 1L:X8} ");

                    // Iterate these number of bytes for the ASCII codes
                    long byteNum;
                    for (byteNum = 0; byteNum < 16 && CurrentByteNumber + byteNum <= EndByte; byteNum++)
                    {
                        byte CurrentByte = FileByte[(int)(CurrentByteNumber + byteNum - 1)];
                        var byteColor = ByteContent == CurrentByte ? highlightedColor : unhighlightedColor;
                        DebugWriter.WriteDebug(DebugLevel.I, "Byte: {0}", vars: [CurrentByte]);
                        if (colors)
                            builder.Append(byteColor.VTSequenceForeground());
                        builder.Append($"{CurrentByte:X2} ");
                    }

                    // Pad the remaining ASCII byte display
                    int remaining = (int)(16 - byteNum);
                    int padTimes = remaining * 3;
                    string padded = new(' ', padTimes);
                    builder.Append(padded);

                    // Iterate these number of bytes for the actual rendered characters
                    for (byteNum = 0; byteNum < 16 && CurrentByteNumber + byteNum <= EndByte; byteNum++)
                    {
                        byte CurrentByte = FileByte[(int)(CurrentByteNumber + byteNum - 1)];
                        DebugWriter.WriteDebug(DebugLevel.I, "Byte: {0}", vars: [CurrentByte]);
                        char ProjectedByteChar = Convert.ToChar(CurrentByte);
                        DebugWriter.WriteDebug(DebugLevel.I, "Projected byte char: {0}", vars: [ProjectedByteChar]);
                        char RenderedByteChar = '.';
                        if (!char.IsWhiteSpace(ProjectedByteChar) & !char.IsControl(ProjectedByteChar) & !char.IsHighSurrogate(ProjectedByteChar) & !char.IsLowSurrogate(ProjectedByteChar) & ProjectedByteChar != (char)0xAD)
                        {
                            // The renderer will actually render the character, not as a dot.
                            DebugWriter.WriteDebug(DebugLevel.I, "Char is not a whitespace.");
                            RenderedByteChar = ProjectedByteChar;
                        }
                        DebugWriter.WriteDebug(DebugLevel.I, "Rendered byte char: {0}", vars: [ProjectedByteChar]);
                        var byteColor = ByteContent == CurrentByte ? highlightedColor : unhighlightedColor;
                        if (colors)
                            builder.Append(byteColor.VTSequenceForeground());
                        builder.Append($"{RenderedByteChar}");
                    }
                    builder.AppendLine();
                }
                return builder.ToString();
            }
            else if (StartByte > FileByte.LongLength)
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_STARTBYTENUMTOOLARGE"));
            else if (EndByte > FileByte.LongLength)
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_ENDBYTENUMTOOLARGE"));
            else
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_BYTENUMISINVALID"));
        }

        /// <inheritdoc/>
        public virtual string RenderContentsInHex(long ByteHighlight, long StartByte, long EndByte, byte[] FileByte, bool colors = true)
        {
            // Get the un-highlighted and highlighted colors
            var entryColor = ThemeColorsTools.GetColor(ThemeColorType.ListEntry);
            var unhighlightedColorBackground = ThemeColorsTools.GetColor(ThemeColorType.Background);
            var highlightedColorBackground = ThemeColorsTools.GetColor(ThemeColorType.Success);

            // Now, do the job!
            DebugWriter.WriteDebug(DebugLevel.I, "File Bytes: {0}", vars: [FileByte.LongLength]);
            StartByte.SwapIfSourceLarger(ref EndByte);
            if (StartByte < 1)
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_BYTENUMISZERO"));

            if (StartByte <= FileByte.LongLength && EndByte <= FileByte.LongLength)
            {
                // We need to know how to write the bytes and their contents in this shape:
                // -> 0x00000010  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
                //    0x00000020  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
                //    0x00000030  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
                // ... and so on.
                var builder = new StringBuilder();
                for (long CurrentByteNumber = StartByte; CurrentByteNumber <= EndByte; CurrentByteNumber += 16)
                {
                    if (colors)
                    {
                        builder.Append(ConsoleColoring.RenderSetConsoleColor(unhighlightedColorBackground, true));
                        builder.Append(entryColor.VTSequenceForeground());
                    }
                    builder.Append($"0x{CurrentByteNumber - 1L:X8} ");

                    // Iterate these number of bytes for the ASCII codes
                    long byteNum;
                    for (byteNum = 0; byteNum < 16 && CurrentByteNumber + byteNum <= EndByte; byteNum++)
                    {
                        byte CurrentByte = FileByte[(int)(CurrentByteNumber + byteNum - 1)];
                        var byteColorFg = CurrentByteNumber + byteNum == ByteHighlight ? unhighlightedColorBackground : highlightedColorBackground;
                        var byteColorBg = CurrentByteNumber + byteNum == ByteHighlight ? highlightedColorBackground : unhighlightedColorBackground;
                        DebugWriter.WriteDebug(DebugLevel.I, "Byte: {0}", vars: [CurrentByte]);
                        if (colors)
                        {
                            builder.Append(byteColorFg.VTSequenceForeground());
                            builder.Append(ConsoleColoring.RenderSetConsoleColor(byteColorBg, true));
                        }
                        builder.Append($"{CurrentByte:X2}");
                        if (colors)
                        {
                            builder.Append(highlightedColorBackground.VTSequenceForeground());
                            builder.Append(ConsoleColoring.RenderSetConsoleColor(unhighlightedColorBackground, true));
                        }
                        builder.Append(' ');
                    }

                    // Pad the remaining ASCII byte display
                    int remaining = (int)(16 - byteNum);
                    int padTimes = remaining * 3;
                    string padded = new(' ', padTimes);
                    builder.Append(padded);

                    // Iterate these number of bytes for the actual rendered characters
                    for (byteNum = 0; byteNum < 16 && CurrentByteNumber + byteNum <= EndByte; byteNum++)
                    {
                        byte CurrentByte = FileByte[(int)(CurrentByteNumber + byteNum - 1)];
                        DebugWriter.WriteDebug(DebugLevel.I, "Byte: {0}", vars: [CurrentByte]);
                        char ProjectedByteChar = Convert.ToChar(CurrentByte);
                        DebugWriter.WriteDebug(DebugLevel.I, "Projected byte char: {0}", vars: [ProjectedByteChar]);
                        char RenderedByteChar = '.';
                        if (!char.IsWhiteSpace(ProjectedByteChar) & !char.IsControl(ProjectedByteChar) & !char.IsHighSurrogate(ProjectedByteChar) & !char.IsLowSurrogate(ProjectedByteChar))
                        {
                            // The renderer will actually render the character, not as a dot.
                            DebugWriter.WriteDebug(DebugLevel.I, "Char is not a whitespace.");
                            RenderedByteChar = ProjectedByteChar;
                        }
                        DebugWriter.WriteDebug(DebugLevel.I, "Rendered byte char: {0}", vars: [ProjectedByteChar]);
                        var byteColorFg = CurrentByteNumber + byteNum == ByteHighlight ? unhighlightedColorBackground : highlightedColorBackground;
                        var byteColorBg = CurrentByteNumber + byteNum == ByteHighlight ? highlightedColorBackground : unhighlightedColorBackground;
                        if (colors)
                        {
                            builder.Append(byteColorFg.VTSequenceForeground());
                            builder.Append(ConsoleColoring.RenderSetConsoleColor(byteColorBg, true));
                        }
                        builder.Append($"{RenderedByteChar}");
                    }
                    builder.AppendLine();
                }
                return builder.ToString();
            }
            else if (StartByte > FileByte.LongLength)
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_STARTBYTENUMTOOLARGE"));
            else if (EndByte > FileByte.LongLength)
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_ENDBYTENUMTOOLARGE"));
            else
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_BYTENUMISINVALID"));
        }

        /// <inheritdoc/>
        public virtual bool FileExists(string File, bool Neutralize = false)
        {
            if (Neutralize)
                File = FS.NeutralizePath(File);
            bool exists = System.IO.File.Exists(File);
            DebugWriter.WriteDebug(DebugLevel.I, $"Checked {File} for existence: {exists}");
            return exists;
        }

        /// <inheritdoc/>
        public virtual bool FileExistsInPath(string FilePath, ref string? Result)
        {
            var LookupPaths = GetPathList();
            string ResultingPath;
            foreach (string LookupPath in LookupPaths)
            {
                ResultingPath = FS.NeutralizePath(FilePath, LookupPath);
                if (FS.FileExists(ResultingPath))
                {
                    Result = ResultingPath;
                    return true;
                }
            }
            return false;
        }

        /// <inheritdoc/>
        public virtual bool FolderExists(string Folder, bool Neutralize = false)
        {
            if (Neutralize)
                Folder = FS.NeutralizePath(Folder);
            bool exists = Directory.Exists(Folder);
            DebugWriter.WriteDebug(DebugLevel.I, $"Checked {Folder} for existence: {exists}");
            return exists;
        }

        /// <inheritdoc/>
        public virtual bool Exists(string Path, bool Neutralize = false) =>
            FileExists(Path) || FolderExists(Path);

        /// <inheritdoc/>
        public virtual bool Rooted(string Path) =>
            IOPath.IsPathRooted(Path);

        /// <inheritdoc/>
        public virtual long GetAllSizesInFolder(DirectoryInfo? DirectoryInfo) =>
            GetAllSizesInFolder(DirectoryInfo, Config.MainConfig.FullParseMode);

        /// <inheritdoc/>
        public virtual long GetAllSizesInFolder(DirectoryInfo? DirectoryInfo, bool FullParseMode)
        {
            // Check for null
            if (DirectoryInfo is null)
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_DIRINFONEEDED"));

            // Determine parse mode
            List<FileInfo> Files;
            if (FullParseMode)
                Files = [.. DirectoryInfo.GetFiles("*", SearchOption.AllDirectories)];
            else
                Files = [.. DirectoryInfo.GetFiles("*", SearchOption.TopDirectoryOnly)];
            DebugWriter.WriteDebug(DebugLevel.I, "{0} files to be parsed", vars: [Files.Count]);

            // Get all sizes in bytes
            long TotalSize = 0L;
            for (int i = 0; i < Files.Count; i++)
            {
                FileInfo DFile = Files[i];
                ProgressManager.ReportProgress((i + 1) / Files.Count, nameof(GetAllSizesInFolder), $". {DFile.FullName}");
                if (DFile.Attributes == FileAttributes.Hidden & Config.MainConfig.HiddenFiles | !DFile.Attributes.HasFlag(FileAttributes.Hidden))
                {
                    ProgressManager.ReportProgress((i + 1) / Files.Count, nameof(GetAllSizesInFolder) + "Found", $"+ {DFile.FullName} [{DFile.Length.SizeString()}]");
                    DebugWriter.WriteDebug(DebugLevel.I, "File {0}, Size {1} bytes", vars: [DFile.Name, DFile.Length]);
                    TotalSize += DFile.Length;
                }
            }
            return TotalSize;
        }

        /// <inheritdoc/>
        public virtual string[] GetFilesystemEntries(string Path, bool IsFile = false, bool Recursive = false)
        {
            var Entries = Array.Empty<string>();
            try
            {
                Path = FS.NeutralizePath(Path);
                if (IsFile)
                    return [Path];

                // Check to see if we're calling from the root path
                string Pattern = "*";
                string pathRoot = FS.NeutralizePath(IOPath.GetPathRoot(Path));
                if (pathRoot == Path)
                {
                    Entries = GetFilesystemEntries(Path, Pattern, Recursive);
                    return Entries;
                }

                // Select the pattern index
                int SelectedPatternIndex = 0;
                var SplitPath = Path.Split('/').Skip(1).ToArray();
                var SplitParent = new List<string>() { Path.Split('/')[0] };
                for (int PatternIndex = 0; PatternIndex <= SplitPath.Length - 1; PatternIndex++)
                {
                    // We used to use System.IO.Path.GetInvalidFileNameChars(), but this is under the assumption that the caller
                    // is calling this function under Windows. Linux doesn't consider the wildcard characters, '*' and '?', to be
                    // illegal file name characters, so we have to add an extra check to ensure that such wildcard characters get
                    // the same treatment in Linux as in Windows.
                    if (SplitPath[PatternIndex].ContainsAnyOf(GetInvalidFileChars().Select(Character => Character.ToString()).ToArray()))
                    {
                        SelectedPatternIndex = PatternIndex;
                        break;
                    }
                    SplitParent.Add(SplitPath[PatternIndex]);
                }

                // Split the path and the pattern
                string? Parent = FS.NeutralizePath(IOPath.GetDirectoryName(Path) + "/" + IOPath.GetFileName(Path));
                if (Parent.ContainsAnyOf(FS.GetInvalidPathChars().Select(Character => Character.ToString()).ToArray()))
                {
                    Parent = IOPath.GetDirectoryName(Path);
                    Pattern = IOPath.GetFileName(Path);
                }
                if (SelectedPatternIndex != 0)
                {
                    Parent = string.Join("/", SplitParent);
                    Pattern = string.Join("/", SplitPath.Skip(SelectedPatternIndex));
                }

                // Split the path and the pattern and return the final result
                Entries = GetFilesystemEntries(Parent, Pattern, Recursive);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to combine files: {0}", vars: [ex.Message]);
            }
            return Entries;
        }

        /// <inheritdoc/>
        public virtual string[] GetFilesystemEntries(string? Parent, string Pattern, bool Recursive = false)
        {
            var Entries = Array.Empty<string>();
            try
            {
                Parent = FS.NeutralizePath(Parent);

                // Get the entries
                if (Directory.Exists(Parent))
                {
                    EnumerationOptions options = new()
                    {
                        RecurseSubdirectories = Recursive,
                        AttributesToSkip = Config.MainConfig.HiddenFiles ? FileAttributes.System : FileAttributes.Hidden | FileAttributes.System
                    };
                    Entries = Directory.GetFileSystemEntries(Parent, Pattern, options);
                    DebugWriter.WriteDebug(DebugLevel.I, "Enumerated {0} entries from parent {1} using pattern {2}", vars: [Entries.Length, Parent, Pattern]);
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to combine files: {0}", vars: [ex.Message]);
            }
            return Entries.Select((path) => FS.NeutralizePath(path)).ToArray();
        }

        /// <inheritdoc/>
        public string[] GetFilesystemEntriesRegex(string Parent, string Pattern, bool Recursive = false)
        {
            // Ensure that the regex is valid
            if (!RegexpTools.IsValidRegex(Pattern))
                throw new KernelException(KernelExceptionType.RegularExpression, LanguageTools.GetLocalized("NKS_DRIVERS_REGEXP_BASE_EXCEPTION_INVALID"));

            // Get the entries and match them against the given pattern
            var AllFileEntries = FS.GetFilesystemEntries(Parent, "*", Recursive);
            List<string> entryNames = [];
            foreach (var FileEntry in AllFileEntries)
            {
                // Match the file entry
                var FileEntryMatches = RegexpTools.Matches(FileEntry, Pattern);
                if (FileEntryMatches.Count == 0)
                    // No match.
                    continue;
                entryNames.Add(FileEntry);
            }
            return [.. entryNames];
        }

        /// <inheritdoc/>
        public virtual char[] GetInvalidPathChars()
        {
            var FinalInvalidPathChars = IOPath.GetInvalidPathChars();
            var WindowsInvalidPathChars = new[] { '"', '<', '>' };
            if (KernelPlatform.IsOnWindows())
            {
                // It's weird of .NET 6.0 to not consider the above three Windows invalid directory chars to be invalid,
                // so make them invalid as in .NET Framework.
                Array.Resize(ref FinalInvalidPathChars, 36);
                WindowsInvalidPathChars.CopyTo(FinalInvalidPathChars, FinalInvalidPathChars.Length - 3);
            }
            return FinalInvalidPathChars;
        }

        /// <inheritdoc/>
        public virtual char[] GetInvalidFileChars()
        {
            var FinalInvalidFileChars = IOPath.GetInvalidFileNameChars();
            var WindowsInvalidFileChars = new[] { '*', '?' };
            if (KernelPlatform.IsOnUnix())
            {
                // If running on Linux, ensure that the wildcard characters, '*' and '?', is not valid.
                Array.Resize(ref FinalInvalidFileChars, 4);
                WindowsInvalidFileChars.CopyTo(FinalInvalidFileChars, FinalInvalidFileChars.Length - 2);
            }
            return FinalInvalidFileChars;
        }

        /// <inheritdoc/>
        public virtual FilesystemNewlineStyle GetLineEndingFromFile(string TextFile)
        {
            TextFile = FS.NeutralizePath(TextFile);
            if (!FS.FileExists(TextFile))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_FILENOTFOUND2"), TextFile);

            // Open the file stream
            var NewlineStyle = FS.NewlineStyle;
            var TextFileStream = new FileStream(TextFile, FileMode.Open, FileAccess.Read);
            int CarriageReturnCode = Convert.ToChar(FS.GetLineEndingString(FilesystemNewlineStyle.CR));
            int LineFeedCode = Convert.ToChar(FS.GetLineEndingString(FilesystemNewlineStyle.LF));
            var CarriageReturnSpotted = false;
            var LineFeedSpotted = false;
            var ExitOnSpotted = false;

            // Search for new line style
            while (TextFileStream.Position != TextFileStream.Length)
            {
                int Result = TextFileStream.ReadByte();
                if (Result == LineFeedCode)
                {
                    LineFeedSpotted = true;
                    ExitOnSpotted = true;
                }
                if (Result == CarriageReturnCode)
                {
                    CarriageReturnSpotted = true;
                    ExitOnSpotted = true;
                }
                if (ExitOnSpotted & Result != LineFeedCode & Result != CarriageReturnCode)
                    break;
            }
            TextFileStream.Close();

            // Return the style used
            if (LineFeedSpotted & CarriageReturnSpotted)
            {
                NewlineStyle = FilesystemNewlineStyle.CRLF;
            }
            else if (LineFeedSpotted)
            {
                NewlineStyle = FilesystemNewlineStyle.LF;
            }
            else if (CarriageReturnSpotted)
            {
                NewlineStyle = FilesystemNewlineStyle.CR;
            }
            return NewlineStyle;
        }

        /// <inheritdoc/>
        public virtual string GetNumberedFileName(string? path, string fileName)
        {
            path = FS.NeutralizePath(path);
            string fileNameWithoutExtension = IOPath.GetFileNameWithoutExtension(fileName);
            string fileNameExtension = IOPath.GetExtension(fileName);
            int fileNumber = 0;

            // Check if the target file exists
            bool fileNameValid = false;
            do
            {
                string fileNameGenerated = FS.NeutralizePath($"{fileNameWithoutExtension}-{fileNumber}{fileNameExtension}", path);
                if (!FS.FileExists(fileNameGenerated))
                    return fileNameGenerated;
                fileNumber++;
            }
            while (!fileNameValid);
            return $"{fileNameWithoutExtension}-{fileNumber}{fileNameExtension}";
        }

        /// <inheritdoc/>
        public virtual List<string> GetPathList() =>
            [.. Config.MainConfig.PathsToLookup.Split(Convert.ToChar(PathLookupTools.PathLookupDelimiter))];

        /// <inheritdoc/>
        public string GetRandomFileName() =>
            FS.NeutralizePath(IOPath.GetRandomFileName(), PathsManagement.TempPath);

        /// <inheritdoc/>
        public string GetRandomFolderName() =>
            FS.NeutralizePath(IOPath.GetRandomFileName() + "/", PathsManagement.TempPath);

        /// <inheritdoc/>
        public virtual bool IsBinaryFile(string Path)
        {
            // Neutralize path
            Path = FS.NeutralizePath(Path);

            // Check to see if the file contains these control characters
            using StreamReader reader = new(Path);
            int ch;
            while ((ch = reader.Read()) != -1)
            {
                // Parse character
                if (CharManager.IsControlChar((char)ch))
                    // Our file is binary!
                    return true;
            }

            // Our file is not binary. Return false.
            return false;
        }

        /// <inheritdoc/>
        public virtual bool IsJson(string Path)
        {
            try
            {
                // Neutralize path
                    Path = FS.NeutralizePath(Path);

                // Try to parse the content as JSON object
                try
                {
                    var ParsedObject = JObject.Parse(FS.ReadContentsText(Path));
                    return true;
                }
                catch
                {
                    var ParsedObject = JArray.Parse(FS.ReadContentsText(Path));
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public virtual bool IsSql(string Path)
        {
            try
            {
                // Neutralize path
                Path = FS.NeutralizePath(Path);

                // Try to open an SQL connection
                byte[] sqlFileBytes = new byte[17];
                using (FileStream sqlStream = new(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    sqlStream.ReadExactly(sqlFileBytes, 0, 16);
                string result = System.Text.Encoding.ASCII.GetString(sqlFileBytes);
                return result.Contains("SQLite format");
            }
            catch
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public virtual void MakeDirectory(string NewDirectory, bool ThrowIfDirectoryExists = true)
        {
            NewDirectory = FS.NeutralizePath(NewDirectory);
            DebugWriter.WriteDebug(DebugLevel.I, "New directory: {0} ({1})", vars: [NewDirectory, FS.FolderExists(NewDirectory)]);
            if (!FS.FolderExists(NewDirectory))
            {
                Directory.CreateDirectory(NewDirectory);

                // Raise event
                EventsManager.FireEvent(EventType.DirectoryCreated, NewDirectory);
            }
            else if (ThrowIfDirectoryExists)
            {
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_DIRECTORYEXISTS"), NewDirectory);
            }
        }

        /// <inheritdoc/>
        public virtual void MakeFile(string NewFile, bool ThrowIfFileExists = true)
        {
            NewFile = FS.NeutralizePath(NewFile);
            DebugWriter.WriteDebug(DebugLevel.I, "File path is {0} and .Exists is {1}", vars: [NewFile, FS.FileExists(NewFile)]);
            if (!FS.FileExists(NewFile))
            {
                try
                {
                    var NewFileStream = File.Create(NewFile);
                    DebugWriter.WriteDebug(DebugLevel.I, "File created");
                    NewFileStream.Close();
                    DebugWriter.WriteDebug(DebugLevel.I, "File closed");

                    // Raise event
                    EventsManager.FireEvent(EventType.FileCreated, NewFile);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_FILECREATEFAILED"), ex.Message);
                }
            }
            else if (ThrowIfFileExists)
            {
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_FILEEXISTS"));
            }
        }

        /// <inheritdoc/>
        public virtual void MakeJsonFile(string NewFile, bool ThrowIfFileExists = true, bool useArray = false)
        {
            NewFile = FS.NeutralizePath(NewFile);
            DebugWriter.WriteDebug(DebugLevel.I, "File path is {0} and .Exists is {1}", vars: [NewFile, FS.FileExists(NewFile)]);
            if (!FS.FileExists(NewFile))
            {
                try
                {
                    var NewFileStream = File.Create(NewFile);
                    DebugWriter.WriteDebug(DebugLevel.I, "File created");
                    object NewJsonObject = useArray ? JArray.Parse("[]") : JObject.Parse("{}");
                    var NewFileWriter = new StreamWriter(NewFileStream);
                    NewFileWriter.WriteLine(JsonConvert.SerializeObject(NewJsonObject));
                    NewFileWriter.Flush();
                    NewFileStream.Close();
                    DebugWriter.WriteDebug(DebugLevel.I, "File closed");

                    // Raise event
                    EventsManager.FireEvent(EventType.FileCreated, NewFile);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_FILECREATEFAILED"), ex.Message);
                }
            }
            else if (ThrowIfFileExists)
            {
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_FILEEXISTS"));
            }
        }

        /// <inheritdoc/>
        public virtual void MakeSymlink(string linkName, string target)
        {
                        // Neutralize the paths
            linkName = FS.NeutralizePath(linkName);
            target = FS.NeutralizePath(target);

            // Check for path existence
            if (!FS.Exists(target))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_TARGETNOTFOUND"));
            if (FS.Exists(linkName))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_OVERWRITEWITHSYMLINK"));

            // Now, make a symlink
            File.CreateSymbolicLink(linkName, target);
        }

        /// <inheritdoc/>
        public virtual void MoveDirectory(string Source, string Destination) =>
            MoveDirectory(Source, Destination, Config.MainConfig.ShowFilesystemProgress);

        /// <inheritdoc/>
        public virtual void MoveDirectory(string Source, string Destination, bool ShowProgress)
        {
            if (!FS.FolderExists(Source))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_DIRECTORYNOTFOUND1"), Source);

            // Get all source directories and files
            var SourceDirInfo = new DirectoryInfo(Source);
            var SourceDirectories = SourceDirInfo.GetDirectories();
            DebugWriter.WriteDebug(DebugLevel.I, "Source directories: {0}", vars: [SourceDirectories.Length]);
            var SourceFiles = SourceDirInfo.GetFiles();
            DebugWriter.WriteDebug(DebugLevel.I, "Source files: {0}", vars: [SourceFiles.Length]);

            // Make a destination directory if it doesn't exist
            if (!FS.FolderExists(Destination))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Destination directory {0} doesn't exist. Creating...", vars: [Destination]);
                Directory.CreateDirectory(Destination);
            }

            // Iterate through every file and copy them to destination
            for (int i = 0; i < SourceFiles.Length; i++)
            {
                FileInfo SourceFile = SourceFiles[i];
                string DestinationFilePath = IOPath.Combine(Destination, SourceFile.Name);
                DebugWriter.WriteDebug(DebugLevel.I, "Moving file {0} to destination...", vars: [DestinationFilePath]);
                if (ShowProgress)
                    ProgressManager.RegisterProgressHandler(new(new((perc, message) => TextWriterColor.Write($"{perc}% - {message}")), SourceFile.FullName));
                ProgressManager.ReportProgress((i + 1) / SourceFiles.Length, SourceFile.FullName, $"-> {DestinationFilePath}");
                SourceFile.MoveTo(DestinationFilePath);
            }

            // Iterate through every subdirectory and copy them to destination
            for (int i = 0; i < SourceDirectories.Length; i++)
            {
                DirectoryInfo SourceDirectory = SourceDirectories[i];
                string DestinationDirectoryPath = IOPath.Combine(Destination, SourceDirectory.Name);
                DebugWriter.WriteDebug(DebugLevel.I, "Calling MoveDirectory() with destination {0}...", vars: [DestinationDirectoryPath]);
                if (ShowProgress)
                    ProgressManager.RegisterProgressHandler(new(new((perc, message) => TextWriterColor.Write($"{perc}% - {message}")), SourceDirectory.FullName));
                ProgressManager.ReportProgress((i + 1) / SourceDirectories.Length, SourceDirectory.FullName, $"*  {DestinationDirectoryPath}");
                MoveDirectory(SourceDirectory.FullName, DestinationDirectoryPath);

                // Source subdirectories are removed after moving
                FS.RemoveDirectory(SourceDirectory.FullName);
            }
        }

        /// <inheritdoc/>
        public virtual void MoveFile(string Source, string Destination)
        {
            Source = FS.NeutralizePath(Source);
            DebugWriter.WriteDebug(DebugLevel.I, "Source directory: {0}", vars: [Source]);
            Destination = FS.NeutralizePath(Destination);
            DebugWriter.WriteDebug(DebugLevel.I, "Target directory: {0}", vars: [Destination]);
            string FileName = IOPath.GetFileName(Source);
            DebugWriter.WriteDebug(DebugLevel.I, "Source file name: {0}", vars: [FileName]);
            if (FS.FileExists(Source) & FS.FolderExists(Destination))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Source is a file and destination is a directory");
                File.Move(Source, Destination + "/" + FileName);

                // Raise event
                EventsManager.FireEvent(EventType.FileMoved, Source, Destination + "/" + FileName);
            }
            else if (FS.FileExists(Source))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Source is a file and destination is a file");
                File.Move(Source, Destination);

                // Raise event
                EventsManager.FireEvent(EventType.FileMoved, Source, Destination);
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Source or destination are invalid.");
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_SOURCENOTEXISTS"));
            }
        }

        /// <inheritdoc/>
        public virtual void MoveFileOrDir(string Source, string Destination)
        {
            Source = FS.NeutralizePath(Source);
            DebugWriter.WriteDebug(DebugLevel.I, "Source directory: {0}", vars: [Source]);
            Destination = FS.NeutralizePath(Destination);
            DebugWriter.WriteDebug(DebugLevel.I, "Target directory: {0}", vars: [Destination]);
            string FileName = IOPath.GetFileName(Source);
            DebugWriter.WriteDebug(DebugLevel.I, "Source file name: {0}", vars: [FileName]);
            if (FS.FolderExists(Source))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Source and destination are directories");
                MoveDirectory(Source, Destination);

                // Raise event
                EventsManager.FireEvent(EventType.DirectoryMoved, Source, Destination);
            }
            else if (FS.FileExists(Source) & FS.FolderExists(Destination))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Source is a file and destination is a directory");
                File.Move(Source, Destination + "/" + FileName);

                // Raise event
                EventsManager.FireEvent(EventType.FileMoved, Source, Destination + "/" + FileName);
            }
            else if (FS.FileExists(Source))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Source is a file and destination is a file");
                File.Move(Source, Destination);

                // Raise event
                EventsManager.FireEvent(EventType.FileMoved, Source, Destination);
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Source or destination are invalid.");
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_NOTFILEORDIR"));
            }
        }

        /// <inheritdoc/>
        public virtual string RenderContents(string filename, bool colors = true) =>
            RenderContents(filename, Config.MainConfig.PrintLineNumbers, colors);

        /// <inheritdoc/>
        public virtual string RenderContents(string filename, bool PrintLineNumbers, bool ForcePlain = false, bool colors = true)
        {
            // Some variables
            var builder = new StringBuilder();

            // Check the path
            filename = FS.NeutralizePath(filename);

            // If interacting with the binary file, display it in hex. Otherwise, display it as if it is text except if forced to view binaries as texts.
            // Read the contents
            string[] array = FS.GetFilesystemEntries(filename, true);
            for (int i = 0; i < array.Length; i++)
            {
                string FilePath = array[i];
                var entryColor = ThemeColorsTools.GetColor(ThemeColorType.ListEntry);
                var valueColor =
                    PrintLineNumbers ?
                    ThemeColorsTools.GetColor(ThemeColorType.ListValue) :
                    ThemeColorsTools.GetColor(ThemeColorType.NeutralText);

                if (array.Length > 1)
                    builder.AppendLine(FilePath);

                // Determine the file type
                if (FS.IsBinaryFile(FilePath) && !ForcePlain)
                {
                    byte[] bytes = ReadAllBytes(FilePath);
                    builder.Append(RenderContentsInHex(1, bytes.LongLength, bytes, colors));
                }
                else
                {
                    var Contents = FS.ReadContents(FilePath);
                    int digits = Contents.Length.GetDigits();
                    if (PrintLineNumbers)
                    {
                        for (int ContentIndex = 0; ContentIndex <= Contents.Length - 1; ContentIndex++)
                        {
                            int spaces = digits - (ContentIndex + 1).GetDigits();
                            if (colors)
                                builder.Append(entryColor.VTSequenceForeground());
                            builder.Append($"{new string(' ', spaces)}{ContentIndex + 1}: ");
                            if (colors)
                                builder.Append(valueColor.VTSequenceForeground());
                            builder.AppendLine(Contents[ContentIndex]);
                        }
                    }
                    else
                    {
                        if (colors)
                            builder.Append(valueColor.VTSequenceForeground());
                        for (int ContentIndex = 0; ContentIndex <= Contents.Length - 1; ContentIndex++)
                            builder.AppendLine(Contents[ContentIndex]);
                    }
                }
                if (i != array.Length - 1)
                    builder.AppendLine();
            }
            return builder.ToString();
        }

        /// <inheritdoc/>
        public virtual void PrintContents(string filename, bool colors = true) =>
            PrintContents(filename, Config.MainConfig.PrintLineNumbers, colors);

        /// <inheritdoc/>
        public virtual void PrintContents(string filename, bool PrintLineNumbers, bool ForcePlain = false, bool colors = true)
        {
            // Check the path
            filename = FS.NeutralizePath(filename);

            // Now, render the contents
            string rendered = RenderContents(filename, PrintLineNumbers, ForcePlain, colors);
            TextWriterRaw.WriteRaw(rendered);
        }

        /// <inheritdoc/>
        public virtual void PrintDirectoryInfo(FileSystemEntry DirectoryInfo) =>
            PrintDirectoryInfo(DirectoryInfo, Config.MainConfig.ShowFileDetailsList);

        /// <inheritdoc/>
        public virtual void PrintDirectoryInfo(FileSystemEntry DirectoryInfo, bool ShowDirectoryDetails)
        {
            if (DirectoryInfo.Type == FileSystemEntryType.Directory)
            {
                // Get all file sizes in a folder
                var finalDirInfo = DirectoryInfo.BaseEntry as DirectoryInfo ??
                    throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_DIRINFONEEDED"));
                long TotalSize = FS.GetAllSizesInFolder(finalDirInfo);

                // Print information
                if (finalDirInfo.Attributes == FileAttributes.Hidden & Config.MainConfig.HiddenFiles | !finalDirInfo.Attributes.HasFlag(FileAttributes.Hidden))
                {
                    TextWriterColor.Write("- " + finalDirInfo.Name + "/", false, ThemeColorType.ListEntry);
                    if (ShowDirectoryDetails)
                    {
                        TextWriterColor.Write(": ", false, ThemeColorType.ListEntry);
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_INFODATE"), false, ThemeColorType.ListValue, TotalSize.SizeString(), finalDirInfo.CreationTime.ToShortDateString(), finalDirInfo.CreationTime.ToShortTimeString(), finalDirInfo.LastWriteTime.ToShortDateString(), finalDirInfo.LastWriteTime.ToShortTimeString());
                    }
                    TextWriterRaw.Write();
                }
            }
            else
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_DIRECTORYNOTFOUND2"), true, ThemeColorType.Error, DirectoryInfo.FilePath);
                DebugWriter.WriteDebug(DebugLevel.I, "Folder doesn't exist. {0}", vars: [DirectoryInfo.FilePath]);
            }
        }

        /// <inheritdoc/>
        public virtual void PrintFileInfo(FileSystemEntry FileInfo) =>
            PrintFileInfo(FileInfo, Config.MainConfig.ShowFileDetailsList);

        /// <inheritdoc/>
        public virtual void PrintFileInfo(FileSystemEntry FileInfo, bool ShowFileDetails)
        {
            if (FileInfo.Type == FileSystemEntryType.File)
            {
                var finalDirInfo = FileInfo.BaseEntry as FileInfo ??
                    throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_FILEINFONEEDED"));
                if (finalDirInfo.Attributes == FileAttributes.Hidden & Config.MainConfig.HiddenFiles | !finalDirInfo.Attributes.HasFlag(FileAttributes.Hidden))
                {
                    if (finalDirInfo.Name.EndsWith(".mesh"))
                    {
                        TextWriterColor.Write("- " + finalDirInfo.Name, false, ThemeColorType.Stage);
                        if (ShowFileDetails)
                            TextWriterColor.Write(": ", false, ThemeColorType.Stage);
                    }
                    else
                    {
                        TextWriterColor.Write("- " + finalDirInfo.Name, false, ThemeColorType.ListEntry);
                        if (ShowFileDetails)
                            TextWriterColor.Write(": ", false, ThemeColorType.ListEntry);
                    }
                    if (ShowFileDetails)
                    {
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_INFODATE"), false, ThemeColorType.ListValue, ((FileInfo)FileInfo.BaseEntry).Length.SizeString(), FileInfo.BaseEntry.CreationTime.ToShortDateString(), FileInfo.BaseEntry.CreationTime.ToShortTimeString(), FileInfo.BaseEntry.LastWriteTime.ToShortDateString(), FileInfo.BaseEntry.LastWriteTime.ToShortTimeString());
                    }
                    TextWriterRaw.Write();
                }
            }
            else
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_FILENOTFOUND"), true, ThemeColorType.Error, FileInfo.FilePath);
                DebugWriter.WriteDebug(DebugLevel.I, "File doesn't exist. {0}", vars: [FileInfo.FilePath]);
            }
        }

        /// <inheritdoc/>
        public virtual byte[] ReadAllBytes(string path)
        {
            // Read the bytes
            path = FS.NeutralizePath(path);
            return File.ReadAllBytes(path);
        }

        /// <inheritdoc/>
        public virtual byte[] ReadAllBytesNoBlock(string path)
        {
            // Read all the bytes, bypassing the restrictions.
            path = FS.NeutralizePath(path);
            long size = new FileSystemEntry(path).FileSize;
            var AllBytesList = new byte[size];
            var FOpen = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            FOpen.ReadExactly(AllBytesList, 0, (int)size);
            FOpen.Close();
            return AllBytesList;
        }

        /// <inheritdoc/>
        public virtual string[] ReadAllLinesNoBlock(string path)
        {
            // Read all the lines, bypassing the restrictions.
            path = FS.NeutralizePath(path);
            var AllLnList = new List<string>();
            var FOpen = new StreamReader(File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
            while (!FOpen.EndOfStream)
                AllLnList.Add(FOpen.ReadLine() ?? "");
            FOpen.Close();
            return [.. AllLnList];
        }

        /// <inheritdoc/>
        public virtual string[] ReadContents(string filename)
        {
            // Read the contents
            filename = FS.NeutralizePath(filename);
            return File.ReadAllLines(filename);
        }

        /// <inheritdoc/>
        public virtual string ReadAllTextNoBlock(string path)
        {
            // Read all the lines, bypassing the restrictions.
            path = FS.NeutralizePath(path);
            var fileContentBuilder = new StringBuilder();
            var FOpen = new StreamReader(File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
            while (!FOpen.EndOfStream)
                fileContentBuilder.AppendLine(FOpen.ReadLine());
            FOpen.Close();
            return fileContentBuilder.ToString();
        }

        /// <inheritdoc/>
        public virtual string ReadContentsText(string filename)
        {
            // Read the contents
            filename = FS.NeutralizePath(filename);
            return File.ReadAllText(filename);
        }

        /// <inheritdoc/>
        public virtual void WriteAllBytes(string path, byte[] contents)
        {
            // Write the bytes
            path = FS.NeutralizePath(path);
            File.WriteAllBytes(path, contents);
        }

        /// <inheritdoc/>
        public virtual void WriteAllLinesNoBlock(string path, string[] contents)
        {
            // Write all the lines, bypassing the restrictions.
            path = FS.NeutralizePath(path);
            var FOpen = new StreamWriter(File.Open(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite));
            foreach (var content in contents)
                FOpen.WriteLine(content);
            FOpen.Close();
        }

        /// <inheritdoc/>
        public virtual void WriteContents(string filename, string[] contents)
        {
            // Write the contents
            filename = FS.NeutralizePath(filename);
            File.WriteAllLines(filename, contents);
        }

        /// <inheritdoc/>
        public virtual void WriteAllTextNoBlock(string path, string contents)
        {
            // Write all the lines, bypassing the restrictions.
            path = FS.NeutralizePath(path);
            var FOpen = new StreamWriter(File.Open(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite));
            FOpen.WriteLine(contents);
            FOpen.Close();
        }

        /// <inheritdoc/>
        public virtual void WriteContentsText(string filename, string contents)
        {
            // Write the contents
            filename = FS.NeutralizePath(filename);
            File.WriteAllText(filename, contents);
        }

        /// <inheritdoc/>
        public virtual void WriteAllBytesNoBlock(string path, byte[] contents)
        {
            // Write all the bytes, bypassing the restrictions.
            path = FS.NeutralizePath(path);
            var FOpen = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
            FOpen.Write(contents, 0, contents.Length);
            FOpen.Close();
        }

        /// <inheritdoc/>
        public virtual void RemoveAttributeFromFile(string FilePath, FileAttributes Attributes)
        {
            FilePath = FS.NeutralizePath(FilePath);
            var Attrib = File.GetAttributes(FilePath);
            DebugWriter.WriteDebug(DebugLevel.I, "File attributes: {0}", vars: [Attrib]);
            Attrib = Attrib.RemoveAttribute(Attributes);
            DebugWriter.WriteDebug(DebugLevel.I, "Setting file attribute to {0}...", vars: [Attrib]);
            File.SetAttributes(FilePath, Attrib);

            // Raise event
            EventsManager.FireEvent(EventType.FileAttributeRemoved, FilePath, Attributes);
        }

        /// <inheritdoc/>
        public virtual void RemoveDirectory(string Target) =>
            RemoveDirectory(Target, Config.MainConfig.ShowFilesystemProgress);

        /// <inheritdoc/>
        public virtual void RemoveDirectory(string Target, bool ShowProgress, bool secureRemove = false)
        {
            if (!FS.FolderExists(Target))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_DIRECTORYNOTFOUND1"), Target);

            // Get all source directories and files
            var SourceDirInfo = new DirectoryInfo(Target);
            var SourceDirectories = SourceDirInfo.GetDirectories();
            DebugWriter.WriteDebug(DebugLevel.I, "Source directories: {0}", vars: [SourceDirectories.Length]);
            var SourceFiles = SourceDirInfo.GetFiles();
            DebugWriter.WriteDebug(DebugLevel.I, "Source files: {0}", vars: [SourceFiles.Length]);

            // Iterate through every file and delete them
            for (int i = 0; i < SourceFiles.Length; i++)
            {
                FileInfo SourceFile = SourceFiles[i];
                string DestinationFilePath = IOPath.Combine(Target, SourceFile.Name);
                DebugWriter.WriteDebug(DebugLevel.I, "Removing file {0}...", vars: [DestinationFilePath]);
                if (ShowProgress)
                    ProgressManager.RegisterProgressHandler(new(new((perc, message) => TextWriterColor.Write($"{perc}% - {message}")), SourceFile.FullName));
                ProgressManager.ReportProgress((i + 1) / SourceFiles.Length, SourceFile.FullName, $"-> {DestinationFilePath}");
                RemoveFile(DestinationFilePath, secureRemove);
            }

            // Iterate through every subdirectory and delete them
            for (int i = 0; i < SourceDirectories.Length; i++)
            {
                DirectoryInfo SourceDirectory = SourceDirectories[i];
                string DestinationDirectoryPath = IOPath.Combine(Target, SourceDirectory.Name);
                DebugWriter.WriteDebug(DebugLevel.I, "Calling RemoveDirectory() with destination {0}...", vars: [DestinationDirectoryPath]);
                if (ShowProgress)
                    ProgressManager.RegisterProgressHandler(new(new((perc, message) => TextWriterColor.Write($"{perc}% - {message}")), SourceDirectory.FullName));
                ProgressManager.ReportProgress((i + 1) / SourceDirectories.Length, SourceDirectory.FullName, $"*  {DestinationDirectoryPath}");
                RemoveDirectory(DestinationDirectoryPath, ShowProgress, secureRemove);
            }

            // Raise event
            Directory.Delete(Target, true);
            EventsManager.FireEvent(EventType.DirectoryRemoved, Target);
        }

        /// <inheritdoc/>
        public virtual void RemoveFile(string Target, bool secureRemove = false)
        {
            string Dir = FS.NeutralizePath(Target);
            if (secureRemove)
            {
                // Open the file stream and fill it with zeroes
                if (!FS.FileExists(Dir))
                    throw new KernelException(KernelExceptionType.Filesystem);

                var target = File.OpenWrite(Dir);
                byte[] zeroes = new byte[target.Length];
                target.Write(zeroes, 0, zeroes.Length);
                target.Close();
            }
            File.Delete(Dir);

            // Raise event
            EventsManager.FireEvent(EventType.FileRemoved, Target);
        }

        /// <inheritdoc/>
        public virtual void RemoveFileOrDir(string Target, bool secureRemove = false)
        {
            if (FS.FileExists(Target))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "{0} is a file. Removing...", vars: [Target]);
                RemoveFile(Target, secureRemove);
            }
            else if (FS.FolderExists(Target))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "{0} is a folder. Removing...", vars: [Target]);
                RemoveDirectory(Target, secureRemove);
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Trying to remove {0} which is not found.", vars: [Target]);
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_REMOVENONEXISTENTFILE"), Target);
            }
        }

        /// <inheritdoc/>
        public virtual void RemoveFromPathLookup(string Path)
        {
            var LookupPaths = GetPathList();
            Path = FS.NeutralizePath(Path);
            LookupPaths.Remove(Path);
            Config.MainConfig.PathsToLookup = string.Join(PathLookupTools.PathLookupDelimiter, LookupPaths);
        }

        /// <inheritdoc/>
        public virtual void RemoveFromPathLookup(string Path, string RootPath)
        {
            var LookupPaths = GetPathList();
            Path = FS.NeutralizePath(Path, RootPath);
            LookupPaths.Remove(Path);
            Config.MainConfig.PathsToLookup = string.Join(PathLookupTools.PathLookupDelimiter, LookupPaths);
        }

        /// <inheritdoc/>
        public virtual List<string> SearchFileForString(string FilePath, string StringLookup)
        {
            try
            {
                    FilePath = FS.NeutralizePath(FilePath);
                var Matches = new List<string>();
                var Filebyte = FS.ReadContents(FilePath);
                int MatchNum = 1;
                int LineNumber = 1;
                for (int i = 0; i < Filebyte.Length; i++)
                {
                    string Str = Filebyte[i];
                    ProgressManager.ReportProgress((i + 1) / Filebyte.Length, nameof(SearchFileForString), $". {Str}");
                    if (Str.Contains(StringLookup))
                    {
                        ProgressManager.ReportProgress((i + 1) / Filebyte.Length, nameof(SearchFileForString) + "Matched", $"+ {Str}");
                        Matches.Add($"[{LineNumber}] " + TextTools.FormatString(LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_MATCH"), MatchNum, Str));
                        MatchNum += 1;
                    }
                    LineNumber += 1;
                }
                return Matches;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_FINDFILEFORMATCHFAILED"), StringLookup, ex.Message);
            }
        }

        /// <inheritdoc/>
        public virtual List<string> SearchFileForStringRegexp(string FilePath, Regex StringLookup)
        {
            try
            {
                    FilePath = FS.NeutralizePath(FilePath);
                var Matches = new List<string>();
                var Filebyte = FS.ReadContents(FilePath);
                int MatchNum = 1;
                int LineNumber = 1;
                for (int i = 0; i < Filebyte.Length; i++)
                {
                    string Str = Filebyte[i];
                    ProgressManager.ReportProgress((i + 1) / Filebyte.Length, nameof(SearchFileForStringRegexp), $". {Str}");
                    if (StringLookup.IsMatch(Str))
                    {
                        ProgressManager.ReportProgress((i + 1) / Filebyte.Length, nameof(SearchFileForStringRegexp) + "Matched", $"+ {Str}");
                        Matches.Add($"[{LineNumber}] " + TextTools.FormatString(LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_MATCH"), MatchNum, Str));
                        MatchNum += 1;
                    }
                    LineNumber += 1;
                }
                return Matches;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_FINDFILEFORMATCHFAILED"), StringLookup, ex.Message);
            }
        }

        /// <inheritdoc/>
        public List<(string, MatchCollection)> SearchFileForStringRegexpMatches(string FilePath, Regex StringLookup)
        {
            try
            {
                    FilePath = FS.NeutralizePath(FilePath);
                var Matches = new List<(string, MatchCollection)>();
                var Filebyte = FS.ReadContents(FilePath);
                for (int i = 0; i < Filebyte.Length; i++)
                {
                    string Str = Filebyte[i];
                    ProgressManager.ReportProgress((i + 1) / Filebyte.Length, nameof(SearchFileForStringRegexpMatches), $". {Str}");
                    if (StringLookup.IsMatch(Str))
                    {
                        ProgressManager.ReportProgress((i + 1) / Filebyte.Length, nameof(SearchFileForStringRegexpMatches) + "Matched", $"+ {Str}");
                        Matches.Add((Str, StringLookup.Matches(Str)));
                    }
                }
                return Matches;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_FINDFILEFORMATCHFAILED"), StringLookup, ex.Message);
            }
        }

        /// <inheritdoc/>
        public virtual string SortSelector(FileSystemEntry FileSystemEntry, int MaxLength) =>
            Config.MainConfig.SortMode switch
            {
                (int)FilesystemSortOptions.FullName => FileSystemEntry.FilePath,
                (int)FilesystemSortOptions.Length => (FileSystemEntry.BaseEntry is FileInfo fileInfo ? fileInfo.Length : 0L).ToString().PadLeft(MaxLength, '0'),
                (int)FilesystemSortOptions.CreationTime => Convert.ToString(FileSystemEntry.BaseEntry.CreationTime),
                (int)FilesystemSortOptions.LastAccessTime => Convert.ToString(FileSystemEntry.BaseEntry.LastAccessTime),
                (int)FilesystemSortOptions.LastWriteTime => Convert.ToString(FileSystemEntry.BaseEntry.LastWriteTime),
                (int)FilesystemSortOptions.Extension => FileSystemEntry.BaseEntry.Extension,
                (int)FilesystemSortOptions.CreationTimeUtc => Convert.ToString(FileSystemEntry.BaseEntry.CreationTimeUtc),
                (int)FilesystemSortOptions.LastAccessTimeUtc => Convert.ToString(FileSystemEntry.BaseEntry.LastAccessTimeUtc),
                (int)FilesystemSortOptions.LastWriteTimeUtc => Convert.ToString(FileSystemEntry.BaseEntry.LastWriteTimeUtc),
                _ => FileSystemEntry.FilePath,
            };

        /// <inheritdoc/>
        public virtual bool TryParseFileName(string Name)
        {
            try
            {
                    return !(Name.IndexOfAny(IOPath.GetInvalidFileNameChars()) >= 0);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to parse file name {0}: {1}", vars: [Name, ex.Message]);
            }
            return false;
        }

        /// <inheritdoc/>
        public virtual bool TryParsePath(string Path)
        {
            try
            {
                    return !(Path.IndexOfAny(GetInvalidPathChars()) >= 0);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to parse path {0}: {1}", vars: [Path, ex.Message]);
            }
            return false;
        }

        /// <inheritdoc/>
        public virtual string ReadToEndAndSeek(ref StreamReader stream)
        {
            string StreamString = stream.ReadToEnd();
            if (stream.BaseStream.CanSeek)
                stream.BaseStream.Seek(0L, SeekOrigin.Begin);
            return StreamString;
        }

        /// <inheritdoc/>
        public virtual void WrapTextFile(string path) =>
            WrapTextFile(path, 78);

        /// <inheritdoc/>
        public virtual void WrapTextFile(string path, int columns)
        {
            // Check to see if we're interacting with a text file
            if (IsBinaryFile(path) || IsJson(path) || IsSql(path))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_NEEDSREALTEXTFILE"), path);

            // Now, do the wrapping!
            string contents = ReadContentsText(path);
            var wrapped = TextTools.GetWrappedSentences(contents, columns);
            WriteContents(path, wrapped);
        }

        /// <inheritdoc/>
        public (int line, string one, string two)[] Compare(string pathOne, string pathTwo)
        {
            pathOne = FS.NeutralizePath(pathOne);
            pathTwo = FS.NeutralizePath(pathTwo);

            // Check to see if we're interacting with a text file
            if (IsBinaryFile(pathOne) || IsSql(pathOne))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_SOURCEISNOTREALTEXTFILE"), pathOne);
            if (IsBinaryFile(pathTwo) || IsSql(pathTwo))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_TARGETISNOTREALTEXTFILE"), pathTwo);

            // Now, filter the lines according to the added and the removed lines
            var sourceLines = ReadContents(pathOne).Select((ln, lnidx) => $"{lnidx + 1}|{ln}").ToDictionary((spec) => spec[..spec.IndexOf('|')], (spec) => spec[(spec.IndexOf('|') + 1)..]);
            var targetLines = ReadContents(pathTwo).Select((ln, lnidx) => $"{lnidx + 1}|{ln}").ToDictionary((spec) => spec[..spec.IndexOf('|')], (spec) => spec[(spec.IndexOf('|') + 1)..]);
            var added = targetLines.Values.Except(sourceLines.Values);
            var removed = sourceLines.Values.Except(targetLines.Values);
            var addedFinal = targetLines.Where((kvp) => added.Contains(kvp.Value)).ToDictionary((spec) => Convert.ToInt32(spec.Key), (spec) => ("", spec.Value));
            var removedFinal = sourceLines.Where((kvp) => removed.Contains(kvp.Value)).ToDictionary((spec) => Convert.ToInt32(spec.Key), (spec) => (spec.Value, ""));
            Dictionary<int, (string, string)> modifiedFinal = [];
            var sameLines = addedFinal.Where((kvp) => removedFinal.ContainsKey(kvp.Key)).Select((kvp) => kvp.Key);
            foreach (var idx in sameLines)
            {
                modifiedFinal.Add(idx, (removedFinal[idx].Value, addedFinal[idx].Value));
                addedFinal.Remove(idx);
                removedFinal.Remove(idx);
            }

            // Finally, make a list of affected lines
            List<(int line, string one, string two)> affectedLines = [];
            foreach (var line in addedFinal)
                affectedLines.Add((line.Key, line.Value.Item1, line.Value.Value));
            foreach (var line in removedFinal)
                affectedLines.Add((line.Key, line.Value.Value, line.Value.Item2));
            foreach (var line in modifiedFinal)
                affectedLines.Add((line.Key, line.Value.Item1, line.Value.Item2));
            affectedLines = [.. affectedLines.OrderBy((kvp) => kvp.line)];

            return [.. affectedLines];
        }

        /// <inheritdoc/>
        public string[] SplitFile(string inputFile, string outputDirectory, long chunkSize = 104857600)
        {
            inputFile = FS.NeutralizePath(inputFile);
            outputDirectory = FS.NeutralizePath(outputDirectory);

            // Check the directory and the input file
            if (!FS.FolderExists(outputDirectory))
                FS.MakeDirectory(outputDirectory);
            if (!FS.FileExists(inputFile))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_FILENOTFOUND2"), inputFile);

            // Check the chunk size
            if (chunkSize < 4096)
                chunkSize = 104857600;

            // Check to see if this file needs splitting
            var fileStream = File.OpenRead(inputFile);
            if (fileStream.Length <= chunkSize)
                // There is no need to make file chunks
                return [];

            // Open the file stream and read
            string inputFileName = IOPath.GetFileName(inputFile);
            string pathToChunk = FS.NeutralizePath(inputFileName + ".C0000", outputDirectory);
            var chunkStream = File.Create(pathToChunk);
            List<string> chunksMade = [pathToChunk];
            int processedChunks = 0;
            int chunkByte;
            while ((chunkByte = fileStream.ReadByte()) != -1)
            {
                // Check to see if we need to create another chunk
                if (chunkStream.Length >= chunkSize)
                {
                    // Another chunk is needed! Close the current chunk and create a new chunk file
                    processedChunks++;
                    pathToChunk = FS.NeutralizePath(inputFileName + $".C{processedChunks:0000}", outputDirectory);
                    chunkStream.Flush();
                    chunkStream.Close();
                    chunkStream = File.Create(pathToChunk);
                    chunksMade.Add(pathToChunk);
                }

                // Write this byte to the chunk
                chunkStream.WriteByte((byte)chunkByte);
            }

            // Flush the last chunk written
            chunkStream.Flush();
            chunkStream.Close();
            return [.. chunksMade];
        }

        /// <inheritdoc/>
        public string GroupFile(string inputFile, string outputDirectory)
        {
            inputFile = FS.NeutralizePath(inputFile);
            outputDirectory = FS.NeutralizePath(outputDirectory);

            // Check the directory
            if (!FS.FolderExists(outputDirectory))
                FS.MakeDirectory(outputDirectory);

            // Check the chunk files, but we need to sort those chunks logically in case we have more than 10000 chunks
            string inputFileName = IOPath.GetFileName(inputFile);
            var enumeratedChunks = FS.GetFilesystemEntries(inputFile + ".C*").OrderBy((fileName) => fileName, new LogicalComparer());
            if (!enumeratedChunks.Any())
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_FILENOTFOUND2"), inputFile + ".C0000");
            string finalOutputFile = FS.NeutralizePath(inputFileName, outputDirectory);
            using var fileStream = File.Create(finalOutputFile);
            foreach (var chunk in enumeratedChunks)
            {
                // Open the chunk file and create the input file
                using var chunkFile = File.OpenRead(chunk);

                // Write the chunk bytes
                int byteRead;
                while ((byteRead = chunkFile.ReadByte()) != -1)
                    fileStream.WriteByte((byte)byteRead);
            }
            fileStream.Flush();
            fileStream.Close();
            return finalOutputFile;
        }
    }
}
