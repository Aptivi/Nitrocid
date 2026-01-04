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
using Nitrocid.Base.Files.Instances;
using Nitrocid.Base.Files.LineEndings;
using Nitrocid.Base.Kernel.Configuration;

namespace Nitrocid.Base.Drivers.Filesystem
{
    /// <summary>
    /// Filesystem driver interface for drivers
    /// </summary>
    public interface IFilesystemDriver : IDriver
    {

        /// <summary>
        /// Adds attribute to file
        /// </summary>
        /// <param name="FilePath">File path</param>
        /// <param name="Attributes">Attributes</param>
        void AddAttributeToFile(string FilePath, FileAttributes Attributes);

        /// <summary>
        /// Removes attribute from file
        /// </summary>
        /// <param name="FilePath">File path</param>
        /// <param name="Attributes">Attributes</param>
        void RemoveAttributeFromFile(string FilePath, FileAttributes Attributes);

        /// <summary>
        /// Creates a list of files and directories
        /// </summary>
        /// <param name="folder">Full path to folder</param>
        /// <param name="Sorted">Whether the list is sorted or not</param>
        /// <param name="Recursive">Whether the list is recursive or not</param>
        /// <returns>List of filesystem entries if any. Empty list if folder is not found or is empty.</returns>
        List<FileSystemEntry> CreateList(string folder, bool Sorted = false, bool Recursive = false);

        /// <summary>
        /// Helper for sorting filesystem entries
        /// </summary>
        /// <param name="FileSystemEntry">File system entry</param>
        /// <param name="MaxLength">For size, how many zeroes to pad the size string to the left?</param>
        string SortSelector(FileSystemEntry FileSystemEntry, int MaxLength);

        /// <summary>
        /// Gets the filesystem entries of the parent with the specified pattern (wildcards, ...)
        /// </summary>
        /// <param name="Path">The path, including the pattern</param>
        /// <param name="IsFile">Is the entry a file?</param>
        /// <param name="Recursive">Whether the list is recursive or not</param>
        /// <returns>The array of full paths</returns>
        string[] GetFilesystemEntries(string Path, bool IsFile = false, bool Recursive = false);

        /// <summary>
        /// Gets the filesystem entries of the parent with the specified pattern (wildcards, ...)
        /// </summary>
        /// <param name="Parent">The parent path. It can be neutralized if necessary</param>
        /// <param name="Pattern">The pattern</param>
        /// <param name="Recursive">Whether the list is recursive or not</param>
        /// <returns>The array of full paths</returns>
        string[] GetFilesystemEntries(string Parent, string Pattern, bool Recursive = false);

        /// <summary>
        /// Gets the filesystem entries of the parent using regular expressions
        /// </summary>
        /// <param name="Parent">The parent path. It can be neutralized if necessary</param>
        /// <param name="Pattern">The regular expression pattern</param>
        /// <param name="Recursive">Whether the list is recursive or not</param>
        /// <returns>The array of full paths</returns>
        string[] GetFilesystemEntriesRegex(string Parent, string Pattern, bool Recursive = false);

        /// <summary>
        /// Converts the line endings to the newline style for the current platform
        /// </summary>
        /// <param name="TextFile">Text file name with extension or file path</param>
        void ConvertLineEndings(string TextFile);

        /// <summary>
        /// Converts the line endings to the specified newline style
        /// </summary>
        /// <param name="TextFile">Text file name with extension or file path</param>
        /// <param name="LineEndingStyle">Line ending style</param>
        void ConvertLineEndings(string TextFile, FilesystemNewlineStyle LineEndingStyle);

        /// <summary>
        /// Gets the line ending style from file
        /// </summary>
        /// <param name="TextFile">Target text file</param>
        FilesystemNewlineStyle GetLineEndingFromFile(string TextFile);

        /// <summary>
        /// Combines the text files and puts the combined output to the array
        /// </summary>
        /// <param name="Input">An input file</param>
        /// <param name="TargetInputs">The target inputs to merge</param>
        string[] CombineTextFiles(string Input, string[] TargetInputs);

