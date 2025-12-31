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

using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Files.Instances;
using Nitrocid.Base.Files.LineEndings;

namespace Nitrocid.Base.Drivers.Filesystem.Bases
{
    internal class DefaultFilesystemDebug : BaseFilesystemDriver, IFilesystemDriver
    {
        public override string DriverName => "DefaultDebug";

        public override DriverTypes DriverType => DriverTypes.Filesystem;

        public override void AddAttributeToFile(string FilePath, FileAttributes Attributes)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(AddAttributeToFile)}({FilePath}, {Attributes}) entry");
            base.AddAttributeToFile(FilePath, Attributes);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(AddAttributeToFile)}({FilePath}, {Attributes}) exit");
        }

        public override void AddToPathLookup(string Path)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(AddToPathLookup)}({Path}) entry");
            base.AddToPathLookup(Path);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(AddToPathLookup)}({Path}) exit");
        }

        public override void AddToPathLookup(string Path, string RootPath)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(AddToPathLookup)}({Path}, {RootPath}) entry");
            base.AddToPathLookup(Path, RootPath);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(AddToPathLookup)}({Path}, {RootPath}) exit");
        }

        public override void ClearFile(string Path)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(ClearFile)}({Path}) entry");
            base.ClearFile(Path);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(ClearFile)}({Path}) exit");
        }

        public override byte[] CombineBinaryFiles(string Input, string[] TargetInputs)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(CombineBinaryFiles)}({Input}, {nameof(TargetInputs)}[{TargetInputs.Length}]) entry");
            var result = base.CombineBinaryFiles(Input, TargetInputs);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(CombineBinaryFiles)}({Input}, {nameof(TargetInputs)}[{TargetInputs.Length}]) exit with result byte[{result.Length}]");
            return result;
        }

        public override string[] CombineTextFiles(string Input, string[] TargetInputs)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(CombineTextFiles)}({Input}, {nameof(TargetInputs)}[{TargetInputs.Length}]) entry");
            var result = base.CombineTextFiles(Input, TargetInputs);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(CombineTextFiles)}({Input}, {nameof(TargetInputs)}[{TargetInputs.Length}]) exit with result byte[{result.Length}]");
            return result;
        }

        public override void ConvertLineEndings(string TextFile)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(ConvertLineEndings)}({TextFile}) entry");
            base.ConvertLineEndings(TextFile);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(ConvertLineEndings)}({TextFile}) exit");
        }

        public override void ConvertLineEndings(string TextFile, FilesystemNewlineStyle LineEndingStyle)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(ConvertLineEndings)}({TextFile}, {LineEndingStyle}) entry");
            base.ConvertLineEndings(TextFile, LineEndingStyle);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(ConvertLineEndings)}({TextFile}, {LineEndingStyle}) exit");
        }

        public override void CopyDirectory(string Source, string Destination)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(CopyDirectory)}({Source}, {Destination}) entry");
            base.CopyDirectory(Source, Destination);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(CopyDirectory)}({Source}, {Destination}) exit");
        }

        public override void CopyDirectory(string Source, string Destination, bool ShowProgress)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(CopyDirectory)}({Source}, {Destination}, {ShowProgress}) entry");
            base.CopyDirectory(Source, Destination, ShowProgress);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(CopyDirectory)}({Source}, {Destination}, {ShowProgress}) exit");
        }

        public override void CopyFile(string Source, string Destination)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(CopyFile)}({Source}, {Destination}) entry");
            base.CopyFile(Source, Destination);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(CopyFile)}({Source}, {Destination}) exit");
        }

        public override void CopyFileOrDir(string Source, string Destination)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(CopyFileOrDir)}({Source}, {Destination}) entry");
            base.CopyFileOrDir(Source, Destination);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(CopyFileOrDir)}({Source}, {Destination}) exit");
        }

        public override List<FileSystemEntry> CreateList(string folder, bool Sorted = false, bool Recursive = false)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(CreateList)}({folder}, {Sorted}, {Recursive}) entry");
            var result = base.CreateList(folder, Sorted, Recursive);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(CreateList)}({folder}, {Sorted}, {Recursive}) exit with result FileSystemEntry[{result.Count}]");
            return result;
        }

        public override void DisplayInHex(long StartByte, long EndByte, byte[] FileByte)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(DisplayInHex)}({StartByte}, {EndByte}, {nameof(FileByte)}[{FileByte.Length}]) entry");
            base.DisplayInHex(StartByte, EndByte, FileByte);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(DisplayInHex)}({StartByte}, {EndByte}, {nameof(FileByte)}[{FileByte.Length}]) exit");
        }

        public override void DisplayInHex(byte ByteContent, bool HighlightResults, long StartByte, long EndByte, byte[] FileByte)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(DisplayInHex)}({ByteContent}, {HighlightResults}, {StartByte}, {EndByte}, {nameof(FileByte)}[{FileByte.Length}]) entry");
            base.DisplayInHex(ByteContent, HighlightResults, StartByte, EndByte, FileByte);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(DisplayInHex)}({ByteContent}, {HighlightResults}, {StartByte}, {EndByte}, {nameof(FileByte)}[{FileByte.Length}]) exit");
        }

        public override void DisplayInHexDumbMode(long StartByte, long EndByte, byte[] FileByte)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(DisplayInHexDumbMode)}({StartByte}, {EndByte}, {nameof(FileByte)}[{FileByte.Length}]) entry");
            base.DisplayInHexDumbMode(StartByte, EndByte, FileByte);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(DisplayInHexDumbMode)}({StartByte}, {EndByte}, {nameof(FileByte)}[{FileByte.Length}]) exit");
        }

        public override void DisplayInHexDumbMode(byte ByteContent, bool HighlightResults, long StartByte, long EndByte, byte[] FileByte)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(DisplayInHexDumbMode)}({ByteContent}, {HighlightResults}, {StartByte}, {EndByte}, {nameof(FileByte)}[{FileByte.Length}]) entry");
            base.DisplayInHexDumbMode(ByteContent, HighlightResults, StartByte, EndByte, FileByte);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(DisplayInHexDumbMode)}({ByteContent}, {HighlightResults}, {StartByte}, {EndByte}, {nameof(FileByte)}[{FileByte.Length}]) exit");
        }

        public override string RenderContentsInHex(long StartByte, long EndByte, byte[] FileByte)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(RenderContentsInHex)}({StartByte}, {EndByte}, byte[{FileByte.Length}]) entry");
            string result = RenderContentsInHex(StartByte, EndByte, FileByte);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(RenderContentsInHex)}({StartByte}, {EndByte}, byte[{FileByte.Length}]) exit with result length [{result.Length}]");
            return result;
        }

        public override string RenderContentsInHex(byte ByteContent, bool HighlightResults, long StartByte, long EndByte, byte[] FileByte)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(RenderContentsInHex)}({ByteContent}, {HighlightResults}, {StartByte}, {EndByte}, byte[{FileByte.Length}]) entry");
            string result = RenderContentsInHex(ByteContent, HighlightResults, StartByte, EndByte, FileByte);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(RenderContentsInHex)}({ByteContent}, {HighlightResults}, {StartByte}, {EndByte}, byte[{FileByte.Length}]) exit with result length [{result.Length}]");
            return result;
        }

        public override string RenderContents(string filename)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(RenderContents)}({filename}) entry");
            string result = RenderContents(filename, Config.MainConfig.PrintLineNumbers);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(RenderContents)}({filename}) exit with result length [{result.Length}]");
            return result;
        }

        public override string RenderContents(string filename, bool PrintLineNumbers, bool ForcePlain = false)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(RenderContents)}({filename}, {PrintLineNumbers}, {ForcePlain}) entry");
            string result = RenderContents(filename, PrintLineNumbers, ForcePlain);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(RenderContents)}({filename}, {PrintLineNumbers}, {ForcePlain}) exit with result length [{result.Length}]");
            return result;
        }

        public override bool Exists(string Path, bool Neutralize = false)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(Exists)}({Path}, {Neutralize}) entry");
            var result = base.Exists(Path, Neutralize);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(Exists)}({Path}, {Neutralize}) exit with result {result}");
            return result;
        }

        public override bool FileExists(string File, bool Neutralize = false)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(FileExists)}({File}, {Neutralize}) entry");
            var result = base.FileExists(File, Neutralize);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(FileExists)}({File}, {Neutralize}) exit with result {result}");
            return result;
        }

        public override bool FileExistsInPath(string FilePath, ref string? Result)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(FileExistsInPath)}({FilePath}, {Result ?? ""}) entry");
            var result = base.FileExistsInPath(FilePath, ref Result);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(FileExistsInPath)}({FilePath}, {Result ?? ""}) exit with result {result}");
            return result;
        }

        public override bool FolderExists(string Folder, bool Neutralize = false)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(FolderExists)}({Folder}, {Neutralize}) entry");
            var result = base.FolderExists(Folder, Neutralize);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(FolderExists)}({Folder}, {Neutralize}) exit with result {result}");
            return result;
        }

        public override long GetAllSizesInFolder(DirectoryInfo? DirectoryInfo)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(GetAllSizesInFolder)}({nameof(DirectoryInfo)}) entry");
            var result = base.GetAllSizesInFolder(DirectoryInfo);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(GetAllSizesInFolder)}({nameof(DirectoryInfo)}) exit with result {result}");
            return result;
        }

        public override long GetAllSizesInFolder(DirectoryInfo? DirectoryInfo, bool FullParseMode)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(GetAllSizesInFolder)}({nameof(DirectoryInfo)}, {FullParseMode}) entry");
            var result = base.GetAllSizesInFolder(DirectoryInfo, FullParseMode);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(GetAllSizesInFolder)}({nameof(DirectoryInfo)}, {FullParseMode}) exit with result {result}");
            return result;
        }

        public override string[] GetFilesystemEntries(string Path, bool IsFile = false, bool Recursive = false)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(GetFilesystemEntries)}({Path}, {IsFile}, {Recursive}) entry");
            var result = base.GetFilesystemEntries(Path, IsFile, Recursive);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(GetFilesystemEntries)}({Path}, {IsFile}, {Recursive}) exit with result string[{result.Length}]");
            return result;
        }

        public override string[] GetFilesystemEntries(string? Parent, string Pattern, bool Recursive = false)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(GetFilesystemEntries)}({Parent}, {Pattern}, {Recursive}) entry");
            var result = base.GetFilesystemEntries(Parent, Pattern, Recursive);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(GetFilesystemEntries)}({Parent}, {Pattern}, {Recursive}) exit with result string[{result.Length}]");
            return result;
        }

        public override char[] GetInvalidFileChars()
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(GetInvalidFileChars)}() entry");
            var result = base.GetInvalidFileChars();
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(GetInvalidFileChars)}() exit with result char[{result.Length}]");
            return result;
        }

        public override char[] GetInvalidPathChars()
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(GetInvalidPathChars)}() entry");
            var result = base.GetInvalidPathChars();
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(GetInvalidPathChars)}() exit with result char[{result.Length}]");
            return result;
        }

        public override FilesystemNewlineStyle GetLineEndingFromFile(string TextFile)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(GetLineEndingFromFile)}({TextFile}) entry");
            var result = base.GetLineEndingFromFile(TextFile);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(GetLineEndingFromFile)}({TextFile}) exit with result {result}");
            return result;
        }

        public override string GetNumberedFileName(string? path, string fileName)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(GetNumberedFileName)}({path}, {fileName}) entry");
            var result = base.GetNumberedFileName(path, fileName);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(GetNumberedFileName)}({path}, {fileName}) exit with result {result}");
            return result;
        }

        public override List<string> GetPathList()
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(GetPathList)}() entry");
            var result = base.GetPathList();
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(GetPathList)}() exit with result string[{result.Count}]");
            return result;
        }

        public override bool IsBinaryFile(string Path)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(IsBinaryFile)}({Path}) entry");
            var result = base.IsBinaryFile(Path);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(IsBinaryFile)}({Path}) exit with result {result}");
            return result;
        }

        public override bool IsJson(string Path)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(IsJson)}({Path}) entry");
            var result = base.IsJson(Path);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(IsJson)}({Path}) exit with result {result}");
            return result;
        }

        public override bool IsSql(string Path)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(IsSql)}({Path}) entry");
            var result = base.IsSql(Path);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(IsSql)}({Path}) exit with result {result}");
            return result;
        }

        public override void MakeDirectory(string NewDirectory, bool ThrowIfDirectoryExists = true)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(MakeDirectory)}({NewDirectory}, {ThrowIfDirectoryExists}) entry");
            base.MakeDirectory(NewDirectory, ThrowIfDirectoryExists);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(MakeDirectory)}({NewDirectory}, {ThrowIfDirectoryExists}) exit");
        }

        public override void MakeFile(string NewFile, bool ThrowIfFileExists = true)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(MakeFile)}({NewFile}, {ThrowIfFileExists}) entry");
            base.MakeFile(NewFile, ThrowIfFileExists);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(MakeFile)}({NewFile}, {ThrowIfFileExists}) exit");
        }

        public override void MakeJsonFile(string NewFile, bool ThrowIfFileExists = true, bool useArray = false)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(MakeJsonFile)}({NewFile}, {ThrowIfFileExists}, {useArray}) entry");
            base.MakeJsonFile(NewFile, ThrowIfFileExists, useArray);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(MakeJsonFile)}({NewFile}, {ThrowIfFileExists}, {useArray}) exit");
        }

        public override void MakeSymlink(string linkName, string target)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(MakeSymlink)}({linkName}, {target}) entry");
            base.MakeSymlink(linkName, target);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(MakeSymlink)}({linkName}, {target}) exit");
        }

        public override void MoveDirectory(string Source, string Destination)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(MoveDirectory)}({Source}, {Destination}) entry");
            base.MoveDirectory(Source, Destination);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(MoveDirectory)}({Source}, {Destination}) exit");
        }

        public override void MoveDirectory(string Source, string Destination, bool ShowProgress)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(MoveDirectory)}({Source}, {Destination}, {ShowProgress}) entry");
            base.MoveDirectory(Source, Destination, ShowProgress);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(MoveDirectory)}({Source}, {Destination}, {ShowProgress}) exit");
        }

        public override void MoveFile(string Source, string Destination)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(MoveFile)}({Source}, {Destination}) entry");
            base.MoveFile(Source, Destination);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(MoveFile)}({Source}, {Destination}) exit");
        }

        public override void MoveFileOrDir(string Source, string Destination)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(MoveFileOrDir)}({Source}, {Destination}) entry");
            base.MoveFileOrDir(Source, Destination);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(MoveFileOrDir)}({Source}, {Destination}) exit");
        }

        public override void PrintContents(string filename)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(PrintContents)}({filename}) entry");
            base.PrintContents(filename);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(PrintContents)}({filename}) exit");
        }

        public override void PrintContents(string filename, bool PrintLineNumbers, bool ForcePlain = false)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(PrintContents)}({filename}, {PrintLineNumbers}, {ForcePlain}) entry");
            base.PrintContents(filename, PrintLineNumbers, ForcePlain);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(PrintContents)}({filename}, {PrintLineNumbers}, {ForcePlain}) exit");
        }

        public override void PrintDirectoryInfo(FileSystemEntry DirectoryInfo)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(PrintDirectoryInfo)}({nameof(DirectoryInfo)}) entry");
            base.PrintDirectoryInfo(DirectoryInfo);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(PrintDirectoryInfo)}({nameof(DirectoryInfo)}) exit");
        }

        public override void PrintDirectoryInfo(FileSystemEntry DirectoryInfo, bool ShowDirectoryDetails)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(PrintDirectoryInfo)}({nameof(DirectoryInfo)}, {ShowDirectoryDetails}) entry");
            base.PrintDirectoryInfo(DirectoryInfo, ShowDirectoryDetails);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(PrintDirectoryInfo)}({nameof(DirectoryInfo)}, {ShowDirectoryDetails}) exit");
        }

        public override void PrintFileInfo(FileSystemEntry FileInfo)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(PrintFileInfo)}({nameof(FileInfo)}) entry");
            base.PrintFileInfo(FileInfo);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(PrintFileInfo)}({nameof(FileInfo)}) exit");
        }

        public override void PrintFileInfo(FileSystemEntry FileInfo, bool ShowFileDetails)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(PrintFileInfo)}({nameof(FileInfo)}, {ShowFileDetails}) entry");
            base.PrintFileInfo(FileInfo, ShowFileDetails);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(PrintFileInfo)}({nameof(FileInfo)}, {ShowFileDetails}) exit");
        }

        public override byte[] ReadAllBytes(string path)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(ReadAllBytes)}({path}) entry");
            var result = base.ReadAllBytes(path);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(ReadAllBytes)}({path}) exit with result char[{result.Length}]");
            return result;
        }

        public override string[] ReadAllLinesNoBlock(string path)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(ReadAllLinesNoBlock)}({path}) entry");
            var result = base.ReadAllLinesNoBlock(path);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(ReadAllLinesNoBlock)}({path}) exit with result string[{result.Length}]");
            return result;
        }

        public override string ReadAllTextNoBlock(string path)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(ReadAllTextNoBlock)}({path}) entry");
            var result = base.ReadAllTextNoBlock(path);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(ReadAllTextNoBlock)}({path}) exit with result length {result.Length}");
            return result;
        }

        public override string[] ReadContents(string filename)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(ReadContents)}({filename}) entry");
            var result = base.ReadContents(filename);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(ReadContents)}({filename}) exit with result string[{result.Length}]");
            return result;
        }

        public override string ReadContentsText(string filename)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(ReadContentsText)}({filename}) entry");
            var result = base.ReadContentsText(filename);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(ReadContentsText)}({filename}) exit with result length {result.Length}");
            return result;
        }

        public override byte[] ReadAllBytesNoBlock(string path)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(ReadAllBytesNoBlock)}({path}) entry");
            var result = base.ReadAllBytesNoBlock(path);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(ReadAllBytesNoBlock)}({path}) exit with result length {result.Length}");
            return result;
        }

        public override string ReadToEndAndSeek(ref StreamReader stream)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(ReadToEndAndSeek)}({nameof(stream)}) entry");
            var result = base.ReadToEndAndSeek(ref stream);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(ReadToEndAndSeek)}({nameof(stream)}) exit with result length {result.Length}");
            return result;
        }

        public override void RemoveAttributeFromFile(string FilePath, FileAttributes Attributes)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(RemoveAttributeFromFile)}({FilePath}, {Attributes}) entry");
            base.RemoveAttributeFromFile(FilePath, Attributes);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(RemoveAttributeFromFile)}({FilePath}, {Attributes}) exit");
        }

        public override void RemoveDirectory(string Target)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(RemoveDirectory)}({Target}) entry");
            base.RemoveDirectory(Target);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(RemoveDirectory)}({Target}) exit");
        }

        public override void RemoveDirectory(string Target, bool ShowProgress, bool secureRemove = false)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(RemoveDirectory)}({Target}, {ShowProgress}, {secureRemove}) entry");
            base.RemoveDirectory(Target, ShowProgress, secureRemove);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(RemoveDirectory)}({Target}, {ShowProgress}, {secureRemove}) exit");
        }

        public override void RemoveFile(string Target, bool secureRemove = false)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(RemoveFile)}({Target}, {secureRemove}) entry");
            base.RemoveFile(Target, secureRemove);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(RemoveFile)}({Target}, {secureRemove}) exit");
        }

        public override void RemoveFileOrDir(string Target, bool secureRemove = false)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(RemoveFileOrDir)}({Target}, {secureRemove}) entry");
            base.RemoveFileOrDir(Target, secureRemove);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(RemoveFileOrDir)}({Target}, {secureRemove}) exit");
        }

        public override void RemoveFromPathLookup(string Path)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(RemoveFromPathLookup)}({Path}) entry");
            base.RemoveFromPathLookup(Path);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(RemoveFromPathLookup)}({Path}) exit");
        }

        public override void RemoveFromPathLookup(string Path, string RootPath)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(RemoveFromPathLookup)}({Path}, {RootPath}) entry");
            base.RemoveFromPathLookup(Path, RootPath);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(RemoveFromPathLookup)}({Path}, {RootPath}) exit");
        }

        public override bool Rooted(string Path)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(Rooted)}({Path}) entry");
            var result = base.Rooted(Path);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(Rooted)}({Path}) exit with result {result}");
            return result;
        }

        public override List<string> SearchFileForString(string FilePath, string StringLookup)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(SearchFileForString)}({FilePath}, {StringLookup}) entry");
            var result = base.SearchFileForString(FilePath, StringLookup);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(SearchFileForString)}({FilePath}, {StringLookup}) exit with result string[{result.Count}]");
            return result;
        }

        public override List<string> SearchFileForStringRegexp(string FilePath, Regex StringLookup)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(SearchFileForStringRegexp)}({FilePath}, {nameof(StringLookup)}) entry");
            var result = base.SearchFileForStringRegexp(FilePath, StringLookup);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(SearchFileForStringRegexp)}({FilePath}, {nameof(StringLookup)}) exit with result string[{result.Count}]");
            return result;
        }

        public override string SortSelector(FileSystemEntry FileSystemEntry, int MaxLength)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(SortSelector)}({nameof(FileSystemEntry)}, {MaxLength}) entry");
            var result = base.SortSelector(FileSystemEntry, MaxLength);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(SortSelector)}({nameof(FileSystemEntry)}, {MaxLength}) exit with result {result}");
            return result;
        }

        public override bool TryParseFileName(string Name)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(TryParseFileName)}({Name}) entry");
            var result = base.TryParseFileName(Name);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(TryParseFileName)}({Name}) exit with result {result}");
            return result;
        }

        public override bool TryParsePath(string Path)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(TryParseFileName)}({Path}) entry");
            var result = base.TryParsePath(Path);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(TryParseFileName)}({Path}) exit with result {result}");
            return result;
        }

        public override void WriteAllBytes(string path, byte[] contents)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(WriteAllBytes)}({path}, byte[{contents.Length}]) entry");
            base.WriteAllBytes(path, contents);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(WriteAllBytes)}({path}, byte[{contents.Length}]) exit");
        }

        public override void WriteAllBytesNoBlock(string path, byte[] contents)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(WriteAllBytesNoBlock)}({path}, byte[{contents.Length}]) entry");
            base.WriteAllBytesNoBlock(path, contents);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(WriteAllBytesNoBlock)}({path}, byte[{contents.Length}]) exit");
        }

        public override void WriteAllLinesNoBlock(string path, string[] contents)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(WriteAllLinesNoBlock)}({path}, string[{contents.Length}]) entry");
            base.WriteAllLinesNoBlock(path, contents);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(WriteAllLinesNoBlock)}({path}, string[{contents.Length}]) exit");
        }

        public override void WriteAllTextNoBlock(string path, string contents)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(WriteAllTextNoBlock)}({path}, length {contents.Length}) entry");
            base.WriteAllTextNoBlock(path, contents);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(WriteAllTextNoBlock)}({path}, length {contents.Length}) exit");
        }

        public override void WriteContents(string filename, string[] contents)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(WriteContents)}({filename}, string[{contents.Length}]) entry");
            base.WriteContents(filename, contents);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(WriteContents)}({filename}, string[{contents.Length}]) exit");
        }

        public override void WriteContentsText(string filename, string contents)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(WriteContentsText)}({filename}, length {contents.Length}) entry");
            base.WriteContentsText(filename, contents);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(WriteContentsText)}({filename}, length {contents.Length}) exit");
        }

        public override void WrapTextFile(string path)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(WrapTextFile)}({path}) entry");
            base.WrapTextFile(path);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(WrapTextFile)}({path}) exit");
        }

        public override void WrapTextFile(string path, int columns)
        {
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(WrapTextFile)}({path}, {columns}) entry");
            base.WrapTextFile(path, columns);
            DebugWriter.WriteDebug(DebugLevel.I, $"{nameof(WrapTextFile)}({path}, {columns}) exit");
        }
    }
}
