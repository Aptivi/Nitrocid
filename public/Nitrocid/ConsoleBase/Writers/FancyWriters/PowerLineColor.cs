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

using System;
using System.Threading;
using System.Collections.Generic;
using Terminaux.Colors;
using System.Text;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using Nitrocid.ConsoleBase.Writers.FancyWriters.Tools;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers.ConsoleWriters;

namespace Nitrocid.ConsoleBase.Writers.FancyWriters
{
    /// <summary>
    /// PowerLine writer
    /// </summary>
    public static class PowerLineColor
    {

        /// <summary>
        /// Writes the PowerLine text
        /// </summary>
        /// <param name="Segments">Segments to write</param>
        /// <param name="Line">Write new line after writing the segments</param>
        public static void WritePowerLinePlain(List<PowerLineSegment> Segments, bool Line = false)
        {
            try
            {
                TextWriterColor.WritePlain(RenderPowerLine(Segments, Line), false);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Writes the PowerLine text
        /// </summary>
        /// <param name="Segments">List of PowerLine segments</param>
        /// <param name="EndingColor">A color that will be changed at the end of the transition</param>
        /// <param name="Line">Write new line after writing the segments</param>
        public static void WritePowerLine(List<PowerLineSegment> Segments, ConsoleColors EndingColor, bool Line = false) =>
            WritePowerLine(Segments, new Color(EndingColor), Line);

        /// <summary>
        /// Writes the PowerLine text
        /// </summary>
        /// <param name="Segments">List of PowerLine segments</param>
        /// <param name="EndingColor">A color that will be changed at the end of the transition</param>
        /// <param name="Line">Write new line after writing the segments</param>
        public static void WritePowerLine(List<PowerLineSegment> Segments, Color EndingColor, bool Line = false)
        {
            try
            {
                TextWriterColor.WritePlain(RenderPowerLine(Segments, EndingColor, Line), false);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Renders the PowerLine text
        /// </summary>
        /// <param name="Segments">Segments to write</param>
        /// <param name="Line">Write new line after writing the segments</param>
        public static string RenderPowerLine(List<PowerLineSegment> Segments, bool Line = false) =>
            RenderPowerLine(Segments, KernelColorTools.GetColor(KernelColorType.Background), Line);

        /// <summary>
        /// Renders the PowerLine text
        /// </summary>
        /// <param name="Segments">Segments to write</param>
        /// <param name="EndingColor">A color that will be changed at the end of the transition</param>
        /// <param name="Line">Write new line after writing the segments</param>
        public static string RenderPowerLine(List<PowerLineSegment> Segments, Color EndingColor, bool Line = false)
        {
            var segment = new StringBuilder();
            segment.Append(PowerLineTools.RenderSegments(Segments, EndingColor));
            if (Line)
                segment.AppendLine();
            return segment.ToString();
        }

    }
}
