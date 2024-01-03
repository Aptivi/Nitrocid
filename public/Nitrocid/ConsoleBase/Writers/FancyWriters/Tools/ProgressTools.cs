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

namespace Nitrocid.ConsoleBase.Writers.FancyWriters.Tools
{
    /// <summary>
    /// Progress tools
    /// </summary>
    public static class ProgressTools
    {

        internal static char progressUpperLeftCornerChar = '╔';
        internal static char progressUpperRightCornerChar = '╗';
        internal static char progressLowerLeftCornerChar = '╚';
        internal static char progressLowerRightCornerChar = '╝';
        internal static char progressUpperFrameChar = '═';
        internal static char progressLowerFrameChar = '═';
        internal static char progressLeftFrameChar = '║';
        internal static char progressRightFrameChar = '║';

        /// <summary>
        /// Upper left corner character for the progress bar
        /// </summary>
        public static char ProgressUpperLeftCornerChar
        {
            get => progressUpperLeftCornerChar;
            set => progressUpperLeftCornerChar = value;
        }
        /// <summary>
        /// Upper right corner character for the progress bar
        /// </summary>
        public static char ProgressUpperRightCornerChar
        {
            get => progressUpperRightCornerChar;
            set => progressUpperRightCornerChar = value;
        }
        /// <summary>
        /// Lower left corner character for the progress bar
        /// </summary>
        public static char ProgressLowerLeftCornerChar
        {
            get => progressLowerLeftCornerChar;
            set => progressLowerLeftCornerChar = value;
        }
        /// <summary>
        /// Lower right corner character for the progress bar
        /// </summary>
        public static char ProgressLowerRightCornerChar
        {
            get => progressLowerRightCornerChar;
            set => progressLowerRightCornerChar = value;
        }
        /// <summary>
        /// Upper frame character for the progress bar
        /// </summary>
        public static char ProgressUpperFrameChar
        {
            get => progressUpperFrameChar;
            set => progressUpperFrameChar = value;
        }
        /// <summary>
        /// Lower frame character for the progress bar
        /// </summary>
        public static char ProgressLowerFrameChar
        {
            get => progressLowerFrameChar;
            set => progressLowerFrameChar = value;
        }
        /// <summary>
        /// Left frame character for the progress bar
        /// </summary>
        public static char ProgressLeftFrameChar
        {
            get => progressLeftFrameChar;
            set => progressLeftFrameChar = value;
        }
        /// <summary>
        /// Right frame character for the progress bar
        /// </summary>
        public static char ProgressRightFrameChar
        {
            get => progressRightFrameChar;
            set => progressRightFrameChar = value;
        }

    }
}
