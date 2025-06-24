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

using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Starting.Bootloader;
using Nitrocid.Base.Kernel.Starting.Bootloader.Style.Styles;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Terminaux.Base.Buffered;

namespace Nitrocid.Base.Kernel.Starting.Bootloader.Style
{
    /// <summary>
    /// Bootloader style management class
    /// </summary>
    public static class BootStyleManager
    {
        private static Thread? timeoutThread;
        private static readonly Dictionary<string, BaseBootStyle> bootStyles = new()
        {
            { "Default", new DefaultBootStyle() },
            { "Standard", new StandardBootStyle() },
            { "Ntldr", new NtldrBootStyle() },
            { "GRUB", new GrubBootStyle() },
            { "GRUBLegacy", new GrubLegacyBootStyle() },
            { "LILO", new LiloBootStyle() },
            { "BootMgr", new BootMgrBootStyle() },
        };
        private static readonly Dictionary<string, BaseBootStyle> customBootStyles = [];

        /// <summary>
        /// Gets the boot style from the name
        /// </summary>
        /// <param name="name">The name</param>
        /// <returns>The base boot style</returns>
        public static BaseBootStyle GetBootStyle(string name)
        {
            // Use the base boot styles first
            DebugWriter.WriteDebug(DebugLevel.I, "Getting boot style {0} from base boot styles...", vars: [name]);
            bootStyles.TryGetValue(name, out BaseBootStyle? bootStyle);

            // If not found, use the custom one
            if (bootStyle == null)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Getting boot style {0} from custom boot styles...", vars: [name]);
                customBootStyles.TryGetValue(name, out bootStyle);
            }

            // If still not found, use Default
            if (bootStyle == null)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Still nothing. Using the default...");
                bootStyle = bootStyles["Default"];
            }

            // Return it.
            return bootStyle;
        }

        /// <summary>
        /// Renders the boot menu
        /// </summary>
        /// <param name="chosenBootEntry">Chosen boot entry index (from 0)</param>
        public static string RenderMenu(int chosenBootEntry)
        {
            // Render it.
            var bootStyle = GetCurrentBootStyle();
            var rendered = new StringBuilder();
            DebugWriter.WriteDebug(DebugLevel.I, "Rendering menu with chosen boot entry {0}...", vars: [chosenBootEntry]);
            rendered.Append(
                bootStyle.Render() +
                bootStyle.RenderHighlight(chosenBootEntry)
            );
            return rendered.ToString();
        }

        /// <summary>
        /// Renders the boot message
        /// </summary>
        /// <param name="chosenBootName">Chosen boot name</param>
        public static string RenderBootingMessage(string chosenBootName)
        {
            // Render it.
            var bootStyle = GetCurrentBootStyle();
            DebugWriter.WriteDebug(DebugLevel.I, "Rendering booting message with chosen boot name {0}...", vars: [chosenBootName]);
            return bootStyle.RenderBootingMessage(chosenBootName);
        }

        /// <summary>
        /// Renders the modal dialog box
        /// </summary>
        /// <param name="content">Message to display in the box</param>
        public static string RenderDialog(string content)
        {
            // Render it.
            var bootStyle = GetCurrentBootStyle();
            DebugWriter.WriteDebug(DebugLevel.I, "Rendering modal dialog with content: {0}...", vars: [content]);
            return bootStyle.RenderModalDialog(content);
        }

        /// <summary>
        /// Renders the selection timeout
        /// </summary>
        /// <param name="timeout">Timeout interval in seconds</param>
        public static void RenderSelectTimeout(int timeout)
        {
            timeoutThread = new((timeout) => SelectTimeoutHandler((int?)timeout ?? 60));
            if (!timeoutThread.IsAlive && timeout > 0 && BootloaderState.WaitingForFirstBootKey)
                timeoutThread.Start(timeout);
        }

        /// <summary>
        /// Gets the current boot style
        /// </summary>
        /// <returns>The current boot style instance</returns>
        public static BaseBootStyle GetCurrentBootStyle()
        {
            string bootStyleStr = Config.MainConfig.BootStyle;
            var bootStyle = GetBootStyle(bootStyleStr);
            DebugWriter.WriteDebug(DebugLevel.I, "Got boot style from {0}...", vars: [bootStyleStr]);
            return bootStyle;
        }

        private static void SelectTimeoutHandler(int timeout)
        {
            var style = GetCurrentBootStyle();
            int timeoutElapsed = 0;
            var bufferClear = new ScreenPart();
            try
            {
                while (timeoutElapsed < timeout && BootloaderState.WaitingForFirstBootKey)
                {
                    // Render the timeout area since it isn't elapsed
                    var buffer = new ScreenPart();
                    buffer.AddDynamicText(() => style.RenderSelectTimeout(timeout - timeoutElapsed));
                    BootloaderMain.bootloaderScreen.AddBufferedPart("Timeout", buffer);
                    ScreenTools.Render();
                    SpinWait.SpinUntil(() => !BootloaderState.WaitingForFirstBootKey, 1000);
                    BootloaderMain.bootloaderScreen.RemoveBufferedPart("Timeout");
                    timeoutElapsed += 1;
                }
            }
            catch
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Failed to render select timeout.");
            }

            // Clear the timeout area
            bufferClear.AddDynamicText(style.ClearSelectTimeout);
            BootloaderMain.bootloaderScreen.AddBufferedPart("Clear timeout", bufferClear);
            ScreenTools.Render();
            BootloaderMain.bootloaderScreen.RemoveBufferedPart("Clear timeout");
        }
    }
}
