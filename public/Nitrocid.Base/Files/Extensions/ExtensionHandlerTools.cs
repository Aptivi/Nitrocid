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

using Newtonsoft.Json;
using Nitrocid.Base.Drivers;
using Nitrocid.Base.Drivers.Encryption;
using Nitrocid.Base.Files.Paths;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.Base.Files.Extensions
{
    /// <summary>
    /// Tools for extension handling
    /// </summary>
    public static class ExtensionHandlerTools
    {
        internal static Dictionary<string, string> defaultHandlers = new()
        {
            { ".bin", "NitrocidBin" },
        };
        internal static readonly List<ExtensionHandler> extensionHandlers =
        [
            new ExtensionHandler(".bin", "NitrocidBin", (path) => FilesystemTools.OpenEditor(path, false, false, true), (path) => $"{LanguageTools.GetLocalized("NKS_FILES_EXTENSIONS_BIN_HASHSUM")}: {Encryption.GetEncryptedFile(path, DriverHandler.CurrentEncryptionDriver.DriverName)}"),
        ];
        internal static readonly List<ExtensionHandler> customHandlers = [];

        /// <summary>
        /// Gets all extension handlers
        /// </summary>
        /// <returns></returns>
        public static ExtensionHandler[] GetExtensionHandlers() =>
            extensionHandlers.Union(customHandlers).ToArray();

        /// <summary>
        /// Checks to see if the extension handler is registered
        /// </summary>
        /// <param name="extension">Extension to check</param>
        /// <returns>True if registered; False otherwise. Also false if the extension doesn't start with the dot.</returns>
        public static bool IsHandlerRegistered(string extension)
        {
            // Check to see if this handler is built-in
            if (IsHandlerBuiltin(extension))
                return true;

            // If nothing is registered, indicate that it isn't registered
            if (customHandlers.Count == 0)
                return false;

            // Extensions must start with a dot
            if (!extension.StartsWith("."))
                return false;

            // Now, check to see if we have this handler
            return customHandlers.Any((ext) => ext.Extension == extension);
        }

        /// <summary>
        /// Checks to see if the extension handler is registered with the implementer
        /// </summary>
        /// <param name="extension">Extension to check</param>
        /// <param name="implementer">Implementer to check</param>
        /// <returns>True if registered; False otherwise. Also false if the extension doesn't start with the dot.</returns>
        public static bool IsHandlerRegisteredSpecific(string extension, string implementer)
        {
            // Check to see if this handler is built-in
            if (IsHandlerBuiltinSpecific(extension, implementer))
                return true;

            // If nothing is registered, indicate that it isn't registered
            if (customHandlers.Count == 0)
                return false;

            // Extensions must start with a dot
            if (!extension.StartsWith("."))
                return false;

            // Now, check to see if we have this handler
            return customHandlers.Any((ext) => ext.Extension == extension && ext.Implementer == implementer);
        }

        /// <summary>
        /// Checks to see if the extension handler is registered
        /// </summary>
        /// <param name="extension">Extension to check</param>
        /// <returns>True if registered; False otherwise. Also false if the extension doesn't start with the dot.</returns>
        public static bool IsHandlerBuiltin(string extension)
        {
            // If nothing is registered, indicate that it isn't registered
            if (extensionHandlers.Count == 0)
                return false;

            // Extensions must start with a dot
            if (!extension.StartsWith("."))
                return false;

            // Now, check to see if we have this handler
            return extensionHandlers.Any((ext) => ext.Extension == extension);
        }

        /// <summary>
        /// Checks to see if the extension handler is registered
        /// </summary>
        /// <param name="extension">Extension to check</param>
        /// <param name="implementer">Implementer to check</param>
        /// <returns>True if registered; False otherwise. Also false if the extension doesn't start with the dot.</returns>
        public static bool IsHandlerBuiltinSpecific(string extension, string implementer)
        {
            // If nothing is registered, indicate that it isn't registered
            if (extensionHandlers.Count == 0)
                return false;

            // Extensions must start with a dot
            if (!extension.StartsWith("."))
                return false;

            // Now, check to see if we have this handler
            return extensionHandlers.Any((ext) => ext.Extension == extension && ext.Implementer == implementer);
        }

        /// <summary>
        /// Gets the extension handler from the extension and the default implementer
        /// </summary>
        /// <param name="extension">Extension to check</param>
        /// <returns>An instance of <see cref="ExtensionHandler"/> containing info about the extension, or null if there is no handler.</returns>
        public static ExtensionHandler? GetExtensionHandler(string extension)
        {
            // Check to see if we have the extension in the default handlers list
            if (!defaultHandlers.TryGetValue(extension, out string? defHandlerName))
            {
                if (IsHandlerRegistered(extension))
                {
                    var handler = GetFirstExtensionHandler(extension) ??
                        throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXTENSIONS_EXCEPTION_NOHANDLER") + $" {extension}");
                    defHandlerName = handler.Implementer;
                    SetExtensionHandler(extension, defHandlerName);
                }
                else
                    throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXTENSIONS_EXCEPTION_NOHANDLER") + $" {extension}");
            }

            // Now, get the default handler name and get the handler instance from it
            string handlerName = defHandlerName;
            return GetExtensionHandler(extension, handlerName);
        }

        /// <summary>
        /// Gets the extension handler from the extension and the implementer
        /// </summary>
        /// <param name="extension">Extension to check</param>
        /// <param name="implementer">Implementer to check</param>
        /// <returns>An instance of <see cref="ExtensionHandler"/> containing info about the extension, or null if there is no handler.</returns>
        public static ExtensionHandler? GetExtensionHandler(string extension, string implementer)
        {
            // If nothing is registered, indicate that it isn't registered
            if (!IsHandlerRegisteredSpecific(extension, implementer))
                return null;

            // Extensions must start with a dot
            if (!extension.StartsWith("."))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXTENSIONS_EXCEPTION_NEEDSADOT") + $" .{extension}");

            // Get the handler
            return GetExtensionHandlers().First((ext) => ext.Extension == extension && ext.Implementer == implementer);
        }

        /// <summary>
        /// Gets the first extension handler from the extension
        /// </summary>
        /// <param name="extension">Extension to check</param>
        /// <returns>An instance of <see cref="ExtensionHandler"/> containing info about the extension, or null if there is no handler.</returns>
        public static ExtensionHandler? GetFirstExtensionHandler(string extension)
        {
            // If nothing is registered, indicate that it isn't registered
            if (!IsHandlerRegistered(extension))
                return null;

            // Extensions must start with a dot
            if (!extension.StartsWith("."))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXTENSIONS_EXCEPTION_NEEDSADOT") + $" .{extension}");

            // Get the handler
            return GetExtensionHandlers().First((ext) => ext.Extension == extension);
        }

        /// <summary>
        /// Gets extension handlers from the extension
        /// </summary>
        /// <param name="extension">Extension to check</param>
        /// <returns>An array of <see cref="ExtensionHandler"/> containing info about the extension, or an empty array if there is no handler.</returns>
        public static ExtensionHandler[] GetExtensionHandlers(string extension)
        {
            // If nothing is registered, indicate that it isn't registered
            if (!IsHandlerRegistered(extension))
                return [];

            // Extensions must start with a dot
            if (!extension.StartsWith("."))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXTENSIONS_EXCEPTION_NEEDSADOT") + $" .{extension}");

            // Get the handler
            return GetExtensionHandlers().Where((ext) => ext.Extension == extension).ToArray();
        }

        /// <summary>
        /// Sets the default extension handler of the extension to the specified implementer
        /// </summary>
        /// <param name="extension">Extension to query</param>
        /// <param name="implementer">Implementer to set</param>
        public static void SetExtensionHandler(string extension, string implementer)
        {
            // If nothing is registered, indicate that it isn't registered
            if (!IsHandlerRegisteredSpecific(extension, implementer))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXTENSIONS_EXCEPTION_NEEDSIMPLEMENTER") + $" .{extension}, {implementer}");

            // Extensions must start with a dot
            if (!extension.StartsWith("."))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXTENSIONS_EXCEPTION_NEEDSADOT") + $" .{extension}");

            // Set the handler
            if (!defaultHandlers.TryAdd(extension, implementer))
                defaultHandlers[extension] = implementer;
        }

        /// <summary>
        /// Registers the handler
        /// </summary>
        /// <param name="extension">Extension to register</param>
        /// <param name="implementer">The implementer name to add to the handler</param>
        /// <param name="handlerAction">Action containing a function that opens the specified file</param>
        /// <param name="infoHandlerAction">Action containing a function that gets information about the specified file</param>
        public static void RegisterHandler(string extension, string implementer, Action<string> handlerAction, Func<string, string> infoHandlerAction)
        {
            // Extension and implementer must not be null
            if (string.IsNullOrEmpty(extension))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXTENSIONS_EXCEPTION_NOEXTENSION"));
            if (string.IsNullOrEmpty(implementer))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXTENSIONS_EXCEPTION_NOIMPLEMENTER"));

            // Extensions must start with a dot
            if (!extension.StartsWith("."))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXTENSIONS_EXCEPTION_NEEDSADOT") + $" .{extension}");

            // Add the handler
            var handler = new ExtensionHandler(extension, implementer, handlerAction, infoHandlerAction);
            customHandlers.Add(handler);

            // Check to see if the extension is found in the default handler list
            defaultHandlers.TryAdd(extension, implementer);
        }

        /// <summary>
        /// Unregisters the handler
        /// </summary>
        /// <param name="extension">Extension to unregister</param>
        /// <param name="implementer">The implementer name to remove from the handler</param>
        public static void UnregisterHandler(string extension, string implementer)
        {
            // Don't register if the handler is not registered
            if (!IsHandlerRegisteredSpecific(extension, implementer))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXTENSIONS_EXCEPTION_HANDLERNOTREGGED") + $" {extension}");

            // Extensions must start with a dot
            if (!extension.StartsWith("."))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXTENSIONS_EXCEPTION_NEEDSADOT") + $" .{extension}");

            // Remove the handler
            var handler = GetExtensionHandler(extension, implementer);
            UnregisterHandler(extension, handler);
        }

        /// <summary>
        /// Unregisters the handler
        /// </summary>
        /// <param name="extension">Extension to unregister</param>
        /// <param name="handler">Handler to look for when removing</param>
        public static void UnregisterHandler(string extension, ExtensionHandler? handler)
        {
            // Don't register if the handler is not registered
            if (!IsHandlerRegistered(extension))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXTENSIONS_EXCEPTION_HANDLERNOTREGGED") + $" {extension}");
            if (handler is null)
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXTENSIONS_EXCEPTION_NEEDSHANDLER") + $" {extension}");

            // Extensions must start with a dot
            if (!extension.StartsWith("."))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXTENSIONS_EXCEPTION_NEEDSADOT") + $" .{extension}");

            // Remove the handler
            customHandlers.Remove(handler);

            // Check to see if the extension is found in the default handler list
            if (defaultHandlers.ContainsKey(extension))
            {
                // There are two cases: one in which we still have at least one implementer, and one in which we don't have any more
                // implementers.
                if (IsHandlerRegistered(extension))
                    defaultHandlers[extension] = GetFirstExtensionHandler(extension)?.Implementer ??
                        throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXTENSIONS_EXCEPTION_CANTGETDEFAULTHANDLER"));
                else
                    defaultHandlers.Remove(extension);
            }
        }

        /// <summary>
        /// Unregisters the handlers
        /// </summary>
        /// <param name="extension">Extension to unregister</param>
        public static void UnregisterHandlers(string extension)
        {
            // Don't register if the handler is not registered
            if (!IsHandlerRegistered(extension))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXTENSIONS_EXCEPTION_HANDLERNOTREGGED") + $" {extension}");

            // Extensions must start with a dot
            if (!extension.StartsWith("."))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXTENSIONS_EXCEPTION_NEEDSADOT") + $" .{extension}");

            // Remove the handler
            var handlers = GetExtensionHandlers(extension);
            foreach (var handler in handlers)
                UnregisterHandler(extension, handler);
        }

        /// <summary>
        /// Unregisters all handlers
        /// </summary>
        public static void UnregisterAllHandlers()
        {
            for (int i = customHandlers.Count - 1; i >= 0; i--)
            {
                var handler = customHandlers[i];
                UnregisterHandler(handler.Extension, handler.Implementer);
            }
        }

        internal static void SaveAllHandlers()
        {
            string serialized = JsonConvert.SerializeObject(defaultHandlers);
            FilesystemTools.WriteContentsText(PathsManagement.ExtensionHandlersPath, serialized);
        }

        internal static void LoadAllHandlers()
        {
            if (!FilesystemTools.FileExists(PathsManagement.ExtensionHandlersPath))
                SaveAllHandlers();
            string contents = FilesystemTools.ReadContentsText(PathsManagement.ExtensionHandlersPath);
            defaultHandlers = JsonConvert.DeserializeObject<Dictionary<string, string>>(contents) ??
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXTENSIONS_EXCEPTION_CANTGETDEFAULTHANDLERS"));
        }
    }
}
