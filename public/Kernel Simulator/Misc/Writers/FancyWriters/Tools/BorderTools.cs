
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
    /// All border tools here
    /// </summary>
    public static class BorderTools
    {
        private static string _borderUpperLeftCornerChar = "╔";
        private static string _borderUpperRightCornerChar = "╗";
        private static string _borderLowerLeftCornerChar = "╚";
        private static string _borderLowerRightCornerChar = "╝";
        private static string _borderUpperFrameChar = "═";
        private static string _borderLowerFrameChar = "═";
        private static string _borderLeftFrameChar = "║";
        private static string _borderRightFrameChar = "║";

        /// <summary>
        /// Upper left corner character 
        /// </summary>
        public static string BorderUpperLeftCornerChar
        {
            get
            {
                return _borderUpperLeftCornerChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╔";
                _borderUpperLeftCornerChar = value;
            }
        }
        /// <summary>
        /// Upper right corner character 
        /// </summary>
        public static string BorderUpperRightCornerChar
        {
            get
            {
                return _borderUpperRightCornerChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╗";
                _borderUpperRightCornerChar = value;
            }
        }
        /// <summary>
        /// Lower left corner character 
        /// </summary>
        public static string BorderLowerLeftCornerChar
        {
            get
            {
                return _borderLowerLeftCornerChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╚";
                _borderLowerLeftCornerChar = value;
            }
        }
        /// <summary>
        /// Lower right corner character 
        /// </summary>
        public static string BorderLowerRightCornerChar
        {
            get
            {
                return _borderLowerRightCornerChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╝";
                _borderLowerRightCornerChar = value;
            }
        }
        /// <summary>
        /// Upper frame character 
        /// </summary>
        public static string BorderUpperFrameChar
        {
            get
            {
                return _borderUpperFrameChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "═";
                _borderUpperFrameChar = value;
            }
        }
        /// <summary>
        /// Lower frame character 
        /// </summary>
        public static string BorderLowerFrameChar
        {
            get
            {
                return _borderLowerFrameChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "═";
                _borderLowerFrameChar = value;
            }
        }
        /// <summary>
        /// Left frame character 
        /// </summary>
        public static string BorderLeftFrameChar
        {
            get
            {
                return _borderLeftFrameChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "║";
                _borderLeftFrameChar = value;
            }
        }
        /// <summary>
        /// Right frame character 
        /// </summary>
        public static string BorderRightFrameChar
        {
            get
            {
                return _borderRightFrameChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "║";
                _borderRightFrameChar = value;
            }
        }
    }
}