        /// <summary>
        /// Combines the binary files and puts the combined output to the array
        /// </summary>
        /// <param name="Input">An input file</param>
        /// <param name="TargetInputs">The target inputs to merge</param>
        byte[] CombineBinaryFiles(string Input, string[] TargetInputs);

        /// <summary>
        /// Copies a file or directory
        /// </summary>
        /// <param name="Source">Source file or directory</param>
        /// <param name="Destination">Target file or directory</param>
        /// <exception cref="IOException"></exception>
        void CopyFileOrDir(string Source, string Destination);

        /// <summary>
        /// Copies the directory from source to destination
        /// </summary>
        /// <param name="Source">Source directory</param>
        /// <param name="Destination">Target directory</param>
        void CopyDirectory(string Source, string Destination);

        /// <summary>
        /// Copies the directory from source to destination
        /// </summary>
        /// <param name="Source">Source directory</param>
        /// <param name="Destination">Target directory</param>
        /// <param name="ShowProgress">Whether or not to show what files are being copied</param>
        void CopyDirectory(string Source, string Destination, bool ShowProgress);

        /// <summary>
        /// Copies the file from source to destination
        /// </summary>
        /// <param name="Source">Source file</param>
        /// <param name="Destination">Target directory</param>
        void CopyFile(string Source, string Destination);

        /// <summary>
        /// Makes a directory
        /// </summary>
        /// <param name="NewDirectory">New directory</param>
        /// <param name="ThrowIfDirectoryExists">If directory exists, throw an exception.</param>
        /// <exception cref="IOException"></exception>
        void MakeDirectory(string NewDirectory, bool ThrowIfDirectoryExists = true);

        /// <summary>
        /// Makes a file
        /// </summary>
        /// <param name="NewFile">New file</param>
        /// <param name="ThrowIfFileExists">If file exists, throw an exception.</param>
        /// <exception cref="IOException"></exception>
        void MakeFile(string NewFile, bool ThrowIfFileExists = true);

        /// <summary>
        /// Makes an empty JSON file
        /// </summary>
        /// <param name="NewFile">New JSON file</param>
        /// <param name="ThrowIfFileExists">If file exists, throw an exception.</param>
        /// <param name="useArray">Use array instead of object</param>
        /// <exception cref="IOException"></exception>
        void MakeJsonFile(string NewFile, bool ThrowIfFileExists = true, bool useArray = false);

        /// <summary>
        /// Makes a symbolic link
        /// </summary>
        /// <param name="linkName">New symbolic link</param>
        /// <param name="target">Target file to link to</param>
        /// <exception cref="IOException"></exception>
        void MakeSymlink(string linkName, string target);

        /// <summary>
        /// Moves a file or directory
        /// </summary>
        /// <param name="Source">Source file or directory</param>
        /// <param name="Destination">Target file or directory</param>
        /// <exception cref="IOException"></exception>
        void MoveFileOrDir(string Source, string Destination);

        /// <summary>
        /// Moves the directory from source to destination
        /// </summary>
        /// <param name="Source">Source directory</param>
        /// <param name="Destination">Target directory</param>
        void MoveDirectory(string Source, string Destination);

        /// <summary>
        /// Moves the directory from source to destination
        /// </summary>
        /// <param name="Source">Source directory</param>
        /// <param name="Destination">Target directory</param>
        /// <param name="ShowProgress">Whether or not to show what files are being moved</param>
        void MoveDirectory(string Source, string Destination, bool ShowProgress);

        /// <summary>
        /// Moves the file from source to destination
        /// </summary>
        /// <param name="Source">Source file</param>
        /// <param name="Destination">Target directory</param>
        void MoveFile(string Source, string Destination);

        /// <summary>
        /// Removes a directory
        /// </summary>
        /// <param name="Target">Target directory</param>
        void RemoveDirectory(string Target);

        /// <summary>
        /// Removes a directory
        /// </summary>
        /// <param name="Target">Target directory</param>
        /// <param name="ShowProgress">Whether or not to show what files are being removed</param>
        /// <param name="secureRemove">Securely remove file by filling it with zeroes</param>
        void RemoveDirectory(string Target, bool ShowProgress, bool secureRemove = false);

