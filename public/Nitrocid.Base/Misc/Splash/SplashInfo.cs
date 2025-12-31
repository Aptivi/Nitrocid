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

namespace Nitrocid.Base.Misc.Splash
{
    /// <summary>
    /// Splash information class
    /// </summary>
    public class SplashInfo
    {

        /// <summary>
        /// Splash name
        /// </summary>
        public string SplashName { get; private set; }
        /// <summary>
        /// Does the splash display progress?
        /// </summary>
        public bool DisplaysProgress { get; private set; }
        /// <summary>
        /// Splash entry point
        /// </summary>
        public ISplash EntryPoint { get; private set; }

        /// <summary>
        /// Installs a new class instance of splash info
        /// </summary>
        /// <param name="SplashName">Splash name</param>
        /// <param name="DisplaysProgress">Does the splash display progress?</param>
        /// <param name="EntryPoint">Splash entry point</param>
        protected internal SplashInfo(string SplashName, ISplash EntryPoint, bool DisplaysProgress = true)
        {
            this.SplashName = SplashName;
            this.DisplaysProgress = DisplaysProgress;
            this.EntryPoint = EntryPoint;
        }

    }
}
