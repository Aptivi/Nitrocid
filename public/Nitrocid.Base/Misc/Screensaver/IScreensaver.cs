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

using Nitrocid.Base.ConsoleBase;

namespace Nitrocid.Base.Misc.Screensaver
{
    /// <summary>
    /// Screensaver (KS built-in or custom) interface
    /// </summary>
    public interface IScreensaver
    {
        /// <summary>
        /// Shows the seizure warning before the preparation
        /// </summary>
        void ScreensaverSeizureWarning();
        /// <summary>
        /// Prepare the screensaver before displaying.
        /// </summary>
        void ScreensaverPreparation();
        /// <summary>
        /// Display a screensaver. This is executed inside the loop.
        /// </summary>
        void ScreensaverLogic();
        /// <summary>
        /// Resize synchronization logic. This is executed by <see cref="ConsoleResizeHandler.HandleResize(int, int, int, int)"/>
        /// </summary>
        void ScreensaverResizeSync();
        /// <summary>
        /// The outro logic once the screensaver is done displaying.
        /// </summary>
        void ScreensaverOutro();
        /// <summary>
        /// The name of screensaver, usually the assembly name of the custom screensaver
        /// </summary>
        string ScreensaverName { get; }
        /// <summary>
        /// Whether the screensaver contains flashing images
        /// </summary>
        bool ScreensaverContainsFlashingImages { get; }
    }
}
