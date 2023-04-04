
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

using KS.Kernel.Configuration;

namespace KS.Misc.Writers.FancyWriters.Tools
{
    /// <summary>
    /// All border tools here
    /// </summary>
    public static class BorderTools
    {
        internal static string _borderUpperLeftCornerChar = "╔";
        internal static string _borderUpperRightCornerChar = "╗";
        internal static string _borderLowerLeftCornerChar = "╚";
        internal static string _borderLowerRightCornerChar = "╝";
        internal static string _borderUpperFrameChar = "═";
        internal static string _borderLowerFrameChar = "═";
        internal static string _borderLeftFrameChar = "║";
        internal static string _borderRightFrameChar = "║";

        /// <summary>
        /// Upper left corner character 
        /// </summary>
        public static string BorderUpperLeftCornerChar =>
            Config.MainConfig.BorderUpperLeftCornerChar;
        /// <summary>
        /// Upper right corner character 
        /// </summary>
        public static string BorderUpperRightCornerChar =>
            Config.MainConfig.BorderUpperRightCornerChar;
        /// <summary>
        /// Lower left corner character 
        /// </summary>
        public static string BorderLowerLeftCornerChar =>
            Config.MainConfig.BorderLowerLeftCornerChar;
        /// <summary>
        /// Lower right corner character 
        /// </summary>
        public static string BorderLowerRightCornerChar =>
            Config.MainConfig.BorderLowerRightCornerChar;
        /// <summary>
        /// Upper frame character 
        /// </summary>
        public static string BorderUpperFrameChar =>
            Config.MainConfig.BorderUpperFrameChar;
        /// <summary>
        /// Lower frame character 
        /// </summary>
        public static string BorderLowerFrameChar =>
            Config.MainConfig.BorderLowerFrameChar;
        /// <summary>
        /// Left frame character 
        /// </summary>
        public static string BorderLeftFrameChar =>
            Config.MainConfig.BorderLeftFrameChar;
        /// <summary>
        /// Right frame character 
        /// </summary>
        public static string BorderRightFrameChar =>
            Config.MainConfig.BorderRightFrameChar;
    }
}