        /// <summary>
        /// Removes a file
        /// </summary>
        /// <param name="Target">Target directory</param>
        /// <param name="secureRemove">Securely remove file by filling it with zeroes</param>
        void RemoveFile(string Target, bool secureRemove = false);

        /// <summary>
        /// Removes file or directory
        /// </summary>
        /// <param name="Target">Path to file or directory</param>
        /// <param name="secureRemove">Securely remove file by filling it with zeroes</param>
        void RemoveFileOrDir(string Target, bool secureRemove = false);

        /// <summary>
        /// Gets the lookup path list
        /// </summary>
        List<string> GetPathList();

        /// <summary>
        /// Gets the randomized file name
        /// </summary>
        /// <returns>Randomized file name in the temporary directory for your system</returns>
        string GetRandomFileName();

        /// <summary>
        /// Gets the randomized folder name
        /// </summary>
        /// <returns>Randomized folder name in the temporary directory for your system</returns>
        string GetRandomFolderName();

        /// <summary>
        /// Adds a (non-)neutralized path to lookup
        /// </summary>
        void AddToPathLookup(string Path);

        /// <summary>
        /// Adds a (non-)neutralized path to lookup
        /// </summary>
        void AddToPathLookup(string Path, string RootPath);

        /// <summary>
        /// Removes an existing (non-)neutralized path from lookup
        /// </summary>
        void RemoveFromPathLookup(string Path);

        /// <summary>
        /// Removes an existing (non-)neutralized path from lookup
        /// </summary>
        void RemoveFromPathLookup(string Path, string RootPath);

        /// <summary>
        /// Checks to see if the file exists in PATH and writes the result (path to file) to a string variable, if any.
        /// </summary>
        /// <param name="FilePath">A full path to file or just a file name</param>
        /// <param name="Result">The neutralized path</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        bool FileExistsInPath(string FilePath, ref string? Result);

        /// <summary>
        /// Prints the directory information to the console
        /// </summary>
        void PrintDirectoryInfo(FileSystemEntry DirectoryInfo);

        /// <summary>
        /// Prints the directory information to the console
        /// </summary>
        void PrintDirectoryInfo(FileSystemEntry DirectoryInfo, bool ShowDirectoryDetails);

        /// <summary>
        /// Prints the file information to the console
        /// </summary>
        void PrintFileInfo(FileSystemEntry FileInfo);

        /// <summary>
        /// Prints the file information to the console
        /// </summary>
        void PrintFileInfo(FileSystemEntry FileInfo, bool ShowFileDetails);

        /// <summary>
        /// Prints the contents of a file to the console
        /// </summary>
        /// <param name="filename">Full path to file</param>
        void PrintContents(string filename);

        /// <summary>
        /// Prints the contents of a file to the console
        /// </summary>
        /// <param name="filename">Full path to file with wildcards supported</param>
        /// <param name="PrintLineNumbers">Whether to also print the line numbers or not</param>
        /// <param name="ForcePlain">Forces binary files to be printed verbatim</param>
        void PrintContents(string filename, bool PrintLineNumbers, bool ForcePlain = false);

        /// <summary>
        /// Renders the contents of a file
        /// </summary>
        /// <param name="filename">Full path to file</param>
        string RenderContents(string filename);

        /// <summary>
        /// Renders the contents of a file
        /// </summary>
        /// <param name="filename">Full path to file with wildcards supported</param>
        /// <param name="PrintLineNumbers">Whether to also print the line numbers or not</param>
        /// <param name="ForcePlain">Forces binary files to be printed verbatim</param>
        string RenderContents(string filename, bool PrintLineNumbers, bool ForcePlain = false);

        /// <summary>
        /// Renders the file in hex
        /// </summary>
        /// <param name="StartByte">Start byte position</param>
        /// <param name="EndByte">End byte position</param>
        /// <param name="FileByte">File content in bytes</param>
        void DisplayInHex(long StartByte, long EndByte, byte[] FileByte);

