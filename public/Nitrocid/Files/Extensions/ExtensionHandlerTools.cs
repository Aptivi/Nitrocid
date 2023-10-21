
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.Drivers;
using KS.Drivers.Encryption;
using KS.Kernel.Exceptions;
using KS.Languages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KS.Files.Extensions
{
    /// <summary>
    /// Tools for extension handling
    /// </summary>
    public static class ExtensionHandlerTools
    {
        internal static readonly List<ExtensionHandler> extensionHandlers = new()
        {
            new ExtensionHandler(".bin", (path) => Opening.OpenEditor(path, false, false, true), (path) => $"{Translate.DoTranslation("File hash sum")}: {Encryption.GetEncryptedFile(path, DriverHandler.CurrentEncryptionDriver.DriverName)}"),
        };
        internal static readonly List<ExtensionHandler> customHandlers = new();

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
        /// Gets the extension handler from the extension
        /// </summary>
        /// <param name="extension">Extension to check</param>
        /// <returns>An instance of <see cref="ExtensionHandler"/> containing info about the extension, or null if there is no handler.</returns>
        public static ExtensionHandler GetExtensionHandler(string extension)
        {
            // If nothing is registered, indicate that it isn't registered
            if (!IsHandlerRegistered(extension))
                return null;

            // Extensions must start with a dot
            if (!extension.StartsWith("."))
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Extensions must start with the dot. Hint:") + $" .{extension}");

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
                return Array.Empty<ExtensionHandler>();

            // Extensions must start with a dot
            if (!extension.StartsWith("."))
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Extensions must start with the dot. Hint:") + $" .{extension}");

            // Get the handler
            return GetExtensionHandlers().Where((ext) => ext.Extension == extension).ToArray();
        }

        /// <summary>
        /// Registers the handler
        /// </summary>
        /// <param name="extension">Extension to register</param>
        /// <param name="handlerAction">Action containing a function that opens the specified file</param>
        /// <param name="infoHandlerAction">Action containing a function that gets information about the specified file</param>
        public static void RegisterHandler(string extension, Action<string> handlerAction, Func<string, string> infoHandlerAction)
        {
            // Extensions must start with a dot
            if (!extension.StartsWith("."))
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Extensions must start with the dot. Hint:") + $" .{extension}");

            // Add the handler
            var handler = new ExtensionHandler(extension, handlerAction, infoHandlerAction);
            customHandlers.Add(handler);
        }

        /// <summary>
        /// Unregisters the handler
        /// </summary>
        /// <param name="extension">Extension to unregister</param>
        /// <param name="handlerIndex">Extension handler index</param>
        public static void UnregisterHandler(string extension, int handlerIndex)
        {
            // Don't register if the handler is not registered
            if (!IsHandlerRegistered(extension))
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Handler for extension is not registered.") + $" {extension}");

            // Extensions must start with a dot
            if (!extension.StartsWith("."))
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Extensions must start with the dot. Hint:") + $" .{extension}");

            // Remove the handler
            var handlers = GetExtensionHandlers(extension);
            if (handlerIndex >= handlers.Length)
                handlerIndex = handlers.Length - 1;
            if (handlerIndex < 0)
                handlerIndex = 0;
            customHandlers.RemoveAt(handlerIndex);
        }

        /// <summary>
        /// Unregisters the handler
        /// </summary>
        /// <param name="extension">Extension to unregister</param>
        /// <param name="handler">Handler to look for when removing</param>
        public static void UnregisterHandler(string extension, ExtensionHandler handler)
        {
            // Don't register if the handler is not registered
            if (!IsHandlerRegistered(extension))
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Handler for extension is not registered.") + $" {extension}");

            // Extensions must start with a dot
            if (!extension.StartsWith("."))
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Extensions must start with the dot. Hint:") + $" .{extension}");

            // Remove the handler
            customHandlers.Remove(handler);
        }

        /// <summary>
        /// Unregisters the handlers
        /// </summary>
        /// <param name="extension">Extension to unregister</param>
        public static void UnregisterHandlers(string extension)
        {
            // Don't register if the handler is not registered
            if (!IsHandlerRegistered(extension))
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Handler for extension is not registered.") + $" {extension}");

            // Extensions must start with a dot
            if (!extension.StartsWith("."))
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Extensions must start with the dot. Hint:") + $" .{extension}");

            // Remove the handler
            var handlers = GetExtensionHandlers(extension);
            foreach (var handler in handlers)
                customHandlers.Remove(handler);
        }

        /// <summary>
        /// Unregisters all handlers
        /// </summary>
        public static void UnregisterAllHandlers()
        {
            for (int i = customHandlers.Count - 1; i >= 0; i--)
                customHandlers.RemoveAt(i);
        }
    }
}
