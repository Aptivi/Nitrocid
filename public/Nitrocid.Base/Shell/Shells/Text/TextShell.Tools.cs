extern alias TextifyDep;
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Nitrocid.Base.Files;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Shells;
using TextifyDep::Textify.General;

namespace Nitrocid.Base.Shell.Shells.Text
{
    /// <summary>
    /// The text editor shell
    /// </summary>
    public partial class TextShell : BaseShell, IShell
    {

        /// <summary>
        /// Opens the text file
        /// </summary>
        /// <param name="File">Target file. We recommend you to use <see cref="FilesystemTools.NeutralizePath(string, bool)"></see> to neutralize path.</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public bool OpenTextFile(string File)
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to open file {0}...", vars: [File]);
                fileStream = new FileStream(File, FileMode.Open);
                if (FileStream is null)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_NOTOPENYET"));
                fileLines ??= [];
                FileLinesOrig ??= [];
                DebugWriter.WriteDebug(DebugLevel.I, "File {0} is open. Length: {1}, Pos: {2}", vars: [File, FileStream.Length, FileStream.Position]);
                var TextFileStreamReader = new StreamReader(FileStream);
                while (!TextFileStreamReader.EndOfStream)
                {
                    string StreamLine = TextFileStreamReader.ReadLine() ?? "";
                    FileLines.Add(StreamLine);
                    FileLinesOrig.Add(StreamLine);
                }
                FileStream.Seek(0L, SeekOrigin.Begin);
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
        public bool CloseTextFile()
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to close file...");
                FileStream?.Close();
                fileStream = null;
                DebugWriter.WriteDebug(DebugLevel.I, "File is no longer open.");
                FileLines.Clear();
                FileLinesOrig.Clear();
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
        public bool SaveTextFile(bool ClearLines)
        {
            try
            {
                if (FileStream is null)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_NOTOPENYET"));
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to save file...");
                FileStream.SetLength(0L);
                DebugWriter.WriteDebug(DebugLevel.I, "Length set to 0.");
                var FileLinesByte = Encoding.Default.GetBytes(string.Join(CharManager.NewLine, [.. FileLines]));
                DebugWriter.WriteDebug(DebugLevel.I, "Converted lines to bytes. Length: {0}", vars: [FileLinesByte.Length]);
                FileStream.Write(FileLinesByte, 0, FileLinesByte.Length);
                FileStream.Flush();
                DebugWriter.WriteDebug(DebugLevel.I, "File is saved.");
                if (ClearLines)
                    FileLines.Clear();
                FileLinesOrig.Clear();
                FileLinesOrig.AddRange(FileLines);
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
        /// Was text edited?
        /// </summary>
        public bool WasTextEdited()
        {
            if (FileLines is not null && FileLinesOrig is not null)
                return !FileLines.SequenceEqual(FileLinesOrig);
            return false;
        }

        /// <summary>
        /// Adds a new line to the current text
        /// </summary>
        /// <param name="Content">New line content</param>
        public void AddNewLine(string Content)
        {
            if (FileStream is not null)
                FileLines.Add(Content);
            else
                throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_NOFILESTREAM"));
        }

        /// <summary>
        /// Adds the new lines to the current text
        /// </summary>
        /// <param name="Lines">New lines</param>
        public void AddNewLines(string[] Lines)
        {
            if (FileStream is not null)
            {
                foreach (string Line in Lines)
                    FileLines.Add(Line);
            }
            else
                throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_NOFILESTREAM"));
        }

        /// <summary>
        /// Removes a line from the current text
        /// </summary>
        /// <param name="LineNumber">The line number to remove</param>
        public void RemoveLine(int LineNumber)
        {
            if (FileStream is not null)
            {
                int LineIndex = LineNumber - 1;
                DebugWriter.WriteDebug(DebugLevel.I, "Got line index: {0}", vars: [LineIndex]);
                DebugWriter.WriteDebug(DebugLevel.I, "Old file lines: {0}", vars: [FileLines.Count]);
                if (LineNumber <= FileLines.Count)
                {
                    FileLines.RemoveAt(LineIndex);
                    DebugWriter.WriteDebug(DebugLevel.I, "New file lines: {0}", vars: [FileLines.Count]);
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
        public void ReplaceRegex(string From, string With)
        {
            if (string.IsNullOrEmpty(From))
                throw new KernelException(KernelExceptionType.TextEditor, nameof(From));
            if (FileStream is not null)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Source: {0}, Target: {1}", vars: [From, With]);
                for (int LineIndex = 0; LineIndex <= FileLines.Count - 1; LineIndex++)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Replacing \"{0}\" with \"{1}\" in line {2}", vars: [From, With, LineIndex + 1]);
                    FileLines[LineIndex] = Regex.Replace(FileLines[LineIndex], From, With);
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
        public void ReplaceRegex(string From, string With, int LineNumber)
        {
            if (string.IsNullOrEmpty(From))
                throw new KernelException(KernelExceptionType.TextEditor, nameof(From));
            if (FileStream is not null)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Source: {0}, Target: {1}, Line Number: {2}", vars: [From, With, LineNumber]);
                DebugWriter.WriteDebug(DebugLevel.I, "File lines: {0}", vars: [FileLines.Count]);
                long LineIndex = LineNumber - 1;
                if (LineNumber <= FileLines.Count)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Replacing \"{0}\" with \"{1}\" in line {2}", vars: [From, With, LineIndex + 1L]);
                    FileLines[(int)LineIndex] = Regex.Replace(FileLines[(int)LineIndex], From, With);
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
        public void Replace(string From, string With)
        {
            if (string.IsNullOrEmpty(From))
                throw new KernelException(KernelExceptionType.TextEditor, nameof(From));
            if (FileStream is not null)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Source: {0}, Target: {1}", vars: [From, With]);
                for (int LineIndex = 0; LineIndex <= FileLines.Count - 1; LineIndex++)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Replacing \"{0}\" with \"{1}\" in line {2}", vars: [From, With, LineIndex + 1]);
                    FileLines[LineIndex] = FileLines[LineIndex].Replace(From, With);
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
        public void Replace(string From, string With, int LineNumber)
        {
            if (string.IsNullOrEmpty(From))
                throw new KernelException(KernelExceptionType.TextEditor, nameof(From));
            if (FileStream is not null)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Source: {0}, Target: {1}, Line Number: {2}", vars: [From, With, LineNumber]);
                DebugWriter.WriteDebug(DebugLevel.I, "File lines: {0}", vars: [FileLines.Count]);
                long LineIndex = LineNumber - 1;
                if (LineNumber <= FileLines.Count)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Replacing \"{0}\" with \"{1}\" in line {2}", vars: [From, With, LineIndex + 1L]);
                    FileLines[(int)LineIndex] = FileLines[(int)LineIndex].Replace(From, With);
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
        public void DeleteWord(string Word, int LineNumber)
        {
            if (string.IsNullOrEmpty(Word))
                throw new KernelException(KernelExceptionType.TextEditor, nameof(Word));
            if (FileStream is not null)
            {
                int LineIndex = LineNumber - 1;
                DebugWriter.WriteDebug(DebugLevel.I, "Word/Phrase: {0}, Line: {1}", vars: [Word, LineNumber]);
                DebugWriter.WriteDebug(DebugLevel.I, "Got line index: {0}", vars: [LineIndex]);
                DebugWriter.WriteDebug(DebugLevel.I, "File lines: {0}", vars: [FileLines.Count]);
                if (LineNumber <= FileLines.Count)
                {
                    FileLines[LineIndex] = FileLines[LineIndex].Replace(Word, "");
                    DebugWriter.WriteDebug(DebugLevel.I, "Removed {0}. Result: {1}", vars: [LineIndex, FileLines.Count]);
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
        public void DeleteChar(int CharNumber, int LineNumber)
        {
            if (FileStream is not null)
            {
                int LineIndex = LineNumber - 1;
                int CharIndex = CharNumber - 1;
                DebugWriter.WriteDebug(DebugLevel.I, "Char number: {0}, Line: {1}", vars: [CharNumber, LineNumber]);
                DebugWriter.WriteDebug(DebugLevel.I, "Got line index: {0}", vars: [LineIndex]);
                DebugWriter.WriteDebug(DebugLevel.I, "Got char index: {0}", vars: [CharIndex]);
                DebugWriter.WriteDebug(DebugLevel.I, "File lines: {0}", vars: [FileLines.Count]);
                if (LineNumber <= FileLines.Count)
                {
                    FileLines[LineIndex] = FileLines[LineIndex].Remove(CharIndex, 1);
                    DebugWriter.WriteDebug(DebugLevel.I, "Removed {0}. Result: {1}", vars: [LineIndex, FileLines[LineIndex]]);
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
        public List<(int, int[])> QueryChar(char Char)
        {
            if (FileStream is not null)
            {
                var Lines = new List<(int, int[])>();
                DebugWriter.WriteDebug(DebugLevel.I, "Char: {0}", vars: [Char]);
                DebugWriter.WriteDebug(DebugLevel.I, "File lines: {0}", vars: [FileLines.Count]);
                for (int LineIndex = 0; LineIndex <= FileLines.Count - 1; LineIndex++)
                {
                    List<int> charIndexes = [];
                    for (int CharIndex = 0; CharIndex <= FileLines[LineIndex].Length - 1; CharIndex++)
                    {
                        if (FileLines[LineIndex][CharIndex] == Char)
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
        public List<int> QueryChar(char Char, int LineNumber)
        {
            if (FileStream is not null)
            {
                int LineIndex = LineNumber - 1;
                var Results = new List<int>();
                DebugWriter.WriteDebug(DebugLevel.I, "Char: {0}, Line: {1}", vars: [Char, LineNumber]);
                DebugWriter.WriteDebug(DebugLevel.I, "Got line index: {0}", vars: [LineIndex]);
                DebugWriter.WriteDebug(DebugLevel.I, "File lines: {0}", vars: [FileLines.Count]);
                if (LineNumber <= FileLines.Count)
                {
                    for (int CharIndex = 0; CharIndex <= FileLines[LineIndex].Length - 1; CharIndex++)
                    {
                        if (FileLines[LineIndex][CharIndex] == Char)
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
        public List<(int, int[])> QueryWord(string Word)
        {
            if (FileStream is not null)
            {
                var Lines = new List<(int, int[])>();
                DebugWriter.WriteDebug(DebugLevel.I, "Word: {0}", vars: [Word]);
                DebugWriter.WriteDebug(DebugLevel.I, "File lines: {0}", vars: [FileLines.Count]);
                for (int LineIndex = 0; LineIndex <= FileLines.Count - 1; LineIndex++)
                {
                    var Words = FileLines[LineIndex].Split(' ');
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
        public List<int> QueryWord(string Word, int LineNumber)
        {
            if (FileStream is not null)
            {
                int LineIndex = LineNumber - 1;
                var Results = new List<int>();
                DebugWriter.WriteDebug(DebugLevel.I, "Word: {0}, Line: {1}", vars: [Word, LineNumber]);
                DebugWriter.WriteDebug(DebugLevel.I, "Got line index: {0}", vars: [LineIndex]);
                DebugWriter.WriteDebug(DebugLevel.I, "File lines: {0}", vars: [FileLines.Count]);
                if (LineNumber <= FileLines.Count)
                {
                    var Words = FileLines[LineIndex].Split(' ');
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
        public List<(int, int[])> QueryWordRegex(string Word)
        {
            if (FileStream is not null)
            {
                var Lines = new List<(int, int[])>();
                DebugWriter.WriteDebug(DebugLevel.I, "Word: {0}", vars: [Word]);
                DebugWriter.WriteDebug(DebugLevel.I, "File lines: {0}", vars: [FileLines.Count]);
                for (int LineIndex = 0; LineIndex <= FileLines.Count - 1; LineIndex++)
                {
                    var LineMatches = Regex.Matches(FileLines[LineIndex], Word);
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
        public List<int> QueryWordRegex(string Word, int LineNumber)
        {
            if (FileStream is not null)
            {
                int LineIndex = LineNumber - 1;
                var Results = new List<int>();
                DebugWriter.WriteDebug(DebugLevel.I, "Word: {0}, Line: {1}", vars: [Word, LineNumber]);
                DebugWriter.WriteDebug(DebugLevel.I, "Got line index: {0}", vars: [LineIndex]);
                DebugWriter.WriteDebug(DebugLevel.I, "File lines: {0}", vars: [FileLines.Count]);
                if (LineNumber <= FileLines.Count)
                {
                    var LineMatches = Regex.Matches(FileLines[LineIndex], Word);
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

        private static void HandleAutoSaveTextFile(TextShell? shell)
        {
            if (shell is null)
                throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_LASTSHELLTYPEMISMATCH"));
            if (Config.MainConfig.TextEditAutoSaveFlag)
            {
                try
                {
                    Thread.Sleep(Config.MainConfig.TextEditAutoSaveInterval * 1000);
                    if (shell.FileStream is not null)
                        shell.SaveTextFile(false);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }
        }
    }
}
