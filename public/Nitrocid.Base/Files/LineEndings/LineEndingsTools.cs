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
using Textify.General;
using Nitrocid.Base.Drivers;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Files.LineEndings;

namespace Nitrocid.Base.Files
{
    /// <summary>
    /// Line endings tools module
    /// </summary>
    public static partial class FilesystemTools
    {

        /// <summary>
        /// The new line style used for the current platform
        /// </summary>
        public static FilesystemNewlineStyle NewlineStyle
        {
            get
            {
                if (CharManager.NewLine == $"{Convert.ToChar(13)}{Convert.ToChar(10)}")
                    return FilesystemNewlineStyle.CRLF;
                else if (CharManager.NewLine == Convert.ToChar(13).ToString())
                    return FilesystemNewlineStyle.CR;
                else if (CharManager.NewLine == Convert.ToChar(10).ToString())
                    return FilesystemNewlineStyle.LF;
                else
                    return FilesystemNewlineStyle.CRLF;
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
                        return $"{Convert.ToChar(13)}{Convert.ToChar(10)}";
                    }
                case FilesystemNewlineStyle.LF:
                    {
                        return Convert.ToChar(10).ToString();
                    }
                case FilesystemNewlineStyle.CR:
                    {
                        return Convert.ToChar(13).ToString();
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
        public static FilesystemNewlineStyle GetLineEndingFromFile(string TextFile) =>
            GetLineEndingFromFile(TextFile, false);

        /// <summary>
        /// Gets the line ending style from file
        /// </summary>
        /// <param name="TextFile">Target text file</param>
        /// <param name="force">Forces line ending conversion</param>
        public static FilesystemNewlineStyle GetLineEndingFromFile(string TextFile, bool force)
        {
            if (DriverHandler.CurrentFilesystemDriverLocal.IsBinaryFile(TextFile) && !force)
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_NEEDSTEXTFILE"));
            return DriverHandler.CurrentFilesystemDriverLocal.GetLineEndingFromFile(TextFile);
        }

    }
}
