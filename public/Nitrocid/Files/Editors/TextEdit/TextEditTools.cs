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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Shell.Shells.Text;
using Textify.General;

namespace Nitrocid.Files.Editors.TextEdit
{
    /// <summary>
    /// Text editor tools module
    /// </summary>
    public static class TextEditTools
    {

        /// <summary>
        /// Opens the text file
        /// </summary>
        /// <param name="File">Target file. We recommend you to use <see cref="FilesystemTools.NeutralizePath(string, bool)"></see> to neutralize path.</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool OpenTextFile(string File)
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to open file {0}...", vars: [File]);
                TextEditShellCommon.fileStream = new FileStream(File, FileMode.Open);
                if (TextEditShellCommon.FileStream is null)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_NOTOPENYET"));
                TextEditShellCommon.fileLines ??= [];
                TextEditShellCommon.FileLinesOrig ??= [];
                DebugWriter.WriteDebug(DebugLevel.I, "File {0} is open. Length: {1}, Pos: {2}", vars: [File, TextEditShellCommon.FileStream.Length, TextEditShellCommon.FileStream.Position]);
                var TextFileStreamReader = new StreamReader(TextEditShellCommon.FileStream);
                while (!TextFileStreamReader.EndOfStream)
                {
                    string StreamLine = TextFileStreamReader.ReadLine() ?? "";
                    TextEditShellCommon.FileLines.Add(StreamLine);
                    TextEditShellCommon.FileLinesOrig.Add(StreamLine);
                }
                TextEditShellCommon.FileStream.Seek(0L, SeekOrigin.Begin);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Open file {0} failed: {1}", vars: [File, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
        }

