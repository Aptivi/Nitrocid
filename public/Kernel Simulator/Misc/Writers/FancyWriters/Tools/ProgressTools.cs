
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

namespace KS.Misc.Writers.FancyWriters.Tools
{
    /// <summary>
    /// Progress tools
    /// </summary>
    public static class ProgressTools
    {

        private static string progressUpperLeftCornerChar = "╔";
        private static string progressUpperRightCornerChar = "╗";
        private static string progressLowerLeftCornerChar = "╚";
        private static string progressLowerRightCornerChar = "╝";
        private static string progressUpperFrameChar = "═";
        private static string progressLowerFrameChar = "═";
        private static string progressLeftFrameChar = "║";
        private static string progressRightFrameChar = "║";

        /// <summary>
        /// Upper left corner character for the progress bar
        /// </summary>
        public static string ProgressUpperLeftCornerChar
        {
            get => progressUpperLeftCornerChar;
            set => progressUpperLeftCornerChar = string.IsNullOrEmpty(value) ? "╔" : value[0].ToString();
        }
        /// <summary>
        /// Upper right corner character for the progress bar
        /// </summary>
        public static string ProgressUpperRightCornerChar
        {
            get => progressUpperRightCornerChar;
            set => progressUpperRightCornerChar = string.IsNullOrEmpty(value) ? "╗" : value[0].ToString();
        }
        /// <summary>
        /// Lower left corner character for the progress bar
        /// </summary>
        public static string ProgressLowerLeftCornerChar
        {
            get => progressLowerLeftCornerChar;
            set => progressLowerLeftCornerChar = string.IsNullOrEmpty(value) ? "╚" : value[0].ToString();
        }
        /// <summary>
        /// Lower right corner character for the progress bar
        /// </summary>
        public static string ProgressLowerRightCornerChar
        {
            get => progressLowerRightCornerChar;
            set => progressLowerRightCornerChar = string.IsNullOrEmpty(value) ? "╝" : value[0].ToString();
        }
        /// <summary>
        /// Upper frame character for the progress bar
        /// </summary>
        public static string ProgressUpperFrameChar
        {
            get => progressUpperFrameChar;
            set => progressUpperFrameChar = string.IsNullOrEmpty(value) ? "═" : value[0].ToString();
        }
        /// <summary>
        /// Lower frame character for the progress bar
        /// </summary>
        public static string ProgressLowerFrameChar
        {
            get => progressLowerFrameChar;
            set => progressLowerFrameChar = string.IsNullOrEmpty(value) ? "═" : value[0].ToString();
        }
        /// <summary>
        /// Left frame character for the progress bar
        /// </summary>
        public static string ProgressLeftFrameChar
        {
            get => progressLeftFrameChar;
            set => progressLeftFrameChar = string.IsNullOrEmpty(value) ? "║" : value[0].ToString();
        }
        /// <summary>
        /// Right frame character for the progress bar
        /// </summary>
        public static string ProgressRightFrameChar
        {
            get => progressRightFrameChar;
            set => progressRightFrameChar = string.IsNullOrEmpty(value) ? "║" : value[0].ToString();
        }

    }
}
