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

using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Shells;
using System.IO;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Files.Extensions;

#if NKS_EXTENSIONS
using Nitrocid.Base.Kernel.Extensions;
#endif

namespace Nitrocid.Base.Files
{
    /// <summary>
    /// Routines related to opening the files
    /// </summary>
    public static partial class FilesystemTools
    {
        /// <summary>
        /// Opens the editor deterministically
        /// </summary>
        /// <param name="path">A path to any file that exists</param>
        /// <param name="forceText">Forces text shell</param>
        /// <param name="forceJson">Forces JSON shell</param>
        /// <param name="forceHex">Forces hex shell</param>
        /// <param name="forceSql">Forces SQL shell</param>
        public static void OpenEditor(string path, bool forceText = false, bool forceJson = false, bool forceHex = false, bool forceSql = false)
        {
            bool fileExists = FilesystemTools.FileExists(path);

#if NKS_EXTENSIONS
            // Check the addons
            bool hasShells = AddonTools.GetAddon(InterAddonTranslations.GetAddonName(KnownAddons.AddonShellPacks)) is not null;
#endif

            // Check to see if the file exists
            DebugWriter.WriteDebug(DebugLevel.I, "File path is {0} and .Exists is {1}", vars: [path, fileExists]);
            DebugWriter.WriteDebug(DebugLevel.I, "Force text: {0}", vars: [forceText]);
            DebugWriter.WriteDebug(DebugLevel.I, "Force JSON: {0}", vars: [forceJson]);
            DebugWriter.WriteDebug(DebugLevel.I, "Force Hex: {0}", vars: [forceHex]);
            DebugWriter.WriteDebug(DebugLevel.I, "Force SQL: {0}", vars: [forceSql]);
            if (!fileExists)
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_DRIVERS_ENCODING_BASE_FILENOTFOUND"));

            // This variable chooses opening mode, with forced types first
            OpeningMode mode =
                forceText ? OpeningMode.Text :
                forceHex ? OpeningMode.Binary :
                forceJson ? OpeningMode.Json :
                forceSql ? OpeningMode.Sql :
                OpeningMode.None;

            // If none, assume type from file contents
            if (mode == OpeningMode.None)
            {
                if (IsBinaryFile(path))
                    mode = IsSql(path) ? OpeningMode.Sql : OpeningMode.Binary;
                else
                    mode = IsJson(path) ? OpeningMode.Json : OpeningMode.Text;
            }

            // Now, select how we open the file
            switch (mode)
            {
                case OpeningMode.Text:
                    ShellManager.StartShell("TextShell", path);
                    break;
                case OpeningMode.Binary:
                    ShellManager.StartShell("HexShell", path);
                    break;
                case OpeningMode.Json:
#if NKS_EXTENSIONS
                    if (hasShells)
                        ShellManager.StartShell("JsonShell", path);
                    else
                    {
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_FILES_OPEN_NEEDSJSONSHELLADDON"), ThemeColorType.Warning);
#endif
                        ShellManager.StartShell("TextShell", path);
#if NKS_EXTENSIONS
                    }
#endif
                    break;
                case OpeningMode.Sql:
#if NKS_EXTENSIONS
                    if (hasShells)
                        ShellManager.StartShell("SqlShell", path);
                    else
                    {
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_FILES_OPEN_NEEDSSQLSHELLADDON"), ThemeColorType.Warning);
#endif
                        ShellManager.StartShell("HexShell", path);
#if NKS_EXTENSIONS
                    }
#endif
                    break;
            }
        }

        /// <summary>
        /// Opens the file deterministically using the extension handlers or the deterministic text editor if the target file is a text.
        /// </summary>
        /// <param name="file">File to open</param>
        /// <exception cref="KernelException"></exception>
        public static void OpenDeterministically(string file)
        {
            // Check the file for existence
            bool fileExists = FilesystemTools.FileExists(file);
            if (!fileExists)
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_DRIVERS_ENCODING_BASE_FILENOTFOUND"));

            // Now, check to see if the file is a text or a binary file
            if (FilesystemTools.IsBinaryFile(file))
            {
                // This file is a binary file.
                string extension = Path.GetExtension(file);
                var handler = ExtensionHandlerTools.GetExtensionHandler(extension) ??
                    throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_OPEN_EXCEPTION_NOHANDLER") + $" {extension}");

                // Now that we have the handler, we'll execute it.
                handler.Handler(file);
            }
            else
            {
                // This file is a text file.
                OpenEditor(file);
            }
        }
    }

    internal enum OpeningMode
    {
        None,
        Binary,
        Sql,
        Text,
        Json,
    }
}
