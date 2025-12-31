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

using System.Collections.Generic;

namespace Nitrocid.Base.Misc.Widgets
{
    /// <summary>
    /// Widget interface for the modern logon handler (built-in)
    /// </summary>
    public interface IWidget
    {
        /// <summary>
        /// Options for the widget
        /// </summary>
        Dictionary<string, object> Options { get; set; }

        /// <summary>
        /// Renders this widget in a specified location
        /// </summary>
        /// <param name="left">Left position of the widget</param>
        /// <param name="top">Top position of the widget</param>
        /// <param name="width">Width of the widget</param>
        /// <param name="height">Height of the widget</param>
        /// <returns>A string that represents the rendered widget with VT sequences and other console control sequences.</returns>
        string Render(int left, int top, int width, int height);

        /// <summary>
        /// Initializes this widget in a specified location
        /// </summary>
        /// <param name="left">Left position of the widget</param>
        /// <param name="top">Top position of the widget</param>
        /// <param name="width">Width of the widget</param>
        /// <param name="height">Height of the widget</param>
        /// <returns>A string that contains control sequences to render to the console as a sequence to initialize the widget.</returns>
        string Initialize(int left, int top, int width, int height);

        /// <summary>
        /// Cleans this widget up in a specified location
        /// </summary>
        /// <param name="left">Left position of the widget</param>
        /// <param name="top">Top position of the widget</param>
        /// <param name="width">Width of the widget</param>
        /// <param name="height">Height of the widget</param>
        /// <returns>A string that contains control sequences to render to the console as a sequence to clean the widget up.</returns>
        string Cleanup(int left, int top, int width, int height);
    }
}
