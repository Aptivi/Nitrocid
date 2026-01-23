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

using System.Linq;
using Terminaux.Images.Icons;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Extras.Images.Icons.Tools
{
    /// <summary>
    /// Tools for the icons
    /// </summary>
    public static class IconsTools
    {
        /// <summary>
        /// Gets the icon names
        /// </summary>
        /// <returns>An array of all icon names</returns>
        public static string[] GetIconNames() =>
            IconsManager.GetIconNames();

        /// <summary>
        /// Checks an icon
        /// </summary>
        /// <param name="iconName">Icon name</param>
        /// <returns>True if this icon is found; false otherwise.</returns>
        public static bool HasIcon(string iconName) =>
            GetIconNames().Contains(iconName);

        /// <summary>
        /// Renders the icon to a string that you can print to the console
        /// </summary>
        /// <param name="iconName">Icon name</param>
        /// <param name="width">Width of the resulting icon</param>
        /// <param name="height">Height of the resulting icon</param>
        /// <param name="left">Zero-based console left position to start writing the icon to</param>
        /// <param name="top">Zero-based console top position to start writing the icon to</param>
        /// <returns>A string that contains the resulting pixels that you can print to the console using the <see cref="TextWriterRaw.WriteRaw(string, object[])"/> function</returns>
        public static string RenderIcon(string iconName, int width, int height, int left, int top) =>
            IconsManager.RenderIcon(iconName, width, height, left, top);

        /// <summary>
        /// Prompts the user for an icon
        /// </summary>
        /// <param name="initialIcon">Initial icon to use</param>
        /// <returns>Selected icon</returns>
        public static string PromptForIcons(string initialIcon) =>
            IconsSelector.PromptForIcons(initialIcon);
    }
}
