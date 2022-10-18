
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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

using Extensification.StringExts;
using KS.Misc.Writers.ConsoleWriters;
using KS.ConsoleBase.Colors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KS.Misc.Writers.FancyWriters
{
    /// <summary>
    /// Border writer with color support
    /// </summary>
    public static class BorderColor
    {
        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        public static void WriteBorderPlain(int Left, int Top, int InteriorWidth, int InteriorHeight) =>
            WriteBorderPlain(Left, Top, InteriorWidth, InteriorHeight, "╔", "╚", "╗", "╝", "═", "═", "║", "║");

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for border</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for border</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for border</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for border</param>
        /// <param name="UpperFrameChar">Upper frame character for border</param>
        /// <param name="LowerFrameChar">Lower frame character for border</param>
        /// <param name="LeftFrameChar">Left frame character for border</param>
        /// <param name="RightFrameChar">Right frame character for border</param>
        public static void WriteBorderPlain(int Left, int Top, int InteriorWidth, int InteriorHeight, 
                                            string UpperLeftCornerChar, string LowerLeftCornerChar, string UpperRightCornerChar, string LowerRightCornerChar, 
                                            string UpperFrameChar, string LowerFrameChar, string LeftFrameChar, string RightFrameChar)
        {
            TextWriterWhereColor.WriteWhere(UpperLeftCornerChar + UpperFrameChar.Repeat(InteriorWidth) + UpperRightCornerChar, Left, Top, true, ColorTools.ColTypes.Separator);
            for (int i = 1; i <= InteriorHeight; i++)
            {
                TextWriterWhereColor.WriteWhere(LeftFrameChar, Left, Top + i, true, ColorTools.ColTypes.Separator);
                TextWriterWhereColor.WriteWhere(RightFrameChar, Left + InteriorWidth + 1, Top + i, true, ColorTools.ColTypes.Separator);
            }
            TextWriterWhereColor.WriteWhere(LowerLeftCornerChar + LowerFrameChar.Repeat(InteriorWidth) + LowerRightCornerChar, Left, Top + InteriorHeight + 1, true, ColorTools.ColTypes.Separator);
        }
    }
}