        /// <summary>
        /// Closes text file
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool CloseTextFile()
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to close file...");
                TextEditShellCommon.FileStream?.Close();
                TextEditShellCommon.fileStream = null;
                DebugWriter.WriteDebug(DebugLevel.I, "File is no longer open.");
                TextEditShellCommon.FileLines.Clear();
                TextEditShellCommon.FileLinesOrig.Clear();
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Closing file failed: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
        }

        /// <summary>
        /// Saves text file
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool SaveTextFile(bool ClearLines)
        {
            try
            {
                if (TextEditShellCommon.FileStream is null)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_NOTOPENYET"));
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to save file...");
                TextEditShellCommon.FileStream.SetLength(0L);
                DebugWriter.WriteDebug(DebugLevel.I, "Length set to 0.");
                var FileLinesByte = Encoding.Default.GetBytes(string.Join(CharManager.NewLine, [.. TextEditShellCommon.FileLines]));
                DebugWriter.WriteDebug(DebugLevel.I, "Converted lines to bytes. Length: {0}", vars: [FileLinesByte.Length]);
                TextEditShellCommon.FileStream.Write(FileLinesByte, 0, FileLinesByte.Length);
                TextEditShellCommon.FileStream.Flush();
                DebugWriter.WriteDebug(DebugLevel.I, "File is saved.");
                if (ClearLines)
                    TextEditShellCommon.FileLines.Clear();
                TextEditShellCommon.FileLinesOrig.Clear();
                TextEditShellCommon.FileLinesOrig.AddRange(TextEditShellCommon.FileLines);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Saving file failed: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
        }

        /// <summary>
        /// Handles autosave
        /// </summary>
        public static void HandleAutoSaveTextFile()
        {
            if (Config.MainConfig.TextEditAutoSaveFlag)
            {
                try
                {
                    Thread.Sleep(Config.MainConfig.TextEditAutoSaveInterval * 1000);
                    if (TextEditShellCommon.FileStream is not null)
                        SaveTextFile(false);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }
        }

        /// <summary>
        /// Was text edited?
        /// </summary>
        public static bool WasTextEdited()
        {
            if (TextEditShellCommon.FileLines is not null && TextEditShellCommon.FileLinesOrig is not null)
                return !TextEditShellCommon.FileLines.SequenceEqual(TextEditShellCommon.FileLinesOrig);
            return false;
        }

        /// <summary>
        /// Adds a new line to the current text
        /// </summary>
        /// <param name="Content">New line content</param>
        public static void AddNewLine(string Content)
        {
            if (TextEditShellCommon.FileStream is not null)
                TextEditShellCommon.FileLines.Add(Content);
            else
                throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_NOFILESTREAM"));
        }

        /// <summary>
        /// Adds the new lines to the current text
        /// </summary>
        /// <param name="Lines">New lines</param>
        public static void AddNewLines(string[] Lines)
        {
            if (TextEditShellCommon.FileStream is not null)
            {
                foreach (string Line in Lines)
                    TextEditShellCommon.FileLines.Add(Line);
            }
            else
                throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_NOFILESTREAM"));
        }

        /// <summary>
        /// Removes a line from the current text
        /// </summary>
        /// <param name="LineNumber">The line number to remove</param>
        public static void RemoveLine(int LineNumber)
        {
            if (TextEditShellCommon.FileStream is not null)
            {
                int LineIndex = LineNumber - 1;
                DebugWriter.WriteDebug(DebugLevel.I, "Got line index: {0}", vars: [LineIndex]);
                DebugWriter.WriteDebug(DebugLevel.I, "Old file lines: {0}", vars: [TextEditShellCommon.FileLines.Count]);
                if (LineNumber <= TextEditShellCommon.FileLines.Count)
                {
                    TextEditShellCommon.FileLines.RemoveAt(LineIndex);
                    DebugWriter.WriteDebug(DebugLevel.I, "New file lines: {0}", vars: [TextEditShellCommon.FileLines.Count]);
                }
                else
                    throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_LINENUMEXCEEDSLASTNUM"));
            }
            else
                throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_NOFILESTREAM"));
        }

        /// <summary>
        /// Replaces every occurence of a string with the replacement using regular expressions
        /// </summary>
        /// <param name="From">Regular expression to be replaced</param>
        /// <param name="With">String to replace with</param>
        public static void ReplaceRegex(string From, string With)
        {
            if (string.IsNullOrEmpty(From))
                throw new KernelException(KernelExceptionType.TextEditor, nameof(From));
            if (TextEditShellCommon.FileStream is not null)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Source: {0}, Target: {1}", vars: [From, With]);
                for (int LineIndex = 0; LineIndex <= TextEditShellCommon.FileLines.Count - 1; LineIndex++)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Replacing \"{0}\" with \"{1}\" in line {2}", vars: [From, With, LineIndex + 1]);
                    TextEditShellCommon.FileLines[LineIndex] = Regex.Replace(TextEditShellCommon.FileLines[LineIndex], From, With);
                }
            }
            else
                throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_NOFILESTREAM"));
        }

        /// <summary>
        /// Replaces every occurence of a string with the replacement using regular expressions
        /// </summary>
        /// <param name="From">Regular expression to be replaced</param>
        /// <param name="With">String to replace with</param>
        /// <param name="LineNumber">The line number</param>
        public static void ReplaceRegex(string From, string With, int LineNumber)
        {
            if (string.IsNullOrEmpty(From))
                throw new KernelException(KernelExceptionType.TextEditor, nameof(From));
            if (TextEditShellCommon.FileStream is not null)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Source: {0}, Target: {1}, Line Number: {2}", vars: [From, With, LineNumber]);
                DebugWriter.WriteDebug(DebugLevel.I, "File lines: {0}", vars: [TextEditShellCommon.FileLines.Count]);
                long LineIndex = LineNumber - 1;
                if (LineNumber <= TextEditShellCommon.FileLines.Count)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Replacing \"{0}\" with \"{1}\" in line {2}", vars: [From, With, LineIndex + 1L]);
                    TextEditShellCommon.FileLines[(int)LineIndex] = Regex.Replace(TextEditShellCommon.FileLines[(int)LineIndex], From, With);
                }
                else
                    throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_LINENUMEXCEEDSLASTNUM"));
            }
            else
                throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_NOFILESTREAM"));
        }

        /// <summary>
        /// Replaces every occurence of a string with the replacement
        /// </summary>
        /// <param name="From">String to be replaced</param>
        /// <param name="With">String to replace with</param>
        public static void Replace(string From, string With)
        {
            if (string.IsNullOrEmpty(From))
                throw new KernelException(KernelExceptionType.TextEditor, nameof(From));
            if (TextEditShellCommon.FileStream is not null)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Source: {0}, Target: {1}", vars: [From, With]);
                for (int LineIndex = 0; LineIndex <= TextEditShellCommon.FileLines.Count - 1; LineIndex++)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Replacing \"{0}\" with \"{1}\" in line {2}", vars: [From, With, LineIndex + 1]);
                    TextEditShellCommon.FileLines[LineIndex] = TextEditShellCommon.FileLines[LineIndex].Replace(From, With);
                }
            }
            else
                throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_NOFILESTREAM"));
        }

        /// <summary>
        /// Replaces every occurence of a string with the replacement
        /// </summary>
        /// <param name="From">String to be replaced</param>
        /// <param name="With">String to replace with</param>
        /// <param name="LineNumber">The line number</param>
        public static void Replace(string From, string With, int LineNumber)
        {
            if (string.IsNullOrEmpty(From))
                throw new KernelException(KernelExceptionType.TextEditor, nameof(From));
            if (TextEditShellCommon.FileStream is not null)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Source: {0}, Target: {1}, Line Number: {2}", vars: [From, With, LineNumber]);
                DebugWriter.WriteDebug(DebugLevel.I, "File lines: {0}", vars: [TextEditShellCommon.FileLines.Count]);
                long LineIndex = LineNumber - 1;
                if (LineNumber <= TextEditShellCommon.FileLines.Count)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Replacing \"{0}\" with \"{1}\" in line {2}", vars: [From, With, LineIndex + 1L]);
                    TextEditShellCommon.FileLines[(int)LineIndex] = TextEditShellCommon.FileLines[(int)LineIndex].Replace(From, With);
                }
                else
                    throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_LINENUMEXCEEDSLASTNUM"));
            }
            else
                throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_NOFILESTREAM"));
        }

        /// <summary>
        /// Deletes a word or a phrase from the line
        /// </summary>
        /// <param name="Word">The word or phrase</param>
        /// <param name="LineNumber">The line number</param>
        public static void DeleteWord(string Word, int LineNumber)
        {
            if (string.IsNullOrEmpty(Word))
                throw new KernelException(KernelExceptionType.TextEditor, nameof(Word));
            if (TextEditShellCommon.FileStream is not null)
            {
                int LineIndex = LineNumber - 1;
                DebugWriter.WriteDebug(DebugLevel.I, "Word/Phrase: {0}, Line: {1}", vars: [Word, LineNumber]);
                DebugWriter.WriteDebug(DebugLevel.I, "Got line index: {0}", vars: [LineIndex]);
                DebugWriter.WriteDebug(DebugLevel.I, "File lines: {0}", vars: [TextEditShellCommon.FileLines.Count]);
                if (LineNumber <= TextEditShellCommon.FileLines.Count)
                {
                    TextEditShellCommon.FileLines[LineIndex] = TextEditShellCommon.FileLines[LineIndex].Replace(Word, "");
                    DebugWriter.WriteDebug(DebugLevel.I, "Removed {0}. Result: {1}", vars: [LineIndex, TextEditShellCommon.FileLines.Count]);
                }
                else
                    throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_LINENUMEXCEEDSLASTNUM"));
            }
            else
                throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_NOFILESTREAM"));
        }

        /// <summary>
        /// Deletes a character from the line
        /// </summary>
        /// <param name="CharNumber">The character number</param>
        /// <param name="LineNumber">The line number</param>
        public static void DeleteChar(int CharNumber, int LineNumber)
        {
            if (TextEditShellCommon.FileStream is not null)
            {
                int LineIndex = LineNumber - 1;
                int CharIndex = CharNumber - 1;
                DebugWriter.WriteDebug(DebugLevel.I, "Char number: {0}, Line: {1}", vars: [CharNumber, LineNumber]);
                DebugWriter.WriteDebug(DebugLevel.I, "Got line index: {0}", vars: [LineIndex]);
                DebugWriter.WriteDebug(DebugLevel.I, "Got char index: {0}", vars: [CharIndex]);
                DebugWriter.WriteDebug(DebugLevel.I, "File lines: {0}", vars: [TextEditShellCommon.FileLines.Count]);
                if (LineNumber <= TextEditShellCommon.FileLines.Count)
                {
                    TextEditShellCommon.FileLines[LineIndex] = TextEditShellCommon.FileLines[LineIndex].Remove(CharIndex, 1);
                    DebugWriter.WriteDebug(DebugLevel.I, "Removed {0}. Result: {1}", vars: [LineIndex, TextEditShellCommon.FileLines[LineIndex]]);
                }
                else
                    throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_LINENUMEXCEEDSLASTNUM"));
            }
            else
                throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_NOFILESTREAM"));
        }

        /// <summary>
        /// Queries a character in all lines.
        /// </summary>
        /// <param name="Char">The character to query</param>
        public static List<(int, int[])> QueryChar(char Char)
        {
            if (TextEditShellCommon.FileStream is not null)
            {
                var Lines = new List<(int, int[])>();
                DebugWriter.WriteDebug(DebugLevel.I, "Char: {0}", vars: [Char]);
                DebugWriter.WriteDebug(DebugLevel.I, "File lines: {0}", vars: [TextEditShellCommon.FileLines.Count]);
                for (int LineIndex = 0; LineIndex <= TextEditShellCommon.FileLines.Count - 1; LineIndex++)
                {
                    List<int> charIndexes = [];
                    for (int CharIndex = 0; CharIndex <= TextEditShellCommon.FileLines[LineIndex].Length - 1; CharIndex++)
                    {
                        if (TextEditShellCommon.FileLines[LineIndex][CharIndex] == Char)
                            charIndexes.Add(CharIndex);
                    }
                    Lines.Add((LineIndex, charIndexes.ToArray()));
                }
                return Lines;
            }
            else
                throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_NOFILESTREAM"));
        }

        /// <summary>
        /// Queries a character in specific line.
        /// </summary>
        /// <param name="Char">The character to query</param>
        /// <param name="LineNumber">The line number</param>
        public static List<int> QueryChar(char Char, int LineNumber)
        {
            if (TextEditShellCommon.FileStream is not null)
            {
                int LineIndex = LineNumber - 1;
                var Results = new List<int>();
                DebugWriter.WriteDebug(DebugLevel.I, "Char: {0}, Line: {1}", vars: [Char, LineNumber]);
                DebugWriter.WriteDebug(DebugLevel.I, "Got line index: {0}", vars: [LineIndex]);
                DebugWriter.WriteDebug(DebugLevel.I, "File lines: {0}", vars: [TextEditShellCommon.FileLines.Count]);
                if (LineNumber <= TextEditShellCommon.FileLines.Count)
                {
                    for (int CharIndex = 0; CharIndex <= TextEditShellCommon.FileLines[LineIndex].Length - 1; CharIndex++)
                    {
                        if (TextEditShellCommon.FileLines[LineIndex][CharIndex] == Char)
                            Results.Add(CharIndex);
                    }
                }
                else
                    throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_LINENUMEXCEEDSLASTNUM"));
                return Results;
            }
            else
                throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_NOFILESTREAM"));
        }

        /// <summary>
        /// Queries a word in all lines.
        /// </summary>
        /// <param name="Word">The word to query</param>
        public static List<(int, int[])> QueryWord(string Word)
        {
            if (TextEditShellCommon.FileStream is not null)
            {
                var Lines = new List<(int, int[])>();
                DebugWriter.WriteDebug(DebugLevel.I, "Word: {0}", vars: [Word]);
                DebugWriter.WriteDebug(DebugLevel.I, "File lines: {0}", vars: [TextEditShellCommon.FileLines.Count]);
                for (int LineIndex = 0; LineIndex <= TextEditShellCommon.FileLines.Count - 1; LineIndex++)
                {
                    var Words = TextEditShellCommon.FileLines[LineIndex].Split(' ');
                    List<int> wordIndexes = [];
                    for (int WordIndex = 0; WordIndex <= Words.Length - 1; WordIndex++)
                    {
                        if (Words[WordIndex].ToLower().Contains(Word.ToLower()))
                            wordIndexes.Add(WordIndex);
                    }
                    Lines.Add((LineIndex, wordIndexes.ToArray()));
                }
                return Lines;
            }
            else
                throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_NOFILESTREAM"));
        }

        /// <summary>
        /// Queries a word in specific line.
        /// </summary>
        /// <param name="Word">The word to query</param>
        /// <param name="LineNumber">The line number</param>
        public static List<int> QueryWord(string Word, int LineNumber)
        {
            if (TextEditShellCommon.FileStream is not null)
            {
                int LineIndex = LineNumber - 1;
                var Results = new List<int>();
                DebugWriter.WriteDebug(DebugLevel.I, "Word: {0}, Line: {1}", vars: [Word, LineNumber]);
                DebugWriter.WriteDebug(DebugLevel.I, "Got line index: {0}", vars: [LineIndex]);
                DebugWriter.WriteDebug(DebugLevel.I, "File lines: {0}", vars: [TextEditShellCommon.FileLines.Count]);
                if (LineNumber <= TextEditShellCommon.FileLines.Count)
                {
                    var Words = TextEditShellCommon.FileLines[LineIndex].Split(' ');
                    for (int WordIndex = 0; WordIndex <= Words.Length - 1; WordIndex++)
                    {
                        if (Words[WordIndex].ToLower().Contains(Word.ToLower()))
                            Results.Add(WordIndex);
                    }
                }
                else
                    throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_LINENUMEXCEEDSLASTNUM"));
                return Results;
            }
            else
                throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_NOFILESTREAM"));
        }

        /// <summary>
        /// Queries a word in all lines using regular expressions
        /// </summary>
        /// <param name="Word">The regular expression to query</param>
        public static List<(int, int[])> QueryWordRegex(string Word)
        {
            if (TextEditShellCommon.FileStream is not null)
            {
                var Lines = new List<(int, int[])>();
                DebugWriter.WriteDebug(DebugLevel.I, "Word: {0}", vars: [Word]);
                DebugWriter.WriteDebug(DebugLevel.I, "File lines: {0}", vars: [TextEditShellCommon.FileLines.Count]);
                for (int LineIndex = 0; LineIndex <= TextEditShellCommon.FileLines.Count - 1; LineIndex++)
                {
                    var LineMatches = Regex.Matches(TextEditShellCommon.FileLines[LineIndex], Word);
                    List<int> wordIndexes = [];
                    for (int MatchIndex = 0; MatchIndex <= LineMatches.Count - 1; MatchIndex++)
                        wordIndexes.Add(MatchIndex);
                    Lines.Add((LineIndex, wordIndexes.ToArray()));
                }
                return Lines;
            }
            else
                throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_NOFILESTREAM"));
        }

        /// <summary>
        /// Queries a word in specific line using regular expressions
        /// </summary>
        /// <param name="Word">The regular expression to query</param>
        /// <param name="LineNumber">The line number</param>
        public static List<int> QueryWordRegex(string Word, int LineNumber)
        {
            if (TextEditShellCommon.FileStream is not null)
            {
                int LineIndex = LineNumber - 1;
                var Results = new List<int>();
                DebugWriter.WriteDebug(DebugLevel.I, "Word: {0}, Line: {1}", vars: [Word, LineNumber]);
                DebugWriter.WriteDebug(DebugLevel.I, "Got line index: {0}", vars: [LineIndex]);
                DebugWriter.WriteDebug(DebugLevel.I, "File lines: {0}", vars: [TextEditShellCommon.FileLines.Count]);
                if (LineNumber <= TextEditShellCommon.FileLines.Count)
                {
                    var LineMatches = Regex.Matches(TextEditShellCommon.FileLines[LineIndex], Word);
                    for (int MatchIndex = 0; MatchIndex <= LineMatches.Count - 1; MatchIndex++)
                        Results.Add(MatchIndex);
                }
                else
                    throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_LINENUMEXCEEDSLASTNUM"));
                return Results;
            }
            else
                throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_NOFILESTREAM"));
        }

        /// <summary>
        /// Adds a new line to the current text
        /// </summary>
        /// <param name="lines">List of text lines</param>
        /// <param name="Content">New line content</param>
        public static List<string> AddNewLine(List<string> lines, string Content)
        {
            if (lines is not null)
                lines.Add(Content);
            else
                throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_NOLIST"));
            return lines;
        }

        /// <summary>
        /// Adds the new lines to the current text
        /// </summary>
        /// <param name="lines">List of text lines</param>
        /// <param name="Lines">New lines</param>
        public static List<string> AddNewLines(List<string> lines, string[] Lines)
        {
            if (lines is not null)
            {
                foreach (string Line in Lines)
                    lines.Add(Line);
            }
            else
                throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_NOLIST"));
            return lines;
        }

        /// <summary>
        /// Removes a line from the current text
        /// </summary>
        /// <param name="lines">List of text lines</param>
        /// <param name="LineNumber">The line number to remove</param>
        public static List<string> RemoveLine(List<string> lines, int LineNumber)
        {
            if (lines is not null)
            {
                int LineIndex = LineNumber - 1;
                DebugWriter.WriteDebug(DebugLevel.I, "Got line index: {0}", vars: [LineIndex]);
                DebugWriter.WriteDebug(DebugLevel.I, "Old file lines: {0}", vars: [lines.Count]);
                if (LineNumber <= lines.Count)
                {
                    lines.RemoveAt(LineIndex);
                    DebugWriter.WriteDebug(DebugLevel.I, "New file lines: {0}", vars: [lines.Count]);
                }
                else
                    throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_LINENUMEXCEEDSLASTNUM"));
            }
            else
                throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_NOLIST"));
            return lines;
        }

        /// <summary>
        /// Replaces every occurence of a string with the replacement using regular expressions
        /// </summary>
        /// <param name="lines">List of text lines</param>
        /// <param name="From">Regular expression to be replaced</param>
        /// <param name="With">String to replace with</param>
        public static List<string> ReplaceRegex(List<string> lines, string From, string With)
        {
            if (string.IsNullOrEmpty(From))
                throw new KernelException(KernelExceptionType.TextEditor, nameof(From));
            if (lines is not null)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Source: {0}, Target: {1}", vars: [From, With]);
                for (int LineIndex = 0; LineIndex <= lines.Count - 1; LineIndex++)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Replacing \"{0}\" with \"{1}\" in line {2}", vars: [From, With, LineIndex + 1]);
                    lines[LineIndex] = Regex.Replace(lines[LineIndex], From, With);
                }
            }
            else
                throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_NOLIST"));
            return lines;
        }

        /// <summary>
        /// Replaces every occurence of a string with the replacement using regular expressions
        /// </summary>
        /// <param name="lines">List of text lines</param>
        /// <param name="From">Regular expression to be replaced</param>
        /// <param name="With">String to replace with</param>
        /// <param name="LineNumber">The line number</param>
        public static List<string> ReplaceRegex(List<string> lines, string From, string With, int LineNumber)
        {
            if (string.IsNullOrEmpty(From))
                throw new KernelException(KernelExceptionType.TextEditor, nameof(From));
            if (lines is not null)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Source: {0}, Target: {1}, Line Number: {2}", vars: [From, With, LineNumber]);
                DebugWriter.WriteDebug(DebugLevel.I, "File lines: {0}", vars: [lines.Count]);
                long LineIndex = LineNumber - 1;
                if (LineNumber <= lines.Count)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Replacing \"{0}\" with \"{1}\" in line {2}", vars: [From, With, LineIndex + 1L]);
                    lines[(int)LineIndex] = Regex.Replace(lines[(int)LineIndex], From, With);
                }
                else
                    throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_LINENUMEXCEEDSLASTNUM"));
            }
            else
                throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_NOLIST"));
            return lines;
        }

        /// <summary>
        /// Replaces every occurence of a string with the replacement
        /// </summary>
        /// <param name="lines">List of text lines</param>
        /// <param name="From">String to be replaced</param>
        /// <param name="With">String to replace with</param>
        public static List<string> Replace(List<string> lines, string From, string With)
        {
            if (string.IsNullOrEmpty(From))
                throw new KernelException(KernelExceptionType.TextEditor, nameof(From));
            if (lines is not null)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Source: {0}, Target: {1}", vars: [From, With]);
                for (int LineIndex = 0; LineIndex <= lines.Count - 1; LineIndex++)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Replacing \"{0}\" with \"{1}\" in line {2}", vars: [From, With, LineIndex + 1]);
                    lines[LineIndex] = lines[LineIndex].Replace(From, With);
                }
            }
            else
                throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_NOLIST"));
            return lines;
        }

        /// <summary>
        /// Replaces every occurence of a string with the replacement
        /// </summary>
        /// <param name="lines">List of text lines</param>
        /// <param name="From">String to be replaced</param>
        /// <param name="With">String to replace with</param>
        /// <param name="LineNumber">The line number</param>
        public static List<string> Replace(List<string> lines, string From, string With, int LineNumber)
        {
            if (string.IsNullOrEmpty(From))
                throw new KernelException(KernelExceptionType.TextEditor, nameof(From));
            if (lines is not null)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Source: {0}, Target: {1}, Line Number: {2}", vars: [From, With, LineNumber]);
                DebugWriter.WriteDebug(DebugLevel.I, "File lines: {0}", vars: [lines.Count]);
                long LineIndex = LineNumber - 1;
                if (LineNumber <= lines.Count)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Replacing \"{0}\" with \"{1}\" in line {2}", vars: [From, With, LineIndex + 1L]);
                    lines[(int)LineIndex] = lines[(int)LineIndex].Replace(From, With);
                }
                else
                    throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_LINENUMEXCEEDSLASTNUM"));
            }
            else
                throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_NOLIST"));
            return lines;
        }

        /// <summary>
        /// Deletes a word or a phrase from the line
        /// </summary>
        /// <param name="lines">List of text lines</param>
        /// <param name="Word">The word or phrase</param>
        /// <param name="LineNumber">The line number</param>
        public static List<string> DeleteWord(List<string> lines, string Word, int LineNumber)
        {
            if (string.IsNullOrEmpty(Word))
                throw new KernelException(KernelExceptionType.TextEditor, nameof(Word));
            if (lines is not null)
            {
                int LineIndex = LineNumber - 1;
                DebugWriter.WriteDebug(DebugLevel.I, "Word/Phrase: {0}, Line: {1}", vars: [Word, LineNumber]);
                DebugWriter.WriteDebug(DebugLevel.I, "Got line index: {0}", vars: [LineIndex]);
                DebugWriter.WriteDebug(DebugLevel.I, "File lines: {0}", vars: [lines.Count]);
                if (LineNumber <= lines.Count)
                {
                    lines[LineIndex] = lines[LineIndex].Replace(Word, "");
                    DebugWriter.WriteDebug(DebugLevel.I, "Removed {0}. Result: {1}", vars: [LineIndex, lines.Count]);
                }
                else
                    throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_LINENUMEXCEEDSLASTNUM"));
            }
            else
                throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_NOLIST"));
            return lines;
        }

        /// <summary>
        /// Deletes a character from the line
        /// </summary>
        /// <param name="lines">List of text lines</param>
        /// <param name="CharNumber">The character number</param>
        /// <param name="LineNumber">The line number</param>
        public static List<string> DeleteChar(List<string> lines, int CharNumber, int LineNumber)
        {
            if (lines is not null)
            {
                int LineIndex = LineNumber - 1;
                int CharIndex = CharNumber - 1;
                DebugWriter.WriteDebug(DebugLevel.I, "Char number: {0}, Line: {1}", vars: [CharNumber, LineNumber]);
                DebugWriter.WriteDebug(DebugLevel.I, "Got line index: {0}", vars: [LineIndex]);
                DebugWriter.WriteDebug(DebugLevel.I, "Got char index: {0}", vars: [CharIndex]);
                DebugWriter.WriteDebug(DebugLevel.I, "File lines: {0}", vars: [lines.Count]);
                if (LineNumber <= lines.Count)
                {
                    lines[LineIndex] = lines[LineIndex].Remove(CharIndex, 1);
                    DebugWriter.WriteDebug(DebugLevel.I, "Removed {0}. Result: {1}", vars: [LineIndex, lines[LineIndex]]);
                }
                else
                    throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_LINENUMEXCEEDSLASTNUM"));
            }
            else
                throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_NOLIST"));
            return lines;
        }

        /// <summary>
        /// Queries a character in all lines.
        /// </summary>
        /// <param name="lines">List of text lines</param>
        /// <param name="Char">The character to query</param>
        public static List<(int, int[])> QueryChar(List<string> lines, char Char)
        {
            if (lines is not null)
            {
                var Lines = new List<(int, int[])>();
                DebugWriter.WriteDebug(DebugLevel.I, "Char: {0}", vars: [Char]);
                DebugWriter.WriteDebug(DebugLevel.I, "File lines: {0}", vars: [lines.Count]);
                for (int LineIndex = 0; LineIndex <= lines.Count - 1; LineIndex++)
                {
                    List<int> charIndexes = [];
                    for (int CharIndex = 0; CharIndex <= lines[LineIndex].Length - 1; CharIndex++)
                    {
                        if (lines[LineIndex][CharIndex] == Char)
                            charIndexes.Add(CharIndex);
                    }
                    Lines.Add((LineIndex, charIndexes.ToArray()));
                }
                return Lines;
            }
            else
                throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_NOLIST"));
        }

        /// <summary>
        /// Queries a character in specific line.
        /// </summary>
        /// <param name="lines">List of text lines</param>
        /// <param name="Char">The character to query</param>
        /// <param name="LineNumber">The line number</param>
        public static List<int> QueryChar(List<string> lines, char Char, int LineNumber)
        {
            if (lines is not null)
            {
                int LineIndex = LineNumber - 1;
                var Results = new List<int>();
                DebugWriter.WriteDebug(DebugLevel.I, "Char: {0}, Line: {1}", vars: [Char, LineNumber]);
                DebugWriter.WriteDebug(DebugLevel.I, "Got line index: {0}", vars: [LineIndex]);
                DebugWriter.WriteDebug(DebugLevel.I, "File lines: {0}", vars: [lines.Count]);
                if (LineNumber <= lines.Count)
                {
                    for (int CharIndex = 0; CharIndex <= lines[LineIndex].Length - 1; CharIndex++)
                    {
                        if (lines[LineIndex][CharIndex] == Char)
                            Results.Add(CharIndex);
                    }
                }
                else
                    throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_LINENUMEXCEEDSLASTNUM"));
                return Results;
            }
            else
                throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_NOLIST"));
        }

        /// <summary>
        /// Queries a word in all lines.
        /// </summary>
        /// <param name="lines">List of text lines</param>
        /// <param name="Word">The word to query</param>
        public static List<(int, int[])> QueryWord(List<string> lines, string Word)
        {
            if (lines is not null)
            {
                var Lines = new List<(int, int[])>();
                DebugWriter.WriteDebug(DebugLevel.I, "Word: {0}", vars: [Word]);
                DebugWriter.WriteDebug(DebugLevel.I, "File lines: {0}", vars: [lines.Count]);
                for (int LineIndex = 0; LineIndex <= lines.Count - 1; LineIndex++)
                {
                    var Words = lines[LineIndex].Split(' ');
                    List<int> wordIndexes = [];
                    for (int WordIndex = 0; WordIndex <= Words.Length - 1; WordIndex++)
                    {
                        if (Words[WordIndex].ToLower().Contains(Word.ToLower()))
                            wordIndexes.Add(WordIndex);
                    }
                    Lines.Add((LineIndex, wordIndexes.ToArray()));
                }
                return Lines;
            }
            else
                throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_NOLIST"));
        }

        /// <summary>
        /// Queries a word in specific line.
        /// </summary>
        /// <param name="lines">List of text lines</param>
        /// <param name="Word">The word to query</param>
        /// <param name="LineNumber">The line number</param>
        public static List<int> QueryWord(List<string> lines, string Word, int LineNumber)
        {
            if (lines is not null)
            {
                int LineIndex = LineNumber - 1;
                var Results = new List<int>();
                DebugWriter.WriteDebug(DebugLevel.I, "Word: {0}, Line: {1}", vars: [Word, LineNumber]);
                DebugWriter.WriteDebug(DebugLevel.I, "Got line index: {0}", vars: [LineIndex]);
                DebugWriter.WriteDebug(DebugLevel.I, "File lines: {0}", vars: [lines.Count]);
                if (LineNumber <= lines.Count)
                {
                    var Words = lines[LineIndex].Split(' ');
                    for (int WordIndex = 0; WordIndex <= Words.Length - 1; WordIndex++)
                    {
                        if (Words[WordIndex].ToLower().Contains(Word.ToLower()))
                            Results.Add(WordIndex);
                    }
                }
                else
                    throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_LINENUMEXCEEDSLASTNUM"));
                return Results;
            }
            else
                throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_NOLIST"));
        }

        /// <summary>
        /// Queries a word in all lines using regular expressions
        /// </summary>
        /// <param name="lines">List of text lines</param>
        /// <param name="Word">The regular expression to query</param>
        public static List<(int, int[])> QueryWordRegex(List<string> lines, string Word)
        {
            if (lines is not null)
            {
                var Lines = new List<(int, int[])>();
                DebugWriter.WriteDebug(DebugLevel.I, "Word: {0}", vars: [Word]);
                DebugWriter.WriteDebug(DebugLevel.I, "File lines: {0}", vars: [lines.Count]);
                for (int LineIndex = 0; LineIndex <= lines.Count - 1; LineIndex++)
                {
                    var LineMatches = Regex.Matches(lines[LineIndex], Word);
                    List<int> wordIndexes = [];
                    for (int MatchIndex = 0; MatchIndex <= LineMatches.Count - 1; MatchIndex++)
                        wordIndexes.Add(MatchIndex);
                    Lines.Add((LineIndex, wordIndexes.ToArray()));
                }
                return Lines;
            }
            else
                throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_NOLIST"));
        }

        /// <summary>
        /// Queries a word in specific line using regular expressions
        /// </summary>
        /// <param name="lines">List of text lines</param>
        /// <param name="Word">The regular expression to query</param>
        /// <param name="LineNumber">The line number</param>
        public static List<int> QueryWordRegex(List<string> lines, string Word, int LineNumber)
        {
            if (lines is not null)
            {
                int LineIndex = LineNumber - 1;
                var Results = new List<int>();
                DebugWriter.WriteDebug(DebugLevel.I, "Word: {0}, Line: {1}", vars: [Word, LineNumber]);
                DebugWriter.WriteDebug(DebugLevel.I, "Got line index: {0}", vars: [LineIndex]);
                DebugWriter.WriteDebug(DebugLevel.I, "File lines: {0}", vars: [lines.Count]);
                if (LineNumber <= lines.Count)
                {
                    var LineMatches = Regex.Matches(lines[LineIndex], Word);
                    for (int MatchIndex = 0; MatchIndex <= LineMatches.Count - 1; MatchIndex++)
                        Results.Add(MatchIndex);
                }
                else
                    throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_LINENUMEXCEEDSLASTNUM"));
                return Results;
            }
            else
                throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_NOLIST"));
        }

    }
}