        /// <summary>
        /// Renders the file in hex
        /// </summary>
        /// <param name="ByteContent">Content to highlight</param>
        /// <param name="HighlightResults">Whether to highlight the results. For querying.</param>
        /// <param name="StartByte">Start byte position</param>
        /// <param name="EndByte">End byte position</param>
        /// <param name="FileByte">File content in bytes</param>
        void DisplayInHex(byte ByteContent, bool HighlightResults, long StartByte, long EndByte, byte[] FileByte);

        /// <summary>
        /// Renders the file in hex in dumb mode
        /// </summary>
        /// <param name="StartByte">Start byte position</param>
        /// <param name="EndByte">End byte position</param>
        /// <param name="FileByte">File content in bytes</param>
        void DisplayInHexDumbMode(long StartByte, long EndByte, byte[] FileByte);

        /// <summary>
        /// Renders the file in hex in dumb mode
        /// </summary>
        /// <param name="ByteContent">Content to highlight</param>
        /// <param name="HighlightResults">Whether to highlight the results. For querying.</param>
        /// <param name="StartByte">Start byte position</param>
        /// <param name="EndByte">End byte position</param>
        /// <param name="FileByte">File content in bytes</param>
        void DisplayInHexDumbMode(byte ByteContent, bool HighlightResults, long StartByte, long EndByte, byte[] FileByte);

        /// <summary>
        /// Renders the file in hex
        /// </summary>
        /// <param name="StartByte">Start byte position</param>
        /// <param name="EndByte">End byte position</param>
        /// <param name="FileByte">File content in bytes</param>
        string RenderContentsInHex(long StartByte, long EndByte, byte[] FileByte);

        /// <summary>
        /// Renders the file in hex
        /// </summary>
        /// <param name="ByteContent">Content to highlight</param>
        /// <param name="HighlightResults">Whether to highlight the results. For querying.</param>
        /// <param name="StartByte">Start byte position</param>
        /// <param name="EndByte">End byte position</param>
        /// <param name="FileByte">File content in bytes</param>
        string RenderContentsInHex(byte ByteContent, bool HighlightResults, long StartByte, long EndByte, byte[] FileByte);

        /// <summary>
        /// Renders the file in hex
        /// </summary>
        /// <param name="ByteHighlight">Byte position to highlight</param>
        /// <param name="StartByte">Start byte position</param>
        /// <param name="EndByte">End byte position</param>
        /// <param name="FileByte">File content in bytes</param>
        string RenderContentsInHex(long ByteHighlight, long StartByte, long EndByte, byte[] FileByte);

        /// <summary>
        /// Checks to see if the file exists. Windows 10/11 bug aware.
        /// </summary>
        /// <param name="File">Target file</param>
        /// <param name="Neutralize">Whether to neutralize the path</param>
        /// <returns>True if exists; False if not. Throws on trying to trigger the Windows 10/11 BSOD/corruption bug</returns>
        bool FileExists(string File, bool Neutralize = false);

        /// <summary>
        /// Checks to see if the folder exists. Windows 10/11 bug aware.
        /// </summary>
        /// <param name="Folder">Target folder</param>
        /// <param name="Neutralize">Whether to neutralize the path</param>
        /// <returns>True if exists; False if not. Throws on trying to trigger the Windows 10/11 BSOD/corruption bug</returns>
        bool FolderExists(string Folder, bool Neutralize = false);

        /// <summary>
        /// Checks to see if the file or the folder exists. Windows 10/11 bug aware.
        /// </summary>
        /// <param name="Path">Target path</param>
        /// <param name="Neutralize">Whether to neutralize the path</param>
        /// <returns>True if exists; False if not. Throws on trying to trigger the Windows 10/11 BSOD/corruption bug</returns>
        bool Exists(string Path, bool Neutralize = false);

        /// <summary>
        /// Checks to see if the file or the folder contains the root path or not. Windows 10/11 bug aware.
        /// </summary>
        /// <param name="Path">Target path</param>
        /// <returns>True if rooted; False if not. Throws on trying to trigger the Windows 10/11 BSOD/corruption bug</returns>
        bool Rooted(string Path);

