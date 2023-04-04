
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

using Extensification.StringExts;
using KS.Files.Attributes;
using KS.Files.Folders;
using KS.Files.LineEndings;
using KS.Files.Querying;
using KS.Files.Read;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using FS = KS.Files.Filesystem;
using static Extensification.CharExts.Querying;
using KS.Misc.Writers.ConsoleWriters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using KS.Files.Operations;
using FluentFTP.Helpers;
using KS.Kernel;
using KS.ConsoleBase.Colors;
using Extensification.LongExts;
using KS.Misc.Text;
using KS.Kernel.Events;
using KS.Files;
using KS.Kernel.Configuration;

namespace KS.Drivers.Filesystem
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
        public virtual void AddAttributeToFile(string FilePath, FileAttributes Attributes)
        {
            FS.ThrowOnInvalidPath(FilePath);
            FilePath = FS.NeutralizePath(FilePath);
            DebugWriter.WriteDebug(DebugLevel.I, "Setting file attribute to {0}...", Attributes);
            File.SetAttributes(FilePath, Attributes);

            // Raise event
            EventsManager.FireEvent(EventType.FileAttributeAdded, FilePath, Attributes);
        }

        /// <inheritdoc/>
        public virtual void AddToPathLookup(string Path)
        {
            FS.ThrowOnInvalidPath(Path);
            var LookupPaths = GetPathList();
            Path = FS.NeutralizePath(Path);
            LookupPaths.Add(Path);
            Config.MainConfig.PathsToLookup = string.Join(Shell.Shell.PathLookupDelimiter, LookupPaths);
        }

        /// <inheritdoc/>
        public virtual void AddToPathLookup(string Path, string RootPath)
        {
            FS.ThrowOnInvalidPath(Path);
            FS.ThrowOnInvalidPath(RootPath);
            var LookupPaths = GetPathList();
            Path = FS.NeutralizePath(Path, RootPath);
            LookupPaths.Add(Path);
            Config.MainConfig.PathsToLookup = string.Join(Shell.Shell.PathLookupDelimiter, LookupPaths);
        }

        /// <inheritdoc/>
        public virtual void ClearFile(string Path)
        {
            FS.ThrowOnInvalidPath(Path);
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
                FS.ThrowOnInvalidPath(Input);
                if (!Parsing.IsBinaryFile(Input))
                    throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("To combine text files, use the appropriate function.") + " " + nameof(CombineTextFiles) + "(" + Input + ")");
                CombinedContents.AddRange(FileRead.ReadAllBytes(Input));

                // Enumerate the target inputs
                foreach (string TargetInput in TargetInputs)
                {
                    FS.ThrowOnInvalidPath(TargetInput);
                    if (!Parsing.IsBinaryFile(TargetInput))
                        throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("To combine text files, use the appropriate function.") + " " + nameof(CombineTextFiles) + "(" + TargetInput + ")");
                    CombinedContents.AddRange(FileRead.ReadAllBytes(TargetInput));
                }

                // Return the combined contents
                return CombinedContents.ToArray();
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to combine files: {0}", ex.Message);
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Failed to combine files."), ex);
            }
        }

        /// <inheritdoc/>
        public virtual string[] CombineTextFiles(string Input, string[] TargetInputs)
        {
            try
            {
                var CombinedContents = new List<string>();

                // Add the input contents
                FS.ThrowOnInvalidPath(Input);
                if (Parsing.IsBinaryFile(Input))
                    throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("To combine binary files, use the appropriate function.") + " " + nameof(CombineBinaryFiles) + "(" + Input + ")");
                CombinedContents.AddRange(FileRead.ReadContents(Input));

                // Enumerate the target inputs
                foreach (string TargetInput in TargetInputs)
                {
                    FS.ThrowOnInvalidPath(TargetInput);
                    if (Parsing.IsBinaryFile(TargetInput))
                        throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("To combine binary files, use the appropriate function.") + " " + nameof(CombineBinaryFiles) + "(" + TargetInput + ")");
                    CombinedContents.AddRange(FileRead.ReadContents(TargetInput));
                }

                // Return the combined contents
                return CombinedContents.ToArray();
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to combine files: {0}", ex.Message);
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Failed to combine files."), ex);
            }
        }

        /// <inheritdoc/>
        public virtual void ConvertLineEndings(string TextFile) =>
            ConvertLineEndings(TextFile, LineEndingsTools.NewlineStyle);

        /// <inheritdoc/>
        public virtual void ConvertLineEndings(string TextFile, FilesystemNewlineStyle LineEndingStyle)
        {
            FS.ThrowOnInvalidPath(TextFile);
            TextFile = FS.NeutralizePath(TextFile);
            if (!Checking.FileExists(TextFile))
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("File {0} not found.").FormatString(TextFile));

            // Get all the file lines, regardless of the new line style on the target file
            var FileContents = FileRead.ReadAllLinesNoBlock(TextFile);
            DebugWriter.WriteDebug(DebugLevel.I, "Got {0} lines. Converting newlines in {1} to {2}...", FileContents.Length, TextFile, LineEndingStyle.ToString());

            // Get the newline string according to the current style
            string NewLineString = LineEndingsTools.GetLineEndingString(LineEndingStyle);

            // Convert the newlines now
            var Result = new StringBuilder();
            foreach (string FileContent in FileContents)
                Result.Append(FileContent + NewLineString);

            // Save the changes
            File.WriteAllText(TextFile, Result.ToString());
        }

        /// <inheritdoc/>
        public virtual void CopyDirectory(string Source, string Destination) =>
            CopyDirectory(Source, Destination, FS.ShowFilesystemProgress);

        /// <inheritdoc/>
        public virtual void CopyDirectory(string Source, string Destination, bool ShowProgress)
        {
            FS.ThrowOnInvalidPath(Source);
            FS.ThrowOnInvalidPath(Destination);
            if (!Checking.FolderExists(Source))
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Directory {0} not found.").FormatString(Source));

            // Get all source directories and files
            var SourceDirInfo = new DirectoryInfo(Source);
            var SourceDirectories = SourceDirInfo.GetDirectories();
            DebugWriter.WriteDebug(DebugLevel.I, "Source directories: {0}", SourceDirectories.Length);
            var SourceFiles = SourceDirInfo.GetFiles();
            DebugWriter.WriteDebug(DebugLevel.I, "Source files: {0}", SourceFiles.Length);

            // Make a destination directory if it doesn't exist
            if (!Checking.FolderExists(Destination))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Destination directory {0} doesn't exist. Creating...", Destination);
                Directory.CreateDirectory(Destination);
            }

            // Iterate through every file and copy them to destination
            foreach (FileInfo SourceFile in SourceFiles)
            {
                string DestinationFilePath = Path.Combine(Destination, SourceFile.Name);
                DebugWriter.WriteDebug(DebugLevel.I, "Copying file {0} to destination...", DestinationFilePath);
                if (ShowProgress)
                    TextWriterColor.Write("-> {0}", DestinationFilePath);
                SourceFile.CopyTo(DestinationFilePath, true);
            }

            // Iterate through every subdirectory and copy them to destination
            foreach (DirectoryInfo SourceDirectory in SourceDirectories)
            {
                string DestinationDirectoryPath = Path.Combine(Destination, SourceDirectory.Name);
                DebugWriter.WriteDebug(DebugLevel.I, "Calling CopyDirectory() with destination {0}...", DestinationDirectoryPath);
                if (ShowProgress)
                    TextWriterColor.Write("* {0}", DestinationDirectoryPath);
                CopyDirectory(SourceDirectory.FullName, DestinationDirectoryPath);
            }
        }

        /// <inheritdoc/>
        public virtual void CopyFileOrDir(string Source, string Destination)
        {
            FS.ThrowOnInvalidPath(Source);
            FS.ThrowOnInvalidPath(Destination);
            Source = FS.NeutralizePath(Source);
            DebugWriter.WriteDebug(DebugLevel.I, "Source directory: {0}", Source);
            Destination = FS.NeutralizePath(Destination);
            DebugWriter.WriteDebug(DebugLevel.I, "Target directory: {0}", Destination);
            string FileName = Path.GetFileName(Source);
            DebugWriter.WriteDebug(DebugLevel.I, "Source file name: {0}", FileName);
            if (Checking.FolderExists(Source))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Source and destination are directories");
                CopyDirectory(Source, Destination);

                // Raise event
                EventsManager.FireEvent(EventType.DirectoryCopied, Source, Destination);
            }
            else if (Checking.FileExists(Source) & Checking.FolderExists(Destination))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Source is a file and destination is a directory");
                File.Copy(Source, Destination + "/" + FileName, true);

                // Raise event
                EventsManager.FireEvent(EventType.FileCopied, Source, Destination + "/" + FileName);
            }
            else if (Checking.FileExists(Source))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Source is a file and destination is a file");
                File.Copy(Source, Destination, true);

                // Raise event
                EventsManager.FireEvent(EventType.FileCopied, Source, Destination);
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Source or destination are invalid.");
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("The path is neither a file nor a directory."));
            }
        }

        /// <inheritdoc/>
        public virtual List<FileSystemInfo> CreateList(string folder, bool Sorted = false, bool Recursive = false)
        {
            FS.ThrowOnInvalidPath(folder);
            DebugWriter.WriteDebug(DebugLevel.I, "Folder {0} will be listed...", folder);
            var FilesystemEntries = new List<FileSystemInfo>();

            // List files and folders
            folder = FS.NeutralizePath(folder);
            if (Checking.FolderExists(folder) | folder.ContainsAnyOf(new[] { "?", "*" }))
            {
                IEnumerable<string> enumeration;
                try
                {
                    enumeration = GetFilesystemEntries(folder, false, Recursive);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to make a list of filesystem entries for directory {0}: {1}", folder, ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                    throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Failed to make a list of filesystem entries for directory") + " {0}", ex, folder);
                }
                foreach (string Entry in enumeration)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Enumerating {0}...", Entry);
                    try
                    {
                        if (Checking.FileExists(Entry))
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Entry is a file. Adding {0} to list...", Entry);
                            FilesystemEntries.Add(new FileInfo(Entry));
                        }
                        else if (Checking.FolderExists(Entry))
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Entry is a folder. Adding {0} to list...", Entry);
                            FilesystemEntries.Add(new DirectoryInfo(Entry));
                        }
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, "Failed to enumerate {0} for directory {1}: {2}", Entry, folder, ex.Message);
                        DebugWriter.WriteDebugStackTrace(ex);
                    }
                }
            }

            // Return the resulting list immediately if not sorted. Otherwise, sort it.
            if (Sorted & !(FilesystemEntries.Count == 0))
            {
                // We define the max string length for the largest size. This is to overcome the limitation of sorting when it comes to numbers.
                int MaxLength = FilesystemEntries.Max(x => x as FileInfo is not null ? (x as FileInfo).Length.GetDigits() : 1);

                // Select whether or not to sort descending.
                switch (Listing.SortDirection)
                {
                    case FilesystemSortDirection.Ascending:
                        {
                            FilesystemEntries = FilesystemEntries.OrderBy(x => SortSelector(x, MaxLength), StringComparer.OrdinalIgnoreCase).ToList();
                            break;
                        }
                    case FilesystemSortDirection.Descending:
                        {
                            FilesystemEntries = FilesystemEntries.OrderByDescending(x => SortSelector(x, MaxLength), StringComparer.OrdinalIgnoreCase).ToList();
                            break;
                        }
                }
            }
            return FilesystemEntries;
        }

        /// <inheritdoc/>
        public virtual void DisplayInHex(long StartByte, long EndByte, byte[] FileByte)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "File Bytes: {0}", FileByte.LongLength);
            StartByte.SwapIfSourceLarger(ref EndByte);
            if (StartByte <= FileByte.LongLength & EndByte <= FileByte.LongLength)
            {
                // We need to know how to write the bytes and their contents in this shape:
                // -> 0x00000010  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
                //    0x00000020  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
                //    0x00000030  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
                // ... and so on.
                TextWriterColor.Write($"0x{StartByte - 1L:X8}", false, KernelColorType.ListEntry);
                int ByteWritePositionX = $"0x{StartByte - 1L:X8}".Length + 2;
                int ByteCharWritePositionX = 61 + (ByteWritePositionX - 12);
                int ByteNumberEachSixteen = 1;
                for (long CurrentByteNumber = StartByte; CurrentByteNumber <= EndByte; CurrentByteNumber++)
                {
                    // Write the byte and the contents
                    DebugWriter.WriteDebug(DebugLevel.I, "Byte write position: {0}", ByteWritePositionX);
                    DebugWriter.WriteDebug(DebugLevel.I, "Byte char write position: {0}", ByteCharWritePositionX);
                    DebugWriter.WriteDebug(DebugLevel.I, "Byte number each sixteen: {0}", ByteNumberEachSixteen);
                    byte CurrentByte = FileByte[(int)(CurrentByteNumber - 1L)];
                    DebugWriter.WriteDebug(DebugLevel.I, "Byte: {0}", CurrentByte);
                    char ProjectedByteChar = Convert.ToChar(CurrentByte);
                    DebugWriter.WriteDebug(DebugLevel.I, "Projected byte char: {0}", ProjectedByteChar);
                    char RenderedByteChar = '.';
                    if (!char.IsWhiteSpace(ProjectedByteChar) & !char.IsControl(ProjectedByteChar) & !char.IsHighSurrogate(ProjectedByteChar) & !char.IsLowSurrogate(ProjectedByteChar))
                    {
                        // The renderer will actually render the character, not as a dot.
                        DebugWriter.WriteDebug(DebugLevel.I, "Char is not a whitespace.");
                        RenderedByteChar = ProjectedByteChar;
                    }
                    DebugWriter.WriteDebug(DebugLevel.I, "Rendered byte char: {0}", ProjectedByteChar);
                    TextWriterWhereColor.WriteWhere($"{CurrentByte:X2}", ByteWritePositionX + 3 * (ByteNumberEachSixteen - 1), ConsoleBase.ConsoleWrapper.CursorTop, false, KernelColorType.ListValue);
                    TextWriterWhereColor.WriteWhere($"{RenderedByteChar}", ByteCharWritePositionX + (ByteNumberEachSixteen - 1), ConsoleBase.ConsoleWrapper.CursorTop, false, KernelColorType.ListValue);

                    // Increase the byte number
                    ByteNumberEachSixteen += 1;

                    // Check to see if we've exceeded 16 bytes
                    if (ByteNumberEachSixteen > 16)
                    {
                        // OK, let's increase the byte iteration and get the next line ready
                        TextWriterColor.Write(CharManager.NewLine + $"0x{CurrentByteNumber:X8}", false, KernelColorType.ListEntry);
                        ByteWritePositionX = $"0x{CurrentByteNumber:X8}".Length + 2;
                        ByteCharWritePositionX = 61 + (ByteWritePositionX - 12);
                        ByteNumberEachSixteen = 1;
                    }
                }
                TextWriterColor.Write();
            }
            else if (StartByte > FileByte.LongLength)
            {
                TextWriterColor.Write(Translate.DoTranslation("The specified start byte number may not be larger than the file size."), true, KernelColorType.Error);
            }
            else if (EndByte > FileByte.LongLength)
            {
                TextWriterColor.Write(Translate.DoTranslation("The specified end byte number may not be larger than the file size."), true, KernelColorType.Error);
            }
        }

        /// <inheritdoc/>
        public virtual bool FileExists(string File, bool Neutralize = false)
        {
            FS.ThrowOnInvalidPath(File);
            if (Neutralize)
                File = FS.NeutralizePath(File);
            return System.IO.File.Exists(File);
        }

        /// <inheritdoc/>
        public virtual bool FileExistsInPath(string FilePath, ref string Result)
        {
            FS.ThrowOnInvalidPath(FilePath);
            var LookupPaths = GetPathList();
            string ResultingPath;
            foreach (string LookupPath in LookupPaths)
            {
                ResultingPath = FS.NeutralizePath(FilePath, LookupPath);
                if (Checking.FileExists(ResultingPath))
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
            FS.ThrowOnInvalidPath(Folder);
            if (Neutralize)
                Folder = FS.NeutralizePath(Folder);
            return Directory.Exists(Folder);
        }

        /// <inheritdoc/>
        public virtual long GetAllSizesInFolder(DirectoryInfo DirectoryInfo) =>
            GetAllSizesInFolder(DirectoryInfo, Flags.FullParseMode);

        /// <inheritdoc/>
        public virtual long GetAllSizesInFolder(DirectoryInfo DirectoryInfo, bool FullParseMode)
        {
            List<FileInfo> Files;
            if (FullParseMode)
            {
                Files = DirectoryInfo.EnumerateFiles("*", SearchOption.AllDirectories).ToList();
            }
            else
            {
                Files = DirectoryInfo.EnumerateFiles("*", SearchOption.TopDirectoryOnly).ToList();
            }
            DebugWriter.WriteDebug(DebugLevel.I, "{0} files to be parsed", Files.Count);
            long TotalSize = 0L; // In bytes
            foreach (FileInfo DFile in Files)
            {
                if (DFile.Attributes == FileAttributes.Hidden & Flags.HiddenFiles | !DFile.Attributes.HasFlag(FileAttributes.Hidden))
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "File {0}, Size {1} bytes", DFile.Name, DFile.Length);
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
                FS.ThrowOnInvalidPath(Path);

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
                string Parent = FS.NeutralizePath(System.IO.Path.GetDirectoryName(Path) + "/" + System.IO.Path.GetFileName(Path));
                string Pattern = IsFile ? "" : "*";
                if (Parent.ContainsAnyOf(Parsing.GetInvalidPathChars().Select(Character => Character.ToString()).ToArray()))
                {
                    Parent = System.IO.Path.GetDirectoryName(Path);
                    Pattern = System.IO.Path.GetFileName(Path);
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
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to combine files: {0}", ex.Message);
            }
            return Entries;
        }

        /// <inheritdoc/>
        public virtual string[] GetFilesystemEntries(string Parent, string Pattern, bool Recursive = false)
        {
            var Entries = Array.Empty<string>();
            try
            {
                FS.ThrowOnInvalidPath(Parent);
                FS.ThrowOnInvalidPath(Pattern);
                Parent = FS.NeutralizePath(Parent);

                // Get the entries
                if (Directory.Exists(Parent))
                {
                    EnumerationOptions options = new()
                    {
                        RecurseSubdirectories = Recursive,
                        AttributesToSkip = Flags.HiddenFiles ? FileAttributes.System : FileAttributes.Hidden | FileAttributes.System
                    };
                    Entries = Directory.EnumerateFileSystemEntries(Parent, Pattern, options).ToArray();
                    DebugWriter.WriteDebug(DebugLevel.I, "Enumerated {0} entries from parent {1} using pattern {2}", Entries.Length, Parent, Pattern);
                }
                else
                {
                    Entries = new[] { Parent };
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to combine files: {0}", ex.Message);
            }
            return Entries;
        }

        /// <inheritdoc/>
        public virtual char[] GetInvalidPathChars()
        {
            var FinalInvalidPathChars = Path.GetInvalidPathChars();
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
            var FinalInvalidFileChars = Path.GetInvalidFileNameChars();
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
            FS.ThrowOnInvalidPath(TextFile);
            TextFile = FS.NeutralizePath(TextFile);
            if (!Checking.FileExists(TextFile))
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("File {0} not found.").FormatString(TextFile));

            // Open the file stream
            var NewlineStyle = LineEndingsTools.NewlineStyle;
            var TextFileStream = new FileStream(TextFile, FileMode.Open, FileAccess.Read);
            int CarriageReturnCode = Convert.ToChar(LineEndingsTools.GetLineEndingString(FilesystemNewlineStyle.CR)).GetAsciiCode();
            int LineFeedCode = Convert.ToChar(LineEndingsTools.GetLineEndingString(FilesystemNewlineStyle.LF)).GetAsciiCode();
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
        public virtual string GetNumberedFileName(string path, string fileName)
        {
            FS.ThrowOnInvalidPath(path);
            FS.ThrowOnInvalidPath(fileName);
            path = FS.NeutralizePath(path);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string fileNameExtension = Path.GetExtension(fileName);
            int fileNumber = 0;

            // Check if the target file exists
            bool fileNameValid = false;
            do
            {
                string fileNameGenerated = FS.NeutralizePath($"{fileNameWithoutExtension}-{fileNumber}{fileNameExtension}", path);
                if (!Checking.FileExists(fileNameGenerated))
                    return fileNameGenerated;
                fileNumber++;
            }
            while (!fileNameValid);
            return $"{fileNameWithoutExtension}-{fileNumber}{fileNameExtension}";
        }

        /// <inheritdoc/>
        public virtual List<string> GetPathList() =>
            Shell.Shell.PathsToLookup.Split(Convert.ToChar(Shell.Shell.PathLookupDelimiter)).ToList();

        /// <inheritdoc/>
        public string GetRandomFileName() =>
            FS.NeutralizePath(Path.GetRandomFileName(), Paths.TempPath);

        /// <inheritdoc/>
        public string GetRandomFolderName() =>
            FS.NeutralizePath(Path.GetRandomFileName() + "/", Paths.TempPath);

        /// <inheritdoc/>
        public virtual bool IsBinaryFile(string Path)
        {
            // Neutralize path
            FS.ThrowOnInvalidPath(Path);
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
                FS.ThrowOnInvalidPath(Path);
                Path = FS.NeutralizePath(Path);

                // Try to parse the content as JSON object
                try
                {
                    var ParsedObject = JObject.Parse(File.ReadAllText(Path));
                    return true;
                }
                catch
                {
                    var ParsedObject = JArray.Parse(File.ReadAllText(Path));
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public virtual void MakeDirectory(string NewDirectory, bool ThrowIfDirectoryExists = true)
        {
            FS.ThrowOnInvalidPath(NewDirectory);
            NewDirectory = FS.NeutralizePath(NewDirectory);
            DebugWriter.WriteDebug(DebugLevel.I, "New directory: {0} ({1})", NewDirectory, Checking.FolderExists(NewDirectory));
            if (!Checking.FolderExists(NewDirectory))
            {
                Directory.CreateDirectory(NewDirectory);

                // Raise event
                EventsManager.FireEvent(EventType.DirectoryCreated, NewDirectory);
            }
            else if (ThrowIfDirectoryExists)
            {
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Directory {0} already exists.").FormatString(NewDirectory));
            }
        }

        /// <inheritdoc/>
        public virtual void MakeFile(string NewFile, bool ThrowIfFileExists = true)
        {
            FS.ThrowOnInvalidPath(NewFile);
            NewFile = FS.NeutralizePath(NewFile);
            DebugWriter.WriteDebug(DebugLevel.I, "File path is {0} and .Exists is {1}", NewFile, Checking.FileExists(NewFile));
            if (!Checking.FileExists(NewFile))
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
                    throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Error trying to create a file: {0}").FormatString(ex.Message));
                }
            }
            else if (ThrowIfFileExists)
            {
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("File already exists."));
            }
        }

        /// <inheritdoc/>
        public virtual void MakeJsonFile(string NewFile, bool ThrowIfFileExists = true, bool useArray = false)
        {
            FS.ThrowOnInvalidPath(NewFile);
            NewFile = FS.NeutralizePath(NewFile);
            DebugWriter.WriteDebug(DebugLevel.I, "File path is {0} and .Exists is {1}", NewFile, Checking.FileExists(NewFile));
            if (!Checking.FileExists(NewFile))
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
                    throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Error trying to create a file: {0}").FormatString(ex.Message));
                }
            }
            else if (ThrowIfFileExists)
            {
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("File already exists."));
            }
        }

        /// <inheritdoc/>
        public virtual void MoveDirectory(string Source, string Destination) =>
            MoveDirectory(Source, Destination, FS.ShowFilesystemProgress);

        /// <inheritdoc/>
        public virtual void MoveDirectory(string Source, string Destination, bool ShowProgress)
        {
            FS.ThrowOnInvalidPath(Source);
            FS.ThrowOnInvalidPath(Destination);
            if (!Checking.FolderExists(Source))
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Directory {0} not found.").FormatString(Source));

            // Get all source directories and files
            var SourceDirInfo = new DirectoryInfo(Source);
            var SourceDirectories = SourceDirInfo.GetDirectories();
            DebugWriter.WriteDebug(DebugLevel.I, "Source directories: {0}", SourceDirectories.Length);
            var SourceFiles = SourceDirInfo.GetFiles();
            DebugWriter.WriteDebug(DebugLevel.I, "Source files: {0}", SourceFiles.Length);

            // Make a destination directory if it doesn't exist
            if (!Checking.FolderExists(Destination))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Destination directory {0} doesn't exist. Creating...", Destination);
                Directory.CreateDirectory(Destination);
            }

            // Iterate through every file and copy them to destination
            foreach (FileInfo SourceFile in SourceFiles)
            {
                string DestinationFilePath = Path.Combine(Destination, SourceFile.Name);
                DebugWriter.WriteDebug(DebugLevel.I, "Moving file {0} to destination...", DestinationFilePath);
                if (ShowProgress)
                    TextWriterColor.Write("-> {0}", DestinationFilePath);
                SourceFile.MoveTo(DestinationFilePath);
            }

            // Iterate through every subdirectory and copy them to destination
            foreach (DirectoryInfo SourceDirectory in SourceDirectories)
            {
                string DestinationDirectoryPath = Path.Combine(Destination, SourceDirectory.Name);
                DebugWriter.WriteDebug(DebugLevel.I, "Calling MoveDirectory() with destination {0}...", DestinationDirectoryPath);
                if (ShowProgress)
                    TextWriterColor.Write("* {0}", DestinationDirectoryPath);
                MoveDirectory(SourceDirectory.FullName, DestinationDirectoryPath);

                // Source subdirectories are removed after moving
                Removing.RemoveDirectory(SourceDirectory.FullName);
            }
        }

        /// <inheritdoc/>
        public virtual void MoveFileOrDir(string Source, string Destination)
        {
            FS.ThrowOnInvalidPath(Source);
            FS.ThrowOnInvalidPath(Destination);
            Source = FS.NeutralizePath(Source);
            DebugWriter.WriteDebug(DebugLevel.I, "Source directory: {0}", Source);
            Destination = FS.NeutralizePath(Destination);
            DebugWriter.WriteDebug(DebugLevel.I, "Target directory: {0}", Destination);
            string FileName = Path.GetFileName(Source);
            DebugWriter.WriteDebug(DebugLevel.I, "Source file name: {0}", FileName);
            if (Checking.FolderExists(Source))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Source and destination are directories");
                MoveDirectory(Source, Destination);

                // Raise event
                EventsManager.FireEvent(EventType.DirectoryMoved, Source, Destination);
            }
            else if (Checking.FileExists(Source) & Checking.FolderExists(Destination))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Source is a file and destination is a directory");
                File.Move(Source, Destination + "/" + FileName);

                // Raise event
                EventsManager.FireEvent(EventType.FileMoved, Source, Destination + "/" + FileName);
            }
            else if (Checking.FileExists(Source))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Source is a file and destination is a file");
                File.Move(Source, Destination);

                // Raise event
                EventsManager.FireEvent(EventType.FileMoved, Source, Destination);
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Source or destination are invalid.");
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("The path is neither a file nor a directory."));
            }
        }

        /// <inheritdoc/>
        public virtual void PrintContents(string filename) =>
            PrintContents(filename, Flags.PrintLineNumbers);

        /// <inheritdoc/>
        public virtual void PrintContents(string filename, bool PrintLineNumbers, bool ForcePlain = false)
        {
            // Check the path
            FS.ThrowOnInvalidPath(filename);
            filename = FS.NeutralizePath(filename);

            // If interacting with the binary file, display it in hex. Otherwise, display it as if it is text. Except if forced to view binaries as texts.
            if (Parsing.IsBinaryFile(filename) && !ForcePlain)
            {
                byte[] bytes = File.ReadAllBytes(filename);
                DisplayInHex(1, bytes.LongLength, bytes);
            }
            else
            {
                // Read the contents
                foreach (string FilePath in Listing.GetFilesystemEntries(filename, true))
                {
                    var Contents = FileRead.ReadContents(FilePath);
                    for (int ContentIndex = 0; ContentIndex <= Contents.Length - 1; ContentIndex++)
                    {
                        if (PrintLineNumbers)
                        {
                            TextWriterColor.Write("{0,4}: ", false, KernelColorType.ListEntry, ContentIndex + 1);
                        }
                        TextWriterColor.Write(Contents[ContentIndex]);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public virtual void PrintDirectoryInfo(FileSystemInfo DirectoryInfo) =>
            PrintDirectoryInfo(DirectoryInfo, Listing.ShowFileDetailsList);

        /// <inheritdoc/>
        public virtual void PrintDirectoryInfo(FileSystemInfo DirectoryInfo, bool ShowDirectoryDetails)
        {
            if (Checking.FolderExists(DirectoryInfo.FullName))
            {
                // Get all file sizes in a folder
                long TotalSize = SizeGetter.GetAllSizesInFolder((DirectoryInfo)DirectoryInfo);

                // Print information
                if (DirectoryInfo.Attributes == FileAttributes.Hidden & Flags.HiddenFiles | !DirectoryInfo.Attributes.HasFlag(FileAttributes.Hidden))
                {
                    TextWriterColor.Write("- " + DirectoryInfo.Name + "/", false, KernelColorType.ListEntry);
                    if (ShowDirectoryDetails)
                    {
                        TextWriterColor.Write(": ", false, KernelColorType.ListEntry);
                        TextWriterColor.Write(Translate.DoTranslation("{0}, Created in {1} {2}, Modified in {3} {4}"), false, KernelColorType.ListValue, TotalSize.FileSizeToString(), DirectoryInfo.CreationTime.ToShortDateString(), DirectoryInfo.CreationTime.ToShortTimeString(), DirectoryInfo.LastWriteTime.ToShortDateString(), DirectoryInfo.LastWriteTime.ToShortTimeString());
                    }
                    TextWriterColor.Write();
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Directory {0} not found"), true, KernelColorType.Error, DirectoryInfo.FullName);
                DebugWriter.WriteDebug(DebugLevel.I, "IO.FolderExists = {0}", Checking.FolderExists(DirectoryInfo.FullName));
            }
        }

        /// <inheritdoc/>
        public virtual void PrintFileInfo(FileSystemInfo FileInfo) =>
            PrintFileInfo(FileInfo, Listing.ShowFileDetailsList);

        /// <inheritdoc/>
        public virtual void PrintFileInfo(FileSystemInfo FileInfo, bool ShowFileDetails)
        {
            if (Checking.FileExists(FileInfo.FullName))
            {
                if (FileInfo.Attributes == FileAttributes.Hidden & Flags.HiddenFiles | !FileInfo.Attributes.HasFlag(FileAttributes.Hidden))
                {
                    if (FileInfo.Name.EndsWith(".uesh"))
                    {
                        TextWriterColor.Write("- " + FileInfo.Name, false, KernelColorType.Stage);
                        if (ShowFileDetails)
                            TextWriterColor.Write(": ", false, KernelColorType.Stage);
                    }
                    else
                    {
                        TextWriterColor.Write("- " + FileInfo.Name, false, KernelColorType.ListEntry);
                        if (ShowFileDetails)
                            TextWriterColor.Write(": ", false, KernelColorType.ListEntry);
                    }
                    if (ShowFileDetails)
                    {
                        TextWriterColor.Write(Translate.DoTranslation("{0}, Created in {1} {2}, Modified in {3} {4}"), false, KernelColorType.ListValue, ((FileInfo)FileInfo).Length.FileSizeToString(), FileInfo.CreationTime.ToShortDateString(), FileInfo.CreationTime.ToShortTimeString(), FileInfo.LastWriteTime.ToShortDateString(), FileInfo.LastWriteTime.ToShortTimeString());
                    }
                    TextWriterColor.Write();
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("File {0} not found"), true, KernelColorType.Error, FileInfo.FullName);
                DebugWriter.WriteDebug(DebugLevel.I, "IO.FileExists = {0}", Checking.FileExists(FileInfo.FullName));
            }
        }

        /// <inheritdoc/>
        public virtual byte[] ReadAllBytes(string path)
        {
            // Read the bytes
            FS.ThrowOnInvalidPath(path);
            path = FS.NeutralizePath(path);
            return File.ReadAllBytes(path);
        }

        /// <inheritdoc/>
        public virtual string[] ReadAllLinesNoBlock(string path)
        {
            FS.ThrowOnInvalidPath(path);

            // Read all the lines, bypassing the restrictions.
            path = FS.NeutralizePath(path);
            var AllLnList = new List<string>();
            var FOpen = new StreamReader(File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
            while (!FOpen.EndOfStream)
                AllLnList.Add(FOpen.ReadLine());
            FOpen.Close();
            return AllLnList.ToArray();
        }

        /// <inheritdoc/>
        public virtual string[] ReadContents(string filename)
        {
            // Read the contents
            FS.ThrowOnInvalidPath(filename);
            var FileContents = new List<string>();
            filename = FS.NeutralizePath(filename);
            using (var FStream = new StreamReader(filename))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Stream to file {0} opened.", filename);
                while (!FStream.EndOfStream)
                    FileContents.Add(FStream.ReadLine());
            }
            return FileContents.ToArray();
        }

        /// <inheritdoc/>
        public virtual void RemoveAttributeFromFile(string FilePath, FileAttributes Attributes)
        {
            FS.ThrowOnInvalidPath(FilePath);
            FilePath = FS.NeutralizePath(FilePath);
            var Attrib = File.GetAttributes(FilePath);
            DebugWriter.WriteDebug(DebugLevel.I, "File attributes: {0}", Attrib);
            Attrib = Attrib.RemoveAttribute(Attributes);
            DebugWriter.WriteDebug(DebugLevel.I, "Setting file attribute to {0}...", Attrib);
            File.SetAttributes(FilePath, Attrib);

            // Raise event
            EventsManager.FireEvent(EventType.FileAttributeRemoved, FilePath, Attributes);
        }

        /// <inheritdoc/>
        public virtual void RemoveDirectory(string Target) =>
            RemoveDirectory(Target, FS.ShowFilesystemProgress);

        /// <inheritdoc/>
        public virtual void RemoveDirectory(string Target, bool ShowProgress, bool secureRemove = false)
        {
            FS.ThrowOnInvalidPath(Target);
            if (!Checking.FolderExists(Target))
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Directory {0} not found.").FormatString(Target));

            // Get all source directories and files
            var SourceDirInfo = new DirectoryInfo(Target);
            var SourceDirectories = SourceDirInfo.GetDirectories();
            DebugWriter.WriteDebug(DebugLevel.I, "Source directories: {0}", SourceDirectories.Length);
            var SourceFiles = SourceDirInfo.GetFiles();
            DebugWriter.WriteDebug(DebugLevel.I, "Source files: {0}", SourceFiles.Length);

            // Iterate through every file and delete them
            foreach (FileInfo SourceFile in SourceFiles)
            {
                string DestinationFilePath = Path.Combine(Target, SourceFile.Name);
                DebugWriter.WriteDebug(DebugLevel.I, "Removing file {0}...", DestinationFilePath);
                if (ShowProgress)
                    TextWriterColor.Write("-> {0}", DestinationFilePath);
                RemoveFile(DestinationFilePath, secureRemove);
            }

            // Iterate through every subdirectory and delete them
            foreach (DirectoryInfo SourceDirectory in SourceDirectories)
            {
                string DestinationDirectoryPath = Path.Combine(Target, SourceDirectory.Name);
                DebugWriter.WriteDebug(DebugLevel.I, "Calling RemoveDirectory() with destination {0}...", DestinationDirectoryPath);
                if (ShowProgress)
                    TextWriterColor.Write("* {0}", DestinationDirectoryPath);
                RemoveDirectory(DestinationDirectoryPath);
            }

            // Raise event
            Directory.Delete(Target, true);
            EventsManager.FireEvent(EventType.DirectoryRemoved, Target);
        }

        /// <inheritdoc/>
        public virtual void RemoveFile(string Target, bool secureRemove = false)
        {
            FS.ThrowOnInvalidPath(Target);
            string Dir = FS.NeutralizePath(Target);
            if (secureRemove)
            {
                // Open the file stream and fill it with zeroes
                if (!Checking.FileExists(Dir))
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
            if (Checking.FileExists(Target))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "{0} is a file. Removing...", Target);
                RemoveFile(Target, secureRemove);
            }
            else if (Checking.FolderExists(Target))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "{0} is a folder. Removing...", Target);
                RemoveDirectory(Target, secureRemove);
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Trying to remove {0} which is not found.", Target);
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Can't remove {0} because it doesn't exist."), Target);
            }
        }

        /// <inheritdoc/>
        public virtual void RemoveFromPathLookup(string Path)
        {
            FS.ThrowOnInvalidPath(Path);
            var LookupPaths = GetPathList();
            Path = FS.NeutralizePath(Path);
            LookupPaths.Remove(Path);
            Config.MainConfig.PathsToLookup = string.Join(Shell.Shell.PathLookupDelimiter, LookupPaths);
        }

        /// <inheritdoc/>
        public virtual void RemoveFromPathLookup(string Path, string RootPath)
        {
            FS.ThrowOnInvalidPath(Path);
            FS.ThrowOnInvalidPath(RootPath);
            var LookupPaths = GetPathList();
            Path = FS.NeutralizePath(Path, RootPath);
            LookupPaths.Remove(Path);
            Config.MainConfig.PathsToLookup = string.Join(Shell.Shell.PathLookupDelimiter, LookupPaths);
        }

        /// <inheritdoc/>
        public virtual List<string> SearchFileForString(string FilePath, string StringLookup)
        {
            try
            {
                FS.ThrowOnInvalidPath(FilePath);
                FilePath = FS.NeutralizePath(FilePath);
                var Matches = new List<string>();
                var Filebyte = File.ReadAllLines(FilePath);
                int MatchNum = 1;
                int LineNumber = 1;
                foreach (string Str in Filebyte)
                {
                    if (Str.Contains(StringLookup))
                    {
                        Matches.Add($"[{LineNumber}] " + Translate.DoTranslation("Match {0}: {1}").FormatString(MatchNum, Str));
                        MatchNum += 1;
                    }
                    LineNumber += 1;
                }
                return Matches;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Unable to find file to match string \"{0}\": {1}").FormatString(StringLookup, ex.Message));
            }
        }

        /// <inheritdoc/>
        public virtual List<string> SearchFileForStringRegexp(string FilePath, Regex StringLookup)
        {
            try
            {
                FS.ThrowOnInvalidPath(FilePath);
                FilePath = FS.NeutralizePath(FilePath);
                var Matches = new List<string>();
                var Filebyte = File.ReadAllLines(FilePath);
                int MatchNum = 1;
                int LineNumber = 1;
                foreach (string Str in Filebyte)
                {
                    if (StringLookup.IsMatch(Str))
                    {
                        Matches.Add($"[{LineNumber}] " + Translate.DoTranslation("Match {0}: {1}").FormatString(MatchNum, Str));
                        MatchNum += 1;
                    }
                    LineNumber += 1;
                }
                return Matches;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Unable to find file to match string \"{0}\": {1}").FormatString(StringLookup, ex.Message));
            }
        }

        /// <inheritdoc/>
        public virtual string SortSelector(FileSystemInfo FileSystemEntry, int MaxLength) =>
            Listing.SortMode switch
            {
                FilesystemSortOptions.FullName          => FileSystemEntry.FullName,
                FilesystemSortOptions.Length            => (FileSystemEntry as FileInfo is not null ? (FileSystemEntry as FileInfo).Length : 0L).ToString().PadLeft(MaxLength, '0'),
                FilesystemSortOptions.CreationTime      => Convert.ToString(FileSystemEntry.CreationTime),
                FilesystemSortOptions.LastAccessTime    => Convert.ToString(FileSystemEntry.LastAccessTime),
                FilesystemSortOptions.LastWriteTime     => Convert.ToString(FileSystemEntry.LastWriteTime),
                FilesystemSortOptions.Extension         => FileSystemEntry.Extension,
                FilesystemSortOptions.CreationTimeUtc   => Convert.ToString(FileSystemEntry.CreationTimeUtc),
                FilesystemSortOptions.LastAccessTimeUtc => Convert.ToString(FileSystemEntry.LastAccessTimeUtc),
                FilesystemSortOptions.LastWriteTimeUtc  => Convert.ToString(FileSystemEntry.LastWriteTimeUtc),
                _                                       => FileSystemEntry.FullName,
            };

        /// <inheritdoc/>
        public virtual bool TryParseFileName(string Name)
        {
            try
            {
                FS.ThrowOnInvalidPath(Name);
                return !(Name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to parse file name {0}: {1}", Name, ex.Message);
            }
            return false;
        }

        /// <inheritdoc/>
        public virtual bool TryParsePath(string Path)
        {
            try
            {
                FS.ThrowOnInvalidPath(Path);
                return !(Path.IndexOfAny(GetInvalidPathChars()) >= 0);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to parse path {0}: {1}", Path, ex.Message);
            }
            return false;
        }
    }
}
