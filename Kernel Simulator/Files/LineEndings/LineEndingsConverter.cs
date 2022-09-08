
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.IO;
using System.Text;
using Extensification.StringExts;
using KS.Files.Querying;
using KS.Files.Read;
using KS.Kernel.Debugging;
using KS.Languages;

namespace KS.Files.LineEndings
{
    public static class LineEndingsConverter
    {

        /// <summary>
        /// Converts the line endings to the newline style for the current platform
        /// </summary>
        /// <param name="TextFile">Text file name with extension or file path</param>
        public static void ConvertLineEndings(string TextFile) => ConvertLineEndings(TextFile, LineEndingsTools.NewlineStyle);

        /// <summary>
        /// Converts the line endings to the specified newline style
        /// </summary>
        /// <param name="TextFile">Text file name with extension or file path</param>
        /// <param name="LineEndingStyle">Line ending style</param>
        public static void ConvertLineEndings(string TextFile, FilesystemNewlineStyle LineEndingStyle)
        {
            Filesystem.ThrowOnInvalidPath(TextFile);
            TextFile = Filesystem.NeutralizePath(TextFile);
            if (!Checking.FileExists(TextFile))
                throw new IOException(Translate.DoTranslation("File {0} not found.").FormatString(TextFile));

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

    }
}