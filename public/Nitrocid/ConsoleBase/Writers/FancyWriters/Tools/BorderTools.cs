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

using Nitrocid.Kernel.Configuration;

namespace Nitrocid.ConsoleBase.Writers.FancyWriters.Tools
{
    /// <summary>
    /// All border tools here
    /// </summary>
    public static class BorderTools
    {
        internal static char _borderUpperLeftCornerChar = '╔';
        internal static char _borderUpperRightCornerChar = '╗';
        internal static char _borderLowerLeftCornerChar = '╚';
        internal static char _borderLowerRightCornerChar = '╝';
        internal static char _borderUpperFrameChar = '═';
        internal static char _borderLowerFrameChar = '═';
        internal static char _borderLeftFrameChar = '║';
        internal static char _borderRightFrameChar = '║';

        /// <summary>
        /// Upper left corner character 
        /// </summary>
        public static char BorderUpperLeftCornerChar =>
            Config.MainConfig.BorderUpperLeftCornerChar;
        /// <summary>
        /// Upper right corner character 
        /// </summary>
        public static char BorderUpperRightCornerChar =>
            Config.MainConfig.BorderUpperRightCornerChar;
        /// <summary>
        /// Lower left corner character 
        /// </summary>
        public static char BorderLowerLeftCornerChar =>
            Config.MainConfig.BorderLowerLeftCornerChar;
        /// <summary>
        /// Lower right corner character 
        /// </summary>
        public static char BorderLowerRightCornerChar =>
            Config.MainConfig.BorderLowerRightCornerChar;
        /// <summary>
        /// Upper frame character 
        /// </summary>
        public static char BorderUpperFrameChar =>
            Config.MainConfig.BorderUpperFrameChar;
        /// <summary>
        /// Lower frame character 
        /// </summary>
        public static char BorderLowerFrameChar =>
            Config.MainConfig.BorderLowerFrameChar;
        /// <summary>
        /// Left frame character 
        /// </summary>
        public static char BorderLeftFrameChar =>
            Config.MainConfig.BorderLeftFrameChar;
        /// <summary>
        /// Right frame character 
        /// </summary>
        public static char BorderRightFrameChar =>
            Config.MainConfig.BorderRightFrameChar;
    }
}
