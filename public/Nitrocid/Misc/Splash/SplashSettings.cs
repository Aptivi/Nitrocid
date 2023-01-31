
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
        public static TextLocation SimpleProgressTextLocation { get; set; } = TextLocation.Top;

        // -> Progress
        /// <summary>
        /// [Progress] The progress color
        /// </summary>
        public static string ProgressProgressColor { get; set; } = ColorTools.GetColor(KernelColorType.Progress).PlainSequence;
        /// <summary>
        /// [Progress] The progress text location
        /// </summary>
        public static TextLocation ProgressProgressTextLocation { get; set; } = TextLocation.Top;

        // -> PowerLineProgress
        /// <summary>
        /// [PowerLineProgress] The progress color
        /// </summary>
        public static string PowerLineProgressProgressColor { get; set; } = ColorTools.GetColor(KernelColorType.Progress).PlainSequence;
        /// <summary>
        /// [PowerLineProgress] The progress text location
        /// </summary>
        public static TextLocation PowerLineProgressProgressTextLocation { get; set; } = TextLocation.Top;

    }
}
