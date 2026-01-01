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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Files.Operations.Querying;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Shells;
using System.IO;

namespace Nitrocid.Files.Extensions
{
    /// <summary>
    /// Routines related to opening the files
    /// </summary>
    public static class Opening
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
            bool fileExists = Checking.FileExists(path);

            // Check the addons
            bool hasJsonShell = AddonTools.GetAddon(InterAddonTranslations.GetAddonName(KnownAddons.ExtrasJsonShell)) is not null;
            bool hasSqlShell = AddonTools.GetAddon(InterAddonTranslations.GetAddonName(KnownAddons.ExtrasSqlShell)) is not null;

            // Check to see if the file exists
            DebugWriter.WriteDebug(DebugLevel.I, "File path is {0} and .Exists is {1}", path, fileExists);
            DebugWriter.WriteDebug(DebugLevel.I, "Force text: {0}", forceText);
            DebugWriter.WriteDebug(DebugLevel.I, "Force JSON: {0}", forceJson);
            DebugWriter.WriteDebug(DebugLevel.I, "Force Hex: {0}", forceHex);
            DebugWriter.WriteDebug(DebugLevel.I, "Force SQL: {0}", forceSql);
            if (!fileExists)
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("File doesn't exist."));

            // First, forced types
            if (forceText)
                ShellManager.StartShell(ShellType.TextShell, path);
            else if (forceHex)
                ShellManager.StartShell(ShellType.HexShell, path);
            else if (forceJson)
            {
                if (!hasJsonShell)
                {
                    TextWriters.Write(Translate.DoTranslation("It looks like that you don't have the JSON shell addon installed. In order to get extra features that it offers, install the addons pack."), KernelColorType.Warning);
                    ShellManager.StartShell(ShellType.TextShell, path);
                }
                else
                    ShellManager.StartShell("JsonShell", path);
            }
            else if (forceSql)
            {
                if (!hasSqlShell)
                {
                    TextWriters.Write(Translate.DoTranslation("It looks like that you don't have the SQL shell addon installed. In order to get extra features that it offers, install the addons pack."), KernelColorType.Warning);
                    ShellManager.StartShell(ShellType.HexShell, path);
                }
                else
                    ShellManager.StartShell("SqlShell", path);
            }

            // Exit if forced types
            if (forceText || forceJson || forceHex || forceSql)
                return;

            // Determine the type
            if (hasSqlShell && Parsing.IsSql(path))
                ShellManager.StartShell("SqlShell", path);
            else if (Parsing.IsBinaryFile(path))
                ShellManager.StartShell(ShellType.HexShell, path);
            else if (Parsing.IsJson(path))
            {
                if (!hasJsonShell)
                {
                    TextWriters.Write(Translate.DoTranslation("It looks like that you don't have the JSON shell addon installed. In order to get extra features that it offers, install the addons pack."), KernelColorType.Warning);
                    ShellManager.StartShell(ShellType.TextShell, path);
                }
                else
                    ShellManager.StartShell("JsonShell", path);
            }
            else
                ShellManager.StartShell(ShellType.TextShell, path);
        }

        /// <summary>
        /// Opens the file deterministically using the extension handlers or the deterministic text editor if the target file is a text.
        /// </summary>
        /// <param name="file">File to open</param>
        /// <exception cref="KernelException"></exception>
        public static void OpenDeterministically(string file)
        {
            // Check the file for existence
            bool fileExists = Checking.FileExists(file);
            if (!fileExists)
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("File doesn't exist."));

            // Now, check to see if the file is a text or a binary file
            if (Parsing.IsBinaryFile(file))
            {
                // This file is a binary file.
                string extension = Path.GetExtension(file);
                var handler = ExtensionHandlerTools.GetExtensionHandler(extension) ??
                    throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("No handler to handle this extension.") + $" {extension}");

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
}
