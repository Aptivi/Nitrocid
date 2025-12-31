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

using Nitrocid.Base.Drivers;
using Nitrocid.Base.Files.LineEndings;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;

namespace Nitrocid.Base.Files
{
    /// <summary>
    /// Line endings conversion module
    /// </summary>
    public static partial class FilesystemTools
    {

        /// <summary>
        /// Converts the line endings to the newline style for the current platform
        /// </summary>
        /// <param name="TextFile">Text file name with extension or file path</param>
        public static void ConvertLineEndings(string TextFile) =>
            ConvertLineEndings(TextFile, false);

        /// <summary>
        /// Converts the line endings to the specified newline style
        /// </summary>
        /// <param name="TextFile">Text file name with extension or file path</param>
        /// <param name="LineEndingStyle">Line ending style</param>
        public static void ConvertLineEndings(string TextFile, FilesystemNewlineStyle LineEndingStyle) =>
            ConvertLineEndings(TextFile, LineEndingStyle, false);

        /// <summary>
        /// Converts the line endings to the newline style for the current platform
        /// </summary>
        /// <param name="TextFile">Text file name with extension or file path</param>
        /// <param name="force">Forces line ending conversion</param>
        public static void ConvertLineEndings(string TextFile, bool force)
        {
            if (DriverHandler.CurrentFilesystemDriverLocal.IsBinaryFile(TextFile) && !force)
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_NEEDSTEXTFILE"));
            DriverHandler.CurrentFilesystemDriverLocal.ConvertLineEndings(TextFile);
        }

        /// <summary>
        /// Converts the line endings to the specified newline style
        /// </summary>
        /// <param name="TextFile">Text file name with extension or file path</param>
        /// <param name="LineEndingStyle">Line ending style</param>
        /// <param name="force">Forces line ending conversion</param>
        public static void ConvertLineEndings(string TextFile, FilesystemNewlineStyle LineEndingStyle, bool force)
        {
            if (DriverHandler.CurrentFilesystemDriverLocal.IsBinaryFile(TextFile) && !force)
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_NEEDSTEXTFILE"));
            DriverHandler.CurrentFilesystemDriverLocal.ConvertLineEndings(TextFile, LineEndingStyle);
        }

    }
}
