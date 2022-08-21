using System;
using System.IO;

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

using static Extensification.CharExts.Querying;
using Extensification.StringExts;
using KS.Files.Querying;
using KS.Languages;
using Microsoft.VisualBasic.CompilerServices;

namespace KS.Files.LineEndings
{
    public static class LineEndingsTools
    {

        /// <summary>
        /// The new line style used for the current platform
        /// </summary>
        public static FilesystemNewlineStyle NewlineStyle
        {
            get
            {
                switch (Environment.NewLine ?? "")
                {
                    case Microsoft.VisualBasic.Constants.vbCrLf:
                        {
                            return FilesystemNewlineStyle.CRLF;
                        }
                    case Microsoft.VisualBasic.Constants.vbLf:
                        {
                            return FilesystemNewlineStyle.LF;
                        }
                    case Microsoft.VisualBasic.Constants.vbCr:
                        {
                            return FilesystemNewlineStyle.CR;
                        }

                    default:
                        {
                            return FilesystemNewlineStyle.CRLF;
                        }
                }
            }
        }

        /// <summary>
        /// Gets the line ending string from the specified line ending style
        /// </summary>
        /// <param name="LineEndingStyle">Line ending style</param>
        public static string GetLineEndingString(FilesystemNewlineStyle LineEndingStyle)
        {
            switch (LineEndingStyle)
            {
                case FilesystemNewlineStyle.CRLF:
                    {
                        return Microsoft.VisualBasic.Constants.vbCrLf;
                    }
                case FilesystemNewlineStyle.LF:
                    {
                        return Microsoft.VisualBasic.Constants.vbLf;
                    }
                case FilesystemNewlineStyle.CR:
                    {
                        return Microsoft.VisualBasic.Constants.vbCr;
                    }

                default:
                    {
                        return Environment.NewLine;
                    }
            }
        }

        /// <summary>
        /// Gets the line ending style from file
        /// </summary>
        /// <param name="TextFile">Target text file</param>
        public static FilesystemNewlineStyle GetLineEndingFromFile(string TextFile)
        {
            Filesystem.ThrowOnInvalidPath(TextFile);
            TextFile = Filesystem.NeutralizePath(TextFile);
            if (!Checking.FileExists(TextFile))
                throw new IOException(Translate.DoTranslation("File {0} not found.").FormatString(TextFile));

            // Open the file stream
            var NewlineStyle = NewlineStyle;
            var TextFileStream = new FileStream(TextFile, FileMode.Open, FileAccess.Read);
            int CarriageReturnCode = Conversions.ToChar(GetLineEndingString(FilesystemNewlineStyle.CR)).GetAsciiCode();
            int LineFeedCode = Conversions.ToChar(GetLineEndingString(FilesystemNewlineStyle.LF)).GetAsciiCode();
            var CarriageReturnSpotted = default(bool);
            var LineFeedSpotted = default(bool);
            var ExitOnSpotted = default(bool);

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

    }
}