
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

using KS.ConsoleBase.Colors;
using KS.Misc.Text;

namespace KS.Misc.Splash
{
    /// <summary>
    /// Splash settings module
    /// </summary>
    public static class SplashSettings
    {

        // -> Simple
        /// <summary>
        /// [Simple] The progress text location
        /// </summary>
        public static TextLocation SimpleProgressTextLocation = TextLocation.Top;

        // -> Progress
        /// <summary>
        /// [Progress] The progress color
        /// </summary>
        public static string ProgressProgressColor = ColorTools.GetColor(KernelColorType.Progress).PlainSequence;
        /// <summary>
        /// [Progress] The progress text location
        /// </summary>
        public static TextLocation ProgressProgressTextLocation = TextLocation.Top;

        // -> PowerLineProgress
        /// <summary>
        /// [PowerLineProgress] The progress color
        /// </summary>
        public static string PowerLineProgressProgressColor = ColorTools.GetColor(KernelColorType.Progress).PlainSequence;
        /// <summary>
        /// [PowerLineProgress] The progress text location
        /// </summary>
        public static TextLocation PowerLineProgressProgressTextLocation = TextLocation.Top;

    }
}