        /// <summary>
        /// Gets the file name with the file number suffix applied
        /// </summary>
        /// <param name="path">Path to the directory that the generated numbered file name will situate</param>
        /// <param name="fileName">The file name with an extension</param>
        /// <returns>Numbered file name with the file number suffix applied in this format: [filename]-[number].[ext]</returns>
        string GetNumberedFileName(string? path, string fileName);

        /// <summary>
        /// Gets all the invalid path characters
        /// </summary>
        char[] GetInvalidPathChars();

        /// <summary>
        /// Tries to parse the path (For file names and only names, use <see cref="TryParseFileName(string)"/> instead.)
        /// </summary>
        /// <param name="Path">The path to be parsed</param>
        /// <returns>True if successful; false if unsuccessful</returns>
        bool TryParsePath(string Path);

        /// <summary>
        /// Tries to parse the file name (For full paths, use <see cref="TryParsePath(string)"/> instead.)
        /// </summary>
        /// <param name="Name">The file name to be parsed</param>
        /// <returns>True if successful; false if unsuccessful</returns>
        bool TryParseFileName(string Name);

        /// <summary>
        /// Is the file a binary file?
        /// </summary>
        /// <param name="Path">Path to file</param>
        bool IsBinaryFile(string Path);

        /// <summary>
        /// Is the file a JSON file?
        /// </summary>
        /// <param name="Path">Path to file</param>
        bool IsJson(string Path);

        /// <summary>
        /// Is the file an SQL file?
        /// </summary>
        /// <param name="Path">Path to file</param>
        bool IsSql(string Path);

        /// <summary>
        /// Searches a file for string
        /// </summary>
        /// <param name="FilePath">File path</param>
        /// <param name="StringLookup">String to find</param>
        /// <returns>The list if successful; null if unsuccessful</returns>
        /// <exception cref="IOException"></exception>
        List<string> SearchFileForString(string FilePath, string StringLookup);

        /// <summary>
        /// Searches a file for string using regexp
        /// </summary>
        /// <param name="FilePath">File path</param>
        /// <param name="StringLookup">String to find</param>
        /// <returns>The list if successful; null if unsuccessful</returns>
        /// <exception cref="IOException"></exception>
        List<string> SearchFileForStringRegexp(string FilePath, Regex StringLookup);

        /// <summary>
        /// Searches a file for string using regexp
        /// </summary>
        /// <param name="FilePath">File path</param>
        /// <param name="StringLookup">String to find</param>
        /// <returns>The list of match collections with their associated line of text</returns>
        /// <exception cref="IOException"></exception>
        List<(string, MatchCollection)> SearchFileForStringRegexpMatches(string FilePath, Regex StringLookup);

        /// <summary>
        /// Gets all file sizes in a folder, depending on the kernel setting <see cref="Config.MainConfig"/>.FullParseMode
        /// </summary>
        /// <param name="DirectoryInfo">Directory information</param>
        /// <returns>Directory Size</returns>
        long GetAllSizesInFolder(DirectoryInfo? DirectoryInfo);

        /// <summary>
        /// Gets all file sizes in a folder, and optionally parses the entire folder
        /// </summary>
        /// <param name="DirectoryInfo">Directory information</param>
        /// <param name="FullParseMode">Whether to parse all the directories</param>
        /// <returns>Directory Size</returns>
        long GetAllSizesInFolder(DirectoryInfo? DirectoryInfo, bool FullParseMode);

        /// <summary>
        /// Reads the contents of a file and writes it to the array. This is blocking and will put a lock on the file until read.
        /// </summary>
        /// <param name="filename">Full path to file</param>
        /// <returns>An array full of file contents</returns>
        string[] ReadContents(string filename);

        /// <summary>
        /// Opens a file, reads all lines, and returns the array of lines
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>Array of lines</returns>
        string[] ReadAllLinesNoBlock(string path);

        /// <summary>
        /// Reads the contents of a file and writes it to the string. This is blocking and will put a lock on the file until read.
        /// </summary>
        /// <param name="filename">Full path to file</param>
        /// <returns>A text full of file contents</returns>
        string ReadContentsText(string filename);

