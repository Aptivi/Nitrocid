
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using KS.Misc.Threading;

namespace KS.Misc.Screensaver
{
    /// <summary>
    /// Base screensaver class
    /// </summary>
    public abstract class BaseScreensaver : IScreensaver
    {

        /// <summary>
        /// Screensaver name
        /// </summary>
        public virtual string ScreensaverName { get; set; } = "BaseScreensaver";

        /// <summary>
        /// Screensaver settings
        /// </summary>
        public virtual Dictionary<string, object> ScreensaverSettings { get; set; }

        /// <summary>
        /// Screensaver preparation logic
        /// </summary>
        public virtual void ScreensaverPreparation()
        {
            ConsoleBase.ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
            ConsoleBase.ConsoleWrapper.ForegroundColor = ConsoleColor.White;
            ConsoleBase.ConsoleWrapper.Clear();
            ConsoleBase.ConsoleWrapper.CursorVisible = false;
        }

        /// <summary>
        /// Screensaver logic
        /// </summary>
        public virtual void ScreensaverLogic() => ThreadManager.SleepNoBlock(10L, ScreensaverDisplayer.ScreensaverDisplayerThread);

        /// <summary>
        /// Screensaver outro
        /// </summary>
        public virtual void ScreensaverOutro() { }
    }
}
