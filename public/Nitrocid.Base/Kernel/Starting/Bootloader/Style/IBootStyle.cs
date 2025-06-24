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

using System.Collections.Generic;
using System;

namespace Nitrocid.Base.Kernel.Starting.Bootloader.Style
{
    /// <summary>
    /// Boot style interface to customize how the bootloader looks
    /// </summary>
    public interface IBootStyle
    {
        /// <summary>
        /// Custom key assignments
        /// </summary>
        Dictionary<ConsoleKeyInfo, Action>? CustomKeys { get; }
        /// <summary>
        /// Renders the bootloader style
        /// </summary>
        string Render();
        /// <summary>
        /// Renders the highlighted boot entry
        /// </summary>
        /// <param name="chosenBootEntry">Chosen boot entry index (from 0)</param>
        string RenderHighlight(int chosenBootEntry);
        /// <summary>
        /// Renders the modal dialog box with content
        /// </summary>
        /// <param name="content">Message to display in the box</param>
        string RenderModalDialog(string content);
        /// <summary>
        /// Renders the booting message
        /// </summary>
        /// <param name="chosenBootName">Chosen boot name</param>
        string RenderBootingMessage(string chosenBootName);
        /// <summary>
        /// Renders the timeout for selection
        /// </summary>
        /// <param name="timeout">Target timeout in seconds to count down from</param>
        string RenderSelectTimeout(int timeout);
        /// <summary>
        /// Clears the timeout for selection
        /// </summary>
        string ClearSelectTimeout();
    }
}