        /// <summary>
        /// Opens a file, reads all lines, and returns the string of lines
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>String of lines</returns>
        string ReadAllTextNoBlock(string path);

        /// <summary>
        /// Reads all the bytes
        /// </summary>
        /// <param name="path">Path to the file</param>
        byte[] ReadAllBytes(string path);

        /// <summary>
        /// Reads all the bytes without blocking
        /// </summary>
        /// <param name="path">Path to the file</param>
        byte[] ReadAllBytesNoBlock(string path);

        /// <summary>
        /// Writes the contents of a file and writes it to the array. This is blocking and will put a lock on the file until read.
        /// </summary>
        /// <param name="filename">Full path to file</param>
        /// <param name="contents">File contents to write to</param>
        void WriteContents(string filename, string[] contents);

        /// <summary>
        /// Opens a file, reads all lines, and returns the array of lines
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <param name="contents">File contents to write to</param>
        void WriteAllLinesNoBlock(string path, string[] contents);

        /// <summary>
        /// Writes the contents of a file and writes it to the string. This is blocking and will put a lock on the file until read.
        /// </summary>
        /// <param name="filename">Full path to file</param>
        /// <param name="contents">File contents to write to</param>
        void WriteContentsText(string filename, string contents);

        /// <summary>
        /// Opens a file, reads all lines, and returns the string of lines
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <param name="contents">File contents to write to</param>
        void WriteAllTextNoBlock(string path, string contents);

        /// <summary>
        /// Writes all the bytes
        /// </summary>
        /// <param name="path">Path to the file</param>
        /// <param name="contents">File contents to write to</param>
        void WriteAllBytes(string path, byte[] contents);

        /// <summary>
        /// Writes all the bytes without blocking
        /// </summary>
        /// <param name="path">Path to the file</param>
        /// <param name="contents">File contents to write to</param>
        void WriteAllBytesNoBlock(string path, byte[] contents);

        /// <summary>
        /// Clears the contents of a file
        /// </summary>
        /// <param name="path">Path to an existing file</param>
        void ClearFile(string path);

        /// <summary>
        /// Reads all the characters in the stream until the end and seeks the stream to the beginning, if possible.
        /// </summary>
        /// <param name="stream">The stream reader</param>
        /// <returns>Contents of the stream</returns>
        string ReadToEndAndSeek(ref StreamReader stream);

        /// <summary>
        /// Wraps the text file contents with 78 columns per line.
        /// </summary>
        /// <param name="path">Path to an existing text file</param>
        void WrapTextFile(string path);

        /// <summary>
        /// Wraps the text file contents with 78 columns per line.
        /// </summary>
        /// <param name="path">Path to an existing text file</param>
        /// <param name="columns">How many columns until wrapping begins?</param>
        void WrapTextFile(string path, int columns);

        /// <summary>
        /// Compares between two text files
        /// </summary>
        /// <param name="pathOne">Path to the first file to be compared</param>
        /// <param name="pathTwo">Path to the second file to be compared</param>
        /// <returns>A list of tuples containing an affected line number, a line from the first file, and a line from the second file</returns>
        (int line, string one, string two)[] Compare(string pathOne, string pathTwo);

        /// <summary>
        /// Splits a single file to multiple chunks (identifiable with <c>&lt;FILENAME&gt;.&lt;EXT&gt;.C0000</c>, and secondaries with the suffix of <c>.C&lt;CHUNKNUM&gt;</c>)
        /// </summary>
        /// <param name="inputFile">Input file to split</param>
        /// <param name="outputDirectory">Output directory</param>
        /// <param name="chunkSize">Chunk size in bytes</param>
        string[] SplitFile(string inputFile, string outputDirectory, long chunkSize = 104_857_600);

        /// <summary>
        /// Groups chunks to a single file
        /// </summary>
        /// <param name="inputFile">Input file to group</param>
        /// <param name="outputDirectory">Output directory</param>
        string GroupFile(string inputFile, string outputDirectory);

    }
}
