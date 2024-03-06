//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using Terminaux.Base.Buffered;

namespace Nitrocid.Extras.Docking.Dock
{
    /// <summary>
    /// Dock interface
    /// </summary>
    public interface IDock
    {
        /// <summary>
        /// Screen dock name
        /// </summary>
        string DockName { get; }
        /// <summary>
        /// Screen docking function. Note that this function must create a <see cref="Screen"/> instance and print it, and
        /// that this function must wait for user input to, at least, exit the screen dock logic and go back to the kernel.
        /// </summary>
        void ScreenDock();
    }
}
